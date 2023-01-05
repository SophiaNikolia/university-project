using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using university_project.Areas.Identity.Data;
using university_project.Data;
using university_project.Models;

var builder = WebApplication.CreateBuilder(args);

var universityConnectionString = builder.Configuration.GetConnectionString("universityContextConnection") ?? throw new InvalidOperationException("Connection string 'universityContextConnection' not found.");
var identityConnectionString = builder.Configuration.GetConnectionString("identityContextConnection") ?? throw new InvalidOperationException("Connection string 'identityContextConnection' not found.");

builder.Services.AddDbContext<universityContext>(options =>
    options.UseSqlite(universityConnectionString));

builder.Services.AddDbContext<identityContext>(options =>
    options.UseSqlite(identityConnectionString));

builder.Services.AddDefaultIdentity<EntityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<identityContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
