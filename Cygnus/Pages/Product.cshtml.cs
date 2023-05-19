using Cygnus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cygnus.Pages
{
    public class ProductModel : PageModel
    {
        public Product Product { get; set; }
        
        private readonly IProductRepository _productRepository;

        public ProductModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public void OnPostAddToCart(int id)
        {
            Product product = _productRepository.GetProductById(id);
            _productRepository.AddToCart(product);
            OnGet(id);
        }

        public void OnGet(int id)
        {
            
            Product = _productRepository.GetProductById(id);

            // Set the "Title" property of the ViewData dictionary
            ViewData["Title"] = Product.Name;
        }
    }
}