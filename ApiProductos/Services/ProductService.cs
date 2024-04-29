using System.ComponentModel.DataAnnotations;
using ApiProductos.Models;
using ApiProductos.Models.Dtos;
using ApiProductos.Repositories.IRespository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiProductos.Services;

public sealed class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository; //accedemos al repoProducts


    //constructor con los dos parametros (repo y mapper)
    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }



    // CrearProducto
public async Task<IActionResult> CreateProduct(CreateProductoDto createProductoDto)
{
    byte[] content = null; // Inicializamos content como nullable

    if (createProductoDto.Imagen != null)
    {
        // Si hay una imagen, la convertimos a bytes
        using (MemoryStream ms = new MemoryStream())
        {
            await createProductoDto.Imagen.CopyToAsync(ms);
            content = ms.ToArray();
        }
    }

    // Crear el producto y asignar el contenido de la imagen
    var product = _mapper.Map<Product>(createProductoDto);
    product.Imagen = content;

        //si  el metodo CreatePdct retorna false
        if (!await _productRepository.CreateProduct(product))
    {
        return new BadRequestObjectResult("Algo salió mal al guardar el producto");
    }

    // Producto creado exitosamente
    return new OkObjectResult(product);
}



    // Método para obtener todos los productos
    public async Task<List<ProductResponseDto>> GetProducts()
    {
        // Obtenemos la lista de productos desde el repositorio de manera asíncrona
        var products = await _productRepository.GetProducts();

        // Inicializamos una lista para almacenar los ProductResponseDto
        var productDtos = new List<ProductResponseDto>();

        // Iteramos sobre cada producto obtenido del repositorio
        foreach (var product in products)
        {
            // Mapeamos el producto a un ProductResponseDto con AutoMapper
            var productDto = _mapper.Map<ProductResponseDto>(product);

            // Cconvertimos las imagenes del producto a base64 y la asignamos
            productDto.ImagenBase64 = ConvertToBase64(product.Imagen);

            // Agregamos el ProductResponseDto a la lista de resultados
            productDtos.Add(productDto);
        }

        return productDtos;
    }




  //Metodo obtener producto por ID
   public async Task<IActionResult> GetProduct(int productId)
{
    var product = await _productRepository.GetProduct(productId);
    if (product == null)
    {
        return new NotFoundObjectResult($"El producto con ID {productId} no fue encontrado.");
    }
    // Mapear el producto a ProductResponseDto
    var productResponseDto = _mapper.Map<ProductResponseDto>(product);
    // Convertir la imagen a Base64
    productResponseDto.ImagenBase64 = ConvertToBase64(product.Imagen);
    return new OkObjectResult(productResponseDto);
}




    //EndPoint Para Elimnar Producto
    public async Task<bool> DeleteProduct(int productId)
    {
        var product = await _productRepository.GetProduct(productId);
        if (product == null)
        {
            return false;
        }
        return await _productRepository.DeleteProduct(product);
    }


    //Actualizar
    public async Task<IActionResult> UpdateProduct(int productId, ProductDto productDto)
    {
        // Llama al método en el repositorio para actualizar el producto
        var result = await _productRepository.UpdateProduct(productId, productDto);

        // Verifica el resultado y devuelve la respuesta correspondiente
        if (result)
        {
            return new OkObjectResult($"El producto con ID {productId} ha sido actualizado exitosamente.");
        }
        else
        {
            return new NotFoundObjectResult($"No se encontró el producto con ID {productId}."); // Devuelve 404 si el producto no fue encontrado
        }
    }

    //Buscar por nombre
    public async Task<List<ProductResponseDto>> GetProductsByName(string name)
    {
        // Llamamos al método del repositorio para obtener los productos por nombre
        var products = await _productRepository.GetProductsByName(name);


        // Mapeamos la lista de productos al DTO de respuesta y la devolvemos
        return _mapper.Map<List<ProductResponseDto>>(products);
    }



    // Implementación del método en la clase del servicio
    public async Task<List<ProductResponseDto>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice, bool ordenAscendente)
    {
        // Llamamos al método  en el repositorio para obtener los productos dentro del rango de precios
        var products = await _productRepository.GetProductsByPriceRange(minPrice, maxPrice, ordenAscendente);

        // Mapeamos los productos a DTO de respuesta antes de devolverlos
        return _mapper.Map<List<ProductResponseDto>>(products);
    }

    //Metodo para buscar Descuentos

    public async Task<List<ProductResponseDto>> GetProductsByDiscount(bool ordenAscendente)
    {
        // Llamamos al método en el repositorio para obtener los productos con Descuento
        var products = await _productRepository.GetProductsByDiscount(ordenAscendente);

        // Mapeamos los productos al DTO de respuesta
        return _mapper.Map<List<ProductResponseDto>>(products);
    }




    // Método para convertir la imagen a una cadena Base64
    private string ConvertToBase64(byte[] image)
    {
        // si la imagen es nula o vacía
        if (image == null || image.Length == 0)
        {
            return null;
        }

        // cadena base64
        return Convert.ToBase64String(image);
    }


}
   




