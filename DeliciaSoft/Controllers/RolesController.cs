using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliciaSoft.Models;
using Microsoft.AspNetCore.Authorization;
using DeliciaSoft.Authorization;
using DeliciaSoft.Repositories.Interfaces;
using DeliciaSoft.Services.Interfaces;
using DeliciaSoft.ViewModels.Rol;

namespace DeliciaSoft.Controllers
{
    [Authorize] // Asegura que todo el controlador requiera autenticación
    public class RolesController : Controller
    {
        private readonly IRolService _rolService;
        private readonly IPermisoRepository _permisoRepository;

        public RolesController(IRolService rolService, IPermisoRepository permisoRepository)
        {
            _rolService = rolService;
            _permisoRepository = permisoRepository;
        }

        // GET: Roles
        [Permiso("Roles", "Ver")]
        public async Task<IActionResult> Index()
        {
            var roles = await _rolService.ObtenerTodosAsync();

            // Para cada rol, verificar si tiene usuarios asociados
            foreach (var rol in roles)
            {
                rol.TieneUsuariosAsociados = await _rolService.TieneUsuariosAsociadosAsync(rol.IdRol);
            }

            return View(roles);
        }

        // GET: Roles/Details/5
        [Permiso("Roles", "Ver")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _rolService.ObtenerPorIdAsync(id.Value);
            if (rol == null)
            {
                return NotFound();
            }

            rol.TieneUsuariosAsociados = await _rolService.TieneUsuariosAsociadosAsync(id.Value);

            return View(rol);
        }

        // GET: Roles/Create
        [Permiso("Roles", "Crear")]
        public async Task<IActionResult> Create()
        {
            var permisos = await _permisoRepository.ObtenerTodosAsync();

            ViewBag.Permisos = permisos.Select(p => new DeliciaSoft.ViewModels.Permiso.PermisoViewModel
            {
                IdPermiso = p.IdPermiso,
                Modulo = p.Modulo,
                Accion = p.Accion,
                Descripcion = p.Descripcion,
                Estado = p.Estado ?? false,
                Asignado = false
            }).ToList();

            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso("Roles", "Crear")]
        public async Task<IActionResult> Create(RolCrearViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _rolService.CrearAsync(modelo);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var permisos = await _permisoRepository.ObtenerTodosAsync();
            ViewBag.Permisos = permisos.Select(p => new DeliciaSoft.ViewModels.Permiso.PermisoViewModel
            {
                IdPermiso = p.IdPermiso,
                Modulo = p.Modulo,
                Accion = p.Accion,
                Descripcion = p.Descripcion,
                Estado = p.Estado ?? false,
                Asignado = modelo.PermisosSeleccionados.Contains(p.IdPermiso)
            }).ToList();

            return View(modelo);
        }


        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso("Roles", "Editar")]
        public async Task<IActionResult> Edit(int id, RolEditarViewModel modelo)
        {
            if (id != modelo.IdRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var resultado = await _rolService.ActualizarAsync(modelo);
                    if (resultado)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    // Aquí podrías registrar el error
                }
            }

            var permisos = await _permisoRepository.ObtenerTodosAsync();
            ViewBag.Permisos = permisos.Select(p => new DeliciaSoft.ViewModels.Permiso.PermisoViewModel
            {
                IdPermiso = p.IdPermiso,
                Modulo = p.Modulo,
                Accion = p.Accion,
                Descripcion = p.Descripcion,
                Estado = p.Estado ?? false,
                Asignado = modelo.PermisosSeleccionados.Contains(p.IdPermiso)
            }).ToList();

            return View(modelo);
        }

        // GET: Roles/Edit/5
        [Permiso("Roles", "Editar")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _rolService.ObtenerPorIdAsync(id.Value);
            if (rol == null)
            {
                return NotFound();
            }

            var modelo = new RolEditarViewModel
            {
                IdRol = rol.IdRol,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion,
                Estado = rol.Estado,
                PermisosSeleccionados = rol.Permisos.Select(p => p.IdPermiso).ToList()
            };

            var permisos = await _permisoRepository.ObtenerTodosAsync();
            ViewBag.Permisos = permisos.Select(p => new DeliciaSoft.ViewModels.Permiso.PermisoViewModel
            {
                IdPermiso = p.IdPermiso,
                Modulo = p.Modulo,
                Accion = p.Accion,
                Descripcion = p.Descripcion,
                Estado = p.Estado ?? false,
                Asignado = modelo.PermisosSeleccionados.Contains(p.IdPermiso)
            }).ToList();

            return View(modelo);
        }

        // GET: Roles/Delete/5
        [Permiso("Roles", "Eliminar")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _rolService.ObtenerPorIdAsync(id.Value);
            if (rol == null)
            {
                return NotFound();
            }

            // Verificar si tiene usuarios asociados
            rol.TieneUsuariosAsociados = await _rolService.TieneUsuariosAsociadosAsync(id.Value);

            // Si tiene usuarios asociados, redirigir a una vista de error o mostrar mensaje
            if (rol.TieneUsuariosAsociados)
            {
                TempData["Error"] = "No se puede eliminar el rol porque tiene usuarios asociados.";
                return RedirectToAction(nameof(Index));
            }

            return View(rol);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Permiso("Roles", "Eliminar")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Verificar nuevamente si tiene usuarios asociados
            var tieneUsuarios = await _rolService.TieneUsuariosAsociadosAsync(id);

            if (tieneUsuarios)
            {
                TempData["Error"] = "No se puede eliminar el rol porque tiene usuarios asociados.";
                return RedirectToAction(nameof(Index));
            }

            var resultado = await _rolService.EliminarAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Vista parcial de permisos
        [Permiso("Roles", "Ver")]
        public async Task<IActionResult> _PermisosPartial(int id)
        {
            var rol = await _rolService.ObtenerPorIdAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return PartialView("_PermisosPartial", rol.Permisos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleEstado(int id, bool estado)
        {
            // Verificar si tiene usuarios asociados cuando se desactiva
            if (!estado)
            {
                var tieneUsuarios = await _rolService.TieneUsuariosAsociadosAsync(id);
                if (tieneUsuarios)
                {
                    return Json(new { success = false, message = "No se puede desactivar el rol porque tiene usuarios asociados." });
                }
            }

            var resultado = await _rolService.CambiarEstadoAsync(id, estado);
            if (!resultado)
            {
                return Json(new { success = false, message = "Rol no encontrado." });
            }

            return Json(new { success = true });
        }
    }
}