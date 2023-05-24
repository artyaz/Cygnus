using System.ComponentModel.DataAnnotations;

namespace Cygnus;

public class CartProduct
{
    [Key]
    public int CartProductId { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Count { get; set; }
    public string OwnerUsername { get; set; }
}