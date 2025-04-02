using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace joro.too.Entities;

public class Movie:IMedia
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string MediaImgSrc { get; set; }
    public string VidSrc { get; set; }
    public List<Comment> Comments { get; set; }
    public List<GenresMovies> Genres { get; set; }
    public List<ActorRolesMovies>? Actors { get; set; }
    
    public long RatingsSum { get; set; }
    public int RatedCount { get; set; }
    
}