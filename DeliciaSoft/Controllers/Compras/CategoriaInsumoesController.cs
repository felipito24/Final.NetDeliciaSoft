using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliciaSoft.Models;
using Microsoft.AspNetCore.Authorization;

namespace DeliciaSoft.Controllers.Compras
{
    [Authorize] // Agregar este atributo para requerir autenticación para todo el controlador
    public class CategoriaInsumoesController : Controller
    {
        private readonly DeliciaSoftContext _context;

        public CategoriaInsumoesController(DeliciaSoftContext context)
        {
            _context = context;
        }

        // GET: CategoriaInsumoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoriaInsumos.ToListAsync());
        }

        // GET: CategoriaInsumoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaInsumo = await _context.CategoriaInsumos
                .FirstOrDefaultAsync(m => m.IdCategoriaInsumos == id);
            if (categoriaInsumo == null)
            {
                return NotFound();
            }

            return View(categoriaInsumo);
        }

        // GET: CategoriaInsumoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaInsumoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoriaInsumos,NombreCategoria,Descripcion,FechaRegistro,CantidadInsumos,Estado")] CategoriaInsumo categoriaInsumo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoriaInsumo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaInsumo);
        }

        // GET: CategoriaInsumoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaInsumo = await _context.CategoriaInsumos.FindAsync(id);
            if (categoriaInsumo == null)
            {
                return NotFound();
            }
            return View(categoriaInsumo);
        }

        // POST: CategoriaInsumoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoriaInsumos,NombreCategoria,Descripcion,FechaRegistro,CantidadInsumos,Estado")] CategoriaInsumo categoriaInsumo)
        {
            if (id != categoriaInsumo.IdCategoriaInsumos)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriaInsumo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaInsumoExists(categoriaInsumo.IdCategoriaInsumos))
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
            return View(categoriaInsumo);
        }

        // GET: CategoriaInsumoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaInsumo = await _context.CategoriaInsumos
                .FirstOrDefaultAsync(m => m.IdCategoriaInsumos == id);
            if (categoriaInsumo == null)
            {
                return NotFound();
            }

            return View(categoriaInsumo);
        }

        // POST: CategoriaInsumoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoriaInsumo = await _context.CategoriaInsumos.FindAsync(id);
            if (categoriaInsumo != null)
            {
                _context.CategoriaInsumos.Remove(categoriaInsumo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaInsumoExists(int id)
        {
            return _context.CategoriaInsumos.Any(e => e.IdCategoriaInsumos == id);
        }

        public IActionResult CambiarEstado(int id, bool nuevoEstado)
        {
            var catInsumos = _context.CategoriaInsumos.FirstOrDefault(i => i.IdCategoriaInsumos == id);
            if (catInsumos != null)
            {
                catInsumos.Estado = nuevoEstado;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
