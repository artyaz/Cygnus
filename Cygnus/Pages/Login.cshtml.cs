using System.Security.Claims;
using Cygnus.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cygnus.Pages;

public class Login : PageModel
{
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Login(AppDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> OnPostAsync(string username, string password)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);

        if (user != null && VerifyPassword(user.PasswordHash, password)) // Implement a password verification function
        {
            var claims = new List<Claim>
            {
                new Claim("usr", user.Username),
                new Claim("role", user.Role)
            };
            var identity = new ClaimsIdentity(claims, "cookie");
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(
                scheme: "Identity.Application",
                principal: principal,
                properties: new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });

            return RedirectToPage("/Index");
        }

        ModelState.AddModelError("", "Invalid login attempt.");
        return Page();
    }

    private bool VerifyPassword(string userPasswordHash, string password)
    {
        // Get the salt, iteration count, and stored hash from the password hash string using a separator
        string[] hashParts = userPasswordHash.Split('.', 3);
        if (hashParts.Length != 3) return false;

        byte[] salt = Convert.FromBase64String(hashParts[0]);
        int iterations = int.Parse(hashParts[1]);
        string storedHash = hashParts[2];

        // Hash the provided password using the same salt and iteration count
        byte[] hashedPassword = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: iterations,
            numBytesRequested: 256 / 8
        );

        // Compare the derived hash with the stored hash
        return Convert.ToBase64String(hashedPassword) == storedHash;
    }

    public void OnGet()
    {
        
    }
}