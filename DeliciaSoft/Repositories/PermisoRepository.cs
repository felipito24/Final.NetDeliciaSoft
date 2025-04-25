using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliciaSoft.Models;
using DeliciaSoft.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliciaSoft.Repositories
{
    public class PermisoRepository : IPermisoRepository
    {
        private readonly DeliciaSoftContext _context;

        public PermisoRepository(DeliciaSoftContext context)
        {
            _context = context;
        }

        public async Task<List<Permiso>> ObtenerTodosAsync()
        {
            return await _context.Permisos.Where(p => p.Estado == true).ToListAsync();
        }

        public async Task<Permiso> ObtenerPorIdAsync(int id)
        {
            return await _context.Permisos.FindAsync(id);
        }

        public async Task<List<Permiso>> ObtenerPorRolIdAsync(int rolId)
        {
            return await _context.RolPermisos
                .Where(rp => rp.IdRol == rolId && rp.Estado == true)
                .Select(rp => rp.IdPermisoNavigation)
                .ToListAsync();
        }
    }
}