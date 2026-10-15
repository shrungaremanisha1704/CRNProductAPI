using System.ComponentModel.DataAnnotations;

namespace CRNProductAPI.Models;

public class CreateProductRequest
{
    [Required, StringLength(255, MinimumLength = 2)]
    [RegularExpression(@".*\S.*")]
    public string ProductName { get; set; } = string.Empty;
    [Required, StringLength(100)]
    [RegularExpression(@".*\S.*")]
    public string CreatedBy { get; set; } = string.Empty;
}
public class UpdateProductRequest
{
    [Required, StringLength(255, MinimumLength = 2)]
    [RegularExpression(@".*\S.*")]
    public string ProductName { get; set; } = string.Empty;
    [Required, StringLength(100)]
    [RegularExpression(@".*\S.*")]
    public string ModifiedBy { get; set; } = string.Empty;
}
public class CreateItemRequest
{
    [Range(1, int.MaxValue)] public int ProductId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
}
public class UpdateItemRequest
{
    [Range(1, int.MaxValue)] public int ProductId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
}
