using System.ComponentModel.DataAnnotations;

namespace ApiProductos.Models.Dtos
{
    public class UserLoginDto
    {
    
        [Required(ErrorMessage = "EL usuario es obligatorio")]
        public string NombreUsuario { get; set; }
        
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
        


    }
}
