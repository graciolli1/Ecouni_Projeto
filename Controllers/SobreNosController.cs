using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecouni_Projeto.Data;
using Ecouni_Projeto.Models;

namespace Ecouni_Projeto.Controllers
{
    public class SobreNosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SobreNosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SobreNos
        public async Task<IActionResult> Index()
        {
            return View(await _context.SobreNos.ToListAsync());
        }

        // GET: SobreNos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sobreNos = await _context.SobreNos
                .FirstOrDefaultAsync(m => m.SobreNosId == id);
            if (sobreNos == null)
            {
                return NotFound();
            }

            return View(sobreNos);
        }

        // GET: SobreNos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SobreNos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SobreNosId")] SobreNos sobreNos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sobreNos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sobreNos);
        }

        // GET: SobreNos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sobreNos = await _context.SobreNos.FindAsync(id);
            if (sobreNos == null)
            {
                return NotFound();
            }
            return View(sobreNos);
        }

        // POST: SobreNos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SobreNosId")] SobreNos sobreNos)
        {
            if (id != sobreNos.SobreNosId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sobreNos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SobreNosExists(sobreNos.SobreNosId))
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
            return View(sobreNos);
        }

        // GET: SobreNos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sobreNos = await _context.SobreNos
                .FirstOrDefaultAsync(m => m.SobreNosId == id);
            if (sobreNos == null)
            {
                return NotFound();
            }

            return View(sobreNos);
        }

        // POST: SobreNos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sobreNos = await _context.SobreNos.FindAsync(id);
            if (sobreNos != null)
            {
                _context.SobreNos.Remove(sobreNos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SobreNosExists(int id)
        {
            return _context.SobreNos.Any(e => e.SobreNosId == id);
        }
    }
}
