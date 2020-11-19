using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{

    /* considerar que se usara el mismo metodo para las alertas en casa propia y alerts en casa vecino , por lo que si la alerta es en casa propia se debe enviar el id de usuario y en el idvecino se debe enviar el mismo id de usuario*/


    [Authorize(Roles = "Administrador,Usuario")]
    [RoutePrefix("api/v1/alerta")]
    public class AlertaController : ApiController
    {
        [HttpPost]
        [Route("Sospecha")]
        //metodo para crear usuarios
        public IHttpActionResult Sospecha(RequestAlerta alerta)
        {
            Alerta alert = new Alerta();

            var Tokens = alert.M_AlertaSospechaIns(alerta.idUsuario, alerta.coordenadas,alerta.texto);

            if (Tokens.Count > 0)
            {
                var mensaje =alert.SendNotification(Tokens.ToArray(), "Alerta por Sospecha", alerta.coordenadas);
                return Ok(mensaje);
            }
            else
                return NotFound();
        }

        [HttpPost]
        [Route("SOS")]
        //metodo para crear usuarios
        public IHttpActionResult SOS(RequestAlerta alerta)
        {
            Alerta alert = new Alerta();
            var Tokens = alert.M_AlertaSOSIns(alerta.idUsuario, alerta.idVecino);

            if (Tokens.Count>0)
            {
                var mensaje = alert.SendNotification(Tokens.ToArray(), "Alerta de Robo", alerta.coordenadas);
                return Ok(mensaje);
            }
            else
                return NotFound();
        }

        [HttpPost]
        [Route("Emergencia")]
        //metodo para crear alerta de emergencia
        public IHttpActionResult Emergencia(RequestAlerta alerta)
        {
            Alerta alert = new Alerta();
            var Tokens = alert.M_AlertaEmergenciaIns(alerta.idUsuario, alerta.idVecino);

            if (Tokens.Count > 0)
            {
                var mensaje = alert.SendNotification(Tokens.ToArray(), "Necesitan tu Ayuda!!", alerta.coordenadas);
                return Ok(mensaje);
            }
            else
                return NotFound();
        }

        [HttpPost]
        [Route("Acudir")]
        //metodo para acuidir a una alerta
        public IHttpActionResult Acudir(RequestAlerta alerta)
        {
            Alerta alert = new Alerta();
            var respuesta = string.Empty;

            if (alert.M_AcudirLlamadoIns(alerta.idUsuario, alerta.idAlerta, out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }

        [HttpPut]
        [Route("FinalizarAlerta")]
        //metodo para acuidir a una alerta
        public IHttpActionResult FinalizarAlerta(RequestAlerta alerta)
        {
            Alerta alert = new Alerta();
            var respuesta = string.Empty;

            if (alert.M_AcudirLlamadoUpd(alerta.idUsuario, alerta.idAlerta, out respuesta))
            {
                return Ok(respuesta);
            }
            else
                return Ok(respuesta);
        }

        [HttpGet]
        [Route("GetAll")]
        //metodo que lista las alertas activas
        public IHttpActionResult GetAll(int idUsuario)
        {
            Alerta alert = new Alerta();
            var respuesta = "Error";

            if (alert.M_AlertaLst(idUsuario)!=null)
            {

                return Ok(alert.M_AlertaLst(idUsuario));
            }
            else
                return Ok(respuesta);
        }

        [HttpGet]
        [Route("Get")]
        //metodo que lista una alerta por id
        public IHttpActionResult Get(int idAlerta, int IdUsuario)
        {
            Alerta alert = new Alerta();
            var respuesta = "Error";

            if (alert.M_AlertaById(idAlerta,IdUsuario) != null)
            {

                return Ok(alert.M_AlertaById(idAlerta, IdUsuario));
            }
            else
                return Ok(respuesta);
        }

    }
}
