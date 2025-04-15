namespace joro.too.Web.Models;

public class EditAccountModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public IFormFile Pfp { get; set; }
    public string PfpSource { get; set; }

}