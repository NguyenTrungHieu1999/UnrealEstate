using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Data.Configurations
{
    public class ListingNoteConfiguration : IEntityTypeConfiguration<ListingNote>
    {
        public void Configure(EntityTypeBuilder<ListingNote> builder)
        {
            builder.ToTable("ListingNotes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.ListingId)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Text)
                .IsRequired(true);

            builder.HasOne(u => u.User)
                .WithMany(ln => ln.ListingNotes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Listing)
                .WithMany(ln => ln.ListingNotes)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
