namespace joro.too.Web.Models;

public class AddRoleToActorModel
{
    public int actorId { get; set; }
    public string actorName { get; set; }
    public List<string> actorRoles { get; set; }
    public List<AllMediaModel> allMedia { get; set; }
}