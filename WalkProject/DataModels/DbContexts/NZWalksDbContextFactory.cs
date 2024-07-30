using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WalkProject.DataModels.DbContexts
{
    public class NZWalksDbContextFactory : IDesignTimeDbContextFactory<NZWalksDbContext>
    {
        public NZWalksDbContext CreateDbContext(string[] args)
        {

            // Build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Get connection string
            var connectionString = configuration.GetConnectionString("ConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder<NZWalksDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new NZWalksDbContext(optionsBuilder.Options);
        }
    }
}
