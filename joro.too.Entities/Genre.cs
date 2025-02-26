using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace joro.too.Entities;

public class Genre
{
    [Key]
    public int Id {  get; set; }
    public string Type { get; set; }
    
    public List<MediaGenres>? Medias { get; set; }
    public List<int>? MediasId { get; set; }
}