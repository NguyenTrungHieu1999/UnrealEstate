using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Data.Configurations
{
    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.ToTable("Bids");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.ListingId)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Price)
                .IsRequired(true)
                .HasColumnType("Decimal(18,0)");

            builder.Property(x => x.CreateDate)
                .IsRequired(true);

            builder.Property(x => x.Comment)
                .IsRequired(true);

            builder.HasOne(u=>u.User)
                .WithMany(b=>b.Bids)
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Listing)
                .WithMany(b => b.Bids)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
