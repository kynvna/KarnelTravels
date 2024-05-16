using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Azure;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<KarnelTravels.Models.KarnelTravelsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KarnelTravelsDatabase")));
// Add services to the container.
builder.Services.AddControllersWithViews();
// Basic operation to enable sessions and other needed settings for the application before launching.
builder.Services.AddAuthentication(options =>
{
    // This sets the default scheme used for authentication.
    // It's important this matches with how you intend to sign users in.
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie("Admin", options =>
    {
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Admin/AccessDenied";
        options.Cookie.Name = "AdminAuthCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout duration
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make the session cookie essential
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensure HTTPS
    options.Cookie.SameSite = SameSiteMode.Lax;
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();