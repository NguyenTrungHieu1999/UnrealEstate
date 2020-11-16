using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Data.Configurations
{
    public class ListingPhotoConfiguration : IEntityTypeConfiguration<ListingPhoto>
    {
        public void Configure(EntityTypeBuilder<ListingPhoto> builder)
        {
            builder.ToTable("ListingPhotos");

            builder.HasKey(x =>x.Id);

            builder.Property(x => x.Id)
                .IsRequired(true);

            builder.Property(x => x.ListingId)
                .IsRequired(true);

            builder.Property(x => x.PhotoUrl)
                .IsRequired(true);

            builder.Property(x => x.IsDefault)
                .IsRequired();

            builder.HasOne(l => l.Listing)
                .WithMany(lp => lp.ListingPhotos)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
