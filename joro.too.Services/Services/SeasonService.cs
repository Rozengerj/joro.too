using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace joro.too.Services;

public class SeasonService:ISeasonService
{
    public MovieDbContext context;
    public DbSet<Season> s;

    public SeasonService(MovieDbContext context)
    {
        this.context = context;
        s = context.Set<Season>();
    }
    public async Task<bool> RemoveSeason(int id)
    {
        if ((await s.FindAsync(id)) is null)
        {
            return false;
        }
        s.Remove(await s.FindAsync(id));
        await context.SaveChangesAsync();
        return true;
    }
    public async Task UpdateSeason(Season episode)
    {
        s.Update(episode);
        await context.SaveChangesAsync();
    }
    public async Task<Season> FindSeasonById(int id)
    {
        return await s.Include(x => x.Episodes).ThenInclude(y => y.Comments).ThenInclude(z=>z.Commenter).Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public Task AddEpisdesToSeason(Season season, List<string> episodeNames, List<string> episodeVidSrcs)
    {
        Console.WriteLine(string.Join(", ", episodeNames));
        List<Episode> episodes = new List<Episode>();
        for (int i = 0; i < episodeNames.Count; i++)
        {
            episodes.Add(new Episode()
            {
                Comments = new List<Comment>(),
                name = episodeNames[i],
                vidsrc = episodeVidSrcs[i],
                Season = season,
                SeasonId = season.Id
            });
        }
        season.Episodes.AddRange(episodes);
        return context.SaveChangesAsync();
    }
}