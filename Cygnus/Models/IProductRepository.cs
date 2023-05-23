namespace Cygnus.Models;

public interface IProductRepository
{
    IEnumerable<Product> GetAllProducts();
    Product GetProductById(int id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(int id);
    IEnumerable<Product> GetProductsByFilters(string roastLevel, string origin, string flavorProfile, bool? organic,
        bool? decaf, string bagSize, double minPrice, double maxPrice);
    void AddToCart(CartProduct cartProduct);
    void UpdateCartProduct(CartProduct cartProduct);
    void RemoveFromCart(int cartProductId);
    void ClearCart(string username);
    IEnumerable<CartProduct> GetAllCartProducts(string ownerUsername);
    
}