using ApiProductos.Models;
using ApiProductos.Models.Dtos;
using ApiProductos.Repositories.IRespository;
using ApiProductos.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiProductos.Controllers
{
    [ApiController] //indica que es un controlador de API
    [Route("api/productos")] //Ruta Base
    public class ProductsController : ControllerBase //proporciona funcionalidades para los controllers
    {
        private readonly IProductService _ProductService; //Contiene una instancia del servicio Ip

        //constructor con los dos parametros (repo y mapper)
        public ProductsController(IProductService productService)
        {

            _ProductService = productService;


        }
        //Decoraciones
        //CrearProducto
        [HttpPost("CrearProducto")]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CreateProduct([FromForm] CreateProductoDto createProducto)
        {
            var result = await _ProductService.CreateProduct(createProducto);//llamamos al metodo  para crear nwProdt

            return result;
        }


        //obtener todos
        [HttpGet("TotalProductos")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _ProductService.GetProducts();
            return Ok(products);
        }




        //Obtener por ID
        [HttpGet("{productId:int}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var result = await _ProductService.GetProduct(productId);

            switch (result)
            {
                case OkObjectResult okResult:
                    return okResult; // Devuelve  respuesta 200 OK 
                case NotFoundObjectResult notFoundResult:
                    return NotFound(notFoundResult.Value); //Retornamos respuesta 404 con el mss que esta en el servicio
                default:
                    return StatusCode(500); // En caso de cualquier otro resultado
            }
        }



        //EndPoint Para Elimnar Producto
        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _ProductService.DeleteProduct(productId);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }




        //Actualizar
        //Decoraciones
        [HttpPatch("{productId:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int productId, [FromForm] ProductDto productDto)
        {
            return await _ProductService.UpdateProduct(productId, productDto);
        }


        //Buscar por nombre
        [HttpGet("BuscarPorNombre")]
        public async Task<IActionResult> GetProductsByName([FromQuery] string name)
        {
            // Llamamos al método del servicio para obtener productos por nombre
            var products = await _ProductService.GetProductsByName(name);

            // Verificamos si se encontraron productos
            if (products == null || products.Count == 0)
            {
                // Devolvemos una respuesta Not Found (404) con un mensaje indicando que no se encontraron productos
                return NotFound($"No se encontraron productos con el nombre '{name}'.");
            }

            // Retornamos los productos como respuesta exitosa (200 OK)
            return Ok(products);
        }

        [HttpGet("BuscarPrecios")]
        public async Task<IActionResult> GetProductsByPriceRange([FromQuery] decimal PrecioMin, [FromQuery] decimal PrecioMax, [FromQuery] bool ordenAscendenteoDesc = true)
        {
            // Llamamos al método correspondiente en el servicio para obtener los productos dentro del rango de precios especificado
            var productos = await _ProductService.GetProductsByPriceRange(PrecioMin, PrecioMax, ordenAscendenteoDesc);

            // Verificamos si se encontraron productos dentro del rango de precios
            if (productos.Count == 0)
            {
                // No se encontraron productos, devolvemos una respuesta HTTP 404 Not Found con un mensaje indicando que no se encontraron productos dentro del rango de precios especificado
                return NotFound("No se encontraron productos dentro del rango de precios especificado.");
            }

            // Se encontraron productos, devolvemos  los productos encontrados
            return Ok(productos);
        }

        //Buscar Descuentos

        [HttpGet("BuscarDescuentos")]
        public async Task<IActionResult> GetProductsByDiscount([FromQuery] bool orden = true)
        {
            var productos = await _ProductService.GetProductsByDiscount(orden);

            return Ok(productos);
        }

    }
}
