using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliciaSoft.Models;
using Microsoft.AspNetCore.Authorization; 
using DeliciaSoft.Authorization; 

namespace DeliciaSoft.Controllers
{
    [Authorize] // Agregar este atributo para requerir autenticación para todo el controlador
    public class ClientesController : Controller
    {
        private readonly DeliciaSoftContext _context;

        public ClientesController(DeliciaSoftContext context)
        {
            _context = context;
        }

        // GET: Clientes
        [Permiso("Clientes", "Ver")] // Verificar permiso para la acción Ver
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        [Permiso("Clientes", "Ver")] // Verificar permiso para la acción Ver
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        [Permiso("Clientes", "Crear")] // Verificar permiso para la acción Crear
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso("Clientes", "Crear")] // Verificar permiso para la acción Crear
        public async Task<IActionResult> Create([Bind("IdCliente,Nombre,Apellido,Correo,Contrasena,Direccion,Barrio,Ciudad,TipoDocumento,NumeroDocumento,FechaNacimiento,Celular,Estado")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        [Permiso("Clientes", "Editar")] // Verificar permiso para la acción Editar
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso("Clientes", "Editar")] // Verificar permiso para la acción Editar
        public async Task<IActionResult> Edit(int id, [Bind("IdCliente,Nombre,Apellido,Correo,Contrasena,Direccion,Barrio,Ciudad,TipoDocumento,NumeroDocumento,FechaNacimiento,Celular,Estado")] Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.IdCliente))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        [Permiso("Clientes", "Eliminar")] // Verificar permiso para la acción Eliminar
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Permiso("Clientes", "Eliminar")] // Verificar permiso para la acción Eliminar
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.IdCliente == id);
        }

        // Agregar este método al final del ClientesController, justo antes del último corchete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso("Clientes", "Editar")]
        public async Task<IActionResult> ToggleEstado(int id, bool estado)
        {
            // Validación de entrada
            if (id <= 0)
            {
                return Json(new { success = false, message = "ID de cliente inválido" });
            }

            try
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    return Json(new { success = false, message = "Cliente no encontrado" });
                }

                // Cambiar el estado - tipo bit en la base de datos
                cliente.Estado = estado;
                _context.Update(cliente);
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
