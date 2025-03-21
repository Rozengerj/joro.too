using joro.too.Entities;

namespace joro.too.Web.Models;

public class ViewShowModel
{
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string imgsrc { get; set; }
    public List<Genre> genres { get; set; } 
    public List<decimal> rating { get; set; }
    public List<ActorInGivenMediaModel>? actors { get; set; }
    //showonly
    public List<List<VideoViewModel>>? EpisodesInSeasons { get; set; }
    public List<string>? SeasonsNames { get; set; }
}