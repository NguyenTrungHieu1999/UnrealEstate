using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UnrealEstate.Data.Entities;
using UnrealEstate.Data.Enums;
using UnrealEstate.Repository.Wrapper;
using UnrealEstate.ViewModels.Catalog.Users;
using UnrealEstate.ViewModels.Common;

namespace UnrealEstate.Web.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;

        public UserService(IMapper mapper, IRepositoryWrapper repository, UserManager<User> userManager,
            SignInManager<User> signInManager, IConfiguration config)
        {
            _mapper = mapper;
            _repository = repository;
            _userManager = userManager;
            _signInManager = signInManager; ;
            _config = config;
        }

        public async Task<ApiResult<string>> AuthenticateAsync(LoginRequest request)
        {
            var user = await _repository.UserRepo.FindByCondition(x => x.Email == request.Email)
                                                    .SingleOrDefaultAsync();

            if (user is null)
            {
                return new ApiErrorResult<string>("Account does not exist");
            }

            if (user.Status == UserStatus.Disable)
            {
                return new ApiErrorResult<string>("Account is locked");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, request.RememberMe, true);

            if (!result.Succeeded)
            {
                return new ApiErrorResult<string>("Login incorrectly");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";", roles)),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["JWT:ValidIssuer"],
                _config["JWT:ValidAudience"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<ApiResult<bool>> DeleteUserAsync(int id)
        {
            var user = await _repository.UserRepo.FindByIdAsync(id);

            if (user is null)
            {
                return new ApiErrorResult<bool>("User does not found");
            }

            user.Status = UserStatus.Disable;

            _repository.UserRepo.Update(user);

            var result = await _repository.SaveChangesAsync();

            if (result != 0)
            {
                return new ApiSuccessResult<bool>();
            }

            return new ApiErrorResult<bool>("Delete failed");
        }

        private async Task<List<UserVm>> GetAllUserAsync(GetUserPagingRequest filter)
        {
            List<User> users = await _repository.UserRepo.FindByCondition(x => x.Status != UserStatus.Disable)
                                                            .ToListAsync();

            List<UserVm> userRequests = _mapper.Map<List<User>, List<UserVm>>(users);

            if (!string.IsNullOrEmpty(filter.Email))
            {
                userRequests = userRequests.Where(x => x.Email.Contains(filter.Email)).ToList();
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                userRequests = userRequests.Where(x => x.FirstName.Contains(filter.Name) || x.LastName.Contains(filter.Name)).ToList();
            }

            if (filter.Limit != null)
            {
                userRequests = userRequests.OrderByDescending(x => x.Id)
                                .Skip(filter.Limit.Value * (filter.Offset.Value - 1))
                                .Take(filter.Limit.Value).ToList();
            }

            if (filter.OrderBy != null)
            {
                userRequests = userRequests.OrderBy(x => x.GetType().GetProperty(filter.OrderBy).GetValue(x, null)).ToList();
            }

            return userRequests;
        }

        public async Task<ApiResult<UserVm>> GetUserByIdAsync(int id)
        {

            User user = await _repository.UserRepo.FindByIdAsync(id);

            if (user is null)
            {
                return new ApiErrorResult<UserVm>("User not found");
            }

            var userView = _mapper.Map<UserVm>(user);

            return new ApiSuccessResult<UserVm>(userView);
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest request)
        {
            var query = await GetAllUserAsync(request);

            int totalRow = query.Count();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => x)
                .ToList();

            var pageResult = new PagedResult<UserVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            if (data is null)
            {
                return new ApiErrorResult<PagedResult<UserVm>>("No records found");
            }

            return new ApiSuccessResult<PagedResult<UserVm>>(pageResult);
        }

        public async Task<ApiResult<bool>> RegisterAsync(UserRegisterRequest request)
        {
            var email = await _repository.UserRepo.FindByCondition(x => x.Email == request.Email)
                                                    .SingleOrDefaultAsync();

            if (email != null)
            {
                return new ApiErrorResult<bool>("Email already exists");
            }

            var user = await _repository.UserRepo.FindByCondition(x => x.UserName == request.UserName)
                                                    .SingleOrDefaultAsync();

            if (user != null)
            {
                return new ApiErrorResult<bool>("Account already exists");
            }

            user = _mapper.Map<User>(request);

            user.Status = UserStatus.Active;

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }

            return new ApiErrorResult<bool>("Registration failed");
        }

        public async Task<ApiResult<bool>> UpdateUserAsync(UserUpdateRequest request)
        {
            var user = await _repository.UserRepo.FindByIdAsync(request.Id);

            user.Map(request);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }

            return new ApiErrorResult<bool>("Change failed");
        }
    }
}
