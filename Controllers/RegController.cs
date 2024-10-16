using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal;
using PayPal.Api;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication9Municipal_Billing_System.Models;

namespace WebApplication9Municipal_Billing_System.Controllers
{
    public class RegController : Controller
    {
        private readonly DBContextClassReg _dbContext;
        // PayPal Live API credentials
        private readonly string clientId = "AdXHEP1jTg0J_HDMelGoKzkmXiJqg65ZVFa8ibAfReLDAq0XecE9z0bGuVfNjLFHtIxOkd-0Mr142NJt"; // Replace with your live client ID
        private readonly string clientSecret = "EBkxRjpt0YuacBgg5WwX2S6tDSu7xV-gcovLblYrpsTzeBurBqP_3P3CUEfqaNUhREvaZeYzYVhotyn8"; // Replace with your live client secret

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
                    // Store the user ID in session
                    HttpContext.Session.SetInt32("UserId", user.UserId);
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

        // GET: /Water/UserDashboard
        [HttpGet]
        public async Task<IActionResult> Water()
        {
            // Get the current user's ID from the session
            var userId = HttpContext.Session.GetInt32("UserId");

            // Redirect to login if not logged in
            if (userId == null)
            {
                return RedirectToAction("Index", "Reg");
            }

            // Fetch water records associated with the logged-in user
            var waters = await _dbContext.waters
                .Where(w => w.RegUserId == userId.Value) // Filter by user ID
                .Include(w => w.Reg)
                .ToListAsync();

            return View(waters);
        }

        // POST: Create Payment
       [HttpPost]
public IActionResult CreatePayment(decimal amount, int waterId)
{
    if (amount <= 0)
    {
        ModelState.AddModelError("", "Amount must be greater than zero.");
        return View(); // Return an appropriate view to inform the user of the error
    }

    var apiContext = new APIContext(new OAuthTokenCredential(clientId, clientSecret).GetAccessToken());
    var payment = new Payment
    {
        intent = "sale",
        payer = new Payer { payment_method = "paypal" },
        transactions = new List<Transaction>
        {
            new Transaction
            {
                description = "Water Bill Payment",
                invoice_number = waterId.ToString(), // Use WaterId as invoice number
                amount = new Amount
                {
                    currency = "ZAR",
                    total = amount.ToString("F2") // Ensure amount is formatted correctly
                }
            }
        },
        redirect_urls = new RedirectUrls
        {
            cancel_url = Url.Action("PaymentCancelled", "Reg", null, Request.Scheme),
            return_url = Url.Action("PaymentSuccess", "Reg", new { waterId }, Request.Scheme)
        }
    };

    try
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(payment)); // Log payment object
        var createdPayment = payment.Create(apiContext);
        var approvalUrl = createdPayment.links.FirstOrDefault(l => l.rel.ToLower() == "approval_url")?.href;

        return Redirect(approvalUrl);
    }
    catch (PaymentsException ex)
    {
        Console.WriteLine($"PayPal Payment Error: {ex.Message}");
        if (ex.Response != null)
        {
            Console.WriteLine($"Error Response: {ex.Response}");
        }
        ModelState.AddModelError("", "Payment processing failed. Please try again.");
        return View(); // Return an appropriate view to inform the user of the error
    }
}


        // GET: Payment Success
        [HttpGet]
        public async Task<IActionResult> PaymentSuccess(int waterId)
        {
            // Fetch the water record and update its status
            var water = await _dbContext.waters.FirstOrDefaultAsync(w => w.WaterId == waterId);
            if (water != null)
            {
                water.status = WStatus.paid; // Update status to paid
                await _dbContext.SaveChangesAsync();
            }
            return View("PaymentSuccess"); // Create this view to confirm payment
        }

        // GET: Payment Cancelled
        [HttpGet]
        public IActionResult PaymentCancelled()
        {
            return View("PaymentCancelled"); // Create this view to inform about cancellation
        }

        // Example logout method to clear session
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index");
        }
    }
}
