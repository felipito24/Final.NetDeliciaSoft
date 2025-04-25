using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliciaSoft.Models;
using DeliciaSoft.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliciaSoft.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly DeliciaSoftContext _context;

        public RolRepository(DeliciaSoftContext context)
        {
            _context = context;
        }

        public async Task<List<Rol>> ObtenerTodosAsync()
        {
            return await _context.Roles.ToListAsync(); 
        }

        public async Task<Rol> ObtenerPorIdAsync(int idRol)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.IdRol == idRol);
        }

        public async Task GuardarAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task<Rol> ObtenerPorIdConPermisosAsync(int id)
        {
            return await _context.Roles
                .Include(r => r.RolPermisos)
                    .ThenInclude(rp => rp.IdPermisoNavigation)
                .FirstOrDefaultAsync(r => r.IdRol == id);
        }

        public async Task<Rol> CrearAsync(Rol rol)
        {
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<bool> ActualizarAsync(Rol rol)
        {
            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
                return false;

            try
            {
                // Primero eliminar relaciones
                var rolPermisos = await _context.RolPermisos
                    .Where(rp => rp.IdRol == id)
                    .ToListAsync();

                _context.RolPermisos.RemoveRange(rolPermisos);

                // Luego eliminar el rol
                _context.Roles.Remove(rol);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Registra el error para depuración
                Console.WriteLine($"Error al eliminar rol: {ex.Message}");
                return false;
            }

        }



        public async Task<bool> AsignarPermisosAsync(int idRol, List<int> idPermisos)
        {
            // Obtener permisos actuales del rol
            var permisosActuales = await _context.RolPermisos
                .Where(rp => rp.IdRol == idRol)
                .ToListAsync();

            // Eliminar permisos que ya no están en la lista
            foreach (var permiso in permisosActuales)
            {
                if (!idPermisos.Contains(permiso.IdPermiso.Value))
                {
                    _context.RolPermisos.Remove(permiso);
                }
                else
                {
                    // Marcar como procesado
                    idPermisos.Remove(permiso.IdPermiso.Value);
                }
            }

            // Agregar nuevos permisos
            foreach (var idPermiso in idPermisos)
            {
                _context.RolPermisos.Add(new RolPermiso
                {
                    IdRol = idRol,
                    IdPermiso = idPermiso,
                    Estado = true
                });
            }

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> TieneUsuariosAsociadosAsync(int idRol)
        {
            return await _context.Usuarios.AnyAsync(u => u.IdRol == idRol);
        }

    }
}