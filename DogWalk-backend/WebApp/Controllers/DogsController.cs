using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.BLL;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.Controllers;

namespace WebApp.Controllers
{
    
    [Authorize]
    public class DogsController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<AppUser> _userManager;

        public DogsController(IAppBLL bll, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _userManager = userManager;
        }

        // GET: Admin/Dogs
        public async Task<IActionResult> Index()
        {
            // TO see only his objects
            // var userId = Guid.Parse(_userManager.GetUserId(User)); 
            //
            // return View(await _uow.Dogs.GetAllAsync(userId));

            var res = await _bll.Dogs.GetAllSortedAsync(
                Guid.Parse(_userManager.GetUserId(User))
            );
            
            
            return View(res);
        }

        // GET: Admin/Dogs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _bll.Dogs
                .FirstOrDefaultAsync(id.Value);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: Admin/Dogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Dogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DogName,Age,Breed,Description,Id")] App.BLL.DTO.Dog dog)
        {
            if (ModelState.IsValid)
            {
                dog.Id = Guid.NewGuid();
                _bll.Dogs.Add(dog);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dog);
        }

        // GET: Admin/Dogs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _bll.Dogs.FirstOrDefaultAsync(id.Value);
            if (dog == null)
            {
                return NotFound();
            }
            return View(dog);
        }

        // POST: Admin/Dogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DogName,Age,Breed,Description,Id")] App.BLL.DTO.Dog dog)
        {
            if (id != dog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.Dogs.Update(dog);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogExists(dog.Id))
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
            return View(dog);
        }

        // GET: Admin/Dogs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _bll.Dogs
                .FirstOrDefaultAsync(id.Value);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: Admin/Dogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var dog = await _bll.Dogs.FirstOrDefaultAsync(id);
            if (dog != null)
            {
                _bll.Dogs.Remove(dog);
            }

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DogExists(Guid id)
        {
            return _bll.Dogs.Exists(id);
        }
    }
}