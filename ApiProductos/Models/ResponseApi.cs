using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ApiProductos.Models
{
    public class ResponseApi
    {

        public ResponseApi()
        {
            ErrorMessages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
