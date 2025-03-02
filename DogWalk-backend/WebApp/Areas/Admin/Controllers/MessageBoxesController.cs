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
    public class MessageBoxesController : Controller
    {
        private readonly AppDbContext _context;

        public MessageBoxesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/MessageBoxes
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.MessageBoxes.Include(m => m.UserInWalk);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/MessageBoxes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageBox = await _context.MessageBoxes
                .Include(m => m.UserInWalk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageBox == null)
            {
                return NotFound();
            }

            return View(messageBox);
        }

        // GET: Admin/MessageBoxes/Create
        public IActionResult Create()
        {
            ViewData["UserInWalkId"] = new SelectList(_context.UserInWalks, "Id", "Id");
            return View();
        }

        // POST: Admin/MessageBoxes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserInWalkId,Message,CreatedAt,Id")] MessageBox messageBox)
        {
            if (ModelState.IsValid)
            {
                messageBox.Id = Guid.NewGuid();
                _context.Add(messageBox);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserInWalkId"] = new SelectList(_context.UserInWalks, "Id", "Id", messageBox.UserInWalkId);
            return View(messageBox);
        }

        // GET: Admin/MessageBoxes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageBox = await _context.MessageBoxes.FindAsync(id);
            if (messageBox == null)
            {
                return NotFound();
            }
            ViewData["UserInWalkId"] = new SelectList(_context.UserInWalks, "Id", "Id", messageBox.UserInWalkId);
            return View(messageBox);
        }

        // POST: Admin/MessageBoxes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserInWalkId,Message,CreatedAt,Id")] MessageBox messageBox)
        {
            if (id != messageBox.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(messageBox);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageBoxExists(messageBox.Id))
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
            ViewData["UserInWalkId"] = new SelectList(_context.UserInWalks, "Id", "Id", messageBox.UserInWalkId);
            return View(messageBox);
        }

        // GET: Admin/MessageBoxes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageBox = await _context.MessageBoxes
                .Include(m => m.UserInWalk)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageBox == null)
            {
                return NotFound();
            }

            return View(messageBox);
        }

        // POST: Admin/MessageBoxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var messageBox = await _context.MessageBoxes.FindAsync(id);
            if (messageBox != null)
            {
                _context.MessageBoxes.Remove(messageBox);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageBoxExists(Guid id)
        {
            return _context.MessageBoxes.Any(e => e.Id == id);
        }
    }
}
