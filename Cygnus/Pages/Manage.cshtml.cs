using Cygnus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Cygnus.Pages
{
    public class ManageModel : PageModel
    {
        //list of products
        public List<Product> Products = new List<Product>();

        private readonly IProductRepository _productRepository;

        public ManageModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void Delete(int id)
        {
            _productRepository.DeleteProduct(id);
        }

        [HttpPost]
        public void OnPost()
        {
            Products = _productRepository.GetAllProducts().ToList();
            int lastProductId = 0;
            if (Products.Count > 0)
            {
                lastProductId = _productRepository.GetAllProducts().Last().Id;    
            }
            

            var newProduct = new Product
            {
                Id = lastProductId + 1,
                Name = Request.Form["name"],
                Description = Request.Form["description"],
                Price = double.Parse(Request.Form["price"]),
                ImageUrl = Request.Form["imageUrl"]
            };

            _productRepository.AddProduct(newProduct);
            
            OnGet();
        }

        public void OnGet()
        {
            Products = _productRepository.GetAllProducts().ToList();
        }
    }
}