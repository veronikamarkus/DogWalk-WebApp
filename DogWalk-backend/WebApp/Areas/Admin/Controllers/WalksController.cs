using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class WalksController : Controller
    {
        private readonly AppDbContext _context;

        public WalksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Walks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Walks.Include(w => w.Location);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Walks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walk = await _context.Walks
                .Include(w => w.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (walk == null)
            {
                return NotFound();
            }

            return View(walk);
        }

        // GET: Admin/Walks/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "City");
            return View();
        }

        // POST: Admin/Walks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationId,TargetStartingTime,TargetDurationMinutes,Price,StartedAt,FinishedAt,Closed,Description,Id")] Walk walk)
        {
            if (ModelState.IsValid)
            {
                walk.Id = Guid.NewGuid();
                _context.Add(walk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "City", walk.LocationId);
            return View(walk);
        }

        // GET: Admin/Walks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walk = await _context.Walks.FindAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "City", walk.LocationId);
            return View(walk);
        }

        // POST: Admin/Walks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LocationId,TargetStartingTime,TargetDurationMinutes,Price,StartedAt,FinishedAt,Closed,Description,Id")] Walk walk)
        {
            if (id != walk.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(walk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalkExists(walk.Id))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "City", walk.LocationId);
            return View(walk);
        }

        // GET: Admin/Walks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walk = await _context.Walks
                .Include(w => w.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (walk == null)
            {
                return NotFound();
            }

            return View(walk);
        }

        // POST: Admin/Walks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var walk = await _context.Walks.FindAsync(id);
            if (walk != null)
            {
                _context.Walks.Remove(walk);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WalkExists(Guid id)
        {
            return _context.Walks.Any(e => e.Id == id);
        }
    }
}
