using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using UnrealEstate.Data.Configurations;
using UnrealEstate.Data.Entities;
using UnrealEstate.Data.Extensitons;

namespace UnrealEstate.Data.EF
{
    public class UnrealEstateDbContext : IdentityDbContext<User, Role, int>
    {
        public UnrealEstateDbContext(DbContextOptions options) : base(options)
        {

        }

        public UnrealEstateDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

                var connectionString = configuration.GetConnectionString("UnrealEstateDb");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new BidConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new FavoriteConfiguration());
            modelBuilder.ApplyConfiguration(new ListingConfiguration());
            modelBuilder.ApplyConfiguration(new ListingNoteConfiguration());
            modelBuilder.ApplyConfiguration(new ListingPhotoConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("AppUserClaims");

            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("AppUserLogins").HasKey(x => new {x.UserId, x.ProviderKey, x.ProviderDisplayName, x.LoginProvider});

            //modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("AppUserLogins").Property(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("AppRoleClaims");

            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

            modelBuilder.Seed();
        }

        public DbSet<Bid> Bids { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<ListingNote> ListingNotes { get; set; }
        public DbSet<ListingPhoto> ListingPhotos { get; set; }
    }
}
