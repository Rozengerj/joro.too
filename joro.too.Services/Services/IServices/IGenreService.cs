using joro.too.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace joro.too.Services.Services.IServices;

public interface IGenreService
{
    public Task<List<Genre>> GetGenres();
}