using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services;
using joro.too.Services.Services.IServices;
using joro.too.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace joro.too.Web.Controllers
{
    public class MediaController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IMediaService _mediaService;

        public MediaController(IGenreService genreService, IMediaService mediaservice)
        {
            _genreService = genreService;
            _mediaService = mediaservice;
        }

        public async Task<IActionResult> SearchResult(string name, decimal rating, SearchResultModel model,
            string[] genres, bool IsShow, bool isMovie)
        {
            //this method is ugly and long and im sure it could be compacted by a lot but honestly if it works like this im not gonna touch it further except if i dont get drunk lmao
            var genreIds = new List<int>();
            model.Genres = new List<SelectListItem>();
            foreach (Genre item in await _genreService.GetGenres())
            {
                model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
            }

            foreach (SelectListItem li in model.Genres)
            {
                if (genres.Contains(li.Value))
                {
                    li.Selected = true;
                    genreIds.Add(int.Parse(li.Value));
                }
            }

            //Console.WriteLine(string.Join(", ",genreIds));
            List<Genre> genresfr = await _genreService.GetGenresById(genreIds);
            List<SearchResultModel> modellist = new List<SearchResultModel>();
            var media = await _mediaService.GetMediasWithGenres(genresfr);
            if (name != null)
            {
                media.Item1.Where(x => x.Name.Contains(name)).ToList();
                media.Item2.Where(x => x.Name.Contains(name)).ToList();
            }

            if (rating != null)
            {
                media.Item1.Where(x => _mediaService.GetAvgRating(x).Result >= rating).ToList();
                media.Item2.Where(x => _mediaService.GetAvgRating(x).Result >= rating).ToList();
            }

            if (IsShow == false && isMovie == false)
            {
                modellist.AddRange(media.Item1.Select(
                    x=>new SearchResultModel(){
                        name=x.Name,
                        desc = x.Description,
                        imgsrc = x.MediaImgSrc,
                        
                        ));
            }


            return View(modellist);
            }

            //checks if its a show
            if (IsShow)
            {
                media = media.Where(x => x.IsShow == true).ToList();
            }
            else if (isMovie)
            {
                media = media.Where(x => x.IsShow == false).ToList();
            }

            foreach (var item in media)
            {
                modellist.Add(new SearchResultModel()
                {
                    name = item.Name,
                    id = item.Id,
                    imgsrc = item.MediaImgSrc,
                    desc = item.Description
                });
            }

            return View(modellist);
        }

        [Route("id")]
        public async Task<IActionResult> ViewMedia(int id)
        {
            var media = await _mediaService.FindMediaById(id);
            var model = new ViewMediaModel()
            {
                id = media.Id, 
                genres = media.Genres.Select(x => x.Genre).ToList(), 
                name = media.Name,
                rating = media.Rating, 
                actors = media.Actors.ToList(), 
                description = media.Description,
                imgsrc = media.MediaImgSrc
            };
            if (!media.IsShow)
            {
                model.movie = new VideoViewModel()
                {
                    name = media.Movie.name, vidsrc = media.Movie.vidsrc,
                    comments = media.Movie.Comments.Select(y =>
                        new ViewCommentsModel()
                        {
                            user = "user",
                            comment = y.text
                        }).ToList()
                };
                return View("ViewMovie", media);
            }
            //for shows only
            model.SeasonsNames = new List<string>();
            model.EpisodesInSeasons = new List<List<VideoViewModel>>();
            foreach (var season in media.Seasons)
            {
                model.EpisodesInSeasons.Add(season.Episodes.Select(x =>
                    new VideoViewModel()
                    {
                        name = x.name,
                        vidsrc = x.vidsrc,
                        comments = x.Comments.Select(y =>
                            new ViewCommentsModel()
                            {
                                user = "user",
                                comment = y.text
                            }).ToList()
                    }).ToList());
                model.SeasonsNames.Add(season.Name);
            }

            return View("ViewShow", media);
        }
    }
}