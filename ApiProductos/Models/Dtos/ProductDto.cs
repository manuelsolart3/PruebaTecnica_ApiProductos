using System.ComponentModel.DataAnnotations;

namespace ApiProductos.Models.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IFormFile Imagen { get; set; }

        [Required(ErrorMessage = "El Precio es obligatorio")]
        public decimal? Precio { get; set; }
        public decimal? Descuento { get; set; }
        
    }
}
