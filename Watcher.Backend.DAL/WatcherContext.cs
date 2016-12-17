using System.Data.Entity;
using Watcher.Backend.DAL.Entities;

namespace Watcher.Backend.DAL
{
    public class WatcherContext : DbContext
    {
        public WatcherContext() : base("WatcherContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<WatcherContext>());
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
