using Cygnus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cygnus.Pages;

public class ProductCatalogModel : PageModel
{
    private readonly ILogger<ProductCatalogModel> _logger;
    private readonly IProductRepository _productRepository;
    public List<Product> Products = new List<Product>();

    public ProductCatalogModel(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public void OnPostApplyFilters(string roastLevel, string origin, string flavorProfile, string organic, string decaf, string bagSize, double minPrice, double maxPrice)
    {
        bool? organicBool = !string.IsNullOrEmpty(organic) ? (organic == "Yes") : (bool?)null;
        bool? decafBool = !string.IsNullOrEmpty(decaf) ? (decaf == "Yes") : (bool?)null;
        IEnumerable<Product> filteredProducts = _productRepository.GetProductsByFilters(roastLevel, origin, flavorProfile, organicBool, decafBool, bagSize, minPrice, maxPrice);
        Products = filteredProducts.ToList();
    }

    public void OnGet()
    {
        Products = _productRepository.GetAllProducts().ToList();
    }
}