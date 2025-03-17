using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace joro.too.Entities;

public class Episode
{
    [Key]
    public int Id { get; set; }
    public string name { get; set; }
    public string vidsrc { get; set; }
    [ForeignKey(nameof(Season))] 
    public Guid SeasonId { get; set; }

    public Season Season { get; set; }
    public List<Comment>? Comments { get; set; }
    


}