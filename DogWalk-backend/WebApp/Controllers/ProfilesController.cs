using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProfilesController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProfilesController(AppDbContext context, UserManager<AppUser> userManager, IAppBLL bll)
        {
            _userManager = userManager;
            _bll = bll;
            _context = context; // how to access identity users with _bbl?
        }

        // GET: Profiles
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(_userManager.GetUserId(User));

            var profile = await _bll.Profiles.FindProfileByUserId(userId);
            
            if (profile == null)
            {
                return NotFound();
            }
            
            return View(profile);
        }

        // GET: Profiles/Details/5
    //     public async Task<IActionResult> Details(Guid? id)
    //     {
    //         if (id == null)
    //         {
    //             return NotFound();
    //         }
    //
    //         var profile = await _context.Profiles
    //             .Include(p => p.AppUser)
    //             .FirstOrDefaultAsync(m => m.Id == id);
    //         if (profile == null)
    //         {
    //             return NotFound();
    //         }
    //
    //         return View(profile);
    //     }
    //
    //     // GET: Profiles/Create
    //     public IActionResult Create()
    //     {
    //         ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName");
    //         return View();
    //     }
    //
    //     // POST: Profiles/Create
    //     // To protect from overposting attacks, enable the specific properties you want to bind to.
    //     // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //     [HttpPost]
    //     [ValidateAntiForgeryToken]
    //     public async Task<IActionResult> Create([Bind("Description,Verified,CreatedAt,AppUserId,Id")] Profile profile)
    //     {
    //         if (ModelState.IsValid)
    //         {
    //             profile.Id = Guid.NewGuid();
    //             _context.Add(profile);
    //             await _context.SaveChangesAsync();
    //             return RedirectToAction(nameof(Index));
    //         }
    //         ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", profile.AppUserId);
    //         return View(profile);
    //     }
    //
    //     // GET: Profiles/Edit/5
    //     public async Task<IActionResult> Edit(Guid? id)
    //     {
    //         if (id == null)
    //         {
    //             return NotFound();
    //         }
    //
    //         var profile = await _context.Profiles.FindAsync(id);
    //         if (profile == null)
    //         {
    //             return NotFound();
    //         }
    //         ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", profile.AppUserId);
    //         return View(profile);
    //     }
    //
    //     // POST: Profiles/Edit/5
    //     // To protect from overposting attacks, enable the specific properties you want to bind to.
    //     // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //     [HttpPost]
    //     [ValidateAntiForgeryToken]
    //     public async Task<IActionResult> Edit(Guid id, [Bind("Description,Verified,CreatedAt,AppUserId,Id")] Profile profile)
    //     {
    //         if (id != profile.Id)
    //         {
    //             return NotFound();
    //         }
    //
    //         if (ModelState.IsValid)
    //         {
    //             try
    //             {
    //                 _context.Update(profile);
    //                 await _context.SaveChangesAsync();
    //             }
    //             catch (DbUpdateConcurrencyException)
    //             {
    //                 if (!ProfileExists(profile.Id))
    //                 {
    //                     return NotFound();
    //                 }
    //                 else
    //                 {
    //                     throw;
    //                 }
    //             }
    //             return RedirectToAction(nameof(Index));
    //         }
    //         ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", profile.AppUserId);
    //         return View(profile);
    //     }
    //
    //     // GET: Profiles/Delete/5
    //     public async Task<IActionResult> Delete(Guid? id)
    //     {
    //         if (id == null)
    //         {
    //             return NotFound();
    //         }
    //
    //         var profile = await _context.Profiles
    //             .Include(p => p.AppUser)
    //             .FirstOrDefaultAsync(m => m.Id == id);
    //         if (profile == null)
    //         {
    //             return NotFound();
    //         }
    //
    //         return View(profile);
    //     }
    //
    //     // POST: Profiles/Delete/5
    //     [HttpPost, ActionName("Delete")]
    //     [ValidateAntiForgeryToken]
    //     public async Task<IActionResult> DeleteConfirmed(Guid id)
    //     {
    //         var profile = await _context.Profiles.FindAsync(id);
    //         if (profile != null)
    //         {
    //             _context.Profiles.Remove(profile);
    //         }
    //
    //         await _context.SaveChangesAsync();
    //         return RedirectToAction(nameof(Index));
    //     }
    //
    //     private bool ProfileExists(Guid id)
    //     {
    //         return _context.Profiles.Any(e => e.Id == id);
    //     }
      }
}
