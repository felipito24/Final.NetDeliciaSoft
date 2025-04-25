using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliciaSoft.Models;
using Microsoft.AspNetCore.Authorization;

namespace DeliciaSoft.Controllers
{
    [Authorize]
    public class ProductoGeneralsController : Controller
    {
        private readonly DeliciaSoftContext _context;

        public ProductoGeneralsController(DeliciaSoftContext context)
        {
            _context = context;
        }

        // GET: ProductoGenerals
        public async Task<IActionResult> Index()
        {
            var deliciaSoftContext = _context.ProductoGenerals
                .Include(p => p.IdCategoriaProductoNavigation);
            return View(await deliciaSoftContext.ToListAsync());
        }

        // GET: ProductoGenerals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoGeneral = await _context.ProductoGenerals
                .Include(p => p.IdCategoriaProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdProductoGeneral == id);
            if (productoGeneral == null)
            {
                return NotFound();
            }

            return View(productoGeneral);
        }

        // GET: ProductoGenerals/Create
        public IActionResult Create()
        {
            // Filtrar para mostrar solo categorías activas
            ViewData["IdCategoriaProducto"] = new SelectList(_context.CategoriaProductos.Where(c => c.Estado == true),
                "IdCategoriaProducto", "NombreCategoria");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProductoGeneral,NombreProducto,IdCategoriaProducto,PrecioProducto,CantidadProducto,Imagen,Estado")] ProductoGeneral productoGeneral)
        {
            // Verificar que la categoría seleccionada está activa
            var categoriaActiva = await _context.CategoriaProductos
                .AnyAsync(c => c.IdCategoriaProducto == productoGeneral.IdCategoriaProducto && c.Estado == true);

            if (!categoriaActiva)
            {
                ModelState.AddModelError("IdCategoriaProducto", "La categoría seleccionada está inactiva o no existe.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(productoGeneral);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores, recargar la lista de categorías activas
            ViewData["IdCategoriaProducto"] = new SelectList(_context.CategoriaProductos.Where(c => c.Estado == true),
                "IdCategoriaProducto", "NombreCategoria", productoGeneral.IdCategoriaProducto);

            return View(productoGeneral);
        }

        // GET: ProductoGenerals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoGeneral = await _context.ProductoGenerals.FindAsync(id);
            if (productoGeneral == null)
            {
                return NotFound();
            }

            ViewData["IdCategoriaProducto"] = new SelectList(_context.CategoriaProductos.Where(c => c.Estado == true),
                "IdCategoriaProducto", "NombreCategoria", productoGeneral.IdCategoriaProducto);
            return View(productoGeneral);
        }

        // POST: ProductoGenerals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProductoGeneral,NombreProducto,IdCategoriaProducto,PrecioProducto,CantidadProducto,Imagen,Estado")] ProductoGeneral productoGeneral)
        {
            if (id != productoGeneral.IdProductoGeneral)
            {
                return NotFound();
            }

            // Verificar que la categoría seleccionada está activa
            var categoriaActiva = await _context.CategoriaProductos
                .AnyAsync(c => c.IdCategoriaProducto == productoGeneral.IdCategoriaProducto && c.Estado == true);

            if (!categoriaActiva)
            {
                ModelState.AddModelError("IdCategoriaProducto", "La categoría seleccionada está inactiva o no existe.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoGeneral);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoGeneralExists(productoGeneral.IdProductoGeneral))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Si hay errores, recargar la lista de categorías activas
            ViewData["IdCategoriaProducto"] = new SelectList(_context.CategoriaProductos.Where(c => c.Estado == true),
                "IdCategoriaProducto", "NombreCategoria", productoGeneral.IdCategoriaProducto);
            return View(productoGeneral);
        }


        // GET: ProductoGenerals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoGeneral = await _context.ProductoGenerals
                .Include(p => p.IdCategoriaProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdProductoGeneral == id);
            if (productoGeneral == null)
            {
                return NotFound();
            }

            return View(productoGeneral);
        }

        // POST: ProductoGenerals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoGeneral = await _context.ProductoGenerals.FindAsync(id);
            if (productoGeneral != null)
            {
                _context.ProductoGenerals.Remove(productoGeneral);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoGeneralExists(int id)
        {
            return _context.ProductoGenerals.Any(e => e.IdProductoGeneral == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleEstado(int id, bool estado)
        {
            var producto = _context.ProductoGenerals.Find(id); // Asegúrate de que _context sea tu contexto de datos
            if (producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado." });
            }

            producto.Estado = estado;  // Cambia el estado
            _context.SaveChanges();  // Guarda los cambios

            return Json(new { success = true });
        }

    }
}
