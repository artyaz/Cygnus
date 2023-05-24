using Cygnus.Data;
using Cygnus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cygnus.Pages;

public class Cart : PageModel
{
    public List<CartProduct> CartProducts { get; set; }
    public List<string> Cities { get; set; }
    public List<string> PostDepartments { get; set; }
    
    private readonly NovaPoshtaApi _novaPoshtaApi;
    private readonly IProductRepository _productRepository;
    private readonly AppDbContext _db;

    public Cart(IProductRepository productRepository, AppDbContext db, NovaPoshtaApi novaPoshtaApi)
    {
        _productRepository = productRepository;
        _db = db;
        _novaPoshtaApi = novaPoshtaApi;
    }

    public async Task OnPostCheckout(string city, string postDepartment)
    {
        string ownerUsername = User.Identity.IsAuthenticated ? User.Claims.ToList()[0].Value : "anonymous";
        CartProducts = _productRepository.GetAllCartProducts(ownerUsername).ToList();

        var order = new Order
        {
            Owner = ownerUsername,
            Invoice = GenerateInvoice(),
            City = city,
            Department = postDepartment,
            OrderProducts = new List<OrderProduct>()
        };

        foreach (var cartProduct in CartProducts)
        {
            order.OrderProducts.Add(new OrderProduct { ProductId = cartProduct.ProductId, Count = cartProduct.Count});
            _productRepository.RemoveFromCart(cartProduct.CartProductId);
        }

        _db.Orders.Add(order);
        _productRepository.ClearCart(ownerUsername);
        await _db.SaveChangesAsync();
        OnGetAsync();
    }

    // public async Task onPostChangeCount(int quantity, int id)
    // {
    //     CartProducts = _productRepository.GetAllCartProducts(ownerUsername).ToList();
    //     _productRepository.UpdateCartProductCount(id, quantity);
    //     await _db.SaveChangesAsync();
    //     OnGetAsync();
    // }
    
    public void OnPostRemove(int id)
    {
        _productRepository.RemoveFromCart(id);
        OnGetAsync();
    }

    public async Task OnGetAsync()
    {
        string ownerUsername = User.Identity.IsAuthenticated ? User.Claims.ToList()[0].Value : "anonymous";
        CartProducts = _productRepository.GetAllCartProducts(ownerUsername).ToList();

        // Fetch the list of cities
        //Cities = await _novaPoshtaApi.GetCitiesAsync();
        Cities = new List<string>() {
            "Київ", "Львів", "Одеса", "Харків", "Дніпро",
            "Вінниця", "Івано-Франківськ", "Чернівці", "Запоріжжя", "Тернопіль",
            "Черкаси", "Житомир", "Ужгород", "Кропивницький", "Кременчук",
            "Миколаїв", "Полтава", "Рівне", "Суми", "Херсон",
            "Чернігів"
        };

        
    }
    public async Task<IActionResult> OnGetPostDepartmentsAsync(string city)
    {
        PostDepartments = await _novaPoshtaApi.GetPostDepartmentsAsync(city);
        return new JsonResult(PostDepartments);
    }

    public string GenerateInvoice()
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return "ORD-" + new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}