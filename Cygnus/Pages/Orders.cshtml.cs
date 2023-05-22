using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cygnus.Data;

namespace Cygnus.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly AppDbContext _context;

        public OrdersModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; }

        public void OnGet()
        {
            Orders = _context.Orders.ToList();
        }
    }
}