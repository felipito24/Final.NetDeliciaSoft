using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliciaSoft.Models;
using DeliciaSoft.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace DeliciaSoft.Controllers.Compras
{
    [Authorize] // Agregar este atributo para requerir autenticación para todo el controlador
    public class ComprasController : Controller
    {
        private readonly DeliciaSoftContext _context;

        public ComprasController(DeliciaSoftContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            var deliciaSoftContext = _context.Compras.Include(c => c.IdProveedorNavigation);
            return View(await deliciaSoftContext.ToListAsync());
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        //GET: Compras/Create
        public IActionResult Create()
        {
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor");
            ViewBag.ProductosCompra = _context.ProductoCompras.ToList(); // o como los cargues

            return View();
        }



        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompra,IdProveedor,FechaCompra,FechaRegistro,MetodoPago,Observaciones,Subtotal,Iva,Total")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", compra.IdProveedor);
            return View(compra);
        }
        ///////////////////////////
        //[HttpPost]
        //public IActionResult AgregarProductosDetalle(List<int> productosSeleccionados)
        //{
        //    if (productosSeleccionados == null || !productosSeleccionados.Any())
        //    {
        //        return BadRequest("No se seleccionaron productos.");
        //    }

        //    var productos = _context.ProductoCompras
        //                            .Where(p => productosSeleccionados.Contains(p.IdProducto))
        //                            .ToList();

        // Mapea los productos seleccionados a la lista de DetalleCompra si así lo necesitas
        //    var detalles = productos.Select(p => new DetalleCompra
        //    {
        //        IdProducto = p.IdProducto,
        //        Cantidad = 1, // Puedes ajustar
        //        UnidadMedida = "Unidad", // Ajusta si aplica
        //        IdProductoNavigation = p
        //    }).ToList();

        //    // Guarda en sesión
        //    HttpContext.Session.SetObjectAsJson("DetalleCompra", detalles);

        //    return PartialView("_DetalleCompraPartial", detalles);
        //}

        [HttpPost]
        public IActionResult AgregarProductosDetalle(List<int> productosSeleccionados)
        {
            // Lógica para buscar productos por ID
            var productos = _context.ProductoCompras
                                    .Where(p => productosSeleccionados.Contains(p.IdProducto))
                                    .ToList();

            // Devolver una vista parcial que actualiza la tabla detalle
            return PartialView("_DetalleCompraPartial", productos);
        }



        [HttpPost]
        public IActionResult EliminarProductoDetalle(int idProducto)
        {
            var detalle = HttpContext.Session.GetObjectFromJson<List<DetalleCompra>>("DetalleCompra")
                          ?? new List<DetalleCompra>();

            var item = detalle.FirstOrDefault(x => x.IdProducto == idProducto);
            if (item != null)
            {
                detalle.Remove(item);
            }

            HttpContext.Session.SetObjectAsJson("DetalleCompra", detalle);

            return PartialView("_DetalleCompraPartial", detalle);
        }

        public IActionResult CargarProductosParcial()
{
    var productos = _context.ProductoCompras.ToList();
    return PartialView("_ProductosCompraPartial", productos);
}






        ///////////////////

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", compra.IdProveedor);
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCompra,IdProveedor,FechaCompra,FechaRegistro,MetodoPago,Observaciones,Subtotal,Iva,Total")] Compra compra)
        {
            if (id != compra.IdCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.IdCompra))
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
            ViewData["IdProveedor"] = new SelectList(_context.Proveedors, "IdProveedor", "IdProveedor", compra.IdProveedor);
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.IdProveedorNavigation)
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra != null)
            {
                _context.Compras.Remove(compra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.IdCompra == id);
        }
    }
}
