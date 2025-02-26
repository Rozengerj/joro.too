using joro.too.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace joro.too.Services.Services.IServices
{
    public interface IMediaService
    {
        public Task AddMedia(string name, string coversrc, bool isShow, List<Genre> genres, string Desc); //create
        public Task<bool> RemoveMedia(int mediaId);//delete
        public Task<List<Media>> GetMediasWithGenres(List<Genre> genres); //read
        public Task<decimal> GetAvgRating(Media media);
        public Task<decimal> UpdateRating(int newRating, Media media);
        public Task UpdateMedia(Media Media); //update
        
    }
}
