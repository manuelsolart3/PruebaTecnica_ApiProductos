using System.Text;
using ApiProductos.Data;
using ApiProductos.Models;
using ApiProductos.ProductsMapper;
using ApiProductos.Repositories;
using ApiProductos.Repositories.IRespository;
using ApiProductos.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Configuramos la cadena conexion a sql server
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
{
    //Configuramos el proveedor de la base de datos
    opciones.UseMySql(builder.Configuration.GetConnectionString("ConexionMysql"), new MySqlServerVersion("8.0"));
});

//Soporte para autenticacion con .NET identity
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

 
//Agregamos los Repo
builder.Services.AddScoped<IProductRepository, ProductRepository >();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//inicializamos la llave secreta
var Key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

//Agregamos el AutoMapper
builder.Services.AddAutoMapper(typeof(ProductsMapper));


//Aqui configuramos la Autenticacion
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    // Configuraciones del token JWT
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        // Parámetros de validación del token JWT
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


//agregamos los servicios
builder.Services.AddScoped<IProductService, ProductService >();
builder.Services.AddScoped<IUserService, UserService>();


// Add services to the container.
//Añadimos un perfil de cache
builder.Services.AddControllers(opcion =>
{
    opcion.CacheProfiles.Add("PorDefecto30", new CacheProfile() { Duration = 30});
}); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Define una nueva definición de seguridad llamada "Bearer" para Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        // Descripción de cómo usar la autenticación JWT con Swagger
        Description =
        "Autenticación JWT usando el esquema Bearer. \r\n\r\n " +
        "Ingresa la palabra 'Bearer' seguida de un [espacio] y después un token en el campo de abajo \r\n\r\n" +
        "Ejemplo: \"Bearer eysdsdas12sd \"",

        // Nombre del encabezado esperado para enviar el token JWT
        Name = "Authorization",

        // Ubicación esperada del token JWT en la solicitud HTTP
        In = ParameterLocation.Header,

        // Esquema de autenticación utilizado, que es Bearer
        Scheme = "Bearer"
    });

    // Agrega un requisito de seguridad que indica que todas las operaciones Swagger requieren el esquema de seguridad Bearer
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            // Especifica el esquema de seguridad Bearer como referencia
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                // Indica el esquema de autenticación utilizado, que es oauth2
                Scheme = "oauth2",
                
                // Nombre del encabezado esperado para enviar el token JWT
                Name = "Authorization",
                
                // Ubicación esperada del token JWT en la solicitud HTTP
                In = ParameterLocation.Header,
            },
            // Lista vacía que indica que no se requieren alcances adicionales para acceder a las operaciones protegidas
            new List<string>()
        }
    });
});



//Soporte para CORS

builder.Services.AddCors(P => P.AddPolicy("PoliticaCors",build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Soporte para CORS

app.UseCors("PoliticaCors");


app.UseAuthentication();

app.UseAuthorization();



app.MapControllers();

app.Run();
