using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace joro.too.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; }
    public string Text { get; set; }
    // ddz
    public User Commenter { get; set; }
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    
}