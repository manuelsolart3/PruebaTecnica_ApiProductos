using System.ComponentModel.DataAnnotations;

namespace ApiProductos.Models.Dtos
{
    public class UserRegisterDto
    {
    
        [Required(ErrorMessage = "EL usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "EL Nombre es obligatorio")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
        public string Rol { get; set; }


    }
}
