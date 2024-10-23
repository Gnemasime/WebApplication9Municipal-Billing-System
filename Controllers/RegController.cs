using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal;
using PayPal.Api;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication9Municipal_Billing_System.Models;
using WebApplication9Municipal_Billing_System.ViewModel;
//using WebApplication9Municipal_Billing_System.Services;

namespace WebApplication9Municipal_Billing_System.Controllers
{
    public class RegController : Controller
    {
       // private readonly NewsService _newsService;
        private readonly DBContextClassReg _dbContext;
        private readonly IConfiguration _configuration;
        // PayPal Live API credentials
        private readonly string clientId = "AdXHEP1jTg0J_HDMelGoKzkmXiJqg65ZVFa8ibAfReLDAq0XecE9z0bGuVfNjLFHtIxOkd-0Mr142NJt"; // Replace with your live client ID
        private readonly string clientSecret = "EBkxRjpt0YuacBgg5WwX2S6tDSu7xV-gcovLblYrpsTzeBurBqP_3P3CUEfqaNUhREvaZeYzYVhotyn8"; // Replace with your live client secret

        // Constructor that injects DBContextClassReg
        public RegController(DBContextClassReg db, IConfiguration con)
        {
            _dbContext = db;
            _configuration = con;
          //  _newsService = newsService;
        }

      /*   public async Task<IActionResult> ElectricityNews()
        {
            var articles = await _newsService.GetNewsAsync();

            ViewBag.Articles = articles;
            //ViewBag.ErrorMessage = errorMessage;

            return View();
        }*/

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
        public async Task<IActionResult> Dashboard()
        {
            var totalUsers = await _dbContext.Regs.CountAsync();
            var activeWBills = await _dbContext.waters.CountAsync(b => b.status == WStatus.unpaid);
            var activeEBills = await _dbContext.electricities.CountAsync(b => b.status == EStatus.unpaid);

            var waterTotal = await _dbContext.waters.SumAsync(t => t.Cost )  ;
            var electricityTotal = await _dbContext.electricities.SumAsync(t => t.Cost)  ;

            var paidBills = await _dbContext.waters.CountAsync(b => b.status == WStatus.paid);
            var unpaidBills = await _dbContext.waters.CountAsync(b => b.status == WStatus.unpaid);
         var overdueBills = await _dbContext.waters.CountAsync(b => b.status == WStatus.overdue);

         ViewBag.TotalUsers = totalUsers;
    ViewBag.ActiveBills = activeEBills;
   // ViewBag.TotalRevenue = totalRevenue;
    ViewBag.WaterTotal = waterTotal;
    ViewBag.ElectricityTotal = electricityTotal;
    ViewBag.PaidBills = paidBills;
    ViewBag.UnpaidBills = unpaidBills;
    ViewBag.OverdueBills = overdueBills;
            return View();
        }

        // GET: Profile (User profile page)
        public async Task<IActionResult> ProfileAsync()
        {
            // Get the current user's ID from the session
           var userId = HttpContext.Session.GetInt32("UserId");

             var wpaidCount = await _dbContext.waters.CountAsync(b => b.RegUserId == userId && b.status ==  WStatus.paid);
            var epaidCount = await _dbContext.electricities.CountAsync(b => b.RegUserId == userId && b.status == EStatus.paid);
            var wunpaidCount = await _dbContext.waters.CountAsync(b => b.RegUserId == userId && b.status == WStatus.unpaid);
            var eunpaidCount = await _dbContext.electricities.CountAsync(b => b.RegUserId == userId && b.status == EStatus.unpaid);
             var woverdueCount = await _dbContext.waters.CountAsync(b => b.RegUserId == userId && b.status == WStatus.overdue);
            var eoverdueCount = await _dbContext.electricities.CountAsync(b => b.RegUserId == userId && b.status == EStatus.overdue);

            var email = HttpContext.Session.GetString("email");

            ViewBag.WaterPaidCount = wpaidCount;
            ViewBag.ElectricityPaidCount = epaidCount;
            ViewBag.WaterUnpaidCount = wunpaidCount;
            ViewBag.ElectricityUnpaidCount = eunpaidCount;
              ViewBag.WaterOverdueCount = woverdueCount;
            ViewBag.ElectricityOverdueCount = eoverdueCount;
            
            ViewBag.Email = email;
            return View();
        }

