using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cygnus.Data;
using Cygnus.Models;
using Microsoft.EntityFrameworkCore;

namespace Cygnus.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly AppDbContext _context;

        public OrdersModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> orders { get; set; }
        
        public void OnGet()
        {
            string user = _context.Users.FirstOrDefault(u => User.Identity != null && u.Username == User.Claims.ToList()[0].Value)?.Username ?? "anonymous";
            orders = _context.Orders.Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Where(o => o.Owner == user)
                .ToList();
        }
    }
}