using Microsoft.AspNetCore.Mvc.Rendering;

namespace joro.too.Web.Models;

public class EditMovieModel
{
    public int id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string imgsrc { get; set; }
    public string videourl { get; set; }
    public List<SelectListItem> Genres { get; set; }
    public List<ActorInGivenMediaModel> actors { get; set; }

}