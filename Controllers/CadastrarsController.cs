using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecouni_Projeto.Data;
using Ecouni_Projeto.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ecouni_Projeto.Controllers
{
    //[Authorize]
    public class CadastrarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CadastrarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cadastrars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cadastrar.ToListAsync());
        }

        // GET: Cadastrars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cadastrar = await _context.Cadastrar
                .FirstOrDefaultAsync(m => m.Cadastrarid == id);
            if (cadastrar == null)
            {
                return NotFound();
            }

            return View(cadastrar);
        }

        // GET: Cadastrars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cadastrars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cadastrarid,Nome,Email,Telefone,Senha,ConfirmarSenha")] Cadastrar cadastrar)
        {
            if (ModelState.IsValid)
            {
                // Verifica se o email já existe
                var emailExists = _context.Cadastrar.Any(u => u.Email == cadastrar.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Este email já está sendo utilizado.");
                    return View(cadastrar);
                }

                // Adiciona o novo usuário
                _context.Add(cadastrar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cadastrar);
        }

        // GET: Cadastrars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cadastrar = await _context.Cadastrar.FindAsync(id);
            if (cadastrar == null)
            {
                return NotFound();
            }
            return View(cadastrar);
        }

        // POST: Cadastrars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cadastrar cadastrar)
        {
            if (id != cadastrar.Cadastrarid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cadastrar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CadastrarExists(cadastrar.Cadastrarid))
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
            return View(cadastrar);
        }

        // GET: Cadastrars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cadastrar = await _context.Cadastrar
                .FirstOrDefaultAsync(m => m.Cadastrarid == id);
            if (cadastrar == null)
            {
                return NotFound();
            }

            return View(cadastrar);
        }

        // POST: Cadastrars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cadastrar = await _context.Cadastrar.FindAsync(id);
            if (cadastrar != null)
            {
                _context.Cadastrar.Remove(cadastrar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CadastrarExists(int id)
        {
            return _context.Cadastrar.Any(e => e.Cadastrarid == id);
        }
    }
}
