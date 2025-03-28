using joro.too.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace joro.too.Services.Services.IServices
{
    public interface IMediaService
    {
        public Task<bool> AddMovie(string name, string coversrc, List<Genre> genres, string Desc, string vidsrc);
        public Task<bool> AddShow(string name, string coversrc, List<Genre> genres, string Desc,
            List<List<Tuple<string, string>>> vidData, List<string> seasonNames);
        public Task<bool> RemoveMedia(Show show);//delete
        public Task<bool> RemoveMedia(Movie movie);//delete
        public Task<Tuple<List<Show>, List<Movie>>> GetMediasWithGenres(List<Genre> genres); //read
        public Task<decimal> GetAvgRating(Movie media);
        public Task<decimal> GetAvgRating(Show media);
        public Task<decimal> UpdateRating(int newRating, Show media);
        public Task<decimal> UpdateRating(int newRating, Movie media);
        public Task UpdateMedia(Show Media,List<List<Tuple<string, string>>>? vidData, List<string>? seasonNames, List<Genre>? newGenres, List<Actor> actors, List<string> actorRoles); //update
        public Task UpdateMedia(Movie Media); //update
        public Task<Movie> FindMovieById(int id);
        public Task<Show> FindShowById(int id);


    }
}
