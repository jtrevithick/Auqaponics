using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auqaponics2.Data;
using Auqaponics2.Models;
using Microsoft.AspNetCore.Authorization;

namespace Auqaponics2.Controllers
{
    
    public class SpecsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpecsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Specs
        public async Task<IActionResult> Index()
        {
            return View(await _context.System.ToListAsync());
        }

        // GET: Specs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specs = await _context.System
                .FirstOrDefaultAsync(m => m.SpecsId == id);
            if (specs == null)
            {
                return NotFound();
            }

            return View(specs);
        }
        [Authorize(Roles = "Admin")]
        // GET: Specs/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        // POST: Specs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecsId,TankSize,Plants,Fish,WaterTemp,pH")] Specs specs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specs);
        }
        [Authorize(Roles = "Admin")]
        // GET: Specs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specs = await _context.System.FindAsync(id);
            if (specs == null)
            {
                return NotFound();
            }
            return View(specs);
        }
        [Authorize(Roles = "Admin")]
        // POST: Specs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpecsId,TankSize,Plants,Fish,WaterTemp,pH")] Specs specs)
        {
            if (id != specs.SpecsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecsExists(specs.SpecsId))
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
            return View(specs);
        }
        [Authorize(Roles = "Admin")]
        // GET: Specs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specs = await _context.System
                .FirstOrDefaultAsync(m => m.SpecsId == id);
            if (specs == null)
            {
                return NotFound();
            }

            return View(specs);
        }
        [Authorize(Roles = "Admin")]
        // POST: Specs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specs = await _context.System.FindAsync(id);
            _context.System.Remove(specs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecsExists(int id)
        {
            return _context.System.Any(e => e.SpecsId == id);
        }
    }
}
