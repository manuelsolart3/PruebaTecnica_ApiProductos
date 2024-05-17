using System.Threading.Tasks;
using ApiProductos.Models;
using ApiProductos.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

public interface IUserService  
{
 

    // Método para obtener todos los users
    Task<ICollection<UserDto>> GetUsers();

    // método para obtener un producto por su id
    Task<IActionResult> GetUser(string Userid);

    // Método para validar si ya hay un usuario creado con el
    Task<bool> IsUniqueUser(string userFind);

    // Método para realizar el login usando las credenciales del DTO y devolviendo una respuesta de login que contiene JWT
    Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);

    // Método para registrar un usuario
    Task<UserDataDto> Register(UserRegisterDto userRegisterDto);






}