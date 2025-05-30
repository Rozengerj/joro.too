using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace joro.too.Services.Services;

public class GenreService:IGenreService
{
    private MovieDbContext context;
    private DbSet<Genre> db;
    public GenreService(MovieDbContext context)
    {
        this.context = context;
        db = context.Set<Genre>();
    }
    public async Task<List<Genre>> GetGenres()
    {
        return context.Genres.ToList();
    }

    public async Task<List<Genre>> GetGenresById(List<int> ids)
    {
        if (ids is null)
        {
            return context.Genres.ToList();
        }
        return db.Where(x => ids.Contains(x.Id)).ToList();
    }
}