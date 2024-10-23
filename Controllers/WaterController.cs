using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication9Municipal_Billing_System.Models;
using System.Threading.Tasks;
using System.Linq;

namespace WebApplication9Municipal_Billing_System.Controllers
{
    public class WaterController : Controller
    {
        private readonly DBContextClassReg _context;

        public WaterController(DBContextClassReg context)
        {
            _context = context;
        }

        // GET: Water
        // GET: Water
        public async Task<IActionResult> Index(WStatus? statusFilter)
        {
            var waters = from w in _context.waters.Include(w => w.Reg)
                         select w;

            if (statusFilter.HasValue)
            {
                waters = waters.Where(w => w.status == statusFilter);
            }
return View(await waters.ToListAsync());
        }
            

        // GET: Water/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var water = await _context.waters
                .Include(w => w.Reg)
                .FirstOrDefaultAsync(m => m.WaterId == id);
            if (water == null)
            {
                return NotFound();
            }

            return View(water);
        }

        // GET: Water/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Water/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Usage,Rate,DueDate,status,RegUserId")] Water water)
        {
            if (ModelState.IsValid)
            {
                water.Cost = water.WaterCost(); // Calculate cost
                _context.Add(water);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(water);
        }

        // GET: Water/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var water = await _context.waters.FindAsync(id);
            if (water == null)
            {
                return NotFound();
            }
            return View(water);
        }

        // POST: Water/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WaterId,Usage,Rate,DueDate,status,RegUserId")] Water water)
        {
            if (id != water.WaterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    water.Cost = water.WaterCost(); // Recalculate cost
                    _context.Update(water);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WaterExists(water.WaterId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(water);
        }

        // GET: Water/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var water = await _context.waters
                .Include(w => w.Reg)
                .FirstOrDefaultAsync(m => m.WaterId == id);
            if (water == null)
            {
                return NotFound();
            }

            return View(water);
        }

        // POST: Water/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var water = await _context.waters.FindAsync(id);
            _context.waters.Remove(water);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WaterExists(int id)
        {
            return _context.waters.Any(w => w.WaterId == id);
        }
    }
}
