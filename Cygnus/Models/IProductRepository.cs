namespace Cygnus.Models;

public interface IProductRepository
{
    IEnumerable<Product> GetAllProducts();
    Product GetProductById(int id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(int id);
    void AddToCart(Product product);
    
    void RemoveFromCart(int id);
    IEnumerable<Product> GetAllCartProducts();
    IEnumerable<Product> GetProductsByFilters(string roastLevel, string origin, string flavorProfile, bool organic, bool decaf, string bagSize, double price);
}