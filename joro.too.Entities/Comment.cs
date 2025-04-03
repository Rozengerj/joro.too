using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace joro.too.Entities;

public class Comment
{
    [Key]
    public int Id { get; set; }
    public string Text { get; set; }
    // ddz
    public User Commenter { get; set; }
    [ForeignKey(nameof(User))]
    [MaxLength(450)]
    public string UserId { get; set; }
    public Movie? Moviee { get; set; }
    [ForeignKey(nameof(Movie))]
    public int? MovieId { get; set; }

    public Episode? Episodee { get; set; }
    [ForeignKey(nameof(Episode))]
    public int? EpisodeId { get; set; }
}