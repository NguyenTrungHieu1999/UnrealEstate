using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Data.Entities;
using UnrealEstate.Repository.Wrapper;
using UnrealEstate.Utilities.Exceptions;
using UnrealEstate.ViewModels.Catalog.Comments;

namespace UnrealEstate.Web.Services.Comments
{
    public class CommentServices : ICommentServices
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;

        public CommentServices(IMapper mapper, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<int> CreateCommentAsync(CommentCreateRequest request)
        {
            Comment comment = _mapper.Map<Comment>(request);

            comment.CreateDate = DateTime.Now;
            comment.ModifiedDate = DateTime.Now;

            await _repository.CommmentRepo.CreateAsync(comment);
            await _repository.SaveChangesAsync();

            return comment.Id;
        }

        public async Task<int> RemoveCommentAsync(int Id)
        {
            Comment comment = await FindCommentByIdAsync(Id);

            _repository.CommmentRepo.Delete(comment);

            return await _repository.SaveChangesAsync();
        }

        public async Task<int> UpdateCommentAsync(CommentUpdateRequest request)
        {
            Comment comment = await FindCommentByIdAsync(request.Id);

            comment.ModifiedDate = DateTime.Now;
            comment.Text = request.Text;

            _repository.CommmentRepo.Update(comment);

            return await _repository.SaveChangesAsync();
        }

        private async Task<Comment> FindCommentByIdAsync(int Id)
        {
            Comment comment = await _repository.CommmentRepo.FindByIdAsync(Id);

            if (comment is null)
            {
                throw new UnrealEstateException($"Cannot find comment with id {Id}");
            }

            return comment;
        }

        public async Task<List<Comment>> GetAllCommentByListingId(int listingId)
        {
            var comments = await _repository.CommmentRepo.FindByCondition(x => x.ListingId == listingId).ToListAsync();

            return comments;
        }
    }
}
