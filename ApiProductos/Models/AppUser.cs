using Microsoft.AspNetCore.Identity;

namespace ApiProductos.Models
{
    public class AppUser : IdentityUser
    {

        //Añadir campos personalizados

        public string Nombre { get; set; }
    }
}
