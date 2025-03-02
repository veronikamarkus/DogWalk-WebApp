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
    public class DogInWalksController : Controller
    {
        private readonly AppDbContext _context;

        public DogInWalksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DogInWalks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.DogInWalks.Include(d => d.Dog).Include(d => d.Walk);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/DogInWalks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogInWalk = await _context.DogInWalks
                .Include(d => d.Dog)
                .Include(d => d.Walk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dogInWalk == null)
            {
                return NotFound();
            }

            return View(dogInWalk);
        }

        // GET: Admin/DogInWalks/Create
        public IActionResult Create()
        {
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Breed");
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description");
            return View();
        }

        // POST: Admin/DogInWalks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WalkId,DogId,Id")] DogInWalk dogInWalk)
        {
            if (ModelState.IsValid)
            {
                dogInWalk.Id = Guid.NewGuid();
                _context.Add(dogInWalk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Breed", dogInWalk.DogId);
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description", dogInWalk.WalkId);
            return View(dogInWalk);
        }

        // GET: Admin/DogInWalks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogInWalk = await _context.DogInWalks.FindAsync(id);
            if (dogInWalk == null)
            {
                return NotFound();
            }
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Breed", dogInWalk.DogId);
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description", dogInWalk.WalkId);
            return View(dogInWalk);
        }

        // POST: Admin/DogInWalks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("WalkId,DogId,Id")] DogInWalk dogInWalk)
        {
            if (id != dogInWalk.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dogInWalk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogInWalkExists(dogInWalk.Id))
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
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Breed", dogInWalk.DogId);
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description", dogInWalk.WalkId);
            return View(dogInWalk);
        }

        // GET: Admin/DogInWalks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogInWalk = await _context.DogInWalks
                .Include(d => d.Dog)
                .Include(d => d.Walk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dogInWalk == null)
            {
                return NotFound();
            }

            return View(dogInWalk);
        }

        // POST: Admin/DogInWalks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var dogInWalk = await _context.DogInWalks.FindAsync(id);
            if (dogInWalk != null)
            {
                _context.DogInWalks.Remove(dogInWalk);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DogInWalkExists(Guid id)
        {
            return _context.DogInWalks.Any(e => e.Id == id);
        }
    }
}
