using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class RequestFotoUpd
    {
        public int id_usuario { get; set; }
        public string Foto { get; set; }
    }
}