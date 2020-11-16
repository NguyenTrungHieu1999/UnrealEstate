using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Data.Entities;
using UnrealEstate.Repository.Wrapper;
using UnrealEstate.ViewModels.Catalog.Bids;

namespace UnrealEstate.Web.Services.Bids
{
    public class BidService : IBidService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public BidService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<Bid> GetBidById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BidViewModel>> GetBidsByListingId(int listingId)
        {
            var bids = await _repository.BidRepo.FindByCondition(x => x.ListingId == listingId).ToListAsync();

            var viewModel = _mapper.Map<List<Bid>, List<BidViewModel>>(bids);

            return viewModel;
        }
    }
}
