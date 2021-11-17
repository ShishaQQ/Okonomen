using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Okonomen.Models;

namespace Okonomen.Controllers
{
    [Authorize("RequireAuthenticatedUser")]
    public class BudgetsController : Controller
    {
        private readonly OkonomenContext _context;
        private readonly Data.ApplicationDbContext _applicationDbContext;


        public BudgetsController(OkonomenContext context,
        Data.ApplicationDbContext applicationDbContext)
        {
            _context = context;
            _applicationDbContext = applicationDbContext;
        }

        // GET: Budgets
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            //Hvis Admin, vis alle budgetter for alle brugere
            if (User.IsInRole("Admin"))
            {
                var item = _context.BudgetItems;
                var okonomenContext = _context.Budgets.Include(b => b.User).Include(b => b.BudgetItems);
                return View(await okonomenContext.ToListAsync());
            }
            else
            {
                //Vis kun budgetter der tilhøre brugeren
                var item = _context.BudgetItems;
                var okonomenContext = _context.Budgets.Where(b => b.User.UserName == userName).Include(b => b.User).Include(b => b.BudgetItems);
                return View(await okonomenContext.ToListAsync());
            }
        }

        // GET: Budgets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budgets
                .Include(b => b.User)
                .Include(b => b.BudgetItems)
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewData["Totalvalue"] = CalculateBudgetItems(budget.BudgetItems);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // GET: Budgets/Create
        public IActionResult Create()
        {
            string userName = User.Identity.Name;
            if (User.IsInRole("Admin"))
            {
                //Admin kan vælge imellem alle brugere
                //Dette er kun til at vise at man kan
                ViewData["Admin"] = true;
                ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            }
            else
            {
                ViewData["UserId"] = new SelectList(_context.AspNetUsers.Where(u => u.UserName == userName).Select(u => u.Id));
            }
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserId")] Budget budget)
        {
            budget.Id = Guid.NewGuid();
            if (ModelState.IsValid)
            {
                //budget.Id = Guid.NewGuid();
                _context.Add(budget);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", budget.UserId);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budgets
                .Include(b => b.User)
                .Include(b => b.BudgetItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (budget == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", budget.UserId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,UserId")] Budget budget)
        {
            if (id != budget.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetExists(budget.Id))
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
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", budget.UserId);
            ViewData["BudgetItems"] = new SelectList(_context.BudgetItems, "Id", "budgetId", budget.BudgetItems);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budget = await _context.Budgets
                .Include(b => b.User)
                .Include(b => b.BudgetItems)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var budget = await _context.Budgets.Include(b => b.BudgetItems).FirstOrDefaultAsync(m => m.Id == id);
            if (budget.BudgetItems.Any())
            {
                foreach (var item in budget.BudgetItems)
                {
                   _context.BudgetItems.Remove(item);
                }
               
            }
            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetExists(Guid id)
        {
            return _context.Budgets.Any(e => e.Id == id);
        }
        public decimal? CalculateBudgetItems(ICollection<BudgetItem> items)
        {
            decimal? TotalVal = 0;
            foreach (var item in items)
            {
               TotalVal += item.Number;
            }
            return TotalVal;
        }
    }
}
