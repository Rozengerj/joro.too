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
    public class MovieDbContext:IdentityDbContext<IdentityUser>
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) :base(options)
        {
                
        }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MediaGenres> MediasGenres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorRoles> ActorsRoles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MediaGenres>().HasKey(k => new { k.MediaId, k.GenreId });
            modelBuilder.Entity<MediaGenres>().HasOne(g => g.Genre).WithMany(m => m.Medias).HasForeignKey(m => m.GenreId);
            modelBuilder.Entity<MediaGenres>().HasOne(m => m.Media).WithMany(g => g.Genres).HasForeignKey(g => g.MediaId);

            modelBuilder.Entity<ActorRoles>().HasKey(k => new { k.MediaId, k.ActorId });
            modelBuilder.Entity<ActorRoles>().HasOne(a => a.Actor).WithMany(r => r.Roles).HasForeignKey(a=>a.ActorId);
            modelBuilder.Entity<ActorRoles>().HasOne(m => m.Media).WithMany(a => a.Actors).HasForeignKey(g => g.MediaId);


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
