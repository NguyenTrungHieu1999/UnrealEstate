using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace UnrealEstate.Data.EF
{
    public class UnrealEstateDbContextFactory : IDesignTimeDbContextFactory<UnrealEstateDbContext>
    {
        public UnrealEstateDbContext CreateDbContext(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("UnrealEstateDb");

            var optionsBuilder = new DbContextOptionsBuilder<UnrealEstateDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new UnrealEstateDbContext(optionsBuilder.Options);
        }
    }
}
