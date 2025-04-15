namespace joro.too.Web.Models;

public class EditActorModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ActorRolesModel> Roles { get; set; }
    public string oldImg { get; set; }
}