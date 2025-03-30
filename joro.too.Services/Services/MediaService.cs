using System.Linq.Expressions;
using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace joro.too.Services.Services;

public class MediaService : IMediaService
{
    public MovieDbContext context;
    public DbSet<Movie> movieTable;

    public DbSet<Show> showTable;

    //public MediasGenresServices mediagenerservice;
    public MediaService(MovieDbContext context)
    {
        this.context = context;
        movieTable = context.Set<Movie>();
        showTable = context.Set<Show>();
        //mediagenerservice = new MediasGenresServices(context);
    }

    public async Task<bool> AddMovie(string name, string coversrc, List<Genre> genres, string Desc, string vidsrc)
    {
        var tempMovie = new Movie()
            { Name = name, MediaImgSrc = coversrc, Description = Desc, Rating = new List<decimal>(), vidsrc = vidsrc };

        await movieTable.AddAsync(tempMovie);
        await AddMovieGenresTable(tempMovie, genres);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddShow(string name, string coversrc, List<Genre> genres, string Desc,
        List<List<Tuple<string, string>>> vidData, List<string> seasonNames)
    {
        var tempShow = new Show()
            { Name = name, MediaImgSrc = coversrc, Description = Desc, Rating = new List<decimal>() };


        List<Season> seasons = new List<Season>();
        for (int i = 1; i <= seasonNames.Count; i++)
        {
            var season = new Season() { Number = i, Episodes = new List<Episode>(), Name = seasonNames[i - 1] };
            var epsForThisSeason = new List<Episode>();
            foreach (var vidinfo in vidData[i - 1])
            {
                var episode = new Episode()
                {
                    name = vidinfo.Item1, vidsrc = vidinfo.Item2, Comments = new List<Comment>(), Season = season,
                    SeasonId = season.Id
                };
                await context.Episodes.AddAsync(episode);
                epsForThisSeason.Add(episode);
            }

            season.Episodes = epsForThisSeason;
            seasons.Add(season);
            await context.Seasons.AddAsync(season);
        }

        tempShow.Seasons = seasons;
        await showTable.AddAsync(tempShow);
        await AddShowGenresTable(tempShow, genres);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveMedia(Movie movie)
    {
        if (!movieTable.Contains(movie))
        {
            return false;
        }

        movieTable.Remove(movie);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveMedia(Show show)
    {
        if (!showTable.Contains(show))
        {
            return false;
        }

        showTable.Remove(show);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> UpdateRating(int newRating, Movie media)
    {
        media.Rating.Add(newRating);
        await context.SaveChangesAsync();
        decimal avg = 0;
        media.Rating.ForEach(x => avg += x);
        return avg / media.Rating.Count;
    }

    public async Task<decimal> UpdateRating(int newRating, Show media)
    {
        media.Rating.Add(newRating);
        await context.SaveChangesAsync();
        decimal avg = 0;
        media.Rating.ForEach(x => avg += x);
        return avg / media.Rating.Count;
    }

    public async Task<decimal> GetAvgRating(Movie media)
    {
        decimal avg = 0;
        if (media.Rating.IsNullOrEmpty())
        {
            return 0;
        }

        media.Rating.ForEach(x => avg += x);
        return avg / media.Rating.Count;
    }

    public async Task<decimal> GetAvgRating(Show media)
    {
        decimal avg = 0;
        if (media.Rating.IsNullOrEmpty())
        {
            return 0;
        }

        media.Rating.ForEach(x => avg += x);
        return avg / media.Rating.Count;
    }

    public async Task<Tuple<List<Show>, List<Movie>>> GetMediasWithGenres(List<Genre> genres)
    {
        if (genres.IsNullOrEmpty())
        {
            return new Tuple<List<Show>, List<Movie>>(showTable.ToList(), movieTable.ToList());
        }

        HashSet<Movie> filteredMovies = new HashSet<Movie>();
        HashSet<Show> filteredShows = new HashSet<Show>();
        var allMovies = movieTable.Include(x => x.Genres).ThenInclude(x => x.Genre).ToList();
        var allShows = showTable.Include(x => x.Genres).ThenInclude(x => x.Genre).ToList();
        foreach (var item in allMovies)
        {
            var mediagenres = item.Genres.Select(x => x.Genre.Type).ToList();
            foreach (var genre in genres)
            {
                if (mediagenres.Contains(genre.Type))
                {
                    filteredMovies.Add(item);
                }
            }
        }

        foreach (var item in allShows)
        {
            var mediagenres = item.Genres.Select(x => x.Genre.Type).ToList();
            foreach (var genre in genres)
            {
                if (mediagenres.Contains(genre.Type))
                {
                    filteredShows.Add(item);
                }
            }
        }

        return new Tuple<List<Show>, List<Movie>>(filteredShows.ToList(), filteredMovies.ToList());
    }

    public async Task UpdateMedia(Movie media)
    {
        movieTable.Update(media);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="media"></param>
    /// Required; otherwise you cant update the media duh
    /// <param name="vidData"></param>
    /// if you submit this you must definitely submit also seasonNames
    /// <param name="seasonNames"></param>
    /// if you submit this you must definitely submit also vidData
    /// <param name="newGenres"></param>
    /// only required if you need new genres for some reason
    public async Task UpdateMedia(Show media, List<List<Tuple<string, string>>>? vidData, List<string>? seasonNames,
        List<Genre>? newGenres, List<Actor>? actors, List<string>? actorRoles)
    {
        if (!actors.IsNullOrEmpty())
        {
            await AddShowActorsTable(media, actors, actorRoles);
        }

        if (!newGenres.IsNullOrEmpty())
        {
            await DeleteGenreTable(media);
            await AddShowGenresTable(media, newGenres);
        }

        //go here
        if (vidData.IsNullOrEmpty() && seasonNames.IsNullOrEmpty())
        {
            showTable.Update(media);
            await context.SaveChangesAsync();
            return;
        }

        List<Season> seasons = new List<Season>();
        for (int i = 0; i < seasonNames.Count; i++)
        {
            var season = new Season()
                { Number = media.Seasons.Count + i, Episodes = new List<Episode>(), Name = seasonNames[i] };
            var epsForThisSeason = new List<Episode>();
            foreach (var vidinfo in vidData[i])
            {
                var episode = new Episode()
                {
                    name = vidinfo.Item1, vidsrc = vidinfo.Item2, Comments = new List<Comment>(), Season = season,
                    SeasonId = season.Id
                };
                await context.Episodes.AddAsync(episode);
                epsForThisSeason.Add(episode);
            }

            season.Episodes = epsForThisSeason;
            seasons.Add(season);
            await context.Seasons.AddAsync(season);
        }

        media.Seasons.AddRange(seasons);
        showTable.Update(media);
        await context.SaveChangesAsync();
    }

    private async Task DeleteGenreTable(Show media)
    {
        context.GenresShows.RemoveRange(media.Genres);
    }

    private async Task AddMovieGenresTable(Movie media, List<Genre> genres)
    {
        List<GenresMovies> GenresForMovies = new List<GenresMovies>();
        foreach (var item in genres)
        {
            GenresForMovies.Add(new GenresMovies()
                { Movie = media, MovieId = media.Id, Genre = item, GenreId = item.Id });
        }

        await context.GenresMovies.AddRangeAsync(GenresForMovies);
        media.Genres = GenresForMovies;
        await context.SaveChangesAsync();
    }

    private async Task AddShowGenresTable(Show media, List<Genre> genres)
    {
        List<GenresShows> GenresForShows = new List<GenresShows>();
        foreach (var item in genres)
        {
            GenresForShows.Add(new GenresShows() { Show = media, ShowId = media.Id, Genre = item, GenreId = item.Id });
        }

        await context.GenresShows.AddRangeAsync(GenresForShows);
        media.Genres = GenresForShows;
        await context.SaveChangesAsync();
    }

    private async Task AddShowActorsTable(Show media, List<Actor> actors, List<string> roles)
    {
        actors.ForEach(x => x.Name.ToLower());
        List<ActorRolesShows> ActorsForShows = new List<ActorRolesShows>();
        HashSet<Actor> actorsSet = new HashSet<Actor>();
        int counter = 0;
        foreach (var actor in actors)
        {
            if (actorsSet.Add(actor))
            {
                ActorsForShows.Add(new ActorRolesShows()
                    { Show = media, ShowId = media.Id, Actor = actor, ActorId = actor.Id, Role = roles[counter] });
                counter++;
                continue;
            }

            var prevactor = actorsSet.Where(x => x.Name.ToLower().Equals(actor.Name)).FirstOrDefault();
            ActorsForShows.Add(new ActorRolesShows()
                { Show = media, ShowId = media.Id, Actor = prevactor, ActorId = prevactor.Id, Role = roles[counter] });
            counter++;
            await context.ActorsRolesShows.AddRangeAsync(ActorsForShows);
            await context.Actors.AddRangeAsync(actorsSet);
            media.Actors = ActorsForShows;
            await context.SaveChangesAsync();
        }
    }

    public async Task<Movie> FindMovieById(int id)
    {
        return await movieTable
            .Include(x => x.Actors)
            .ThenInclude(y => y.Actor)
            .Include(x => x.Comments)
            .Include(x => x.Genres)
            .ThenInclude(y => y.Genre)
            .Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Show> FindShowById(int id)
    {
        return await showTable
            .Include(x => x.Actors)
            .ThenInclude(y => y.Actor)
            .Include(x => x.Seasons)
            .ThenInclude(x => x.Episodes)
            .ThenInclude(x => x.Comments)
            .Include(x => x.Genres)
            .ThenInclude(y => y.Genre)
            .Where(x => x.Id == id).FirstOrDefaultAsync();
    }
}