using System.Collections.Generic;
using DeliciaSoft.ViewModels.Permiso;

namespace DeliciaSoft.ViewModels.Rol
{
    public class RolDetalleViewModel
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public List<PermisoViewModel> Permisos { get; set; } = new List<PermisoViewModel>();
        public bool TieneUsuariosAsociados { get; internal set; }
    }
}