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
    
    public void OnPostApplyFilters()
    {
        
    }

    public void OnGet()
    {
        Products = _productRepository.GetAllProducts().ToList();
    }
}