using joro.too.Entities;

namespace joro.too.Web.Models;

public class ViewMediaModel
{
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string imgsrc { get; set; }
    public List<Genre> genres { get; set; } 
    public List<decimal> rating { get; set; }
    public List<ActorRoles> actors { get; set; }
    //showonly
    public List<List<Video>>? EpisodesInSeasons { get; set; }
    public List<string>? SeasonsNames { get; set; }

}