using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace joro.too.Entities;

public class User:IdentityUser
{
    public string Pfp { get; set; }
    public List<Comment>? Comments { get; set; }
    [ForeignKey(nameof(Comment))]
    public List<int>? CommentsId { get; set; }
    public List<Movie>? RatedMovies { get; set; }
    [ForeignKey(nameof(Movie))]
    public List<int>? RatedMovieIds { get; set; }
    public List<Show>? RatedShows { get; set; }
    [ForeignKey(nameof(Show))]
    public List<int>? RatedShowsIds { get; set; }
}