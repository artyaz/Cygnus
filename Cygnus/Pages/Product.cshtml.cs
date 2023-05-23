using Cygnus.Data;
using Cygnus.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cygnus.Pages;

public class ProductModel : PageModel
{
    public Product Product { get; set; }

    private readonly IProductRepository _productRepository;
    private readonly AppDbContext _db;

    public ProductModel(IProductRepository productRepository, AppDbContext db)
    {
        _productRepository = productRepository;
        _db = db;
    }

    public void OnPostAddToCart(int id)
    {
        string ownerUsername = User.Identity.IsAuthenticated ? User.Claims.ToList()[0].Value : "anonymous";
        var cartProduct = new CartProduct
        {
            ProductId = id,
            Product = _productRepository.GetProductById(id),
            Count = 1,
            OwnerUsername = ownerUsername
        };
        _productRepository.AddToCart(cartProduct);
        OnGet(id);
    }

    public void OnGet(int id)
    {
        Product = _productRepository.GetProductById(id);
        ViewData["Title"] = Product.Name;
    }
}