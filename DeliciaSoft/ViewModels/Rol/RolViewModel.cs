using System.Collections.Generic;

namespace DeliciaSoft.ViewModels.Rol
{
    public class RolViewModel
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }

        public bool TieneUsuariosAsociados { get; set; }
    }
}