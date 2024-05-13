using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Azure;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<KarnelTravels.Models.KarnelTravelsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KarnelTravelsDatabase")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout duration
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make the session cookie essential
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
/*app.MapGet("/Admin/", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    
});*/


/*app.MapGet("/Admin/", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/AdminHotelView", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/ViewCreateAdminHotel", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/AdminProfile", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }

    return Task.CompletedTask;
});
app.MapGet("/Admin/AdminSpotView", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/AdminTourPackage", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/AdminTourView", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/AdminTransportView", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/FeedbackOnComp", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/FeedbackOnObj", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/Sitemap", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/ViewCreateAdminTransport", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/ViewCreateTourPackage", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/ViewEditAdminHotel", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/ViewEditAdminTourPackage", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});
app.MapGet("/Admin/ViewEditAdminTransport", context =>
{
    var admin_id = context.Session.GetString("admin");
    if (admin_id == null)
    {
        context.Response.Redirect("/home");
    }
    return Task.CompletedTask;
});*/
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Login}/{id?}");*/
app.Run();

