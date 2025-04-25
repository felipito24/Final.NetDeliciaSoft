using System.Collections.Generic;
using System.Threading.Tasks;
using DeliciaSoft.Models;

namespace DeliciaSoft.Repositories.Interfaces
{
    public interface IRolRepository
    {
        Task<List<Rol>> ObtenerTodosAsync();
        Task<Rol> ObtenerPorIdAsync(int id);
        Task<Rol> ObtenerPorIdConPermisosAsync(int id);
        Task<Rol> CrearAsync(Rol rol);
        Task<bool> ActualizarAsync(Rol rol);
        Task<bool> EliminarAsync(int id);
        Task<bool> AsignarPermisosAsync(int idRol, List<int> idPermisos);
        Task GuardarAsync();
        Task<bool> TieneUsuariosAsociadosAsync(int idRol);

    }
}