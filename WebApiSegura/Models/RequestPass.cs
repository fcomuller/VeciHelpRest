using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSegura.Models
{
    public class RequestPass
    {
        public int id_usuario { get; set; }
        public string claveAntigua { get; set; }
        public string claveNueva { get; set; }

    }
}