namespace joro.too.Web.Models;

public class ViewActorsModel
{
    public string Name { get; set; }
    public int Id { get; set; }
    public List<ActorRolesModel> Roles { get; set; }
    public string img { get; set; }

}