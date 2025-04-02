using joro.too.Entities;
using Microsoft.AspNetCore.Identity;

namespace joro.too.Services.Services.IServices;

public interface IUserService
{
    public Task WriteComment(string text, IdentityUser user);
    public Task DeleteComment(int id);
}
