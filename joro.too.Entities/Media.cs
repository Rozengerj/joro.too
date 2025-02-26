using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace joro.too.Entities;

public class Media
{
    [Key] public int Id { get; set; }
    public bool IsShow { get; set; }
    public string Name { get; set; }
    [ForeignKey(nameof(Video))]
    public int? VideoId { get; set; }
    public Video? Movie { get; set; }
    [ForeignKey(nameof(Season))]
    public List<int>? SeasonsId { get; set; }
    public List<Season>? Seasons { get; set; }
    public List<decimal>? Rating { get; set; }
    public List<MediaGenres> Genres { get; set; }
    public List<int>? ActorsId { get; set; }
    public List<ActorRoles>? Actors { get; set; }
    public string MediaImgSrc { get; set; }
    public string Description { get; set; }
}