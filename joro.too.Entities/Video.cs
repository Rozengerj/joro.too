using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace joro.too.Entities;

public class Video
{
    [Key]
    public int Id { get; set; }
    public string name { get; set; }
    public string vidsrc { get; set; }
    [ForeignKey(nameof(Comment))]
    public List<Comment>? Comments { get; set; }
    public List<int> CommentsId { get; set; }


}