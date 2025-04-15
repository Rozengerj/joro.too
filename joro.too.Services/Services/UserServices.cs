using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace joro.too.Services.Services;

public class UserServices : IUserService
{
    public MovieDbContext context;
    public DbSet<Comment> comments;

    public UserServices(MovieDbContext context)
    {
        this.context = context;
        comments = context.Set<Comment>();
    }

    public async Task WriteComment(string text, User user, int mediaId, bool isShow)
    {
        if (isShow)
        {
            await comments.AddAsync(new Comment()
            {
                Text = text,
                UserId = user.Id,
                EpisodeId = mediaId,
                Commenter = user
            });
            await context.SaveChangesAsync();
            return;
        }

        await comments.AddAsync(new Comment()
        {
            Text = text,
            UserId = user.Id,
            MovieId = mediaId,
            Commenter = user
        });
        await context.SaveChangesAsync();
    }

    public async Task DeleteComment(int id)
    {
        var comment = comments.Find(id);
        comments.Remove(comment);
        await context.SaveChangesAsync();
    }

    public async Task RateMedia(User user, float rating, int mediaId, bool isShow)
    {
        if (user.RatedShows.IsNullOrEmpty())
        {
            user.RatedShows = new List<int>();
        }
        if (user.RatedMovies.IsNullOrEmpty())
        {
            user.RatedMovies = new List<int>();
        }
        if (isShow)
        {
            var show = await context.Shows.FindAsync(mediaId);
            show.RatedCount++;
            show.RatingsSum += rating;
            user.RatedShows.Add(mediaId);
            await context.SaveChangesAsync();
            return;
        }
        var movie = await context.Shows.FindAsync(mediaId);
        movie.RatedCount++;
        movie.RatingsSum += rating;
        user.RatedMovies.Add(mediaId);
        await context.SaveChangesAsync();
    }
}