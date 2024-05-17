using System.Threading.Tasks;
using ApiProductos.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

public interface IProductService
{
    //Task representa una operacion asincronica
    //IActionResult se usa para devolver respuestas HTTP
    //Metodo para crear Producto
    Task<IActionResult> CreateProduct(CreateProductoDto createProductoDto);

    // Método para obtener todos los productos
    Task<List<ProductResponseDto>> GetProducts();

    // método para obtener un producto por su id
    Task<IActionResult> GetProduct(int productId);

    // Método para eliminar un producto
    Task<bool> DeleteProduct(int product);

    // Método para actualizar un producto
    Task<IActionResult> UpdateProduct(int productId, ProductDto productDto);


    //Metodo para buscar por nombre
    Task<List<ProductResponseDto>> GetProductsByName(string name);

    // Método para Buscar por rango de precios
    Task<List<ProductResponseDto>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice, bool ordenAscendente);

    //Metodo para buscar Descuentos
    Task<List<ProductResponseDto>> GetProductsByDiscount(bool orden);

    //Metodo para Obtener todos
    Task<IEnumerable<GetAllFilters>> SearchProducts(string Namefilter, decimal? Discount, decimal? minPrice, decimal? maxPrice);


}