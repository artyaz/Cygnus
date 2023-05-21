using Cygnus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Cygnus.Pages
{
    [Authorize(Policy = "admin")]
    public class ManageModel : PageModel
    {
        //list of products
        public List<Product> Products = new List<Product>();
        
        public Product currentEdit = new Product();

        private readonly IProductRepository _productRepository;

        public ManageModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void Delete(int id)
        {
            _productRepository.DeleteProduct(id);
        }
        
        public void OnPostCurrentEdit(int edit)
        {
            var product = _productRepository.GetProductById(edit);
            currentEdit = product;
        }
        
        public void OnPostEdit()
        {
            bool organicBool = Request.Form["editOrganic"] == "true";
            bool decafBool = Request.Form["editDecaf"] == "true";
            string name = Request.Form["editName"];
            var product = _productRepository.GetProductById(int.Parse(Request.Form["editId"]));
            product.Name = Request.Form["editName"];
            product.Description = Request.Form["editDescription"];
            product.Price = double.Parse(Request.Form["editPrice"]);
            product.ImageUrl = Request.Form["editImageUrl"];
            product.RoastLevel = Request.Form["editRoastLevel"];
            product.Origin = Request.Form["editOrigin"];
            product.Tag = Request.Form["editTag"];
            product.FlavorProfile = Request.Form["editFlavorProfile"];
            product.Organic = organicBool;
            product.Decaf = decafBool;
            product.BagSize = Request.Form["editBagSize"];
            
            _productRepository.UpdateProduct(product);
            
            OnGet();
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
            bool organicBool = Request.Form["organic"] == "Yes";
            bool decafBool = Request.Form["decaf"] == "Yes";
            var newProduct = new Product
            {
                
                Id = lastProductId + 1,
                Name = Request.Form["name"],
                Description = Request.Form["description"],
                Price = double.Parse(Request.Form["price"]),
                ImageUrl = Request.Form["imageUrl"],
                RoastLevel = Request.Form["roast-level"],
                Origin = Request.Form["origin"],
                Tag = Request.Form["tag"],
                FlavorProfile = Request.Form["flavor-profile"],
                Organic = organicBool,
                Decaf = decafBool,
                BagSize = Request.Form["bag-size"]
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