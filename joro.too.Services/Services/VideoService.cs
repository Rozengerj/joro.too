using joro.too.DataAccess;
using joro.too.Entities;
using Microsoft.EntityFrameworkCore;

namespace joro.too.Services.Services;

public class VideoService
{
    public MovieDbContext context;
    public DbSet<Video> vid;

    public VideoService(MovieDbContext context)
    {
        this.context = context;
        vid = context.Set<Video>();
    }

    public void AddVideo(string name, string vidsrc)
    {
        var Video = new Video() { name = name, vidsrc = vidsrc, Comments = new List<Comment>()};
        vid.AddAsync(Video);
        context.SaveChangesAsync();
    }
    
    
}