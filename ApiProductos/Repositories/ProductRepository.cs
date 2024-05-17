using ApiProductos.Data;
using ApiProductos.Models;
using ApiProductos.Models.Dtos;
using ApiProductos.Repositories.IRespository;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiProductos.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _bd;

        public ProductRepository(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        //Crear producto
        public async Task<bool> CreateProduct(Product product)
        {

            product.FechaCreacion = DateTime.Now;
            await _bd.Product.AddAsync(product);
            return await Guardar();
        }


        //Obtener todos los productos
        public async Task<List<Product>> GetProducts()
        {
            return await _bd.Product.OrderBy(c => c.Nombre).ToListAsync();
        }


        //Obtener por ID
        public async Task<Product> GetProduct(int ProductId)
        {//Realizamos una consulta a la BD
            return await _bd.Product.FirstOrDefaultAsync(c => c.Id == ProductId);
            //El metodo First nos ayuda a obtener el primer producto que coincida con el ID
        }


        //Eliminar producto
        public async Task<bool> DeleteProduct(Product product)
        {
            _bd.Product.Remove(product);
            return await Guardar();
        }


        //actualizar
        public async Task<bool> UpdateProduct(int productId, ProductDto productDto)
        //se busca el producto en la BD
        {
            var existingProduct = await _bd.Product.FindAsync(productId);

            if (existingProduct == null)
            {
                return false; // No se encontró el producto con el ID especificado
            }

            // Actualizamos solo los campos que se hayan proporcionado en el DTO
            if (!string.IsNullOrEmpty(productDto.Nombre))
            {
                existingProduct.Nombre = productDto.Nombre;
            }

            if (!string.IsNullOrEmpty(productDto.Descripcion))
            {
                existingProduct.Descripcion = productDto.Descripcion;
            }

            if (productDto.Descuento != null)
            {
                existingProduct.Descuento = productDto.Descuento.Value;
            }


            // Actualizar el precio si se proporciona en el DTO
            if (productDto.Precio != null)
            {
                existingProduct.Precio = productDto.Precio.Value;
            }

            // Actualizamos el Descuento si se proporciona en el DTO
            if (productDto.Descuento != null)
            {
                existingProduct.Descuento = productDto.Descuento.Value;
            }


            _bd.Product.Update(existingProduct);
            return await Guardar();
        }


        //Buscar por nombre
        public async Task<List<Product>> GetProductsByName(string name)
        {
            // Creamos una consulta IQueryable<Product> basada en la tabla de productos _bd.Product
            IQueryable<Product> query = _bd.Product;

            // si es correcto
            if (!string.IsNullOrEmpty(name))
            {
                //si es correcto agregamos sentencia where
                // para filtrar los productos cuyo nombre contenga el valor proporcionado
                query = query.Where(p => p.Nombre.Contains(name));
            }

            // Ejecutamos la consulta y convertimos los resultados en una lista de productos 
            return await query.ToListAsync();
        }



        //Buscar por rango de precios
        public async Task<List<Product>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice, bool ordenAscendente)
        {
            // Creamos una consulta IQueryable para filtrar los productos por el rango de precios especificado
            IQueryable<Product> query = _bd.Product.Where(p => p.Precio >= minPrice && p.Precio <= maxPrice);

            // ordenamos de las dos formas
            if (ordenAscendente)
            {
                // Ordenamos los productos de manera ascendente según el precio
                query = query.OrderBy(p => p.Precio);
            }
            else
            {
                // Ordenamos los productos de manera descendente según el precio
                query = query.OrderByDescending(p => p.Precio);
            }

            // devolvemos los productos resultantes como una lista
            return await query.ToListAsync();
        }

        //link

        //Buscar Descuentos
        public async Task<List<Product>> GetProductsByDiscount(bool ordenAscendente)
        {
            // Obtenemos los productos y los ordenamos por el descuento en orden ascendente o descendente 
            var query = ordenAscendente ? _bd.Product.OrderBy(p => p.Descuento) : _bd.Product.OrderByDescending(p => p.Descuento);

            return await query.ToListAsync();
        }



        //Metodo GetAll
        public async Task<List<Product>> GetAll(decimal minPrice, decimal maxPrice)
        {
            return await _bd.Product.OrderBy(c => c.Nombre).ToListAsync();
            // Creamos una consulta IQueryable para filtrar los productos por el rango de precios especificado
            IQueryable<Product> queryP = _bd.Product.Where(p => p.Precio >= minPrice && p.Precio <= maxPrice);

            var queryD = _bd.Product.OrderBy(p => p.Descuento);
           

            // devolvemos los productos resultantes como una lista
            return await queryD.ToListAsync();
        }



        //guardar
        public async Task<bool> Guardar()
        {
            return await _bd.SaveChangesAsync() >= 0;
        }

      

        public async Task<List<Product>> SearchProducts(string nameFilter, decimal? minPrice, decimal? maxPrice, decimal? discount)
        {
            IQueryable<Product> queryable = _bd.Product;

            if (!string.IsNullOrEmpty(nameFilter)) queryable = queryable.Where(product => product.Nombre.Contains(nameFilter));
            if (minPrice.HasValue) queryable = queryable.Where(product => product.Precio >= minPrice);
            if (maxPrice.HasValue) queryable = queryable.Where(product => product.Precio <= maxPrice);
            if (discount.HasValue) queryable = queryable.Where(product => product.Descuento <= discount);

            return await queryable
                        .OrderBy(product => product.Nombre)
                        .ToListAsync();
        }
    }
}
