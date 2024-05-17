using ApiProductos.Models;
using System.Net;
using ApiProductos.Models.Dtos;
using ApiProductos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProductos.Controllers
{
    [ApiController] //indica que es un controlador de API
    [Route("api/usuario")] //Ruta Base
    public class UsersController : ControllerBase //proporciona funcionalidades para los controllers
    {
        private readonly IUserService _UserService; //Contiene una instancia del servicio Ip

        //constructor con los dos parametros (repo y mapper)
        public UsersController(IUserService userService)
        {

            _UserService = userService;


        }



        //obtener todos
        [HttpGet("TotalUsuarios")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers()
        {
            var products = await _UserService.GetUsers();
            return Ok(products);
        }



        //Obtener por ID
        [HttpGet("{UserId}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string UserId)
        {
            var result = await _UserService.GetUser(UserId);

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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserRegisterDto userRegisterDto)
        {
            ResponseApi response = new ResponseApi();

            // Registrar el usuario
            UserDataDto registeredUser = await _UserService.Register(userRegisterDto);

            if (registeredUser != null)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                response.Result = registeredUser;
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Error al registrar el usuario");
            }

            return Ok(response);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] UserLoginDto userLoginDto)
        {
            ResponseApi response = new ResponseApi();

            var LoginResponse = await _UserService.Login(userLoginDto);

            if (LoginResponse != null )
            {
                response.StatusCode=HttpStatusCode.OK;
                response.IsSuccess = true;
                response.Result = LoginResponse;    
                
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Error al iniciar sesion verifica tu usuario y contraseña");
            }
            return Ok(response);
        }


    }
}
