using Cygnus.Data;
using Cygnus.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cygnus.Pages;

public class Cart : PageModel
{
    public List<Product> CartProducts = new List<Product>();
    
    private readonly IProductRepository _productRepository;
    private readonly AppDbContext _db;
    public Cart(IProductRepository productRepository, AppDbContext db)
    {
        _productRepository = productRepository;
        _db = db;
    }

    public async Task OnPostCheckout()
    {
        string user = _db.Users.FirstOrDefault(u => User.Identity != null && u.Username == User.Identity.Name)?.Username ?? "anonymous";

        CartProducts = _productRepository.GetAllCartProducts().ToList();
        var order = new Order
        {
            Owner = user,
            Invoice = GenerateInvoice(),
            Products = CartProducts
        };
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
    }

    public void OnPostRemove(int id)
    {
        _productRepository.RemoveFromCart(id);
        OnGet();
    }
    
    public void OnGet()
    {
        CartProducts = _productRepository.GetAllCartProducts().ToList();
    }

    public string GenerateInvoice()
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return "ORD-" + new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
}