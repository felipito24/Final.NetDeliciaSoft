using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliciaSoft.Models;
using Microsoft.AspNetCore.Authorization;

namespace DeliciaSoft.Controllers.Ventas
{
    [Authorize] // Agregar este atributo para requerir autenticación para todo el controlador
    public class ProductoPersonalizadoesController : Controller
    {
        private readonly DeliciaSoftContext _context;

        public ProductoPersonalizadoesController(DeliciaSoftContext context)
        {
            _context = context;
        }

        // GET: ProductoPersonalizadoes
        public async Task<IActionResult> Index()
        {
            var deliciaSoftContext = _context.ProductoPersonalizados.Include(p => p.IdProductoNavigation).Include(p => p.ToppingsNavigation);
            return View(await deliciaSoftContext.ToListAsync());
        }

        // GET: ProductoPersonalizadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoPersonalizado = await _context.ProductoPersonalizados
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.ToppingsNavigation)
                .FirstOrDefaultAsync(m => m.IdProductoPersonalizado == id);
            if (productoPersonalizado == null)
            {
                return NotFound();
            }

            return View(productoPersonalizado);
        }

        // GET: ProductoPersonalizadoes/Create
        public IActionResult Create()
        {
            ViewData["IdProducto"] = new SelectList(_context.ProductoGenerals, "IdProductoGeneral", "NombreProducto");
            ViewData["Toppings"] = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo");

            return View();
        }

        // POST: ProductoPersonalizadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProductoPersonalizado,IdProducto,Cantidad,Descripcion,Total,Decoracion,TemasOmotivos,Toppings,Mensaje,DisenioPersonalizado,ImagenReferencia")] ProductoPersonalizado productoPersonalizado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoPersonalizado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducto"] = new SelectList(_context.ProductoGenerals, "IdProductoGeneral", "IdProductoGeneral", productoPersonalizado.IdProducto);
            ViewData["Toppings"] = new SelectList(_context.Insumos, "IdInsumo", "IdInsumo", productoPersonalizado.Toppings);
            return View(productoPersonalizado);
        }

        // GET: ProductoPersonalizadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoPersonalizado = await _context.ProductoPersonalizados.FindAsync(id);
            if (productoPersonalizado == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.ProductoGenerals, "IdProductoGeneral", "NombreProducto");
            ViewData["Toppings"] = new SelectList(_context.Insumos, "IdInsumo", "NombreInsumo");
            return View(productoPersonalizado);
        }

        // POST: ProductoPersonalizadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProductoPersonalizado,IdProducto,Cantidad,Descripcion,Total,Decoracion,TemasOmotivos,Toppings,Mensaje,DisenioPersonalizado,ImagenReferencia")] ProductoPersonalizado productoPersonalizado)
        {
            if (id != productoPersonalizado.IdProductoPersonalizado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoPersonalizado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoPersonalizadoExists(productoPersonalizado.IdProductoPersonalizado))
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
            ViewData["IdProducto"] = new SelectList(_context.ProductoGenerals, "IdProductoGeneral", "IdProductoGeneral", productoPersonalizado.IdProducto);
            ViewData["Toppings"] = new SelectList(_context.Insumos, "IdInsumo", "IdInsumo", productoPersonalizado.Toppings);
            return View(productoPersonalizado);
        }

        // GET: ProductoPersonalizadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoPersonalizado = await _context.ProductoPersonalizados
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.ToppingsNavigation)
                .FirstOrDefaultAsync(m => m.IdProductoPersonalizado == id);
            if (productoPersonalizado == null)
            {
                return NotFound();
            }

            return View(productoPersonalizado);
        }

        // POST: ProductoPersonalizadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoPersonalizado = await _context.ProductoPersonalizados.FindAsync(id);
            if (productoPersonalizado != null)
            {
                _context.ProductoPersonalizados.Remove(productoPersonalizado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoPersonalizadoExists(int id)
        {
            return _context.ProductoPersonalizados.Any(e => e.IdProductoPersonalizado == id);
        }
    }
}
