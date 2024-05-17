using ApiProductos.Models;
using ApiProductos.Models.Dtos;

namespace ApiProductos.Repositories.IRespository
{
    public interface IUserRepository
    {

        //Metodo para obtener la lista de usuarios
        Task<ICollection<AppUser>> GetUsers();

        Task<AppUser> GetUser(string Userid);

        //validar si ya hay un usuario creado con el nombre
        Task<bool> IsUniqueUser(string userFind);

        //Meotod para realizar el login usando las credenciales del Dto devolviendo una respuesta de login que contiene JWT
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);

        //Metodo para registar un usuario
        Task<UserDataDto> Register(UserRegisterDto userRegisterDto);
       
    }
}
