namespace ApiProductos.Models.Dtos;

public class GetAllFilters
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string Imagen { get; set; }
    public decimal Precio { get; set; }
    public decimal Descuento { get; set; }
    public decimal? PrecioDescuento { get; set; }
}
