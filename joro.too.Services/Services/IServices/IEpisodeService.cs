using joro.too.Entities;

namespace joro.too.Services.Services.IServices;

public interface IEpisodeService
{
    public Task<bool> RemoveEpisode(int id);
    public Task UpdateEpisode(Episode episode);
    public Task<Episode> FindEpisodeById(int id);
}