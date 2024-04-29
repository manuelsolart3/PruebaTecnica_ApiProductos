using ApiProductos.Data;
using ApiProductos.ProductsMapper;
using ApiProductos.Repositories;
using ApiProductos.Repositories.IRespository;
using ApiProductos.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configuramos la cadena conexion a sql server
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
{
    //Configuramos el proveedor de la base de datos
    opciones.UseMySql(builder.Configuration.GetConnectionString("ConexionMysql"), new MySqlServerVersion("8.0"));
});

//Agregamos los Repo
builder.Services.AddScoped<IProductRepository, ProductRepository >();




//Agregamos el AutoMapper
builder.Services.AddAutoMapper(typeof(ProductsMapper));



//agregamos los servicios
builder.Services.AddScoped<IProductService, ProductService >();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
