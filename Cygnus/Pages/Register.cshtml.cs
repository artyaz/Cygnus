using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Cygnus.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cygnus.Pages;

public class Register : PageModel
{
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Register(AppDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> OnPostAsync(string username, string password)
    {
        var salt = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        var hashedPassword = HashPassword(password, salt);
        
        var user = new User
        {
            Username = username,
            PasswordHash = hashedPassword, // Implement a password hashing function
            Role = "admin"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new Claim("usr", user.Username),
            new Claim("role", user.Role)
        };
        var identity = new ClaimsIdentity(claims, "cookie");
        var principal = new ClaimsPrincipal(identity);

        await _httpContextAccessor.HttpContext.SignInAsync(principal);

        return RedirectToPage("/Index");
    }
    public void OnGet()
    {
        
    }
    
    private string HashPassword(string password, byte[] salt)
    {
        int iterations = 10000; // You can adjust the number of iterations for your needs
        byte[] hashedPasswordBytes = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: iterations,
            numBytesRequested: 256 / 8
        );

        // Combine the salt, iterations, and hashed password into a single string
        string hashedPassword = $"{Convert.ToBase64String(salt)}.{iterations}.{Convert.ToBase64String(hashedPasswordBytes)}";
        return hashedPassword;
    }
    
}