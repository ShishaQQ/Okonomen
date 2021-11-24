using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Okonomen.Models;

namespace Okonomen.Controllers
{
    public class BudgetItemsController : Controller
    {
        private readonly OkonomenContext _context;
        public BudgetItemsController(OkonomenContext context)
        {
            _context = context;
        }

        // GET: BudgetItems
        public async Task<IActionResult> Index(Guid? id)
        {
            var okonomenContext = _context.BudgetItems.Include(b => b.Budget).Where(bi => bi.BudgetId == id);
            return View(await okonomenContext.ToListAsync());
        }

        // GET: BudgetItems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetItem = await _context.BudgetItems
                .Include(b => b.Budget)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (budgetItem == null)
            {
                return NotFound();
            }

            return View(budgetItem);
        }

        // GET: BudgetItems/Create
        public IActionResult Create(Guid? Id)
        {
            string userName = User.Identity.Name;

            ViewData["BudgetId"] = new SelectList(_context.Budgets.Where(b => b.User.UserName == userName), "Id", "Name");
            return View();
        }

        // POST: BudgetItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Number,BudgetId")] BudgetItem budgetItem)
        {

            if (ModelState.IsValid)
            {
                budgetItem.Id = Guid.NewGuid();
                _context.Add(budgetItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = budgetItem.BudgetId});
            }
            ViewData["BudgetId"] = new SelectList(_context.Budgets, "Id", "Name", budgetItem.BudgetId);
            return View(budgetItem);
        }

        // GET: BudgetItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetItem = await _context.BudgetItems.FindAsync(id);
            if (budgetItem == null)
            {
                return NotFound();
            }
            ViewData["BudgetId"] = new SelectList(_context.Budgets, "Id", "Name", budgetItem.BudgetId);
            return View(budgetItem);
        }

        // POST: BudgetItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Number,BudgetId")] BudgetItem budgetItem)
        {
            if (id != budgetItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budgetItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetItemExists(budgetItem.Id))
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
            ViewData["BudgetId"] = new SelectList(_context.Budgets, "Id", "Name", budgetItem.BudgetId);
            return View(budgetItem);
        }

        // GET: BudgetItems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetItem = await _context.BudgetItems
                .Include(b => b.Budget)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (budgetItem == null)
            {
                return NotFound();
            }

            return View(budgetItem);
        }

        // POST: BudgetItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var budgetItem = await _context.BudgetItems.FindAsync(id);
            _context.BudgetItems.Remove(budgetItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetItemExists(Guid id)
        {
            return _context.BudgetItems.Any(e => e.Id == id);
        }
    }
}
