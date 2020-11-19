using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VeciHelp.BD;

namespace VeciHelp.Models
{
    public class RequestEnrolar
    {
        public string correo { get; set; }
        public int idOrganizacion { get; set; }

        private BaseDatos bd;

        public bool M_CodigoVerificacionAdministradorGenera(string correo, int idOrganizacion, out string mensaje)
        {
            bd = new BaseDatos();

            return bd.p_CodigoVerificacionAdministradorGenera(correo, idOrganizacion, out mensaje);
        }
    }

    
}