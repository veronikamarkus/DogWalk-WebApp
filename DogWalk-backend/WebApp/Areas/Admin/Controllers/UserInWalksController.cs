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
    public class UserInWalksController : Controller
    {
        private readonly AppDbContext _context;

        public UserInWalksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/UserInWalks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserInWalks.Include(u => u.AppUser).Include(u => u.Walk);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/UserInWalks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInWalk = await _context.UserInWalks
                .Include(u => u.AppUser)
                .Include(u => u.Walk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInWalk == null)
            {
                return NotFound();
            }

            return View(userInWalk);
        }

        // GET: Admin/UserInWalks/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description");
            return View();
        }

        // POST: Admin/UserInWalks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WalkId,AppUserId,Id")] UserInWalk userInWalk)
        {
            if (ModelState.IsValid)
            {
                userInWalk.Id = Guid.NewGuid();
                _context.Add(userInWalk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", userInWalk.AppUserId);
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description", userInWalk.WalkId);
            return View(userInWalk);
        }

        // GET: Admin/UserInWalks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInWalk = await _context.UserInWalks.FindAsync(id);
            if (userInWalk == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", userInWalk.AppUserId);
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description", userInWalk.WalkId);
            return View(userInWalk);
        }

        // POST: Admin/UserInWalks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("WalkId,AppUserId,Id")] UserInWalk userInWalk)
        {
            if (id != userInWalk.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInWalk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInWalkExists(userInWalk.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", userInWalk.AppUserId);
            ViewData["WalkId"] = new SelectList(_context.Walks, "Id", "Description", userInWalk.WalkId);
            return View(userInWalk);
        }

        // GET: Admin/UserInWalks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInWalk = await _context.UserInWalks
                .Include(u => u.AppUser)
                .Include(u => u.Walk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInWalk == null)
            {
                return NotFound();
            }

            return View(userInWalk);
        }

        // POST: Admin/UserInWalks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userInWalk = await _context.UserInWalks.FindAsync(id);
            if (userInWalk != null)
            {
                _context.UserInWalks.Remove(userInWalk);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInWalkExists(Guid id)
        {
            return _context.UserInWalks.Any(e => e.Id == id);
        }
    }
}
