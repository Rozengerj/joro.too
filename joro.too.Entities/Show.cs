using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace joro.too.Entities;

public class Show:IMedia
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public List<Season> Seasons { get; set; }
    public string MediaImgSrc { get; set; }
    public string Description { get; set; }
    public List<GenresShows> Genres { get; set; }
    public List<ActorRolesShows>? Actors { get; set; }
    public long RatingsSum { get; set; }
    public int RatedCount { get; set; }
    
}