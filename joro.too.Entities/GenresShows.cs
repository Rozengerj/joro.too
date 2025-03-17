using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace joro.too.Entities;

public class GenresShows
{
    public Show Show { get; set; }
    public int ShowId { get; set; }
    public Genre Genre { get; set; }
    public int GenreId { get; set; }
}