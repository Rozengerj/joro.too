using joro.too.DataAccess;
using joro.too.Entities;
using Microsoft.EntityFrameworkCore;

namespace joro.too.Services.Services;

public class VideoService
{
    public MovieDbContext context;
    public DbSet<Episode> vid;

    public VideoService(MovieDbContext context)
    {
        this.context = context;
        vid = context.Set<Episode>();
    }
    public async Task<bool> RemoveVideo(int id)
    {
        if (await vid.FindAsync(id) is null)
        {
            return false;
        }
        vid.Remove(await vid.FindAsync(id));
        await context.SaveChangesAsync();
        return true;
    }
    public async Task UpdateVideo(Episode episode)
    {
        vid.Update(episode);
        await context.SaveChangesAsync();
    }
}