using joro.too.Entities;

namespace joro.too.Web.Models;

public class AddMediaModel
{
    public string name { get; set; }
    public string coversrc { get; set; }
    public bool isShow { get; set; }
    public string description { get; set; }
    public List<int> genreIds { get; set; }
    

}