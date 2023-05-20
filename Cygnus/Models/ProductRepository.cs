using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Cygnus.Models;

public class ProductRepository : IProductRepository
{
    private readonly List<Product> _products;
    private readonly List<Product> _cartProducts;

    public ProductRepository()
    {
        // Load the products from a JSON file
        var jsonProducts = File.ReadAllText("products.json");
        _products = JsonConvert.DeserializeObject<List<Product>>(jsonProducts);
        var jsonCart = File.ReadAllText("cart.json");
        _cartProducts = JsonConvert.DeserializeObject<List<Product>>(jsonCart);
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _products;
    }

    public IEnumerable<Product> GetProductsByFilters(string roastLevel, string origin, string flavorProfile, bool organic, bool decaf, string bagSize, double price)
    {
        IEnumerable<Product> products = _products;

        if (!string.IsNullOrEmpty(roastLevel))
        {
            products = products.Where(p => p.RoastLevel.ToLower() == roastLevel.ToLower());
        }

        if (!string.IsNullOrEmpty(origin))
        {
            products = products.Where(p => p.Origin.ToLower() == origin.ToLower());
        }

        if (!string.IsNullOrEmpty(flavorProfile))
        {
            products = products.Where(p => p.FlavorProfile.ToLower() == flavorProfile.ToLower());
        }

        if (organic)
        {
            products = products.Where(p => p.Organic);
        }

        if (decaf)
        {
            products = products.Where(p => p.Decaf);
        }

        if (!string.IsNullOrEmpty(bagSize))
        {
            products = products.Where(p => p.BagSize.ToLower() == bagSize.ToLower());
        }

        if (price > 0)
        {
            products = products.Where(p => p.Price <= price);
        }

        return products;
    }

    
    public void RemoveFromCart(int id)
    {
        var productToRemove = _cartProducts.FirstOrDefault(p => p.Id == id);
        if (productToRemove != null)
        {
            _cartProducts.Remove(productToRemove);

            // Save the products to a temporary file
            var tempFile = Path.GetTempFileName();
            var json = JsonConvert.SerializeObject(_cartProducts);
            File.WriteAllText(tempFile, json);

            // Rename the temporary file to the original file name
            File.Replace(tempFile, "cart.json", null);
            // TODO: Delete the temporary file
        }
    }

    public IEnumerable<Product> GetAllCartProducts()
    {
        return _cartProducts;
    }

    public Product GetProductById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public void AddProduct(Product product)
    {
        _products.Add(product);

        // Save the products to the JSON file
        var json = JsonConvert.SerializeObject(_products);
        File.WriteAllText("products.json", json);
    }
    
    public void AddToCart(Product product)
    {
        _cartProducts.Add(product);

        // Save the products to the JSON file
        var json = JsonConvert.SerializeObject(_cartProducts);
        File.WriteAllText("cart.json", json);
    }
    
    public void PrintAllProducts()
    {
        foreach (var product in _products)
        {
            Console.WriteLine(product.Name);
            Console.WriteLine(product.Description);
            Console.WriteLine(product.Price);
            Console.WriteLine(product.ImageUrl);
        }
    }

    public void UpdateProduct(Product product)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
        if (existingProduct != null)
        {
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
        }
    }

    public void DeleteProduct(int id)
    {
        var productToRemove = _products.FirstOrDefault(p => p.Id == id);
        if (productToRemove != null)
        {
            _products.Remove(productToRemove);

            // Save the products to a temporary file
            var tempFile = Path.GetTempFileName();
            var json = JsonConvert.SerializeObject(_products);
            File.WriteAllText(tempFile, json);

            // Rename the temporary file to the original file name
            File.Replace(tempFile, "products.json", null);
        }
    }


}
