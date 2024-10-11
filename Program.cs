using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication9Municipal_Billing_System.Data;
using WebApplication9Municipal_Billing_System.Models;  // Include your models namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var billingDbConnectionString = builder.Configuration.GetConnectionString("BillingDb");

// Register your ApplicationDbContext for Identity using DefaultConnection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(defaultConnectionString));

// Register your custom DBContextClassReg for your billing database using BillingDb connection string
builder.Services.AddDbContext<DBContextClassReg>(options =>
    options.UseSqlServer(billingDbConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();  // Ensure Identity uses ApplicationDbContext

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Add HSTS for production
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Reg}/{action=index}/{id?}");

app.MapRazorPages();

app.Run();