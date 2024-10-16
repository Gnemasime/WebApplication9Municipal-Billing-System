using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication9Municipal_Billing_System.Models;

namespace WebApplication9Municipal_Billing_System.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DBContextClassReg _context;

        public DashboardController(DBContextClassReg context)
        {
            _context = context;
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
                return RedirectToAction("Login", "Account");
            }

            // Fetch water records associated with the logged-in user
            var waters = await _context.waters
                .Where(w => w.RegUserId == userId.Value) // Filter by user ID
                .Include(w => w.Reg)
                .ToListAsync();

            return View(waters);
        }

// Method to set the UserId in the session (e.g., after login)
        public IActionResult Login(int userId)
        {
            // Store the user ID in session
        
            return RedirectToAction("Water");
        }

        // Example logout method
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Reg");
        }
       

        // GET: /Water/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var water = await _context.waters
                .Where(w => w.WaterId == id && w.RegUserId == userId.Value) // Ensure user owns the record
                .FirstOrDefaultAsync();

            if (water == null)
            {
                return NotFound(); // Not found if the record does not belong to the user
            }
            return View(water);
        }

        
         

     
    }
}
