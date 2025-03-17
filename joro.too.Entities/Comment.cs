using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace joro.too.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; }
    public string Text { get; set; }
    // ddz
    public int UserId { get; set; }
    
    public Episode Episode { get; set; }
    [ForeignKey(nameof(Episode))]
    public int VideoId { get; set; }
}