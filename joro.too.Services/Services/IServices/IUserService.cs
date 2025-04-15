using joro.too.Entities;
using Microsoft.AspNetCore.Identity;

namespace joro.too.Services.Services.IServices;

public interface IUserService
{
    public Task WriteComment(string text, User user, int mediaId, bool isShow);
    public Task DeleteComment(int id);
    public Task RateMedia(User user, float rating, int mediaId, bool isShow);
}
