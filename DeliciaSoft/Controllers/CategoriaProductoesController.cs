using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliciaSoft.Models;
using DeliciaSoft.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace DeliciaSoft.Controllers
{
    [Authorize]
    public class CategoriaProductoesController : Controller
    {
        private readonly DeliciaSoftContext _context;

        public CategoriaProductoesController(DeliciaSoftContext context)
        {
            _context = context;
        }

        // GET: CategoriaProductoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoriaProductos.ToListAsync());
        }

        // GET: CategoriaProductoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaProducto = await _context.CategoriaProductos
                .FirstOrDefaultAsync(m => m.IdCategoriaProducto == id);
            if (categoriaProducto == null)
            {
                return NotFound();
            }

            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaProductoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoriaProducto,NombreCategoria,FechaRegistro,CantidadProducto,DescripcionProducto,Estado")] CategoriaProducto categoriaProducto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoriaProducto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaProducto = await _context.CategoriaProductos.FindAsync(id);
            if (categoriaProducto == null)
            {
                return NotFound();
            }
            return View(categoriaProducto);
        }

        // POST: CategoriaProductoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoriaProducto,NombreCategoria,FechaRegistro,CantidadProducto,DescripcionProducto,Estado")] CategoriaProducto categoriaProducto)
        {
            if (id != categoriaProducto.IdCategoriaProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriaProducto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaProductoExists(categoriaProducto.IdCategoriaProducto))
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
            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaProducto = await _context.CategoriaProductos
                .FirstOrDefaultAsync(m => m.IdCategoriaProducto == id);
            if (categoriaProducto == null)
            {
                return NotFound();
            }

            return View(categoriaProducto);
        }

        // POST: CategoriaProductoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoriaProducto = await _context.CategoriaProductos.FindAsync(id);
            if (categoriaProducto != null)
            {
                _context.CategoriaProductos.Remove(categoriaProducto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaProductoExists(int id)
        {
            return _context.CategoriaProductos.Any(e => e.IdCategoriaProducto == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleEstado(int id, bool estado)
        {
            // Validación de entrada
            if (id <= 0)
            {
                return Json(new { success = false, message = "ID de categoría inválido" });
            }

            try
            {
                var categoriaProducto = await _context.CategoriaProductos.FindAsync(id);
                if (categoriaProducto == null)
                {
                    return Json(new { success = false, message = "Categoría no encontrada" });
                }

                // Cambiar el estado - tipo bit en la base de datos
                categoriaProducto.Estado = estado;
                _context.Update(categoriaProducto);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Estado actualizado correctamente" });
            }
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"Error al cambiar estado: {ex.Message}");
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }


    }
}
