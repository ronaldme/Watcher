using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Watcher.DAL
{
    public class WatcherContextFactory : IDesignTimeDbContextFactory<WatcherDbContext>
    {
        public WatcherDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WatcherDbContext>();
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["WatcherDbContext"].ConnectionString);
            return new WatcherDbContext(optionsBuilder.Options);
        }
    }
}