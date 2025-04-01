using System.Reflection;

namespace joro.too.Web.Models;

public class WatchShowModel
{
    public int id { get; set; }
    public string name { get; set; }
    public List<List<VideoViewModel>> episodesInSeasons { get; set; }
    public List<string>? seasonsNames { get; set; }
    public List<ViewCommentsModel>? comments { get; set; }
}