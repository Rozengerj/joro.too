using System.Runtime.CompilerServices;
using joro.too.Entities;

namespace joro.too.Services.Services.IServices;

public interface IActorService
{
    public Task<int> AddActor(string name);
    public Task AddRoleToActor(int actorId, string role, IMedia media);
    public Task RemoveRolesFromActor(string[] roles, int actorId);
    public Task<Actor> FindActorById(int id);
    public Task<List<Actor>> GetActorsByName(string name);
    public Task RemoveActor(int id);
    public Task UpdateActor(Actor actor);
}