namespace joro.too.Web.Models;

public class VideoViewModel
{
    public int id { get; set; }
    public string name;
    public string vidsrc;
    public List<ViewCommentsModel> comments;
}