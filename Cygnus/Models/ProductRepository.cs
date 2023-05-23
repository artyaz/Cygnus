using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cygnus.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Cygnus.Models;

public class ProductRepository : IProductRepository
{
    private readonly List<Product> _products;
    private readonly List<Product> _cartProducts;
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Product> GetAllProducts()
    {
        return _context.Products.ToList();
    }

    public Product GetProductById(int id)
    {
        return _context.Products.FirstOrDefault(p => p.ProductId == id);
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }

    public void DeleteProduct(int id)
    {
        var productToRemove = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (productToRemove != null)
        {
            _context.Products.Remove(productToRemove);
            _context.SaveChanges();
        }
    }

    public IEnumerable<Product> GetProductsByFilters(string roastLevel, string origin, string flavorProfile,
        bool? organic, bool? decaf, string bagSize, double minPrice, double maxPrice)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(roastLevel))
        {
            query = query.Where(p => p.RoastLevel.ToLower() == roastLevel.ToLower());
        }

        if (!string.IsNullOrEmpty(origin))
        {
            query = query.Where(p => p.Origin.ToLower() == origin.ToLower());
        }

        if (!string.IsNullOrEmpty(flavorProfile))
        {
            query = query.Where(p => p.FlavorProfile.ToLower() == flavorProfile.ToLower());
        }

        switch (organic)
        {
            case true:
                query = query.Where(p => p.Organic);
                break;
            case false:
                query = query.Where(p => !p.Organic);
                break;
        }

        switch (decaf)
        {
            case true:
                query = query.Where(p => p.Decaf);
                break;
            case false:
                query = query.Where(p => !p.Decaf);
                break;
        }

        if (!string.IsNullOrEmpty(bagSize))
        {
            query = query.Where(p => p.BagSize.ToLower() == bagSize.ToLower());
        }

        if (minPrice > 0)
        {
            query = query.Where(p => p.Price >= minPrice);
        }

        if (maxPrice > minPrice)
        {
            query = query.Where(p => p.Price <= maxPrice);
        }

        return query.ToList();
    }
    public void AddToCart(CartProduct cartProduct)
    {
        _context.CartProducts.Add(cartProduct);
        _context.SaveChanges();
    }

    public void UpdateCartProduct(CartProduct cartProduct)
    {
        _context.CartProducts.Update(cartProduct);
        _context.SaveChanges();
    }

    public void RemoveFromCart(int cartProductId)
    {
        var cartProduct = _context.CartProducts.FirstOrDefault(cp => cp.CartProductId == cartProductId);
        if (cartProduct != null)
        {
            _context.CartProducts.Remove(cartProduct);
            _context.SaveChanges();
        }
    }

    public void ClearCart(string username)
    {
        var cartProducts = GetAllCartProducts(username);
        foreach (var cartProduct in cartProducts)
        {
            RemoveFromCart(cartProduct.CartProductId);
        }
    }

    public IEnumerable<CartProduct> GetAllCartProducts(string ownerUsername)
    {
        return _context.CartProducts
            .Include(cp => cp.Product)
            .Where(cp => cp.OwnerUsername == ownerUsername)
            .ToList();
    }

}
