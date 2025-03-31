namespace joro.too.Web.Models;

public class WatchMovieModel
{
    public string name { get; set; }
    public string vidSrc { get; set; }
    public List<ViewCommentsModel>? Comments { get; set; }
}