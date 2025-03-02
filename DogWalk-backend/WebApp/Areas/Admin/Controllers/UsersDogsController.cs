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
    public class UsersDogsController : Controller
    {
        private readonly AppDbContext _context;

        public UsersDogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/UsersDogs
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UsersDogs.Include(u => u.AppUser).Include(u => u.Dog);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/UsersDogs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersDog = await _context.UsersDogs
                .Include(u => u.AppUser)
                .Include(u => u.Dog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersDog == null)
            {
                return NotFound();
            }

            return View(usersDog);
        }

        // GET: Admin/UsersDogs/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Id");
            return View();
        }

        // POST: Admin/UsersDogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DogId,AppUserId,Id")] UsersDog usersDog)
        {
            if (ModelState.IsValid)
            {
                usersDog.Id = Guid.NewGuid();
                _context.Add(usersDog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", usersDog.AppUserId);
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Id", usersDog.DogId);
            return View(usersDog);
        }

        // GET: Admin/UsersDogs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersDog = await _context.UsersDogs.FindAsync(id);
            if (usersDog == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", usersDog.AppUserId);
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Breed", usersDog.DogId);
            return View(usersDog);
        }

        // POST: Admin/UsersDogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DogId,AppUserId,Id")] UsersDog usersDog)
        {
            if (id != usersDog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersDog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersDogExists(usersDog.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", usersDog.AppUserId);
            ViewData["DogId"] = new SelectList(_context.Dogs, "Id", "Breed", usersDog.DogId);
            return View(usersDog);
        }

        // GET: Admin/UsersDogs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersDog = await _context.UsersDogs
                .Include(u => u.AppUser)
                .Include(u => u.Dog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersDog == null)
            {
                return NotFound();
            }

            return View(usersDog);
        }

        // POST: Admin/UsersDogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var usersDog = await _context.UsersDogs.FindAsync(id);
            if (usersDog != null)
            {
                _context.UsersDogs.Remove(usersDog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersDogExists(Guid id)
        {
            return _context.UsersDogs.Any(e => e.Id == id);
        }
    }
}
