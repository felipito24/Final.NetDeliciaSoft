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
    public class InsumoesController : Controller
    {
        private readonly DeliciaSoftContext _context;

        public InsumoesController(DeliciaSoftContext context)
        {
            _context = context;
        }

        // GET: Insumoes
        public async Task<IActionResult> Index()
        {
            var deliciaSoftContext = _context.Insumos.Include(i => i.IdCategoriaInsumosNavigation);
            return View(await deliciaSoftContext.ToListAsync());
        }

        // GET: Insumoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insumo = await _context.Insumos
                .Include(i => i.IdCategoriaInsumosNavigation)
                .FirstOrDefaultAsync(m => m.IdInsumo == id);
            if (insumo == null)
            {
                return NotFound();
            }

            return View(insumo);
        }

        

        // GET: Insumoes/Create
        public IActionResult Create()
        {
            ViewData["IdCategoriaInsumos"] = new SelectList(_context.CategoriaInsumos, "IdCategoriaInsumos", "IdCategoriaInsumos");
            return View();
        }

        // POST: Insumoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdInsumo,NombreInsumo,IdCategoriaInsumos,Cantidad,UnidadMedida,Marca,Imagen,Estado")] Insumo insumo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(insumo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoriaInsumos"] = new SelectList(_context.CategoriaInsumos, "IdCategoriaInsumos", "IdCategoriaInsumos", insumo.IdCategoriaInsumos);
            return View(insumo);
        }

        // GET: Insumoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insumo = await _context.Insumos.FindAsync(id);
            if (insumo == null)
            {
                return NotFound();
            }
            ViewData["IdCategoriaInsumos"] = new SelectList(_context.CategoriaInsumos, "IdCategoriaInsumos", "IdCategoriaInsumos", insumo.IdCategoriaInsumos);
            return View(insumo);
        }

        // POST: Insumoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdInsumo,NombreInsumo,IdCategoriaInsumos,Cantidad,UnidadMedida,Marca,Imagen,Estado")] Insumo insumo)
        {
            if (id != insumo.IdInsumo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insumo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsumoExists(insumo.IdInsumo))
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
            ViewData["IdCategoriaInsumos"] = new SelectList(_context.CategoriaInsumos, "IdCategoriaInsumos", "IdCategoriaInsumos", insumo.IdCategoriaInsumos);
            return View(insumo);
        }

        // GET: Insumoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insumo = await _context.Insumos
                .Include(i => i.IdCategoriaInsumosNavigation)
                .FirstOrDefaultAsync(m => m.IdInsumo == id);
            if (insumo == null)
            {
                return NotFound();
            }

            return View(insumo);
        }

        // POST: Insumoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insumo = await _context.Insumos.FindAsync(id);
            if (insumo != null)
            {
                _context.Insumos.Remove(insumo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsumoExists(int id)
        {
            return _context.Insumos.Any(e => e.IdInsumo == id);
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id, bool nuevoEstado)
        {
            var insumo = _context.Insumos.FirstOrDefault(i => i.IdInsumo == id);
            if (insumo != null)
            {
                insumo.Estado = nuevoEstado;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }



    }
}
