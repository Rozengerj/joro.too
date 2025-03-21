using Microsoft.AspNetCore.Mvc.Rendering;

namespace joro.too.Web.Models;

public class SearchResultModel
{
    public int id;
    public string name;
    public string desc;
    public string imgsrc;
    public List<SelectListItem> Genres;
    public bool isShow;
}