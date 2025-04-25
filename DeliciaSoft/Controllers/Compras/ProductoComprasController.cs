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
    public class ProductoComprasController : Controller
    {
        private readonly DeliciaSoftContext _context;

        public ProductoComprasController(DeliciaSoftContext context)
        {
            _context = context;
        }

        // GET: ProductoCompras
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductoCompras.ToListAsync());
        }

        // GET: ProductoCompras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoCompra = await _context.ProductoCompras
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productoCompra == null)
            {
                return NotFound();
            }

            return View(productoCompra);
        }

        // GET: ProductoCompras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductoCompras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,NombreProducto,UnidadMedida")] ProductoCompra productoCompra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoCompra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productoCompra);
        }

        // GET: ProductoCompras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoCompra = await _context.ProductoCompras.FindAsync(id);
            if (productoCompra == null)
            {
                return NotFound();
            }
            return View(productoCompra);
        }

        // POST: ProductoCompras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,NombreProducto,UnidadMedida")] ProductoCompra productoCompra)
        {
            if (id != productoCompra.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoCompra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoCompraExists(productoCompra.IdProducto))
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
            return View(productoCompra);
        }

        // GET: ProductoCompras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoCompra = await _context.ProductoCompras
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productoCompra == null)
            {
                return NotFound();
            }

            return View(productoCompra);
        }

        // POST: ProductoCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoCompra = await _context.ProductoCompras.FindAsync(id);
            if (productoCompra != null)
            {
                _context.ProductoCompras.Remove(productoCompra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoCompraExists(int id)
        {
            return _context.ProductoCompras.Any(e => e.IdProducto == id);
        }
    }
}
