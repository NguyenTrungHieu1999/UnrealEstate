using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.ListingId)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.CreateDate)
                .IsRequired(true);

            builder.Property(x => x.Text)
                .IsRequired(true);

            builder.Property(x => x.ModifiedDate)
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany(c => c.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Listing)
                .WithMany(c => c.Comments)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
