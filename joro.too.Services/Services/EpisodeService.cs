using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace joro.too.Services.Services;

public class EpisodeService : IEpisodeService
{
    public MovieDbContext context;
    public DbSet<Episode> vid;

    public EpisodeService(MovieDbContext context)
    {
        this.context = context;
        vid = context.Set<Episode>();
    }
    public async Task<bool> RemoveEpisode(int id)
    {
        if (await vid.FindAsync(id) is null)
        {
            return false;
        }
        vid.Remove(await vid.FindAsync(id));
        await context.SaveChangesAsync();
        return true;
    }
    public async Task UpdateEpisode(Episode episode)
    {
        vid.Update(episode);
        await context.SaveChangesAsync();
    }

    public async Task<Episode> FindEpisodeById(int id)
    {
        return await vid.FindAsync(id);
    }
}