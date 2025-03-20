namespace joro.too.Entities;

public class Media
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string MediaImgSrc { get; set; }
    public List<decimal> Rating { get; set; }
}