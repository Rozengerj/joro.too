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

            foreach (SelectListItem li in model.Genres)
            {
                if (genres.Contains(li.Value))
                {
                    li.Selected = true;
                    genreIds.Add(int.Parse(li.Value));
                }
            }

            List<Genre> genresfr = await _genreService.GetGenresById(genreIds);
            List<SearchResultModel> modellist = new List<SearchResultModel>();
            var media = await _mediaService.GetMediasWithGenres(genresfr);
            List<IMedia> allmedias = new List<IMedia>();
            allmedias.AddRange(media.Item1);
            allmedias.AddRange(media.Item2);
            if (!name.IsNullOrEmpty())
            {
                allmedias = allmedias.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
            }

            if (rating > 0)
            {
                allmedias = allmedias.Where(x =>
                {
                    if (x.RatedCount != 0)
                    {
                        return x.RatingsSum / x.RatedCount >= rating;
                    }

                    return false;
                }).ToList();
            }
            Console.WriteLine(isShow);
            Console.WriteLine(isMovie);
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

                if (!isMovie)
                {
                    return View(modellist);
                }
                
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

            return View(modellist);
        }

        public async Task<IActionResult> ViewMedia(int id, bool isShow)
        {
            if (!isShow)
            {
                var movie = await _mediaService.FindMovieById(id);
                var actorsformovies = new List<ActorInGivenMediaModel>();
                foreach (var movieact in movie.Actors)
                {
                    foreach (var role in movieact.Roles)
                    {
                        actorsformovies.Add(new ActorInGivenMediaModel()
                        {
                            Name = movieact.Actor.Name, Id = movieact.Actor.Id, Role = role, img = movieact.Actor.imgsrc
                        });
                    }
                }
                var model = new ViewMovieModel()
                {
                    id = movie.Id,
                    genres = movie.Genres.Select(x => x.Genre).ToList(),
                    name = movie.Name,
                    rating = await _mediaService.GetAvgRating(movie),
                    actors = actorsformovies,
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
            var actors = new List<ActorInGivenMediaModel>();
            if (!show.Actors.IsNullOrEmpty())
            {
                foreach (var showact in show.Actors)
                {
                    foreach (var role in showact.Roles)
                    {
                        actors.Add(new ActorInGivenMediaModel()
                        {
                            Name = showact.Actor.Name, Id = showact.Actor.Id, Role = role, img = showact.Actor.imgsrc
                        });
                    }
                }
            }

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
                Roles = new List<ActorRolesModel>(),
                img = x.imgsrc
            }).ToList();
            for (int i = 0; i < actors.Count; i++)
            {
                foreach (var roles in actors[i].RolesInMovies)
                {
                    foreach (var role in roles.Roles)
                    {
                        model[i].Roles.Add(new ActorRolesModel()
                        {
                            MediaName = roles.Movie.Name,
                            Role = role,
                            MediaId = roles.MovieId,
                            isShow = false,
                            
                        });
                    }
                }

                foreach (var roles in actors[i].RolesInShows)
                {
                    foreach (var role in roles.Roles)
                    {
                        model[i].Roles.Add(new ActorRolesModel()
                        {
                            MediaName = roles.Show.Name,
                            Role = role,
                            MediaId = roles.ShowId,
                            isShow = true
                        });
                    }
                }
            }

            return View(model);
        }
    }
}