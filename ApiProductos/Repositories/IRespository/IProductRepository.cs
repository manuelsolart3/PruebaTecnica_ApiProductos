using ApiProductos.Models;
using ApiProductos.Models.Dtos;

namespace ApiProductos.Repositories.IRespository
{
    public interface IProductRepository
    {
        // Método para crear un nuevo product en la BD
        Task<bool> CreateProduct(Product product);

        // Método para obtener todos los productos
        Task<List<Product>> GetProducts(); // Devuelve una colección de productos de manera async

        // Método para obtener un product por su ID
        Task<Product> GetProduct(int ProductId); // Toma un parámetro de id y devuelve un solo product de manera async

        
        // Método para eliminar un product existente en la BD
        Task<bool> DeleteProduct(Product product);

        // Método para actualizar un product existente en la BD
        Task<bool> UpdateProduct(int productId, ProductDto productDto);

      
        // Metodo para buscar por nombre
        Task<List<Product>> GetProductsByName(string name);

        //Metodo filtrado por un rango de precios
//parametro booleano para ordenar de forma ascendente o descndente
        Task<List<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice, bool ordenAscendente);

        //Metodo filtrado descuentos
        Task<List<Product>> GetProductsByDiscount(bool orden);

        // Método para guardar los cambios en la BD
        Task<bool> Guardar(); 

    }
}
