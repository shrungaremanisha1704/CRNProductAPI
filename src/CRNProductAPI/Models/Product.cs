using System.ComponentModel.DataAnnotations;

namespace CRNProductAPI.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(255, MinimumLength = 2)]
    [RegularExpression(@".*\S.*", ErrorMessage = "Product name cannot contain only spaces.")]
    public string ProductName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [RegularExpression(@".*\S.*", ErrorMessage = "CreatedBy cannot contain only spaces.")]
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }
    [StringLength(100)]
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public ICollection<Item> Items { get; set; } = new List<Item>();
}
