using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using ApiProductos.Data;
using ApiProductos.Models;
using ApiProductos.Models.Dtos;
using ApiProductos.Repositories.IRespository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using XSystem.Security.Cryptography;

namespace ApiProductos.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _bd;


        //Almacenamos la clave secreta que usaremos para firmar el token JWT
        private string KeySecret;

        //Ayudantes o helpers
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        
        //como parametro añadimos Iconfig como parametro para obtener el valor de la KS desde ApiSettings
        public UserRepository(ApplicationDbContext bd, IConfiguration config,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _bd = bd;
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;

            //Obtenemos el valor de ApiSettings
            KeySecret = config.GetValue<string>("ApiSettings:Secret");
        }



        public async Task<AppUser> GetUser(string Userid)

        {

           return  _bd.AppUser.FirstOrDefault(u => u.Id == Userid);
        }

        public async Task<ICollection<AppUser>> GetUsers()
        {
            return await  _bd.AppUser.OrderBy(u => u.Id).ToListAsync();
        }


        public async Task<bool> IsUniqueUser(string userFind)
        {
           var UserBd = _bd.AppUser.FirstOrDefault(u => u.UserName == userFind);
            if (UserBd == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<UserDataDto> Register(UserRegisterDto userRegisterDto)
        {
            // Crear un nuevo objeto de usuario con los datos del DTO
            AppUser user = new AppUser()
            {
                UserName = userRegisterDto.NombreUsuario,  // Nombre de usuario
                Email = userRegisterDto.NombreUsuario,     // Email (en este caso, igual al nombre de usuario)
                NormalizedEmail = userRegisterDto.NombreUsuario.ToUpper(),  // Normalización del email
                Nombre = userRegisterDto.Nombre            // Nombre del usuario
            };

            // Utilizar UserManager para crear el usuario en el sistema de autenticación
            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (result.Succeeded)
            {
                // Verificar si es la primera vez que se ejecuta la aplicación y crear roles de usuario si no existen
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }

                // Asignar el rol de 'admin' al nuevo usuario
                await _userManager.AddToRoleAsync(user, "admin");

                // Obtener el usuario recién creado del contexto de la base de datos
                var UserReturn = _bd.AppUser.FirstOrDefault(u => u.UserName == userRegisterDto.NombreUsuario);

                // Mapear el usuario a un DTO y devolverlo como resultado exitoso
                return _mapper.Map<UserDataDto>(UserReturn);
            }
            else
            {
                // Si la creación del usuario falla, devolver un objeto UserDataDto vacío
                return new UserDataDto();
            }
        }


        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            //se llama al metodo Encrypted para encriptar la pass  proporcionada por userLoginDto
            //var PassEcnrypted = Encrypted(userLoginDto.Password);

            //se busca en la BD un usuario donde su Nombre y pass coincidan con los de userLoginDto
            var user = _bd.AppUser.FirstOrDefault(
                u => u.UserName.ToLower() == userLoginDto.NombreUsuario.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

            if (user == null || isValid == false )
            {//devolvemos un token vacio si no se encuentra a un usuario
                return new UserLoginResponseDto()
                {
                    Token = "",
                    Usuario = null
                };
            }
            //Con este metodo obtenemos los roles del usuario
            var roles = await _userManager.GetRolesAsync(user);

            //Creamos una instancia del gestor de tokens (se utiliza para generar y mantener T)
            var TokenManager = new JwtSecurityTokenHandler();
            //Obtenemos los bytes de la KS
            var Key = Encoding.ASCII.GetBytes(KeySecret);


            //Creamos un descrpitor de Token que contiene la informacion necesaria para generarlo
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                //especificamos el sujeto del token 
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //incluimos dos reclamaciones  nombre y rol
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()),

                }),
                //establecemos la fecha de expiracion
                Expires = DateTime.UtcNow.AddDays(7),

                //configuramos las creedenciales de firma
                SigningCredentials = new (new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
            };

            //Creamos el token utilizando el descriptor recien configurado
            var token = TokenManager.CreateToken(TokenDescriptor);

            //Creamos un objeto UserLoginRDto que contiene el token y el usuario encontrado en la BD
            UserLoginResponseDto userLoginResponseDto = new UserLoginResponseDto()
            {
                //asignamos lo encontrado a los campos que tiene la respuestadelLogin
                Token = TokenManager.WriteToken(token),
                Usuario = _mapper.Map<UserDataDto>(user) 
            };

            return userLoginResponseDto;

        }


        //Método para encriptar contraseña con MD5 se usa tanto en el Acceso como en el Registro
        //public static string Encrypted(string valor)
        //{
        //    MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        //    byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
        //    data = x.ComputeHash(data);
        //    string resp = "";
        //    for (int i = 0; i < data.Length; i++)
        //        resp += data[i].ToString("x2").ToLower();
        //    return resp;
        //}


         public async Task<bool> Guardar()
        {
            return await _bd.SaveChangesAsync() >= 0;
        }
    }
}
