using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: Water
public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, WStatus? status = null)
{
    var query = _context.waters.Include(w => w.Reg).AsQueryable();

    // Apply filtering based on the status
    if (status.HasValue)
    {
        query = query.Where(w => w.status == status.Value);
    }

    var waters = await query
        .OrderBy(w => w.WaterId) // Ensure a consistent order
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var totalRecords = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

    ViewBag.CurrentPage = pageNumber;
    ViewBag.TotalPages = totalPages;
    ViewBag.SelectedStatus = status;

    return View(waters);
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
            ViewData["RegUserId"] = new SelectList(_context.Regs, "UserId", "IdNumber");
            return View();
        }

        // POST: Water/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WaterId,Usage,DueDate,status,RegUserId")] Water water)
        {
            if (ModelState.IsValid)
            {
                //water.DueDate= water.DateTime.Now;
                water.Rate = 0.50m;
                water.Cost = water.WaterCost();
                _context.Add(water);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegUserId"] = new SelectList(_context.Regs, "UserId", "IdNumber", water.RegUserId);
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
            ViewData["RegUserId"] = new SelectList(_context.Regs, "UserId", "IdNumber", water.RegUserId);
            return View(water);
        }

        // POST: Water/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WaterId,Usage,RegUserId")] Water water)
        {
            if (id != water.WaterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    water.Rate = water.CalcRate();
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
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegUserId"] = new SelectList(_context.Regs, "UserId", "IdNumber", water.RegUserId);
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
            return _context.waters.Any(e => e.WaterId == id);
        }
    }
}