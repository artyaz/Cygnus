using Cygnus.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cygnus.Pages;

public class Cart : PageModel
{
    public List<Product> CartProducts = new List<Product>();
    
    private readonly IProductRepository _productRepository;

    public Cart(IProductRepository productRepository)
    {
        _productRepository = productRepository;
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
}