using System.Collections.Generic;
using System.Threading.Tasks;
using DeliciaSoft.ViewModels.Rol;
using DeliciaSoft.ViewModels.Permiso;

namespace DeliciaSoft.Services.Interfaces
{
    public interface IRolService
    {
        Task<List<RolViewModel>> ObtenerTodosAsync();
        Task<RolDetalleViewModel> ObtenerPorIdAsync(int id);
        Task<RolViewModel> CrearAsync(RolCrearViewModel model);
        Task<bool> ActualizarAsync(RolEditarViewModel model);
        Task<bool> EliminarAsync(int id);
        Task<List<PermisoViewModel>> ObtenerPermisosDisponiblesAsync(int rolId);
        Task<bool> ActualizarPermisosDeRolAsync(int id, List<int> list);

        Task<bool> CambiarEstadoAsync(int idRol, bool estado);
        Task<bool> TieneUsuariosAsociadosAsync(int id);

        //Task<bool> CambiarEstadoRolAsync(int id, bool nuevoEstado);

    }
}
