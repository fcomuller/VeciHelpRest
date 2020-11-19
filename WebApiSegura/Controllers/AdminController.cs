using System.Collections.Generic;
using System.Web.Http;
using VeciHelp.Models;
using WebApiSegura.Models;

namespace VeciHelp.Controllers
{
    /// <summary>
    /// customer controller class for testing security token 
    /// </summary>
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("api/v1/admin")]
    public class AdminController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("CrearAdmin")]
        //metodo para crear usuarios  **Listo
        public IHttpActionResult CrearAdmin(Administrador adminis)
        {

            var respuesta = "error";

            if (adminis.M_UsuarioAdministradorIns(out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }

        
        [HttpPost]
        [Route("EnrolarUsr")]
        //metodo para enrolar usuarios     **Listo
        public IHttpActionResult EnrolarUsr(Administrador admin)
        {
            var respuesta = "error";

            if (admin.M_CodigoVerificacionUsuarioGenera(admin.Correo,admin.IdUsuarioCreador, out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }


        [HttpPost]
        [Route("InsAsocVecino")]
        //metodo que crea asociaciones de vecinos   **Listo
        public IHttpActionResult InsAsocVecino(RequestAsoc asoc)
        {
            Administrador adminis = new Administrador();
            var respuesta = "error";

            if (adminis.M_AsociacionVecinoIns(asoc.idUsuario, asoc.idVecino, asoc.idAdmin, out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }

        [HttpPost]
        [Route("DelAsocVecino")]
        //metodo que elimina asociaciones **Listo
        public IHttpActionResult DelAsocVecino(RequestAsoc asoc)
        {
            Administrador adminis = new Administrador();
            var respuesta = "error";

            if (adminis.M_AsociacionVecinoDel(asoc.idUsuario, asoc.idVecino, asoc.idAdmin, out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }

        [HttpGet]
        [Route("GetListaVecinoByCorreo")]
        //metodo que retorna listado de vecinos asociados a un usuario  **Listo
        public IHttpActionResult GetListaVecinoId(string correo)
        {
            var usrLst = new List<Usuario>();

            Administrador adm = new Administrador();

            usrLst = adm.M_AsociacionVecinosByCorreoLst(correo);

            return Ok(usrLst);
        }

        [HttpGet]
        [Route("GetUsuarios")]
        //metodo que retorna los vecinos que pueden ser asociados a un usuario en especifico
        public IHttpActionResult GetUsers(int idUsuario)
        {
            Usuario usr = new Usuario();
            List<Usuario> usrLst = new List<Usuario>();


            usrLst = usr.M_UsuariosLst(idUsuario);

            if (usrLst.Count!=0)
            {
                return Ok(usrLst);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetUsuarioByCorreo")]
        //metodo que retorna los datos de un usuario by id
        public IHttpActionResult GetUsuarioByCorreo(string correo)
        {
            Administrador adm = new Administrador();

            var usr = adm.M_UsuarioByCorreoGet(correo);

            if (usr != null)
            {
                return Ok(usr);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("EliminarUsuario")]
        //metodo que elimina (desactiva) usuarios
        public IHttpActionResult EliminarUsuario(int idUsuario)
        {
            Administrador adminis = new Administrador();
            var respuesta = "error";

            if (adminis.M_UsuarioDel(idUsuario, out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }

    }
}
