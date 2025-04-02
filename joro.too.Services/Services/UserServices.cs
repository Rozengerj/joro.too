using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    public async Task WriteComment(string text, IdentityUser user)
    {
        await comments.AddAsync(new Comment()
        {
            Text = text,
            UserId = user.Id
        });
        await context.SaveChangesAsync();
    }

    public async Task DeleteComment(int id)
    {
        var comment = comments.Find(id);
        comments.Remove(comment);
        await context.SaveChangesAsync();
    }
    
}