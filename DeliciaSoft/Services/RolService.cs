using DeliciaSoft.Models;
using DeliciaSoft.Repositories.Interfaces;
using DeliciaSoft.Services.Interfaces;
using DeliciaSoft.ViewModels.Permiso;
using DeliciaSoft.ViewModels.Rol;
using Microsoft.EntityFrameworkCore;

public class RolService : IRolService
{
    private readonly IRolRepository _rolRepository;
    private readonly IPermisoRepository _permisoRepository;

    public RolService(IRolRepository rolRepository, IPermisoRepository permisoRepository)
    {
        _rolRepository = rolRepository;
        _permisoRepository = permisoRepository;
    }

    public async Task<List<RolViewModel>> ObtenerTodosAsync()
    {
        var roles = await _rolRepository.ObtenerTodosAsync();
        return roles.Select(r => new RolViewModel
        {
            IdRol = r.IdRol,
            Nombre = r.Rol1,
            Descripcion = r.Descripcion,
            Estado = r.Estado ?? false
        }).ToList();
    }

    public async Task<RolDetalleViewModel> ObtenerPorIdAsync(int id)
    {
        var rol = await _rolRepository.ObtenerPorIdConPermisosAsync(id);
        if (rol == null)
            return null;

        var permisos = rol.RolPermisos
            .Where(rp => rp.Estado == true)
            .Select(rp => new PermisoViewModel
            {
                IdPermiso = rp.IdPermisoNavigation.IdPermiso,
                Modulo = rp.IdPermisoNavigation.Modulo,
                Accion = rp.IdPermisoNavigation.Accion,
                Descripcion = rp.IdPermisoNavigation.Descripcion,
                Estado = rp.IdPermisoNavigation.Estado ?? false,
                Asignado = true
            }).ToList();

        return new RolDetalleViewModel
        {
            IdRol = rol.IdRol,
            Nombre = rol.Rol1,
            Descripcion = rol.Descripcion,
            Estado = rol.Estado ?? false,
            Permisos = permisos
        };
    }

    public async Task<RolViewModel> CrearAsync(RolCrearViewModel model)
    {
        var rol = new Rol
        {
            Rol1 = model.Nombre,
            Descripcion = model.Descripcion,
            Estado = model.Estado
        };

        var nuevoRol = await _rolRepository.CrearAsync(rol);

        if (model.PermisosSeleccionados.Any())
        {
            await _rolRepository.AsignarPermisosAsync(nuevoRol.IdRol, model.PermisosSeleccionados);
        }

        return new RolViewModel
        {
            IdRol = nuevoRol.IdRol,
            Nombre = nuevoRol.Rol1,
            Descripcion = nuevoRol.Descripcion,
            Estado = nuevoRol.Estado ?? false
        };
    }

    public async Task<bool> ActualizarAsync(RolEditarViewModel model)
    {
        var rol = await _rolRepository.ObtenerPorIdAsync(model.IdRol);
        if (rol == null)
            return false;

        rol.Rol1 = model.Nombre;
        rol.Descripcion = model.Descripcion;
        rol.Estado = model.Estado;

        var resultado = await _rolRepository.ActualizarAsync(rol);

        if (resultado && model.PermisosSeleccionados != null)
        {
            await _rolRepository.AsignarPermisosAsync(rol.IdRol, model.PermisosSeleccionados);
        }

        return resultado;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        return await _rolRepository.EliminarAsync(id);
    }

    public async Task<List<PermisoViewModel>> ObtenerPermisosDisponiblesAsync(int rolId)
    {
        var todosPermisos = await _permisoRepository.ObtenerTodosAsync();
        var permisosRol = await _permisoRepository.ObtenerPorRolIdAsync(rolId);

        var permisosIds = permisosRol.Select(p => p.IdPermiso).ToList();

        return todosPermisos.Select(p => new PermisoViewModel
        {
            IdPermiso = p.IdPermiso,
            Modulo = p.Modulo,
            Accion = p.Accion,
            Descripcion = p.Descripcion,
            Estado = p.Estado ?? false,
            Asignado = permisosIds.Contains(p.IdPermiso)
        }).ToList();
    }

    //public async Task<bool> CambiarEstadoRolAsync(int id, bool nuevoEstado)
    //{
    //    try
    //    {
    //        var rol = await _rolRepository.ObtenerPorIdAsync(id);
    //        if (rol == null)
    //        {
    //            Console.WriteLine($"No se encontró el rol con ID: {id}");
    //            return false;
    //        }

    //        rol.Estado = nuevoEstado;

    //        var resultado = await _rolRepository.ActualizarAsync(rol);
    //        return resultado;
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error en CambiarEstadoRolAsync: {ex.Message}");
    //        return false;
    //    }
    //}

    public async Task<bool> ActualizarPermisosDeRolAsync(int id, List<int> permisosIds)
    {
        if (id <= 0 || permisosIds == null)
            return false;

        var rol = await _rolRepository.ObtenerPorIdAsync(id);
        if (rol == null)
            return false;

        return await _rolRepository.AsignarPermisosAsync(id, permisosIds);
    }

        // Método para cambiar el estado de un rol
        public async Task<bool> CambiarEstadoAsync(int idRol, bool estado)
        {
            var rol = await _rolRepository.ObtenerPorIdAsync(idRol);  // Usamos el repositorio para obtener el rol
            if (rol == null)
            {
                return false; // El rol no se encuentra
            }

            rol.Estado = estado;
            await _rolRepository.GuardarAsync();  // Usamos el repositorio para guardar los cambios
            return true;
        }

    public async Task<bool> TieneUsuariosAsociadosAsync(int idRol)
    {
        return await _rolRepository.TieneUsuariosAsociadosAsync(idRol);
    }
}

