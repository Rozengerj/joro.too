using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace joro.too.Services.Services;

public class MediaService:IMediaService
{
    public MovieDbContext context;
    public DbSet<Media> db;
    public MediasGenresServices mediagenerservice;
    public MediaService(MovieDbContext context)
    {
        this.context = context;
        db = context.Set<Media>();
        mediagenerservice = new MediasGenresServices(context);
    }

    public async Task AddMedia(string name, string coversrc, bool isShow, List<Genre> genres, string Desc)
    {
        
        var tempMedia = new Media(){Name = name, MediaImgSrc = coversrc, Description =Desc, IsShow = isShow, Rating = new List<decimal>()};
        tempMedia.Genres = mediagenerservice.AddMediaGenresTable(tempMedia, genres);
        if (isShow)
        {
            tempMedia.Movie = null;
            tempMedia.VideoId = null;
            tempMedia.Seasons = new List<Season>();
            tempMedia.SeasonsId = new List<int>();
        }
        else
        {
            tempMedia.SeasonsId = null;
            tempMedia.Seasons = null;
        }
        await db.AddAsync(tempMedia);
        await context.SaveChangesAsync();
    }
    public async Task<bool> RemoveMedia(int mediaId)
    {
        if(db.Where(x => x.Id == mediaId).FirstOrDefaultAsync() == null)
        {
            return false;
        }
        db.Remove(db.FirstOrDefaultAsync(x=>x.Id==mediaId).Result);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> UpdateRating(int newRating, Media media)
    {
        media.Rating.Add(newRating);
        await context.SaveChangesAsync();
        decimal avg = 0;
        media.Rating.ForEach(x=>avg+=x);
        return avg / media.Rating.Count;
    }

    public async Task<decimal> GetAvgRating(Media media)
    {
        decimal avg = 0;
        media.Rating.ForEach(x=>avg+=x);
        return avg / media.Rating.Count;
    }

    public async Task<List<Media>> GetMediasWithGenres(List<Genre> genres)
    {
        if (genres.IsNullOrEmpty())
        {
            return db.ToList();
        }
        HashSet<Media> filtered = new HashSet<Media>();
        var allmedia = db.Include(x => x.Genres).ThenInclude(x => x.Genre).ToList();
        foreach (var item in allmedia)
        { 
            var mediagenres = item.Genres.Select(x => x.Genre.Type).ToList();
            foreach (var item2 in genres)
            {
                if (mediagenres.Contains(item2.Type))
                {
                    filtered.Add(item);
                }
            }
        }
        return filtered.ToList();
    }
    public async Task UpdateMedia(Media media)
    {
        db.Update(media);
        context.SaveChanges();
        return;
    }

}