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
    public class WalkOffersController : Controller
    {
        private readonly AppDbContext _context;

        public WalkOffersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/WalkOffers
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.WalkOffers.Include(w => w.AppUser).Include(w => w.Walk);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/WalkOffers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walkOffer = await _context.WalkOffers
                .Include(w => w.AppUser)
                .Include(w => w.Walk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (walkOffer == null)
            {
                return NotFound();
            }

            return View(walkOffer);
        }

        // GET: Admin/WalkOffers/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description");
            return View();
        }

        // POST: Admin/WalkOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,WalkId,Comment,Accepted,Id")] WalkOffer walkOffer)
        {
            if (ModelState.IsValid)
            {
                walkOffer.Id = Guid.NewGuid();
                _context.Add(walkOffer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", walkOffer.AppUserId);
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description", walkOffer.WalkId);
            return View(walkOffer);
        }

        // GET: Admin/WalkOffers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walkOffer = await _context.WalkOffers.FindAsync(id);
            if (walkOffer == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", walkOffer.AppUserId);
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description", walkOffer.WalkId);
            return View(walkOffer);
        }

        // POST: Admin/WalkOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AppUserId,WalkId,Comment,Accepted,Id")] WalkOffer walkOffer)
        {
            if (id != walkOffer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(walkOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalkOfferExists(walkOffer.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", walkOffer.AppUserId);
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description", walkOffer.WalkId);
            return View(walkOffer);
        }

        // GET: Admin/WalkOffers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walkOffer = await _context.WalkOffers
                .Include(w => w.AppUser)
                .Include(w => w.Walk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (walkOffer == null)
            {
                return NotFound();
            }

            return View(walkOffer);
        }

        // POST: Admin/WalkOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var walkOffer = await _context.WalkOffers.FindAsync(id);
            if (walkOffer != null)
            {
                _context.WalkOffers.Remove(walkOffer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WalkOfferExists(Guid id)
        {
            return _context.WalkOffers.Any(e => e.Id == id);
        }
    }
}
