
using System.Security.Claims;
using Cygnus.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
// builder.Services.AddScoped<AuthService>();
// builder.Services.AddDataProtection();
// builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie");

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.Use((ctx, next) =>
// {
//     var idp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
//     
//     var protector = idp.CreateProtector("auth-cookie");
//     var authCookie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x.Contains("auth="));
//
//     var protectedPayload = authCookie.Split("=").Last();
//     var payload = protector.Unprotect(protectedPayload);
//     var parts = payload.Split(":");
//     var key = parts[0];
//     var username = parts[1];
//
//     var claims = new List<Claim>();
//     claims.Add(new Claim(key, username));
//     var identity = new ClaimsIdentity(claims);
//     ctx.User = new ClaimsPrincipal(identity);
//
//     return next();
// });

app.UseAuthentication();

app.MapGet("/username", (HttpContext ctx) =>
{
    return ctx.User.FindFirst("usr")?.Value;
});

app.MapGet("/login", async (HttpContext ctx) =>
{
    //auth.SignIn();
    
    var claims = new List<Claim>();
    claims.Add(new Claim("usr", "artem"));
    var identity = new ClaimsIdentity(claims, "cookie");
    var user = new ClaimsPrincipal(identity);

    await ctx.SignInAsync("cookie", user);
    
    return "ok";
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

// public class AuthService
// {
//     private readonly IHttpContextAccessor _accessor;
//     private readonly IDataProtectionProvider _idp;
//     public AuthService(IDataProtectionProvider idp, IHttpContextAccessor accessor)
//     {
//         _accessor = accessor;
//         _idp = idp;
//     }
//
//     public void SignIn()
//     {
//         var protector = _idp.CreateProtector("auth-cookie");
//         _accessor.HttpContext.Response.Headers["set-cookie"] = $"auth={protector.Protect("usr:artem")}" ;
//     }
// }