
using System.Linq;
using System.Net;
using System.Text;
using Cygnus.Data;
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
        private readonly AppDbContext _db;

        public IndexModel(IProductRepository productRepository, AppDbContext db)
        {
            _productRepository = productRepository;
            _db = db;
        }

        public void OnPostSearch(string searchText)
        {
            Products = _productRepository.GetAllProducts().ToList();
            var results = Products.Where(h => h.Name.Contains(searchText) || h.Description.Contains(searchText));
            Products = results.ToList();
            

        }

        public async Task OnGet()
        {
            Products = _productRepository.GetAllProducts().ToList();


        }
    }

    }
