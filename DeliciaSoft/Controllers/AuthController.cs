using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DeliciaSoft.Models;
using DeliciaSoft.Repositories.Interfaces;
using DeliciaSoft.ViewModels.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeliciaSoft.Controllers
{
   
    public class AuthController : Controller
    {
        
        private readonly DeliciaSoftContext _context;
        private readonly IPermisoRepository _permisoRepository;

        public AuthController(DeliciaSoftContext context, IPermisoRepository permisoRepository)
        {
            _context = context;
            _permisoRepository = permisoRepository;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // Si el usuario ya está autenticado, redirigir al dashboard
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { showModals = true });
            }

            // Crear una instancia del modelo con el returnUrl (opcional)
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Buscar usuario por correo
                var usuario = await _context.Usuarios
                    .Include(u => u.IdRolNavigation)
                    .FirstOrDefaultAsync(u => u.Correo == model.Correo);

                if (usuario == null)
                {
                    ModelState.AddModelError("", "El correo electrónico no está registrado");
                    return View(model);
                }

                // Verificar contraseña
                if (usuario.Contrasena != model.Contrasena)
                {
                    ModelState.AddModelError("", "La contraseña es incorrecta");
                    return View(model);
                }

                // Verificar estado
                if (usuario.Estado != true)
                {
                    ModelState.AddModelError("", "Esta cuenta está desactivada");
                    return View(model);
                }

                // Obtener permisos del rol del usuario
                var permisos = await _permisoRepository.ObtenerPorRolIdAsync(usuario.IdRol.Value);

                // Crear claims para el usuario
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Role, usuario.IdRolNavigation?.Rol1 ?? "")
        };

                // Agregar claims para cada permiso
                foreach (var permiso in permisos)
                {
                    claims.Add(new Claim("Permission", $"{permiso.Modulo}.{permiso.Accion}"));
                }

                // Crear identidad y principal de autenticación
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Configurar propiedades de autenticación
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RecordarMe,
                    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                };

                // Iniciar sesión
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProperties);

                // Redirigir según el rol
                if (usuario.IdRolNavigation.Rol1 == "Cliente")
                {
                    // Redirigir al controlador 'VistaCliente' y la acción 'Inicio'
                    return RedirectToAction("Inicio", "Clientes"); 
                }

                // Para todos los demás roles (administrador u otros), redirigir a la página de inicio de administración
                return RedirectToAction("Index", "Home"); // Vista predeterminada para otros roles
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Registro()
        {
            // Si el usuario ya está autenticado, redirigir al dashboard
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            // Cargar roles para el dropdown
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "Rol1");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el correo ya existe
                var usuarioExistente = await _context.Usuarios.AnyAsync(u => u.Correo == model.Correo);
                if (usuarioExistente)
                {
                    ModelState.AddModelError("Correo", "Este correo ya está registrado");
                    ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "Rol1", model.IdRol);
                    return View(model);
                }

                // Crear nuevo usuario
                var usuario = new Usuario
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Correo = model.Correo,
                    Contrasena = model.Contrasena,  // En producción, hashear la contraseña
                    Direccion = model.Direccion,
                    Barrio = model.Barrio,
                    Ciudad = model.Ciudad,
                    TipoDocumento = model.TipoDocumento,
                    NumeroDocumento = model.NumeroDocumento,
                    FechaNacimiento = DateOnly.FromDateTime(model.FechaNacimiento),
                    Celular = model.Celular,
                    IdRol = model.IdRol,
                    Estado = true // Usuario activo por defecto
                };

                // Guardar el usuario en la base de datos
                _context.Add(usuario);
                await _context.SaveChangesAsync();

                // Mensaje de éxito
                TempData["SuccessMessage"] = "Registro exitoso. Ahora puedes iniciar sesión.";
                return RedirectToAction(nameof(Login));
            }

            // Si el modelo no es válido, regresamos a la vista con los errores
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "Rol1", model.IdRol);
            return View(model);
        }

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}