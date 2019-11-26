using Microsoft.EntityFrameworkCore;
using Watcher.DAL.Entities;

namespace Watcher.DAL
{
    /// <summary>
    /// Add-Migration NAME -StartupProject Watcher.Service -Project Watcher.DAL
    /// </summary>
    public class WatcherDbContext : DbContext
    {
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Show> Shows { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMovie>().HasKey(bc => new { bc.UserId, bc.MovieId });

            modelBuilder.Entity<UserMovie>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserMovies)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<UserMovie>()
                .HasOne(bc => bc.Movie)
                .WithMany(c => c.UserMovies)
                .HasForeignKey(bc => bc.MovieId);

            modelBuilder.Entity<UserShow>().HasKey(bc => new { bc.UserId, bc.ShowId });

            modelBuilder.Entity<UserShow>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserShows)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<UserShow>()
                .HasOne(bc => bc.Show)
                .WithMany(c => c.UserShows)
                .HasForeignKey(bc => bc.ShowId);

            modelBuilder.Entity<UserPerson>().HasKey(bc => new { bc.UserId, bc.PersonId });

            modelBuilder.Entity<UserPerson>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserPersons)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<UserPerson>()
                .HasOne(bc => bc.Person)
                .WithMany(c => c.UserPersons)
                .HasForeignKey(bc => bc.PersonId);
        }
    }
}