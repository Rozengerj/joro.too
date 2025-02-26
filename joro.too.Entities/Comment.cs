using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace joro.too.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; }
    public string text { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(Video))]
    public Video video { get; set; }
    public int VideoId { get; set; }
}