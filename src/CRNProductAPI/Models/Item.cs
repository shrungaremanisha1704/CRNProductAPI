using System.ComponentModel.DataAnnotations;

namespace CRNProductAPI.Models;

public class Item
{
    public int Id { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    public Product? Product { get; set; }
}
