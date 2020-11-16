using System;
using UnrealEstate.Data.Entities;
using UnrealEstate.ViewModels.Catalog.Listings;

namespace UnrealEstate.Web.Services.Listings
{
    public static class ListingExtensions
    {
        public static void Map(this Listing listing, ListingUpdateRequest request)
        {
            listing.AddressLine1 = request.AddressLine1;
            listing.AddressLine2 = request.AddressLine2;
            listing.Zip = request.Zip;
            listing.City = request.City;
            listing.State = listing.State;
            listing.Status = request.Status;
            listing.Beds = request.Beds;
            listing.Size = request.Size;
            listing.BuiltYear = request.BuiltYear;
            listing.StartingPrice = request.StartingPrice;
            listing.DueDate = request.DueDate;
            listing.Description = request.Description;
            listing.ModifiedDate = DateTime.Now;
            listing.ModifiedBy = request.ModifiedBy;
        }
    }
}
