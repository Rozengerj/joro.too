using joro.too.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace joro.too.Web.Models;

public class AddMovieModel
{
    public string GenreName { get; set; }
    public string GenreId { get; set; }
    public List<SelectListItem> Genres { get; set; }
    public string name { get; set; }
    public string desc;
    public IFormFile img;
    public string[] genres;
    public AddMovieModel model;
    public IFormFile vid;


}