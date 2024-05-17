using ApiProductos.Migrations;

namespace ApiProductos.Models.Dtos
{
    public class UserLoginResponseDto
    {
        //Respuesta una vez loggeado (para retornar los datos del usuario y el token)

        public  UserDataDto Usuario { get; set; }
        public string Token { get; set; }

    }
}
