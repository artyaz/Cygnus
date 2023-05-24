using System.Collections.Generic;
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
    public string City { get; set; }
    [Required]
    public string Department { get; set; }
    [Required]
    public List<OrderProduct> OrderProducts { get; set; }
}