        // GET: Index (Home page)
        public IActionResult Index()
        {
            return View();
        }
         // GET: /Electricity/UserDashboard
       public async Task<IActionResult> Electricity(int page = 1, int pageSize = 10,EStatus? status = null)
{
    // Get the current user's ID from the session
    var userId = HttpContext.Session.GetInt32("UserId");

    // Redirect to login if not logged in
    if (userId == null)
    {
        return RedirectToAction("Index", "Reg");
    }

    // Fetch electricity records associated with the logged-in user
   
    var query = _dbContext.electricities
        .Where(e => e.RegUserId == userId.Value) // Filter by user ID
        .Include(e => e.Reg);

    // Get total count for pagination
    var totalCount = await query.CountAsync();

    // Fetch records with pagination
    var electricities = await query
        .OrderBy(e => e.ElectricityId) // Adjust to the appropriate ID
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

   
    var totalRecords = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = totalPages;
    ViewBag.SelectedStatus = status;

    return View(electricities);
}


        // GET: /Water/UserDashboard
        [HttpGet]
         public async Task<IActionResult> Water(int page = 1, int pageSize = 10,WStatus? status = null)
{
    // Get the current user's ID from the session
    var userId = HttpContext.Session.GetInt32("UserId");

    // Redirect to login if not logged in
    if (userId == null)
    {
        return RedirectToAction("Index", "Reg");
    }

    // Fetch electricity records associated with the logged-in user
   
    var query = _dbContext.waters
        .Where(e => e.RegUserId == userId.Value) // Filter by user ID
        .Include(e => e.Reg);

    // Get total count for pagination
    var totalCount = await query.CountAsync();

    // Fetch records with pagination
    var waters = await query
        .OrderBy(e => e.WaterId) // Adjust to the appropriate ID
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

   
    var totalRecords = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

    ViewBag.CurrentPage = page;
    ViewBag.TotalPages = totalPages;
    ViewBag.SelectedStatus = status;

    return View(waters);
}
        // Example logout method to clear session
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index");
        }


        public IActionResult Pay(int waterId,decimal amount)
        {
            var waterBill = _dbContext.waters.Find(waterId);
            if (waterBill == null || (waterBill.status != WStatus.unpaid && waterBill.status != WStatus.overdue))
            {
                return NotFound();
            }

           // var totalCost = waterBill.WaterCost();
            var payment = CreatePayment(amount);
            return Redirect(payment.GetApprovalUrl());
        }

        private Payment CreatePayment(decimal amount)
        {
            var clientId = _configuration["PayPal:ClientId"];
            var clientSecret = _configuration["PayPal:ClientSecret"];
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
                        invoice_number = Guid.NewGuid().ToString(),
                        amount = new Amount
                        {
                            currency = "USD",
                            total = amount.ToString("F2")
                        }
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    cancel_url = Url.Action("Cancel", "Payment", null, Request.Scheme),
                    return_url = Url.Action("Success", "Payment", null, Request.Scheme)
                }
            };

            return payment.Create(apiContext);
        }

        public IActionResult Success(string paymentId, string token, string PayerID)
        {
            var apiContext = new APIContext(new OAuthTokenCredential(
                _configuration["PayPal:ClientId"],
                _configuration["PayPal:ClientSecret"]).GetAccessToken());

            var paymentExecution = new PaymentExecution { payer_id = PayerID };
            var payment = new Payment { id = paymentId };

            var executedPayment = payment.Execute(apiContext, paymentExecution);

            // Update water bill status to paid
            var waterBill = _dbContext.waters.Find(executedPayment.transactions.First().invoice_number);
            if (waterBill != null)
            {
                waterBill.status = WStatus.paid;
                _dbContext.Update(waterBill);
                _dbContext.SaveChanges();
            }

            return View("PaymentSuccess");
        }

        public IActionResult Cancel()
        {
            return View("PaymentCanceled");
        }
       
    }
}
