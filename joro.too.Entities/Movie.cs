using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace joro.too.Entities;

public class Movie:Media
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public string vidsrc { get; set; }
    public List<Comment> Comments { get; set; }
    public List<decimal>? Rating { get; set; }
    public List<GenresMovies> Genres { get; set; }
    public List<ActorRolesMovies>? Actors { get; set; }
    public string MediaImgSrc { get; set; }
    public string Description { get; set; }
}