namespace ApiProductos.Models.Dtos
{
    public class ProductResponseDto
    {//lo utilizamos para representar lo que se le devolvera al cliente
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenBase64 { get; set; }
        public decimal Precio { get; set; }
        public decimal Descuento { get; set; }
    }
}
