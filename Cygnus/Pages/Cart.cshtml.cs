using Cygnus.Data;
using Cygnus.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cygnus.Pages;

public class Cart : PageModel
{
    public List<CartProduct> CartProducts { get; set; }

    private readonly IProductRepository _productRepository;
    private readonly AppDbContext _db;

    public Cart(IProductRepository productRepository, AppDbContext db)
    {
        _productRepository = productRepository;
        _db = db;
    }

    public async Task OnPostCheckout()
    {
        string ownerUsername = User.Identity.IsAuthenticated ? User.Claims.ToList()[0].Value : "anonymous";
        CartProducts = _productRepository.GetAllCartProducts(ownerUsername).ToList();

        var order = new Order
        {
            Owner = ownerUsername,
            Invoice = GenerateInvoice(),
            OrderProducts = new List<OrderProduct>()
        };

        foreach (var cartProduct in CartProducts)
        {
            order.OrderProducts.Add(new OrderProduct { ProductId = cartProduct.ProductId });
            _productRepository.RemoveFromCart(cartProduct.CartProductId);
        }

        _db.Orders.Add(order);
        _productRepository.ClearCart(ownerUsername);
        await _db.SaveChangesAsync();
        OnGet();
    }

    public void OnPostRemove(int id)
    {
        _productRepository.RemoveFromCart(id);
        OnGet();
    }

    public void OnGet()
    {
        string ownerUsername = User.Identity.IsAuthenticated ? User.Claims.ToList()[0].Value : "anonymous";
        CartProducts = _productRepository.GetAllCartProducts(ownerUsername).ToList();
    }

    public string GenerateInvoice()
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return "ORD-" + new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}