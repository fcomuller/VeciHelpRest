using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using VeciHelp.BD;

namespace WebApiSegura.Models
{
    public class Usuario
    {
        public int id_Usuario { get; set; }
        public string organizacion { get; set; }
        public string correo { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string rut { get; set; }
        public char digito { get; set; }
        public string Foto { get; set; }
        public string antecedentesSalud { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int celular { get; set; }
        public string direccion { get; set; }
        public string clave { get; set; }
        public string codigoVerificacion { get; set; }
        public int id_TipoUsuario { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string nombreCreador { get; set; }
        public string numeroEmergencia { get; set; }
        public string token { get; set; }
        public string mensaje { get; set; }
        public int existe { get; set; }
        public string rolename { get; set; }


        private BaseDatos bd;

        public Usuario()
        {

        }


        public Usuario M_UsuarioGet(int id_user)
        {
            bd = new BaseDatos();
            return bd.p_UsuarioGet(id_user);
        }

        public List<Usuario> M_UsuariosLst(int idUsuario)
        {
            bd = new BaseDatos();

            return bd.p_UsuariosLst(idUsuario);
        }

        public bool M_UsuarioIns(out string mensaje)
        {
            bd = new BaseDatos();
            return bd.p_UsuarioIns(this.correo, this.codigoVerificacion, this.nombre, this.apellido, this.rut, this.digito, this.antecedentesSalud, this.fechaNacimiento, this.celular, this.direccion, this.clave,this.Foto, out  mensaje);
        }

        public bool M_ValidaCorreoyCodigo(out string mensaje,out int tipoUsuario)
        {
            bd = new BaseDatos();
            return bd.p_ValidaCorreoyCodigo(this.correo, this.codigoVerificacion, out mensaje,out tipoUsuario);
        }

        public bool M_UsuarioUpd(out string mensaje)
        {
            bd = new BaseDatos();
            return bd.p_UsuarioUpd(this.id_Usuario, this.nombre, this.apellido, this.rut, this.digito, this.antecedentesSalud, this.fechaNacimiento, this.celular, this.direccion, out  mensaje);
        }

        public bool M_FotoUsuarioUpd(int idTipoUsuario,string foto, out string mensaje)
        {
            bd = new BaseDatos();
            return bd.p_FotoUsuarioUpd(idTipoUsuario, foto, out  mensaje);
        }

        public bool M_ClaveUsuarioUpd(int id_usuario, string claveAntigua,string claveNueva, out string mensaje)
        {
            bd = new BaseDatos();
            return bd.p_ClaveUsuarioUpd(id_usuario, claveAntigua, claveNueva, out mensaje);
        }

        public bool M_RecuperarClaveUsuarioGet(string correo, out string mensaje)
        {
            bd = new BaseDatos();
            return bd.p_RecuperarClaveUsuarioGet(correo, out mensaje);
        }

        public List<Usuario> M_AsociacionVecinosLst(int idUsuario)
        {
            bd = new BaseDatos();

            return bd.p_AsociacionVecinosLst(idUsuario);
        }

        
    }
}