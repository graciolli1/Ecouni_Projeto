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
    public class DownloadAppsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DownloadAppsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DownloadApps
        public async Task<IActionResult> Index()
        {
            return View(await _context.DownloadApps.ToListAsync());
        }

        // GET: DownloadApps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var downloadApp = await _context.DownloadApps
                .FirstOrDefaultAsync(m => m.DownloadAppId == id);
            if (downloadApp == null)
            {
                return NotFound();
            }

            return View(downloadApp);
        }

        // GET: DownloadApps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DownloadApps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DownloadAppId")] DownloadApp downloadApp)
        {
            if (ModelState.IsValid)
            {
                _context.Add(downloadApp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(downloadApp);
        }

        // GET: DownloadApps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var downloadApp = await _context.DownloadApps.FindAsync(id);
            if (downloadApp == null)
            {
                return NotFound();
            }
            return View(downloadApp);
        }

        // POST: DownloadApps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DownloadAppId")] DownloadApp downloadApp)
        {
            if (id != downloadApp.DownloadAppId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(downloadApp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DownloadAppExists(downloadApp.DownloadAppId))
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
            return View(downloadApp);
        }

        // GET: DownloadApps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var downloadApp = await _context.DownloadApps
                .FirstOrDefaultAsync(m => m.DownloadAppId == id);
            if (downloadApp == null)
            {
                return NotFound();
            }

            return View(downloadApp);
        }

        // POST: DownloadApps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var downloadApp = await _context.DownloadApps.FindAsync(id);
            if (downloadApp != null)
            {
                _context.DownloadApps.Remove(downloadApp);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DownloadAppExists(int id)
        {
            return _context.DownloadApps.Any(e => e.DownloadAppId == id);
        }
    }
}
