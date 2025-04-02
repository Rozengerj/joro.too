using joro.too.Entities;

namespace joro.too.Web.Models;

public class ViewMovieModel
{
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string imgsrc { get; set; }
    public List<Genre> genres { get; set; }
    public decimal rating { get; set; }
    public List<ActorInGivenMediaModel> actors { get; set; }
    public VideoViewModel? movie;

}