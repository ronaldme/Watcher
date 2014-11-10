using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Repository
{
    public class MovieContext : DbContext
    {
        public MovieContext()
            : base("MovieContext")
        {
           //Database.SetInitializer<DatalogContext>(new CreateDatabaseIfNotExists<DatalogContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Plc>().ToTable("Plc");
            modelBuilder.Entity<Machine>().ToTable("Machine");
        }
    }
}