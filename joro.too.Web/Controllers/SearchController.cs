using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services;
using joro.too.Services.Services.IServices;
using joro.too.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;

namespace joro.too.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IMediaService _mediaService;
        private readonly IActorService _actorService;

        public SearchController(IGenreService genreService, IMediaService mediaservice, IActorService actorService)
        {
            _genreService = genreService;
            _mediaService = mediaservice;
            _actorService = actorService;
        }

        //[HttpPut]
        public async Task<IActionResult> SearchResult(string name, float rating, SearchResultModel model,
            string[] genres, bool isShow, bool isMovie)
        {
            //this method is ugly and long and im sure it could be compacted by a lot but honestly if it works like this im not gonna touch it further except if i dont get drunk lmao
            List<int> genreIds = new List<int>();
            var list = await _genreService.GetGenres();
            model.Genres = new List<SelectListItem>();
            foreach (Genre item in list)
            {
                model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
            }
            if (genres is not null)
            {
                foreach (SelectListItem li in model.Genres)
                {
                    if (genres.Contains(li.Value))
                    {
                        li.Selected = true;
                        genreIds.Add(int.Parse(li.Value));
                    }
                }
            }
            Console.WriteLine(string.Join(", ", genreIds));
            Console.WriteLine(rating);
            Console.WriteLine(name);
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
            Console.WriteLine(string.Join(", ",genresfr.Select(x=>x.Type)));
            List<IMedia> allmedias = new List<IMedia>();
            allmedias.AddRange(media.Item1);
            allmedias.AddRange(media.Item2);
            //Console.WriteLine("------------------------------------------------------------");
            foreach (var item in allmedias)
            {
                Console.WriteLine(item.Description);
                Console.WriteLine(item.Name);
                Console.WriteLine(name);
                Console.WriteLine();
            }

            if (!name.IsNullOrEmpty())
            {
                allmedias = allmedias.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
            }
            Console.WriteLine(rating);
            if (rating > 0)
            {
                allmedias = allmedias.Where(x =>
                {
                    //Console.WriteLine(x.RatingsSum / x.RatedCount);
                    if (x.RatedCount != 0)
                    {
                        return x.RatingsSum / x.RatedCount >= rating;
                    }
                    
                    return false;
                }).ToList();
            }

            if (isShow == false && isMovie == false)
            {
                modellist.AddRange(allmedias.Where(x => x is Show).Select(
                    x => new SearchResultModel()
                    {
                        name = x.Name,
                        desc = x.Description,
                        imgsrc = x.MediaImgSrc,
                        id = x.Id,
                        isShow = true
                    }));
                modellist.AddRange(allmedias.Where(x => x is Movie).Select(
                    x => new SearchResultModel()
                    {
                        name = x.Name,
                        desc = x.Description,
                        imgsrc = x.MediaImgSrc,
                        id = x.Id,
                        isShow = false
                    }));
                return View(modellist);
            }

            //checks if its a show
            if (isShow)
            {
                foreach (var item in allmedias.Where(x => x is Show))
                {
                    modellist.Add(new SearchResultModel()
                    {
                        name = item.Name,
                        id = item.Id,
                        imgsrc = item.MediaImgSrc,
                        desc = item.Description,
                        isShow = true
                    });
                }

                return View(modellist);
            }

            foreach (var item in allmedias.Where(x => x is Movie))
            {
                modellist.Add(new SearchResultModel()
                {
                    name = item.Name,
                    id = item.Id,
                    imgsrc = item.MediaImgSrc,
                    desc = item.Description,
                    isShow = false
                });
            }
            Console.WriteLine(rating);
            return View(modellist);
        }

        public async Task<IActionResult> ViewMedia(int id, bool isShow)
        {
            if (!isShow)
            {
                var movie = await _mediaService.FindMovieById(id);
                var model = new ViewMovieModel()
                {
                    id = movie.Id,
                    genres = movie.Genres.Select(x => x.Genre).ToList(),
                    name = movie.Name,
                    rating = await _mediaService.GetAvgRating(movie),
                    actors = movie.Actors.Select(x =>
                        new ActorInGivenMediaModel() { Name = x.Actor.Name, Id = x.Actor.Id, Role = x.Role }).ToList(),
                    description = movie.Description,
                    imgsrc = movie.MediaImgSrc,
                    movie = new VideoViewModel()
                    {
                        name = movie.Name, vidsrc = movie.VidSrc,
                        comments = movie.Comments.Select(y =>
                            new ViewCommentsModel()
                            {
                                username = y.Commenter.UserName,
                                comment = y.Text,
                                id = y.Id,
                                pfpsrc = y.Commenter.Pfp
                            }).ToList()
                    }
                };
                return View("ViewMovie", model);
            }

            var show = await _mediaService.FindShowById(id);
            //for shows only
            //Console.WriteLine(id);
            //Console.WriteLine(isShow);
            //Console.WriteLine(show.Name);
            var actors = new List<ActorInGivenMediaModel>();
            if (show.Actors is not null)
            {
                actors = show.Actors.Select(x =>
                    new ActorInGivenMediaModel() { Name = x.Actor.Name, Id = x.Actor.Id, Role = x.Role }).ToList();
            }

            if (show.Genres is null)
            {
                Console.WriteLine("why are the genres null this shit is so ass what am i supposed to do");
            }
            //Console.WriteLine(string.Join(", ",show.Genres.Select(x=>x.Genre).ToList()));

            ViewShowModel modelshow = new ViewShowModel()
            {
                name = show.Name,
                id = show.Id,
                genres = show.Genres.Select(x => x.Genre).ToList(),
                rating = await _mediaService.GetAvgRating(show),
                actors = actors,
                imgsrc = show.MediaImgSrc,
                description = show.Description
            };
            modelshow.SeasonsNames = new List<string>();
            modelshow.EpisodesInSeasons = new List<List<VideoViewModel>>();
            foreach (var season in show.Seasons)
            {
                modelshow.EpisodesInSeasons.Add(season.Episodes.Select(x =>
                    new VideoViewModel()
                    {
                        name = x.name,
                        vidsrc = x.vidsrc,
                        comments = x.Comments.Select(y =>
                            new ViewCommentsModel()
                            {
                                username = y.Commenter.UserName,
                                comment = y.Text,
                                id = y.Id,
                                pfpsrc = y.Commenter.Pfp
                            }).ToList()
                    }).ToList());
                modelshow.SeasonsNames.Add(season.Name);
            }

            return View("ViewShow", modelshow);
        }

        public async Task<IActionResult> ViewActors(string name)
        {
            var actors = await _actorService.GetActorsByName(name);

            List<ViewActorsModel> model = actors.Select(x => new ViewActorsModel()
            {
                Name = x.Name, Id = x.Id,
                Roles = new List<ActorRolesModel>()
            }).ToList();
            for (int i = 0; i < actors.Count; i++)
            {
                model[i].Roles.AddRange(actors[i].RolesInMovies.Select(x => new ActorRolesModel()
                {
                    MediaId = x.MovieId,
                    isShow = false,
                    Role = x.Role,
                    MediaName = x.Movie.Name
                }));
                model[i].Roles.AddRange(actors[i].RolesInShows.Select(x => new ActorRolesModel()
                {
                    MediaId = x.ShowId,
                    isShow = true,
                    Role = x.Role,
                    MediaName = x.Show.Name
                }));
            }
            return View(model);
        }
    }
}