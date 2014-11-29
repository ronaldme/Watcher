using System.Data.Entity;
using Repository.Entities;

namespace Repository
{
    public class WatcherData : DbContext
    {
        public WatcherData()
            : base("name=WatcherData")
        {
        }

        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Show> Shows { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Movies)
                .Map(m => m.ToTable("UsersMovie").MapLeftKey("MovieId").MapRightKey("UsersId"));

            modelBuilder.Entity<Show>()
               .HasMany(e => e.Users)
               .WithMany(e => e.Shows)
               .Map(m => m.ToTable("UsersShow").MapLeftKey("ShowId").MapRightKey("UsersId"));

            modelBuilder.Entity<Person>()
              .HasMany(e => e.Users)
              .WithMany(e => e.Persons)
              .Map(m => m.ToTable("UsersPerson").MapLeftKey("PersonId").MapRightKey("UsersId"));
        }
    }
}
