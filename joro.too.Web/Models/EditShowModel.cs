using Microsoft.AspNetCore.Mvc.Rendering;

namespace joro.too.Web.Models;

public class EditShowModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? MediaImgSrc { get; set; }
    public string Description { get; set; }
    public List<string> SeasonNames { get; set; }
    public List<int> SeasonIds { get; set; }
    public List<List<VideoViewModel>> Episodes { get; set; }
    public List<SelectListItem> Genres { get; set; }
}