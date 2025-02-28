using joro.too.DataAccess;
using joro.too.Entities;
using Microsoft.EntityFrameworkCore;

namespace joro.too.Services.Services;

public class MediasGenresServices
{
    public MovieDbContext context;
    public DbSet<MediaGenres> db;

    public MediasGenresServices(MovieDbContext context)
    {
        this.context = context;
        db = context.Set<MediaGenres>();
    }

    public List<MediaGenres> AddMediaGenresTable(Media media, List<Genre> genres)
    {
        
        List<MediaGenres> GenresForMedia = new List<MediaGenres>();
        foreach (var item in genres)
        {
            GenresForMedia.Add(new MediaGenres(){Media = media, MediaId = media.Id, Genre = item, GenreId = item.Id});
        }
        context.MediasGenres.AddRangeAsync(GenresForMedia);
        context.SaveChangesAsync();
        return GenresForMedia;
    }
    
}