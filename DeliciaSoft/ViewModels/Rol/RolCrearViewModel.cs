using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeliciaSoft.ViewModels.Rol
{
    public class RolCrearViewModel
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string Nombre { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres")]
        public string Descripcion { get; set; }

        public bool Estado { get; set; } = true;

        public List<int> PermisosSeleccionados { get; set; } = new List<int>();
    }
}