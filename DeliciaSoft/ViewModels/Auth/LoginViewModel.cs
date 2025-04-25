// LoginViewModel.cs - Actualizar en la carpeta ViewModels/Auth
using System.ComponentModel.DataAnnotations;

namespace DeliciaSoft.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Display(Name = "Recordarme")]
        public bool RecordarMe { get; set; }

        // El campo ReturnUrl no debe ser requerido
        public string ReturnUrl { get; set; } = string.Empty;
    }
}