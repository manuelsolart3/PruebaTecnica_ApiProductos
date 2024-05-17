using System.ComponentModel.DataAnnotations;
using System.Net;
using ApiProductos.Models;
using ApiProductos.Models.Dtos;
using ApiProductos.Repositories;
using ApiProductos.Repositories.IRespository;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using XAct.Messages;
using XAct.Users;

namespace ApiProductos.Services;

public sealed class UserService : IUserService
{
    private readonly IMapper _mapper;
    protected ResponseApi _responseApi;
    private readonly IUserRepository _userRepository; //accedemos al repoUsers


    //constructor con los dos parametros (repo y mapper)
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        this._responseApi = new ();
    }

    //Metodo para obtener todos los usuarios
    public async Task<ICollection<UserDto>> GetUsers()
    {
        // Obtenemos la lista de usuarios desde el repositorio llamando al método correspondiente
        var userList = await _userRepository.GetUsers();

        // Inicializamos una lista para almacenar los usuarios mapeados a UserDto
        var userListDto = new List<UserDto>();

        // Iteramos sobre cada usuario en la lista obtenida del repositorio
        foreach (var user in userList)
        {
            // Mapeamos cada usuario a un UserDto utilizando AutoMapper y lo agregamos a la lista userListDto
            userListDto.Add(_mapper.Map<UserDto>(user));
        }

        // Retornamos la lista userListDto que contiene los usuarios mapeados a UserDto
        return userListDto;
    }


    //Metodo para obtener por ID
    public async Task<IActionResult> GetUser(string Userid)
    {
        var userRepo = await _userRepository.GetUser(Userid);

        if (userRepo == null) 
        {
            return new NotFoundObjectResult($"El Usuario con el ID {Userid} no fue encontrado");
        }

        //Mapeamos el user a UserDto
        var userDto = _mapper.Map<UserDto>(userRepo);

        return new OkObjectResult(userDto);
    }

    //Metodo para registarar
    public async Task<UserDataDto> Register(UserRegisterDto userRegisterDto)
    {
        var response = new ResponseApi();
        UserDataDto registeredUser = null; // Inicializamos la variable

        try
        {
            // Verificar si el nombre de usuario es único
            bool isUniqueUser = await _userRepository.IsUniqueUser(userRegisterDto.NombreUsuario);
            if (!isUniqueUser)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages.Add("El nombre de usuario ya está en uso");
                return null; // Retornar null en caso de error
            }

            // Registrar el usuario y obtener el usuario registrado
            registeredUser = await _userRepository.Register(userRegisterDto);

            // Configurar la respuesta de éxito
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            response.Result = registeredUser; // Asigna el usuario registrado a la propiedad Result
        }
        catch (Exception ex)
        {
            // Configurar la respuesta de error
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.IsSuccess = false;
            response.ErrorMessages.Add("Error al registrar el usuario: " + ex.Message);
        }

        // Devolver el usuario registrado o null en caso de error
        return registeredUser;
    }









    public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        var response = new ResponseApi(); // Creamos una nueva instancia de ResponseApi

        // Llamamos al método Login del repositorio para intentar iniciar sesión
        var responseLogin = await _userRepository.Login(userLoginDto);

        // Si el usuario o el token devueltos están vacíos
        if (responseLogin.Usuario == null || string.IsNullOrEmpty(responseLogin.Token))
        {
            // Configuramos una respuesta de error en el ResponseApi
            response.StatusCode = HttpStatusCode.BadRequest;
            response.IsSuccess = false;
            response.ErrorMessages.Add("El nombre de usuario o la contraseña son incorrectos");
            return null; // Retornamos null en caso de error
        }

        // Configuramos la respuesta de éxito en el ResponseApi
        response.StatusCode = HttpStatusCode.OK;
        response.IsSuccess = true;
        response.Result = responseLogin;

        // Retornamos la respuesta de inicio de sesión
        return responseLogin;
    }




    public async Task<bool> IsUniqueUser(string userFind)
    {
        return await _userRepository.IsUniqueUser(userFind);
    }

}
   




