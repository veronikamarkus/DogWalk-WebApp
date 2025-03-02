using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.DAL.EF.Repositories;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProfilesController : Controller
    {
        private readonly AppDbContext _context;

        public ProfilesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Profiles
        // [AllowAnonymous] to rewrite authorization attribute
        public async Task<IActionResult> Index()
        {
            // to show only users entities, userId goes into GetAllAsync
            // var userId = Guid.Parse(_userManager.GetUserId(User)!);
            var res = await _context.Profiles
                .Include(p => p.AppUser)
                .ToListAsync();

            return View(res);
        }

        // GET: Admin/Profiles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // GET: Admin/Profiles/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AppUserId"] = new SelectList( await _context.Users.ToListAsync(), "Id", "Id");
            return View();
        }

        // POST: Admin/Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Profile profile) // Create([Bind("Description,Verified,CreatedAt,AppUserId,Id")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                
                profile.Id = Guid.NewGuid();
                _context.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["AppUserId"] = new SelectList(await _context.Users.ToListAsync(), "Id", "Id", profile.AppUserId);
            return View(profile);
        }

        // GET: Admin/Profiles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var profile = await _context.Profiles.FindAsync(id);
            
            if (profile == null || profile.Equals(default))
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(await _context.Users.ToListAsync(), "Id", "Id", profile.AppUserId);
            return View(profile);
        }

        // POST: Admin/Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Description,Verified,CreatedAt,AppUserId,Id")] Profile profile)
        {
            if (id != profile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.Id))
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
            ViewData["AppUserId"] = new SelectList(await _context.Users.ToListAsync(), "Id", "Id", profile.AppUserId);
            return View(profile);
        }

        // GET: Admin/Profiles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var profile = await _context.Profiles
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        // POST: Admin/Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            
            if (profile != null && !profile.Equals(default))
            {
                _context.Profiles.Remove(profile);
              
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private bool ProfileExists(Guid id)
        {
            return _context.Profiles.Any(e => e.Id == id);
        }
        
    }
}
