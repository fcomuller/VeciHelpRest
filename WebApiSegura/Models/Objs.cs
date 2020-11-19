using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VeciHelp.BD;

namespace WebApiSegura.Models
{
    public class Pais
    {
        public int idPais { get; set; }
        public string nombre { get; set; }

        public static List<Pais> M_PaisLst()
        {
            BaseDatos bd = new BaseDatos();
            return bd.P_PaisLst();
        }
    }

    public class RegionModel
    {
        public int idRegion { get; set; }
        public string nombre { get; set; }

        public static List<RegionModel> M_RegionLst(int id)
        {
            BaseDatos bd = new BaseDatos();
            return bd.P_RegionLst(id);
        }
    }

    public class Provincia
    {
        public int idProvincia { get; set; }
        public string nombre { get; set; }

        public static List<Provincia> M_ProvinciaLst(int id)
        {
            BaseDatos bd = new BaseDatos();
            return bd.P_ProvinciaLst(id);
        }
    }

    public class Ciudad
    {
        public int idCiudad { get; set; }
        public string nombre { get; set; }

        public static List<Ciudad> M_CiudadLst(int id)
        {
            BaseDatos bd = new BaseDatos();
            return bd.P_CiudadLst(id);
        }
    }

    public class Comuna
    {
        public int idComuna { get; set; }
        public string nombre { get; set; }

        public static List<Comuna> M_ComunaLst(int id)
        {
           BaseDatos bd = new BaseDatos();
            return bd.P_ComunaLst(id);
        }
    }
}