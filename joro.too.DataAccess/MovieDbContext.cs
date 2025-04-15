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
    public class MovieDbContext : IdentityDbContext<IdentityUser>
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
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
            modelBuilder.Entity<GenresMovies>().HasOne(g => g.Genre).WithMany(m => m.Movies)
                .HasForeignKey(m => m.GenreId);
            modelBuilder.Entity<GenresMovies>().HasOne(m => m.Movie).WithMany(g => g.Genres)
                .HasForeignKey(g => g.MovieId);
            //ShowGenre
            modelBuilder.Entity<GenresShows>().HasKey(k => new { k.ShowId, k.GenreId });
            modelBuilder.Entity<GenresShows>().HasOne(g => g.Genre).WithMany(m => m.Shows)
                .HasForeignKey(m => m.GenreId);
            modelBuilder.Entity<GenresShows>().HasOne(m => m.Show).WithMany(g => g.Genres).HasForeignKey(g => g.ShowId);
            //MoviesActors
            modelBuilder.Entity<ActorRolesMovies>().HasKey(k => new { k.MovieId, k.ActorId });
            modelBuilder.Entity<ActorRolesMovies>().HasOne(a => a.Actor).WithMany(r => r.RolesInMovies)
                .HasForeignKey(a => a.ActorId);
            modelBuilder.Entity<ActorRolesMovies>().HasOne(m => m.Movie).WithMany(a => a.Actors)
                .HasForeignKey(g => g.MovieId);
            //ShowsActors
            modelBuilder.Entity<ActorRolesShows>().HasKey(k => new { k.ShowId, k.ActorId });
            modelBuilder.Entity<ActorRolesShows>().HasOne(a => a.Actor).WithMany(r => r.RolesInShows)
                .HasForeignKey(a => a.ActorId);
            modelBuilder.Entity<ActorRolesShows>().HasOne(m => m.Show).WithMany(a => a.Actors)
                .HasForeignKey(g => g.ShowId);


            modelBuilder.Entity<Genre>().HasData(
                new Genre() { Id = 1, Type = "Comedy" },
                new Genre() { Id = 2, Type = "Action" },
                new Genre() { Id = 3, Type = "Thriller" },
                new Genre() { Id = 4, Type = "Romance" },
                new Genre() { Id = 5, Type = "Science Fiction" }
            );
            modelBuilder.Entity<Movie>().HasData(
                new Movie()
                {
                    Id = 1,
                    Name = "Shrek",
                    Description = "All fairytale creatures get kicked out and get put in the swamp of an grumpy ogre, who has to follow a quest to get them out.",
                    MediaImgSrc = "https://res.cloudinary.com/djubwo5uq/image/upload/v1743439766/wtygxcolnsqykqq2ktzr.jpg",
                    RatedCount = 13,
                    RatingsSum = 100,
                    VidSrc = "https://res.cloudinary.com/djubwo5uq/video/upload/v1743439842/jebwtwrywpzzlmzzlxrs.mp4"
                }
            );
            modelBuilder.Entity<Show>().HasData(
                new Show()
                {
                    Id = 1,
                    Name = "One Piece",
                    MediaImgSrc =
                        "https://res.cloudinary.com/djubwo5uq/image/upload/v1744470967/eycbyrohr6vtvjqamn3e.jpg",
                    Description =
                        "Monkey D Luffy sets out to sea to find the legendary pirate treasure One Piece!",
                    RatedCount = 10,
                    RatingsSum = 85,
                }
            );
            modelBuilder.Entity<Season>().HasData(
                new Season()
                {
                    Id = 1,
                    Number = 1,
                    Name = "Romance Dawn",
                    ShowId = 1,
                },
                new Season()
                {
                    Id = 2,
                    Number = 2,
                    Name = "Orange Town",
                    ShowId = 1,
                }
            );
            modelBuilder.Entity<Episode>().HasData(
                new Episode()
                {
                    Id = 1,
                    name = "Im Luffy, the man who'll become King of the Pirates!",
                    SeasonId = 1,
                    vidsrc = "https://res.cloudinary.com/djubwo5uq/video/upload/v1742844729/vcszzljfpaz5sktkbzh3.mp4"
                },
                new Episode()
                {
                    Id = 2,
                    name = "Pirate Hunter Zoro Appears!",
                    SeasonId = 1,
                    vidsrc = "https://res.cloudinary.com/djubwo5uq/video/upload/v1742844740/gijx1zxfotjdp2jkyqjb.mp4"
                },
                new Episode()
                {
                    Id = 3,
                    name = "The Monstrous Captain Morgan",
                    SeasonId = 1,
                    vidsrc = "https://res.cloudinary.com/djubwo5uq/video/upload/v1742844754/qp8rzyzmn1twghkidywx.mp4"
                },
                new Episode()
                {
                    Id = 4,
                    name = "Luffy's past. The Red-Haired Shanks.",
                    SeasonId = 1,
                    vidsrc = "https://res.cloudinary.com/djubwo5uq/video/upload/v1742844768/jsw1lmzcngmelb9tdir6.mp4"
                },
                new Episode()
                {
                    Id = 5,
                    name = "Fear! The Mysterious Clown Pirate Captain Buggy!",
                    SeasonId = 2,
                    vidsrc = "https://res.cloudinary.com/djubwo5uq/video/upload/v1743544387/oobotrj7rjyrrluueovs.mp4"
                }
            );
            modelBuilder.Entity<GenresShows>().HasData(
                new GenresShows()
                {
                    GenreId = 1,
                    ShowId = 1
                },
                new GenresShows()
                {
                    GenreId = 2,
                    ShowId = 1
                },
                new GenresShows()
                {
                    GenreId = 5,
                    ShowId = 1
                }
            );
            modelBuilder.Entity<GenresMovies>().HasData(
                new GenresMovies()
                {
                    GenreId = 1,
                    MovieId = 1
                },
                new GenresMovies()
                {
                    GenreId = 2,
                    MovieId = 1
                },
                new GenresMovies()
                {
                    GenreId = 4,
                    MovieId = 1
                }
            );
        }
    }
}