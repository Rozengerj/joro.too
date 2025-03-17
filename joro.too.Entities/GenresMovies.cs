using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace joro.too.Entities;

public class GenresMovies
{
    public Movie Movie { get; set; }
    public int MovieId { get; set; }
    public Genre Genre { get; set; }
    public int GenreId { get; set; }
}