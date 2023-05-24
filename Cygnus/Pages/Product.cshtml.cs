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

    public void OnPostAddToCart(int id, int quantity)
    {
        string ownerUsername = User.Identity.IsAuthenticated ? User.Claims.ToList()[0].Value : "anonymous";

        // Check if the product already exists in the cart
        var existingCartProduct = _productRepository.GetCartProductByProductIdAndOwner(id, ownerUsername);

        if (existingCartProduct != null)
        {
            // If the product exists, update the count
            existingCartProduct.Count += quantity;
            _productRepository.UpdateCartProduct(existingCartProduct);
        }
        else
        {
            // If the product doesn't exist, add it to the cart
            var cartProduct = new CartProduct
            {
                ProductId = id,
                Product = _productRepository.GetProductById(id),
                Count = quantity,
                OwnerUsername = ownerUsername
            };
            _productRepository.AddToCart(cartProduct);
        }

        OnGet(id);
    }

    public void OnGet(int id)
    {
        Product = _productRepository.GetProductById(id);
        ViewData["Title"] = Product.Name;
    }
}