using System.Runtime;
using AutoMapper;
using ApiProductos.Models.Dtos;
using ApiProductos.Models;

namespace ApiProductos.ProductsMapper
{
    public class ProductsMapper : Profile
    {
        //Constructor
        public ProductsMapper()
        {//Mapeo de Product a prDto y viceversa
            CreateMap<Product, ProductDto>();

            CreateMap<AppUser, UserDto>();

            

            //Mapeo de createPdTO a Pdct
            CreateMap<CreateProductoDto, Product>()
             .ForMember(dest => dest.Imagen, opt => opt.Ignore()); //Ignoramos la propiedad img durante el mappeo

            // Mapeo de Product a ProductResponseDto y viceversa
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.ImagenBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Imagen))); //convertimos los bytes de la imagen en base64
            
            CreateMap<ProductResponseDto, Product>();

            CreateMap<Product, GetAllFilters>()
                .ForMember(dest => dest.Imagen, opt => opt.MapFrom(src => Convert.ToBase64String(src.Imagen)))
                .ForMember(dest => dest.PrecioDescuento, opt => opt.MapFrom(src => CalcularPrecioDescuento(src.Precio, src.Descuento.Value)));
        }
      
        private static decimal? CalcularPrecioDescuento(decimal precio, decimal? descuento)
        {
            if (!descuento.HasValue) return null;
            return precio * (descuento / 100);
        }
    }
}
