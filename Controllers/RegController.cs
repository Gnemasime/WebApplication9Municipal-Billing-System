using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication9Municipal_Billing_System.Models;

namespace WebApplication9Municipal_Billing_System.Controllers
{
    public class RegController : Controller
    {
        private readonly DBContextClassReg _dbContext;

        // Constructor that injects DBContextClassReg
        public RegController(DBContextClassReg db)
        {
            _dbContext = db;
        }

        // GET: Reg (Display list of registered users)
        public async Task<IActionResult> Users()
        {
            var usersList = await _dbContext.Regs.ToListAsync();
            return View(usersList);
        }

        // GET: Register (Registration page)
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register (Create a new user)
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Reg model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new Reg
                {
                    UserId = model.UserId,
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    IdNumber = model.IdNumber,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };

                try
                {
                    // Add the new user to the database
                    _dbContext.Regs.Add(newUser);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Login");
                }
                catch (DbUpdateException ex)
                {
                    // Handle the database update exception, log the error
                    ModelState.AddModelError("", "Error saving data. Please try again.");
                    Console.WriteLine($"Database Error: {ex.Message}");
                }
            }

            // If we get here, something failed, return the form with validation errors
            return View(model);
        }

        // GET: Login (Login page)
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login (Authenticate user)
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Hardcoded admin login (for testing purposes)
                if (model.Email == "admin@gmail.com" && model.Password == "admin@0123")
                {
                    return RedirectToAction("Dashboard");
                }

                // Find user in the database
                var user = await _dbContext.Regs
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    // Redirect to user profile after successful login
                    return RedirectToAction("Profile");
                }

                // If user is not found, display an error
                ModelState.AddModelError("", "Invalid login attempt.");
            }

            // Return the view with validation errors
            return View(model);
        }

        // GET: Dashboard (Admin dashboard)
        public IActionResult Dashboard()
        {
            return View("Dashboard");
        }

        // GET: Profile (User profile page)
        public IActionResult Profile()
        {
            return View();
        }

        // GET: Index (Home page)
        public IActionResult Index()
        {
            return View();
        }
    }
}
