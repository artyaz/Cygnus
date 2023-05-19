
using System.Linq;
using Cygnus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cygnus.Pages
{
    public class IndexModel : PageModel
    {
        //list ]of products
        public List<Product> Products = new List<Product>();

        private readonly IProductRepository _productRepository;

        public IndexModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void OnPostSearch(string searchText)
        {
            Products = _productRepository.GetAllProducts().ToList();
            var results = Products.Where(h => h.Name.Contains(searchText) || h.Description.Contains(searchText));
            Products = results.ToList();
        }

        public void OnGet()
        {
            Products = _productRepository.GetAllProducts().ToList();
        }
    }

    }
