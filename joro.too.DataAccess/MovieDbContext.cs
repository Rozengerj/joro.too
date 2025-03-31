using joro.too.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace joro.too.DataAccess
{
    public class MovieDbContext:IdentityDbContext<User>
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) :base(options)
        {
                
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenresMovies> GenresMovies { get; set; }
        public DbSet<GenresShows> GenresShows { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorRolesMovies> ActorsRolesMovies { get; set; }
        public DbSet<ActorRolesShows> ActorsRolesShows { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //MovieGenre
            modelBuilder.Entity<GenresMovies>().HasKey(k => new { k.MovieId, k.GenreId });
            modelBuilder.Entity<GenresMovies>().HasOne(g => g.Genre).WithMany(m => m.Movies).HasForeignKey(m => m.GenreId);
            modelBuilder.Entity<GenresMovies>().HasOne(m => m.Movie).WithMany(g => g.Genres).HasForeignKey(g => g.MovieId);
            //ShowGenre
            modelBuilder.Entity<GenresShows>().HasKey(k => new { k.ShowId, k.GenreId });
            modelBuilder.Entity<GenresShows>().HasOne(g => g.Genre).WithMany(m => m.Shows).HasForeignKey(m => m.GenreId);
            modelBuilder.Entity<GenresShows>().HasOne(m => m.Show).WithMany(g => g.Genres).HasForeignKey(g => g.ShowId);
            //MoviesActors
            modelBuilder.Entity<ActorRolesMovies>().HasKey(k => new { k.MovieId, k.ActorId });
            modelBuilder.Entity<ActorRolesMovies>().HasOne(a => a.Actor).WithMany(r => r.RolesInMovies).HasForeignKey(a=>a.ActorId);
            modelBuilder.Entity<ActorRolesMovies>().HasOne(m => m.Movie).WithMany(a => a.Actors).HasForeignKey(g => g.MovieId);
            //ShowsActors
            modelBuilder.Entity<ActorRolesShows>().HasKey(k => new { k.ShowId, k.ActorId });
            modelBuilder.Entity<ActorRolesShows>().HasOne(a => a.Actor).WithMany(r => r.RolesInShows).HasForeignKey(a=>a.ActorId);
            modelBuilder.Entity<ActorRolesShows>().HasOne(m => m.Show).WithMany(a => a.Actors).HasForeignKey(g => g.ShowId);


            modelBuilder.Entity<Genre>().HasData(
                new Genre() { Id = 1, Type = "Comedy" },
                new Genre() { Id = 2, Type = "Action"},
                new Genre() { Id = 3, Type = "Thriller" },
                new Genre() { Id = 4, Type = "Romance" },
                new Genre() { Id = 5, Type = "Science Fiction" }
                );
            
        }

    }
}
