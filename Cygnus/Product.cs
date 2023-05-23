using System.ComponentModel.DataAnnotations;

namespace Cygnus;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    [Required]
    public string Description { get; set; }
    
    public string Tag { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public string RoastLevel { get; set; }
    [Required]
    public string Origin { get; set; }
    [Required]
    public string FlavorProfile { get; set; }
    [Required]
    public bool Organic { get; set; }
    [Required]
    public bool Decaf { get; set; }
    [Required]
    public string BagSize { get; set; }
    
}
