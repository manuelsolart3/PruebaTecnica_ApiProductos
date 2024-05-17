using System.ComponentModel.DataAnnotations;

namespace ApiProductos.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Password{ get; set; }
        public string Rol{ get; set; }
        
     
    }
}
