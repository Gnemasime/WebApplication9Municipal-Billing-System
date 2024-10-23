using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication9Municipal_Billing_System.Models;
using System.Threading.Tasks;
using System.Linq;

namespace WebApplication9Municipal_Billing_System.Controllers
{
    public class ElectricityController : Controller
    {
        private readonly DBContextClassReg _context;

        public ElectricityController(DBContextClassReg context)
        {
            _context = context;
        }

        // GET: Electricity
        public async Task<IActionResult> Index(EStatus? statusFilter)
        {
            var electricities = from w in _context.electricities.Include(w => w.Reg)
                         select w;

            if (statusFilter.HasValue)
            {
                electricities = electricities.Where(w => w.status == statusFilter);
            }
return View(await electricities.ToListAsync());
        }

        // GET: Electricity/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricity = await _context.electricities
                .Include(e => e.Reg)
                .FirstOrDefaultAsync(m => m.ElectricityId == id);
            if (electricity == null)
            {
                return NotFound();
            }

            return View(electricity);
        }

        // GET: Electricity/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Electricity/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Usage,Rate,DueDate,status,RegUserId")] Electricity electricity)
        {
            if (ModelState.IsValid)
            {
                electricity.Cost = electricity.Usage * electricity.Rate; // Calculate cost
                _context.Add(electricity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(electricity);
        }

        // GET: Electricity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricity = await _context.electricities.FindAsync(id);
            if (electricity == null)
            {
                return NotFound();
            }
            return View(electricity);
        }

        // POST: Electricity/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ElectricityId,Usage,Rate,DueDate,status,RegUserId")] Electricity electricity)
        {
            if (id != electricity.ElectricityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    electricity.Cost = electricity.Usage * electricity.Rate; // Recalculate cost
                    _context.Update(electricity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectricityExists(electricity.ElectricityId))
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
            return View(electricity);
        }

        // GET: Electricity/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var electricity = await _context.electricities
                .Include(e => e.Reg)
                .FirstOrDefaultAsync(m => m.ElectricityId == id);
            if (electricity == null)
            {
                return NotFound();
            }

            return View(electricity);
        }

        // POST: Electricity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var electricity = await _context.electricities.FindAsync(id);
            _context.electricities.Remove(electricity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectricityExists(int id)
        {
            return _context.electricities.Any(e => e.ElectricityId == id);
        }
    }
}
