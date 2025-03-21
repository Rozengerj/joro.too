using joro.too.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace joro.too.Web.Models;

public class AddMediaModel
{
    public string GenreName { get; set; }
    public string GenreId { get; set; }
    public List<SelectListItem> Genres { get; set; }
}