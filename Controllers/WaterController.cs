using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication9Municipal_Billing_System.Models;

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
        public async Task<IActionResult> Index()
        {
            var waterRecords = await _context.waters.Include(w => w.Reg).ToListAsync();
            return View(waterRecords);
        }

        // GET: Water/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Water/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Water water)
        {
            if (ModelState.IsValid)
            {
                water.Cost = water.WaterCost();
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
        public async Task<IActionResult> Edit(int id, Water water)
        {
            if (id != water.WaterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    water.Cost = water.WaterCost();
                    _context.Update(water);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WaterExists(water.WaterId))
                    {
                        return NotFound();
                    }
                    throw;
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

            var water = await _context.waters.Include(w => w.Reg).FirstOrDefaultAsync(m => m.WaterId == id);
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
            if (water != null)
            {
                _context.waters.Remove(water);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool WaterExists(int id)
        {
            return _context.waters.Any(e => e.WaterId == id);
        }
    }
}
