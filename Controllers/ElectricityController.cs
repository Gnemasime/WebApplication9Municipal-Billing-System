using Microsoft.AspNetCore.Mvc;  
using WebApplication9Municipal_Billing_System.Models;  
using Microsoft.EntityFrameworkCore;  
  
namespace WebApplication9Municipal_Billing_System.Controllers  
{  
   public class ElectricityController : Controller  
   {  
      private readonly DBContextClassReg _db;  
  
      public ElectricityController(DBContextClassReg db)  
      {  
        _db = db;  
      }  
  
      // GET: Electricity  
      public IActionResult Index()  
      {  
        return View(_db.electricities.ToList());  
      }  
  
      // GET: Electricity/Details/5  
      public IActionResult Details(int id)  
      {  
        Electricity electricity = _db.electricities.Find(id);  
        if (electricity == null)  
        {  
           return NotFound();  
        }  
        return View(electricity);  
      }  
  
      // GET: Electricity/Create  
      public IActionResult Create()  
      {  
        ViewBag.Users = _db.Regs.ToList();  
        return View();  
      }  
  
      // POST: Electricity/Create  
      [HttpPost]  
      public IActionResult Create([Bind("ElectricityId,Usage,DueDate,status")] Electricity electricity)  
      {  
        if (ModelState.IsValid)  
        {  
    
           _db.electricities.Add(electricity);  
           _db.SaveChanges();  
           return RedirectToAction("Index");  
        }  
  
        return View(electricity);  
      }  
  
      // GET: Electricity/Edit/5  
      public IActionResult Edit(int id)  
      {  
        Electricity electricity = _db.electricities.Find(id);  
        // ViewBag.Users = _db.Regs.ToList();  
        if (electricity == null)  
        {  
           return NotFound();  
        }  
        return View(electricity);  
      }  
  
      // POST: Electricity/Edit/5  
      [HttpPost]  
      public IActionResult Edit(Electricity electricity)  
      {  
        if (ModelState.IsValid)  
        {  
           _db.Entry(electricity).State = EntityState.Modified;  
           _db.SaveChanges();  
           return RedirectToAction("Index");  
        }  
        return View(electricity);  
      }  
  
      // GET: Electricity/Delete/5  
      public IActionResult Delete(int id)  
      {  
        Electricity electricity = _db.electricities.Find(id);  
        if (electricity == null)  
        {  
           return NotFound();  
        }  
        return View(electricity);  
      }  
  
      // POST: Electricity/Delete/5  
      [HttpPost, ActionName("Delete")]  
      public IActionResult DeleteConfirmed(int id)  
      {  
        Electricity electricity = _db.electricities.Find(id);  
        _db.electricities.Remove(electricity);  
        _db.SaveChanges();  
        return RedirectToAction("Index");  
      }  
   }  
}
