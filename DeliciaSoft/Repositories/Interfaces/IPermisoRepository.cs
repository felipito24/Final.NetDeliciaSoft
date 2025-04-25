using System.Collections.Generic;
using System.Threading.Tasks;
using DeliciaSoft.Models;

namespace DeliciaSoft.Repositories.Interfaces
{
    public interface IPermisoRepository
    {
        Task<List<Permiso>> ObtenerTodosAsync();
        Task<Permiso> ObtenerPorIdAsync(int id);
        Task<List<Permiso>> ObtenerPorRolIdAsync(int rolId);
    }
}