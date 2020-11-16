using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using UnrealEstate.Data.Entities;
using UnrealEstate.Data.Enums;

namespace UnrealEstate.Data.Extensitons
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Listing>().HasData(
                new Listing
                {
                    Id = 1,
                    AddressLine1 = "test",
                    AddressLine2 = "test",
                    City = "Dong Thap",
                    State = "Dong Thap",
                    Zip = 100000,
                    Status = Status.Active,
                    Beds = 20,
                    Size = 200,
                    BuiltYear = 20,
                    StartingPrice = 1000000000,
                    DueDate = new DateTime(2020, 10, 10),
                    Description = "beautiful",
                    CreateDate = DateTime.Now,
                    CreateBy = "Admin",
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = "Admin"
                },

                new Listing
                {
                    Id = 2,
                    AddressLine1 = "test1",
                    AddressLine2 = "test1",
                    City = "Dong Thap1",
                    State = "Dong Thap1",
                    Zip = 200000,
                    Status = Status.Active,
                    Beds = 20,
                    Size = 200,
                    BuiltYear = 20,
                    StartingPrice = 1000000000,
                    DueDate = new DateTime(2020, 10, 10),
                    Description = "beautiful",
                    CreateDate = DateTime.Now,
                    CreateBy = "Admin",
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = "Admin"
                },

                new Listing
                {
                    Id = 3,
                    AddressLine1 = "test2",
                    AddressLine2 = "test2",
                    City = "Dong Thap2",
                    State = "Dong Thap2",
                    Zip = 300000,
                    Status = Status.Active,
                    Beds = 20,
                    Size = 200,
                    BuiltYear = 20,
                    StartingPrice = 1000000000,
                    DueDate = new DateTime(2020, 10, 10),
                    Description = "beautiful",
                    CreateDate = DateTime.Now,
                    CreateBy = "Admin",
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = "Admin"
                },

                new Listing
                {
                    Id = 4,
                    AddressLine1 = "test4",
                    AddressLine2 = "test4",
                    City = "Dong Thap4",
                    State = "Dong Thap4",
                    Zip = 400000,
                    Status = Status.Active,
                    Beds = 20,
                    Size = 200,
                    BuiltYear = 20,
                    StartingPrice = 1000000000,
                    DueDate = new DateTime(2020, 10, 10),
                    Description = "beautiful",
                    CreateDate = DateTime.Now,
                    CreateBy = "Admin",
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = "Admin"
                }
            );


            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "Administrator role"
                },
                new Role
                {
                    Id = 2,
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "User role"
                }
                );


            var hasher = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "Admin",
                    Email = "hieutanmy321@gmail.com",
                    PasswordHash = hasher.HashPassword(null, "Hieu@123"),
                    FirstName = "Hieu",
                    LastName = "Nguyen",
                    Birthday = new DateTime(1999, 05, 25),
                    PhoneNumber = "0965924083",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "HIEUTANMY321@GMAIL.COM",
                    Status = UserStatus.Active
                },

                new User
                {
                    Id = 2,
                    UserName = "User",
                    Email = "17110298@student.hcmute.edu.vn",
                    PasswordHash = hasher.HashPassword(null, "Hieu@123"),
                    FirstName = "Nam",
                    LastName = "Phan",
                    Birthday = new DateTime(1999, 11, 18),
                    PhoneNumber = "0123456789",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    NormalizedEmail = "17110298@STUDENT.HCMUTE.EDU.VN",
                    NormalizedUserName = "USER",
                    Status = UserStatus.Active
                },

                new User
                {
                    Id = 3,
                    UserName = "User1",
                    Email = "nguyentrunghieu25051999@gmail.com",
                    PasswordHash = hasher.HashPassword(null, "Hieu@123"),
                    FirstName = "Hao",
                    LastName = "Van",
                    Birthday = new DateTime(1999, 10, 20),
                    PhoneNumber = "0123456789",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    NormalizedUserName = "USER1",
                    NormalizedEmail = "NGUYENTRUNGHIEU25051999@GMAIL.COM",
                    Status = UserStatus.Active
                }
                );

            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    RoleId = 1,
                    UserId = 1
                },

                new IdentityUserRole<int>
                {
                    RoleId = 2,
                    UserId = 2
                },

                new IdentityUserRole<int>
                {
                    RoleId = 2,
                    UserId = 3
                }
                );

            modelBuilder.Entity<ListingPhoto>().HasData(
                new ListingPhoto
                {
                    Id = 1,
                    ListingId = 1,
                    IsDefault = true,
                    PhotoUrl = "90461412_1144921282508959_1672053750702800896_o.jpg"
                },
                new ListingPhoto
                {
                    Id = 2,
                    ListingId = 2,
                    IsDefault = true,
                    PhotoUrl = "90564519_1144921415842279_5363427452687220736_o.jpg"
                },
                new ListingPhoto
                {
                    Id = 3,
                    ListingId = 3,
                    IsDefault = true,
                    PhotoUrl = "90565589_1144921305842290_6692510266459947008_o.jpg"
                },
                new ListingPhoto
                {
                    Id = 4,
                    ListingId = 4,
                    IsDefault = true,
                    PhotoUrl = "90618052_1144921235842297_5977573321787572224_o.jpg"
                }
                );

            modelBuilder.Entity<Bid>().HasData(
                new Bid
                {
                    Id = 1,
                    ListingId = 1,
                    UserId = 1,
                    Price = 1100000000,
                    CreateDate = DateTime.Now,
                    Comment = "I will win",
                },
                new Bid
                {
                    Id = 2,
                    ListingId = 1,
                    UserId = 2,
                    Price = 1100000001,
                    CreateDate = DateTime.Now,
                    Comment = "I will win",
                },
                new Bid
                {
                    Id = 3,
                    ListingId = 1,
                    UserId = 1,
                    Price = 1100000002,
                    CreateDate = DateTime.Now,
                    Comment = "I will win",
                },
                new Bid
                {
                    Id = 4,
                    ListingId = 1,
                    UserId = 3,
                    Price = 1100000003,
                    CreateDate = DateTime.Now,
                    Comment = "I will win",
                },
                new Bid
                {
                    Id = 5,
                    ListingId = 2,
                    UserId = 2,
                    Price = 2100000000,
                    CreateDate = DateTime.Now,
                    Comment = "I will win",
                },
                new Bid
                {
                    Id = 6,
                    ListingId = 2,
                    UserId = 3,
                    Price = 2100000001,
                    CreateDate = DateTime.Now,
                    Comment = "I will win",
                }
                );
        }

    }
}