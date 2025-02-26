using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace joro.too.Entities;

public class Season
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    [ForeignKey(nameof(Video))]
    public List<Video> Episodes { get; set; }
    public List<int> EpisodesId { get; set; }
    public int MediaId { get; set; }

}