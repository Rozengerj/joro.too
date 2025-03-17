using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace joro.too.Entities;

public class Season
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }
    public List<Episode> Episodes { get; set; }
    [ForeignKey(nameof(Show))]
    public int ShowId { get; set; }
    public Show Show { get; set; }
}