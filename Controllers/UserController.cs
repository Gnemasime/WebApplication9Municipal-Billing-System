using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication9Municipal_Billing_System.Models;

namespace WebApplication9Municipal_Billing_System.Controllers
{
    public class UserController : Controller
    {
        private readonly DBContextClassReg _context;

        public UserController(DBContextClassReg context)
        {
            _context = context;
        }

        // GET: Reg
        public async Task<IActionResult> Index()
        {
            return View(await _context.Regs.ToListAsync());
        }

        // GET: Reg/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reg = await _context.Regs
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (reg == null)
            {
                return NotFound();
            }

            return View(reg);
        }

        // GET: Reg/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reg/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Surname,Email,Password,ConfirmPassword,IdNumber")] Reg reg)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reg);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reg);
        }

        // GET: Reg/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reg = await _context.Regs.FindAsync(id);
            if (reg == null)
            {
                return NotFound();
            }
            return View(reg);
        }

        // POST: Reg/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Surname,Email,Password,ConfirmPassword,IdNumber")] Reg reg)
        {
            if (id != reg.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reg);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegExists(reg.UserId))
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
            return View(reg);
        }

        // GET: Reg/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reg = await _context.Regs
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (reg == null)
            {
                return NotFound();
            }

            return View(reg);
        }

        // POST: Reg/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reg = await _context.Regs.FindAsync(id);
            _context.Regs.Remove(reg);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegExists(int id)
        {
            return _context.Regs.Any(e => e.UserId == id);
        }
    }
}
