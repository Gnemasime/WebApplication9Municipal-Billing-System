using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication9Municipal_Billing_System.Data;
using WebApplication9Municipal_Billing_System.Models;  // Include your models namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var billingDbConnectionString = builder.Configuration.GetConnectionString("BillingDb");

// Register your ApplicationDbContext for Identity using DefaultConnection (use MySQL)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(defaultConnectionString));

// Register your custom DBContextClassReg for your billing database using BillingDb connection string (use MySQL)
builder.Services.AddDbContext<DBContextClassReg>(options =>
    options.UseMySQL(billingDbConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();  // Ensure Identity uses ApplicationDbContext

builder.Services.AddSession();
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust the timeout as needed
    options.Cookie.HttpOnly = true; // Makes the session cookie accessible only by the server
    options.Cookie.IsEssential = true; // Required for session state
});

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

// Use session before UseAuthentication and UseAuthorization
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Reg}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
