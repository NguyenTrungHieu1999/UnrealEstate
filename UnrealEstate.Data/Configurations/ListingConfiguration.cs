using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Data.Configurations
{
    public class ListingConfiguration : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.ToTable("Listings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.Status)
                .IsRequired(true);

            builder.Property(x => x.Beds)
                .IsRequired(true);

            builder.Property(x => x.Size)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.BuiltYear)
               .IsRequired();

            builder.Property(x => x.StartingPrice)
              .IsRequired()
              .HasColumnType("Decimal(18,0)");

            builder.Property(x => x.DueDate)
              .IsRequired();

            builder.Property(x => x.CreateDate)
              .IsRequired();

            builder.Property(x => x.ModifiedDate)
             .IsRequired();

            builder.Property(x => x.CreateBy)
             .IsRequired()
             .HasMaxLength(30);

            builder.Property(x => x.ModifiedBy)
             .IsRequired()
             .HasMaxLength(30);

            builder.Property(x => x.AddressLine1)
                .IsRequired();

            builder.Property(x => x.AddressLine2)
                .IsRequired();

            builder.Property(x => x.City)
                .IsRequired();

            builder.Property(x => x.Zip)
                .IsRequired();

            builder.Property(x => x.State)
                .IsRequired();
        }
    }
}
