using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize(Roles = "Administrador")]
    [RoutePrefix("api/v1/organizacion")]
    public class OrganizacionController : ApiController
    {
        [HttpGet]
        [Route("Get")]
        //metodo para crear usuarios
        public IHttpActionResult Get(int idUsuario)
        {
            Organizacion org = new Organizacion();

            org = org.M_OrganizacionLst(idUsuario);

            if (org != null)
            {
                return Ok(org);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("Put")]
        public IHttpActionResult Put(Organizacion org)
        {

            var respuesta = "error";

            if (org.M_OrganizacionUpd(out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }
    }
}
