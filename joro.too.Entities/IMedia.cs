namespace joro.too.Entities;

public interface IMedia
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string MediaImgSrc { get; set; }
    public long RatingsSum { get; set; }
    public int RatedCount { get; set; }
}