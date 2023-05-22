
using System.Security.Claims;
using Cygnus;
using Cygnus.Data;
using Cygnus.Models;
using Cygnus.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=mydb;Username=artemcmilenko"));
builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie");

builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("admin", policy =>
    {
        policy.RequireClaim("role", "admin");
    });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync();
    return "logged out";
});

app.MapGet("/registe", async (HttpContext ctx, AppDbContext db) =>
{
    var user = new User
    {
        Username = "test2",
        PasswordHash = "your_password_hash", // Replace this with a hashed password
        Role = "admin"
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    var claims = new List<Claim>
    {
        new Claim("usr", user.Username),
        new Claim("role", user.Role)
    };
    var identity = new ClaimsIdentity(claims, "cookie");
    var principal = new ClaimsPrincipal(identity);

    await ctx.SignInAsync("cookie", principal);

    return "ok";
    
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
