using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace joro.too.Entities;

public class MediaGenres
{
    public Media Media { get; set; }
    public int MediaId { get; set; }
    public Genre Genre { get; set; }
    public int GenreId { get; set; }
}