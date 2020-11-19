using System;
using System.Net;
using System.Threading;
using System.Web.Http;
using VeciHelp.Models;
using VeciHelp.Security;
using WebApiSegura.Models;

namespace VeciHelp.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/v1/login")]
    public class LoginController : ApiController
    {
       
        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(RequestLogin login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            //instancio la clase loginrequest para llamar a metodo que valida el usuario
            RequestLogin lgn = new RequestLogin();
            Usuario usr = new Usuario();

            usr = lgn.Validarlogin(login);

            if (usr.existe==1)
            {
                    usr.token = TokenGenerator.GenerateTokenJwt(login.correo, usr.rolename);
                    return Ok(usr);
            }
            else if (usr.existe == 2)
            {
                usr.token = TokenGenerator.GenerateTokenJwt(login.correo, "Temporal");
                return Ok(usr);
            }
            else if (usr.existe == 0)
            {
                return NotFound();
            }
            
            // Acceso denegado
            return Unauthorized();
        }
    }
}
