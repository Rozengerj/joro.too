using System.Runtime.InteropServices;
using System.Security.AccessControl;
using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace joro.too.Services.Services;

public class ActorService:IActorService
{
    public MovieDbContext context;
    public DbSet<Actor> ac;

    public ActorService(MovieDbContext context)
    {
        this.context = context;
        ac = context.Set<Actor>();
    }

    public async Task<int> AddActor(string name)
    {
        var idk = await ac.AddAsync(new Actor() { Name = name });
        await context.SaveChangesAsync();
        return idk.Entity.Id;
    }

    public async Task AddRoleToActor(int actorId, string role, IMedia media)
    {
        var actor = await ac.FindAsync(actorId);
        if (media is Movie)
        {
            var actorinmovie = new ActorRolesMovies()
            {
                Actor = actor,
                ActorId = actorId,
                Movie = (Movie)media,
                MovieId = media.Id,
                Role = role
            };
            await context.ActorsRolesMovies.AddAsync(actorinmovie);
            await context.SaveChangesAsync();
            return;
        }
        var actorinshow = new ActorRolesShows()
        {
            Actor = actor,
            ActorId = actorId,
            Show = (Show)media,
            ShowId = media.Id,
            Role = role
        };
        await context.ActorsRolesShows.AddAsync(actorinshow);
        await context.SaveChangesAsync();
    }

    public async Task RemoveRolesFromActor(string[] roles, int actorId)
    {
        var rolesMovies = context.ActorsRolesMovies.Where(x => x.ActorId == actorId).ToList();
        var rolesShows = context.ActorsRolesShows.Where(x => x.ActorId == actorId).ToList();
        foreach (var role in roles)
        {
            if (rolesMovies.Exists(x => x.Role.ToLower() == role.ToLower()))
            {
                context.ActorsRolesMovies.Remove(rolesMovies.Find(x => x.Role.ToLower() == role.ToLower()));
            }
            if (rolesShows.Exists(x => x.Role.ToLower() == role.ToLower()))
            {
                context.ActorsRolesShows.Remove(rolesShows.Find(x => x.Role.ToLower() == role.ToLower()));
            }
        }
        await context.SaveChangesAsync();
    }

    public async Task<List<Actor>> GetActorsByName(string name)
    {
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