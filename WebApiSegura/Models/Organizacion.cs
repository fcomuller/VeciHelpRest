using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VeciHelp.BD;

namespace WebApiSegura.Models
{
    public class Organizacion
    {

        //id del usuario administrador
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string nroEmergencia { get; set; }
        public int cantMinAyuda { get; set; }
        public string comuna { get; set; }
        public string ciudad { get; set; }
        public string provincia { get; set; }
        public string region { get; set; }
        public string pais { get; set; }
        public int idComuna { get; set; }
        public int idOrganizacion { get; set; }
        public string mensaje { get; set; }

        private BaseDatos bd;

        public Organizacion()
        {

        }

        public Organizacion M_OrganizacionLst(int idUsuario)
        {
            bd = new BaseDatos();

            return bd.p_OrganizacionLst(idUsuario);
        }

        public bool M_OrganizacionUpd(out string mensaje)
        {
            bd = new BaseDatos();
            return bd.p_OrganizacionUpd(this.idUsuario, this.nombre,this.nroEmergencia,this.cantMinAyuda,out mensaje);
        }

        public bool M_OrganizacionIns(int idComuna,string nombreOrg, out string mensaje, out int idOrganizacion)
        {
            bd = new BaseDatos();
            return bd.P_OrganizacionIns(idComuna,nombreOrg, out mensaje,out idOrganizacion);
        }
    }
}