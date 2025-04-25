using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliciaSoft.Models;
using Microsoft.AspNetCore.Authorization; // Agregar este using
using DeliciaSoft.Authorization; // Agregar este using

namespace DeliciaSoft.Controllers
{
    [Authorize] // Agregar este atributo para requerir autenticación para todo el controlador
    public class UsuariosController : Controller
    {
        private readonly DeliciaSoftContext _context;

        public UsuariosController(DeliciaSoftContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        [Permiso("Usuarios", "Ver")] // Agregar este atributo para verificar permiso específico
        public async Task<IActionResult> Index()
        {
            var deliciaSoftContext = _context.Usuarios.Include(u => u.IdRolNavigation);
            return View(await deliciaSoftContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        [Permiso("Usuarios", "Ver")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        [Permiso("Usuarios", "Crear")]
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "Rol1");
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso("Usuarios", "Crear")]
        public async Task<IActionResult> Create([Bind("IdUsuario,Nombre,Apellido,Correo,Contrasena,Direccion,Barrio,Ciudad,TipoDocumento,NumeroDocumento,FechaNacimiento,Celular,IdRol,Estado")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "Rol1", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        [Permiso("Usuarios", "Editar")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "Rol1", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso("Usuarios", "Editar")]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Nombre,Apellido,Correo,Contrasena,Direccion,Barrio,Ciudad,TipoDocumento,NumeroDocumento,FechaNacimiento,Celular,IdRol,Estado")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
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
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "Rol1", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        [Permiso("Usuarios", "Eliminar")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Permiso("Usuarios", "Eliminar")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }

        // Agregar este método al UsuariosController
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso("Usuarios", "Editar")]
        public async Task<IActionResult> ToggleEstado(int id, bool estado)
        {
            // Validación de entrada
            if (id <= 0)
            {
                return Json(new { success = false, message = "ID de usuario inválido" });
            }

            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return Json(new { success = false, message = "Usuario no encontrado" });
                }

                // Cambiar el estado - tipo bit en la base de datos
                usuario.Estado = estado;
                _context.Update(usuario);
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
