using System.ComponentModel.DataAnnotations;

namespace Cygnus;

public class Order
{
    [Key] 
    public int OrderId { get; set; }
    
    public string Owner { get; set; }
    
    [Required]
    public string Invoice { get; set; }
    
    [Required]
    public List<Product> Products { get; set; }
}