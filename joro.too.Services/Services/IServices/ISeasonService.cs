using joro.too.Entities;

namespace joro.too.Services.Services.IServices;

public interface ISeasonService
{
    public Task<bool> RemoveSeason(int id);
    public Task UpdateSeason(Season season);
    public Task<Season> FindSeasonById(int id);
    public Task AddEpisdesToSeason(Season season,List<string> episodeNames, List<string> episodeVidSrcs);

}