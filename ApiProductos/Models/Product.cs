using System.ComponentModel.DataAnnotations;

namespace ApiProductos.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public byte[] Imagen { get; set; }
        [Required]
        public decimal Precio{ get; set; }
        public decimal? Descuento{ get; set; }
        public DateTime FechaCreacion{ get; set; }= DateTime.UtcNow;
    }
}
