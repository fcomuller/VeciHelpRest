using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [RoutePrefix("api/v1/user")]
    public class UsuarioController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("CrearUser")]
        //metodo para crear usuarios
        public IHttpActionResult CrearUser(Usuario usuario)
        {

            var respuesta = "error";

            if (usuario.M_UsuarioIns(out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ValidarCodigo")]
        //metodo para crear usuarios
        public IHttpActionResult ValidarCodigo(Usuario usuario)
        {
            var respuesta = "Error inesperado";
            var tipoUsuario = 0;

            if (usuario.M_ValidaCorreoyCodigo(out respuesta,out tipoUsuario))
            {
                var obj = new
                {
                    resp = respuesta,
                    tipo = tipoUsuario,
                };
                return Ok(obj);
            }
            else
                return Ok(respuesta);
        }

        [Authorize(Roles = "Administrador,Usuario")]
        [HttpGet]
        [Route("GetUserId")]
        //metodo que retorna los datos de un usuario by id
        public IHttpActionResult GetUserId(int idUsuario)
        {
            Usuario usr = new Usuario();

            usr = usr.M_UsuarioGet(idUsuario);

            if (usr!=null)
            {
                return Ok(usr);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Administrador,Usuario")]
        [HttpPut]
        [Route("UpdateUser")]
        //metodo para actualizar datos del usuario
        public IHttpActionResult UpdateUser(Usuario usuario)
        {

            var respuesta = "error";

            if (usuario.M_UsuarioUpd(out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }

        [Authorize(Roles = "Administrador,Usuario")]
        [HttpPut]
        [Route("UpdatePhoto")]
        //metodo para actualizar foto de perfil
        public IHttpActionResult UpdatePhoto(RequestFotoUpd fotoUpd)
        {
            Usuario usr = new Usuario();
            var respuesta = "error";

            if (usr.M_FotoUsuarioUpd(fotoUpd.id_usuario,fotoUpd.Foto, out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }

        [Authorize(Roles = "Administrador,Usuario,Temporal")]
        [HttpPut]
        [Route("UpdatePass")]
        //metodo para actualizar foto de perfil
        public IHttpActionResult UpdatePass(RequestPass password)
        {
            var respuesta = "error";
            Usuario usr = new Usuario();
            if (usr.M_ClaveUsuarioUpd(password.id_usuario, password.claveAntigua,password.claveNueva, out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return NotFound();
        }

        [Authorize(Roles = "Administrador,Usuario")]
        [HttpGet]
        [Route("GetListaVecinoId")]
        //metodo que retorna listado de vecinos asociados a un usuario  **Listo
        public IHttpActionResult GetListaVecinoId(int id)
        {
            var usrLst = new List<Usuario>();

            Usuario usr = new Usuario();

            usrLst = usr.M_AsociacionVecinosLst(id);

            return Ok(usrLst);
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("RecuperarClave")]
        //metodo que retorna los datos de un usuario by id
        public IHttpActionResult RecuperarClave(string correo)
        {
            var respuesta = "error";

            Usuario usr = new Usuario();
            if (usr.M_RecuperarClaveUsuarioGet(correo, out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }
    }
}
