using System.Runtime.InteropServices;
using System.Security.AccessControl;
using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace joro.too.Services.Services;

public class ActorService : IActorService
{
    public MovieDbContext context;
    public DbSet<Actor> ac;

    public ActorService(MovieDbContext context)
    {
        this.context = context;
        ac = context.Set<Actor>();
    }

    public async Task<int> AddActor(string name, string imgsrc)
    {
        var idk = await ac.AddAsync(new Actor()
        {
            Name = name, imgsrc = imgsrc, RolesInMovies = new List<ActorRolesMovies>(),
            RolesInShows = new List<ActorRolesShows>()
        });
        await context.SaveChangesAsync();
        return idk.Entity.Id;
    }

    public async Task AddRoleToActor(int actorId, string role, IMedia media)
    {
        var actor = await ac.FindAsync(actorId);

        if (media is Movie)
        {
            if (actor.RolesInMovies.IsNullOrEmpty())
            {
                var actorinmovie = new ActorRolesMovies()
                {
                    Actor = actor,
                    ActorId = actorId,
                    Movie = (Movie)media,
                    MovieId = media.Id,
                    Roles = new List<string>()
                };
                await context.ActorsRolesMovies.AddAsync(actorinmovie);
                await context.SaveChangesAsync();
            }

            actor.RolesInMovies.Find(x => x.MovieId == media.Id).Roles.Add(role);
            await context.SaveChangesAsync();
            return;
        }
        if (actor.RolesInShows.IsNullOrEmpty())
        {
            var actorinshow = new ActorRolesShows()
            {
                Actor = actor,
                ActorId = actorId,
                Show = (Show)media,
                ShowId = media.Id,
                Roles = new List<string>()
            };
            await context.ActorsRolesShows.AddAsync(actorinshow);
            await context.SaveChangesAsync();
        }
        actor.RolesInShows.Find(x => x.ShowId == media.Id).Roles.Add(role);
        await context.SaveChangesAsync();
    }

    public async Task RemoveRolesFromActor(string[] roles, int actorId)
    {
        var rolesMovies = context.ActorsRolesMovies.Where(x => x.ActorId == actorId).ToList();
        var rolesShows = context.ActorsRolesShows.Where(x => x.ActorId == actorId).ToList();
        foreach (var role in roles)
        {
            foreach (var things in rolesShows)
            {
                if (things.Roles.Select(x => x.ToLower()).Contains(role.ToLower()))
                {
                    things.Roles.Remove(role);
                }
            }
            foreach (var things in rolesMovies)
            {
                if (things.Roles.Select(x => x.ToLower()).Contains(role.ToLower()))
                {
                    things.Roles.Remove(role);
                }
            }
        }
        await context.SaveChangesAsync();
    }

    public async Task<List<Actor>> GetActorsByName(string name)
    {
        if (name.IsNullOrEmpty())
        {
            return ac.Include(x => x.RolesInMovies).ThenInclude(y => y.Movie)
                .Include(x => x.RolesInShows).ThenInclude(y => y.Show).ToList();
        }

        return ac.Include(x => x.RolesInMovies).ThenInclude(y => y.Movie)
            .Include(x => x.RolesInShows).ThenInclude(y => y.Show)
            .Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
    }

    public async Task RemoveActor(int id)
    {
        ac.Remove(await ac.FindAsync(id));
        await context.SaveChangesAsync();
    }

    public async Task UpdateActor(Actor actor)
    {
        ac.Update(actor);
        await context.SaveChangesAsync();
    }

    public async Task<Actor> FindActorById(int id)
    {
        return await ac.Include(x => x.RolesInMovies).ThenInclude(y => y.Movie)
            .Include(x => x.RolesInShows).ThenInclude(y => y.Show)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}