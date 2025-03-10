using System.Linq.Expressions;
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
    //public MediasGenresServices mediagenerservice;
    public MediaService(MovieDbContext context)
    {
        this.context = context;
        db = context.Set<Media>();
        //mediagenerservice = new MediasGenresServices(context);
    }

    public async Task<Media> AddMedia(string name, string coversrc, bool isShow, List<Genre> genres, string Desc)
    {
        
        var tempMedia = new Media(){Name = name, MediaImgSrc = coversrc, Description =Desc, IsShow = isShow, Rating = new List<decimal>()};
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
        await AddMediaGenresTable(tempMedia, genres);
        await context.SaveChangesAsync();
        return tempMedia;
    }
    public async Task<bool> RemoveMedia(int mediaId)
    {
        if(await db.Where(x => x.Id == mediaId).FirstOrDefaultAsync() == null)
        {
            return false;
        }
        db.Remove(await db.FirstOrDefaultAsync(x=>x.Id==mediaId));
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
        if (media.Rating.IsNullOrEmpty())
        {
            return 0;
        }
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
    
    public async Task AddMediaGenresTable(Media media, List<Genre> genres)
    {
        List<MediaGenres> GenresForMedia = new List<MediaGenres>();
        foreach (var item in genres)
        {
            GenresForMedia.Add(new MediaGenres(){Media = media, MediaId = media.Id, Genre = item, GenreId = item.Id});
        }
        await context.MediasGenres.AddRangeAsync(GenresForMedia);
        await context.SaveChangesAsync();
        
    }
    public async Task<Media> FindMediaById(int id)
    {
        return await db.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddMovie(Media media, string vidsrc)
    {
        var video = new Video() { name = media.Name, vidsrc = vidsrc, Comments = new List<Comment>()};
        await context.Video.AddAsync(video);
        media.Movie = video;
        await context.SaveChangesAsync();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="media"></param>
    /// <param name="vidData">item1 is the video source, and item2 is the episode name, dont worry about it just trust me on this one
    /// </param>
    /// <param name="seasonNames"></param>
    public async Task AddShow(Media media, List<List<Tuple<string,string>>> vidData, List<string> seasonNames)
    {
        List<Season> seasons = new List<Season>();
        for (int i = 1; i <= seasonNames.Count; i++)
        {
            var season = new Season() { Number = i, Episodes = new List<Video>(), Name = seasonNames[i - 1] };
            var epsForThisSeason = new List<Video>();
            await context.Seasons.AddAsync(season);
            foreach (var vidinfo in vidData[i-1] )
            {
                epsForThisSeason.Add(new Video()
                    { name = vidinfo.Item2, vidsrc = vidinfo.Item1, Comments = new List<Comment>() });
            }
            context.Video.AddRangeAsync(epsForThisSeason);
            season.Episodes = epsForThisSeason;
            seasons.Add(season);
        }
        media.Seasons = seasons;
        await context.SaveChangesAsync();
    }
}