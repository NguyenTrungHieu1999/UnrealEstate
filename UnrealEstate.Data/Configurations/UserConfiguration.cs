using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnrealEstate.Data.Entities;
using UnrealEstate.Data.Enums;

namespace UnrealEstate.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Birthday)
                .IsRequired();

            builder.Property(x => x.Gender)
                .IsRequired()
                .HasDefaultValue(Gender.Male);

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Email)
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();
        }
    }
}
