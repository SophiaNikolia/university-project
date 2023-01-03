using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using university_project.Areas.Identity.Data;
using university_project.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("identityContextConnection") ?? throw new InvalidOperationException("Connection string 'identityContextConnection' not found.");

builder.Services.AddDbContext<identityContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<EntityUser>(options => options.SignIn.RequireConfirmedAccount = true)
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
