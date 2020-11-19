using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using WebApiSegura.Models;
using System.Drawing;

namespace VeciHelp.BD
{
    public class BaseDatos
    {
        #region Atributos

        System.Data.SqlClient.SqlConnection cnn;
        public String svr;
        public String dbs;
        public String usr;
        public String pss;
        public String connString;

        #endregion

        #region Constructores

        public BaseDatos()
        {
            cnn = new System.Data.SqlClient.SqlConnection();
            svr = "DESKTOP-U70SU7T\\VHELP";
            dbs = "VeciHelp";
            usr = "usrConsulta";
            pss = DecodificaBase64("Q1gqKTIwa0NpMnNsbQ==").ToString();
            connString = @"Data Source=" + svr + ";Initial Catalog=" + dbs + ";User Id=" + usr + ";Password=" + pss + ";";
        }

        #endregion

        #region Metodos Base
        // Decodificar
        public static string DecodificaBase64(string str)
        {
            try
            {
                byte[] decbuff = Convert.FromBase64String(str);
                return System.Text.Encoding.UTF8.GetString(decbuff);
            }
            catch (Exception e)
            {
                { return ""; }
            }
        }

        public bool Open()
        {
            bool Funciona;
            Funciona = false;
            try
            {
                cnn.ConnectionString = connString;
                cnn.Open();
                Funciona = true;
            }
            catch (Exception)
            {
                throw;
            }
            return Funciona;
        }

        public void Close()
        {
            try
            {
                cnn.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Login

        //metodo que valida si el correo y contraseña son correctos y retorna el tipo de usuario
        public Usuario P_Login(string correo, string clave,string tokenFirebase)
        {
            Usuario usr = new Usuario();

            String _sql = string.Format("P_Login");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@correo", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@clave", SqlDbType.VarChar, 200);
                    sqlComm.Parameters.Add("@TokenFirebase", SqlDbType.VarChar, 300);
                    sqlComm.Parameters.Add("@Existe", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@Id_Usuario ", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@RoleName ", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@CorreoOUT ", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@Nombre", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@Apellido ", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@Rut ", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@Digito ", SqlDbType.VarChar, 1).Direction = ParameterDirection.Output;
                    //sqlComm.Parameters.Add("@Foto ", SqlDbType.VarChar, 2147483647).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@AntecedentesSalud ", SqlDbType.VarChar, 2147483647).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@FechaNacimiento ", SqlDbType.Date).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@Celular ", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@Direccion ", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@NumeroEmergencia ", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@Mensaje ", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    
                    sqlComm.Parameters[0].Value = correo;
                    sqlComm.Parameters[1].Value = clave;
                    sqlComm.Parameters[2].Value = tokenFirebase;

                    sqlComm.ExecuteNonQuery();

                    int.TryParse(sqlComm.Parameters[3].Value.ToString(), out int temp_Existe);
                    usr.existe = temp_Existe;
                    if (usr.existe==1)
                    {
                        
                        int.TryParse(sqlComm.Parameters[4].Value.ToString(), out int temp_Id_Usuario);
                        usr.id_Usuario = temp_Id_Usuario;
                        usr.rolename = sqlComm.Parameters[5].Value.ToString();
                        usr.correo = sqlComm.Parameters[6].Value.ToString();
                        usr.nombre = sqlComm.Parameters[7].Value.ToString();
                        usr.apellido = sqlComm.Parameters[8].Value.ToString();
                        usr.rut = sqlComm.Parameters[9].Value.ToString();
                        usr.digito = Char.Parse(sqlComm.Parameters[10].Value.ToString());
                        //usr.Foto = sqlComm.Parameters[11].Value.ToString();
                        usr.antecedentesSalud = sqlComm.Parameters[11].Value.ToString();
                       
                        DateTime.TryParse(sqlComm.Parameters[12].Value.ToString(), out DateTime temp_fechaNac);
                        usr.fechaNacimiento = temp_fechaNac;
                        int.TryParse(sqlComm.Parameters[13].Value.ToString(), out int temp_Celular);
                        usr.celular = temp_Celular;
                        usr.direccion = sqlComm.Parameters[14].Value.ToString();
                        usr.numeroEmergencia = sqlComm.Parameters[15].Value.ToString();
                        usr.mensaje = sqlComm.Parameters[16].Value.ToString();
                    }
                    else if (usr.existe == 2)
                    {
                        usr.mensaje= sqlComm.Parameters[16].Value.ToString();
                        int.TryParse(sqlComm.Parameters[4].Value.ToString(), out int temp_Id_Usuario);
                        usr.id_Usuario = temp_Id_Usuario;
                    }
                    

                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }
            //devuelvo el objeto response ya lleno
            return usr;
        }
        #endregion

        #region Administrador

        //metodo para que el administrador se registre, esto debe hacerse posterior a ser enrolado
        public bool p_UsuarioAdministradorIns(string correo, string codigoVerificacion,string nombre,string apellido,string rut,char digito,string antecedentesSalud,DateTime fechaNacimiento, int celular,string direccion,string clave,string foto, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_UsuarioAdministradorIns");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Correo", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@CodigoVerificacion", SqlDbType.VarChar, 10);
                    sqlComm.Parameters.Add("@Nombre", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@Apellidos", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@Rut", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Digito", SqlDbType.VarChar, 1);
                    sqlComm.Parameters.Add("@AntecedentesSalud", SqlDbType.VarChar, 500);
                    sqlComm.Parameters.Add("@FechaNacimiento", SqlDbType.Date);
                    sqlComm.Parameters.Add("@Celular", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Direccion", SqlDbType.VarChar, 500);
                    sqlComm.Parameters.Add("@Clave", SqlDbType.VarChar, 200);
                    sqlComm.Parameters.Add("@Foto", SqlDbType.VarChar, 2147483647);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;


                    sqlComm.Parameters[0].Value = correo;
                    sqlComm.Parameters[1].Value = codigoVerificacion;
                    sqlComm.Parameters[2].Value = nombre;
                    sqlComm.Parameters[3].Value = apellido;
                    sqlComm.Parameters[4].Value = rut;
                    sqlComm.Parameters[5].Value = digito;
                    sqlComm.Parameters[6].Value = antecedentesSalud;
                    sqlComm.Parameters[7].Value = fechaNacimiento;
                    sqlComm.Parameters[8].Value = celular;
                    sqlComm.Parameters[9].Value = direccion;
                    sqlComm.Parameters[10].Value = clave;
                    sqlComm.Parameters[11].Value = foto;

                    sqlComm.ExecuteNonQuery();
                    mensaje = sqlComm.Parameters[12].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }
        
        //metodo con el que el administrador enrola a los usuarios(internamente se envia un correo con un codigo de verificacion)
        public bool p_CodigoVerificacionUsuarioGenera(string correo, int idUsuarioCreador, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_CodigoVerificacionUsuarioGenera");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Correo", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = correo;
                    sqlComm.Parameters[1].Value = idUsuarioCreador;

                    sqlComm.ExecuteNonQuery();
                    mensaje = sqlComm.Parameters[2].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }

        //metodo con el cual el administrador asocia los usuarios con los vecinos cercanos
        public bool p_AsociacionVecinoIns(int idUsuario,int idVecino,int idAdmin, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_AsociacionVecinoIns");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Id_Vecino", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Id_Administrador", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = idVecino;
                    sqlComm.Parameters[2].Value = idAdmin;

                    sqlComm.ExecuteNonQuery();

                    mensaje = sqlComm.Parameters[3].Value.ToString();

                    this.Close();
                    return true;
                 }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }

        //metodo con el cual el administrador elimina una asociaciond e vecino cercano de un usuario especifico
        public bool p_AsociacionVecinoDel(int idUsuario, int idVecino, int idAdmin, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_AsociacionVecinoDel");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Id_Vecino", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Id_Administrador", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = idVecino;
                    sqlComm.Parameters[2].Value = idAdmin;

                    sqlComm.ExecuteNonQuery();

                    mensaje = sqlComm.Parameters[3].Value.ToString();
                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }

        //metodo que retora un usuario por correo
        public List<Usuario> p_AsociacionVecinosByCorreoLst(string correo)
        {
            List<Usuario> usrLst = new List<Usuario>();

            String _sql = string.Format("p_AsociacionVecinosByCorreoLst");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Correo", SqlDbType.VarChar);
                    sqlComm.Parameters[0].Value = correo;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Usuario usr = new Usuario();
                            usr.id_Usuario = int.Parse(dr[1].ToString());
                            usr.nombre = dr[2].ToString();
                            usr.apellido = dr[3].ToString();
                            usr.direccion = dr[4].ToString();
                            usr.celular = int.Parse(dr[5].ToString());
                            usrLst.Add(usr);
                        }

                    }
                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }

            return usrLst;
        }

        //metodo que retora listado de vecinos disponibles para asignar a un id esfecifico
        public List<Usuario> p_UsuariosLst(int idUsuario)
        {
            List<Usuario> usrLst = new List<Usuario>();

            String _sql = string.Format("p_UsuariosLst");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters[0].Value = idUsuario;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Usuario usr = new Usuario();
                            usr.id_Usuario = int.Parse(dr[0].ToString());
                            usr.nombre = dr[1].ToString();
                            usr.apellido = dr[2].ToString();
                            usr.direccion = dr[3].ToString();
                            usr.celular = int.Parse(dr[4].ToString());
                            usr.Foto = dr[5].ToString();
                            usrLst.Add(usr);
                        }

                    }
                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }

            return usrLst;
        }

        //metodo que elimina un usuario
        public bool p_UsuarioDel(int idUsuario,out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_UsuarioDel");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = idUsuario;

                    sqlComm.ExecuteNonQuery();

                    mensaje = sqlComm.Parameters[1].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }

        //metodo que retorna los datos de un usuario en especifico por correo
        public Usuario p_UsuarioByCorreoGet(string  correo)
        {
            Usuario usr = new Usuario();

            String _sql = string.Format("p_UsuarioByCorreoGet");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Correo", SqlDbType.VarChar,100);
                    sqlComm.Parameters[0].Value = correo;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            usr.id_Usuario = int.Parse(dr[0].ToString());
                            usr.correo = dr[1].ToString();
                            usr.nombre = dr[2].ToString();
                            usr.apellido = dr[3].ToString();
                            usr.rut = dr[4].ToString();
                            usr.digito = char.Parse(dr[5].ToString());
                            usr.Foto = dr[6].ToString();
                            usr.antecedentesSalud = dr[7].ToString();
                            usr.fechaNacimiento = DateTime.Parse(dr[8].ToString());
                            usr.celular = int.Parse(dr[9].ToString());
                            usr.direccion = dr[10].ToString();
                            usr.numeroEmergencia = dr[11].ToString();
                        }
                    }
                    else
                        usr = null;

                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }

            return usr;
        }
        #endregion

        #region Usuarios
        //metodo que lista los vecinos cercanos de un usuario, para saber cuales son sus casas cercanas
        public List<Usuario> p_AsociacionVecinosLst(int idUsuario)
        {
            List<Usuario> usrLst = new List<Usuario>();

            String _sql = string.Format("p_AsociacionVecinosLst");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters[0].Value = idUsuario;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Usuario usr = new Usuario();
                            usr.id_Usuario = int.Parse(dr[0].ToString());
                            usr.nombre = dr[1].ToString();
                            usr.apellido = dr[2].ToString();
                            usr.direccion = dr[3].ToString();
                            usr.celular = int.Parse(dr[4].ToString());
                            usrLst.Add(usr);
                        }

                    }
                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }

            return usrLst;
        }

        //metodo que retorna los datos de un usuario en especifico por id
        public Usuario p_UsuarioGet(int idUsuario)
        {
            Usuario usr = new Usuario();

            String _sql = string.Format("p_UsuarioGet");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters[0].Value = idUsuario;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            usr.id_Usuario = int.Parse(dr[0].ToString());
                            usr.correo = dr[1].ToString();
                            usr.nombre = dr[2].ToString();
                            usr.apellido = dr[3].ToString();
                            usr.rut = dr[4].ToString();
                            usr.digito = char.Parse(dr[5].ToString());
                            usr.Foto = dr[6].ToString();
                            usr.antecedentesSalud = dr[7].ToString();
                            usr.fechaNacimiento = DateTime.Parse(dr[8].ToString());
                            usr.celular = int.Parse(dr[9].ToString());
                            usr.direccion = dr[10].ToString();
                            usr.numeroEmergencia = dr[11].ToString();
                        }
                    }
                    else
                        usr = null;

                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }

            return usr;
        }

        //metodo que valida si el correo y el codigo de verificacion son validos para crear el usuario
        public bool p_ValidaCorreoyCodigo(string correo, string codigoVerificacion, out string mensaje,out int tipoUsuario)
        {
            mensaje = string.Empty;
            tipoUsuario = 0;
            String _sql = string.Format("p_ValidaCorreoyCodigo");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Correo", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@CodigoVerificacion", SqlDbType.VarChar, 10);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@TipoUsuario", SqlDbType.Int).Direction = ParameterDirection.Output;


                    sqlComm.Parameters[0].Value = correo;
                    sqlComm.Parameters[1].Value = codigoVerificacion;

                    sqlComm.ExecuteNonQuery();
                    mensaje = sqlComm.Parameters[2].Value.ToString();
                    tipoUsuario = int.Parse(sqlComm.Parameters[3].Value.ToString());

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }


        //metodo para que el usuario se registre, esto debe hacerse posterior a ser enrolado por un administrador
        public bool p_UsuarioIns(string correo, string codigoVerificacion, string nombre, string apellido, string rut, char digito, string antecedentesSalud, DateTime fechaNacimiento, int celular, string direccion, string clave, string foto, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_UsuarioIns");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Correo", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@CodigoVerificacion", SqlDbType.VarChar, 10);
                    sqlComm.Parameters.Add("@Nombre", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@Apellidos", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@Rut", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Digito", SqlDbType.VarChar, 1);
                    sqlComm.Parameters.Add("@AntecedentesSalud", SqlDbType.VarChar, 500);
                    sqlComm.Parameters.Add("@FechaNacimiento", SqlDbType.Date);
                    sqlComm.Parameters.Add("@Celular", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Direccion", SqlDbType.VarChar, 500);
                    sqlComm.Parameters.Add("@Clave", SqlDbType.VarChar, 200);
                    sqlComm.Parameters.Add("@Foto", SqlDbType.VarChar, 2147483647);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;


                    sqlComm.Parameters[0].Value = correo;
                    sqlComm.Parameters[1].Value = codigoVerificacion;
                    sqlComm.Parameters[2].Value = nombre;
                    sqlComm.Parameters[3].Value = apellido;
                    sqlComm.Parameters[4].Value = rut;
                    sqlComm.Parameters[5].Value = digito;
                    sqlComm.Parameters[6].Value = antecedentesSalud;
                    sqlComm.Parameters[7].Value = fechaNacimiento;
                    sqlComm.Parameters[8].Value = celular;
                    sqlComm.Parameters[9].Value = direccion;
                    sqlComm.Parameters[10].Value = clave;
                    sqlComm.Parameters[11].Value = foto;

                    sqlComm.ExecuteNonQuery();
                    mensaje = sqlComm.Parameters[12].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }

        //Metodo con el cual un usuario puede actualizar sus datos personales
        public bool p_UsuarioUpd(int idUsuario, string nombre, string apellido, string rut, char digito, string antecedentesSalud, DateTime fechaNacimiento, int celular, string direccion, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_UsuarioUpd");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure; 

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Nombre", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@Apellidos", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@Rut", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Digito", SqlDbType.VarChar, 1);
                    sqlComm.Parameters.Add("@AntecedentesSalud", SqlDbType.VarChar, 500);
                    sqlComm.Parameters.Add("@FechaNacimiento", SqlDbType.Date);
                    sqlComm.Parameters.Add("@Celular", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Direccion", SqlDbType.VarChar, 500);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = nombre;
                    sqlComm.Parameters[2].Value = apellido;
                    sqlComm.Parameters[3].Value = rut;
                    sqlComm.Parameters[4].Value = digito;
                    sqlComm.Parameters[5].Value = antecedentesSalud;
                    sqlComm.Parameters[6].Value = fechaNacimiento;
                    sqlComm.Parameters[7].Value = celular;
                    sqlComm.Parameters[8].Value = direccion;

                    sqlComm.ExecuteNonQuery();

                    mensaje = sqlComm.Parameters[9].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }
        
        //metodo con el cual el usuario actualiza su foto de perfil
        public bool p_FotoUsuarioUpd(int idUsuario, string foto, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_FotoUsuarioUpd");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;
    
                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Foto", SqlDbType.VarChar, 2147483647);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = foto;

                    sqlComm.ExecuteNonQuery();
                    mensaje = sqlComm.Parameters[2].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }

        public bool p_ClaveUsuarioUpd(int idUsuario, string claveAntigua, string claveNueva, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_ClaveUsuarioUpd");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@ClaveAntigua", SqlDbType.VarChar, 200);
                    sqlComm.Parameters.Add("@ClaveNueva", SqlDbType.VarChar, 200);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = claveAntigua;
                    sqlComm.Parameters[2].Value = claveNueva;

                    sqlComm.ExecuteNonQuery();
                    mensaje = sqlComm.Parameters[3].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }

        //sp que recibe un correo y actualiza la clave de ese usuario
        public bool p_RecuperarClaveUsuarioGet(string correo, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_RecuperarClaveUsuarioGet");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@correo", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = correo;

                    sqlComm.ExecuteNonQuery();
                    mensaje = sqlComm.Parameters[1].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }
        #endregion

        #region Alertas

        //metodo que inserta una alerta por sospecha , tanto en casa propia como en casa vecino
        public List<string> P_AlertaSospechaIns(int idUsuario,string texto,string foto)
        {
            List<string> tokenFireBaseLst=new List<string>();

            String _sql = string.Format("p_AlertaSospechaIns");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@CoordenadaSospechosa", SqlDbType.VarChar,100);
                    sqlComm.Parameters.Add("@TextoSospecha", SqlDbType.VarChar, 2147483647);

                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = texto;
                    sqlComm.Parameters[2].Value = foto;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            tokenFireBaseLst.Add(dr[0].ToString());
                        }
                    }

                    this.Close();
                }
                return tokenFireBaseLst;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return tokenFireBaseLst;
            }
        }

        //metodo que inserta una alerta por SOS , tanto en casa propia como en casa vecino
        public List<string> P_AlertaSOSIns(int idUsuario,int idVecino)
        {
            List<string> tokenFireBaseLst = new List<string>();

            String _sql = string.Format("p_AlertaSOSIns");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Id_Vecino", SqlDbType.Int);

                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = idVecino;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            tokenFireBaseLst.Add(dr[0].ToString());
                        }
                    }

                    this.Close();
                }
                return tokenFireBaseLst;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return tokenFireBaseLst;
            }
        }

        //metodo que inserta una alerta por Emergencia , tanto en casa propia como en casa vecino
        public List<string> P_AlertaEmergenciaIns(int idUsuario, int idVecino)
        {
            List<string> tokenFireBaseLst = new List<string>();

            String _sql = string.Format("p_AlertaEmergenciaIns");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Id_Vecino", SqlDbType.Int);

                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = idVecino;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            tokenFireBaseLst.Add(dr[0].ToString());
                        }
                    }

                    this.Close();
                }
                return tokenFireBaseLst;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return tokenFireBaseLst;
            }
        }

        //metodo que inserta la participacion de un usuario en una alerta 
        public bool P_AcudirLlamadoIns(int idUsuario, int idAlerta, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_AcudirLlamadoIns");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Id_Alerta", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = idAlerta;

                    sqlComm.ExecuteNonQuery();

                    mensaje = sqlComm.Parameters[2].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }

        //metodo con el que un usuario finaliza una alerta
        public bool P_AcudirLlamadoUpd(int idUsuario, int idAlerta, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_AcudirLlamadoUpd");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Id_Alerta", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = idAlerta;

                    sqlComm.ExecuteNonQuery();

                    mensaje = sqlComm.Parameters[2].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }

        //metodo que lista las alertas activas en curso
        public List<Alerta> P_AlertaLst(int idUsuario)
        {
            List<Alerta> alertLst = new List<Alerta>();

            String _sql = string.Format("p_AlertaLst");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);

                    sqlComm.Parameters[0].Value = idUsuario;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Alerta alert = new Alerta();

                            alert.idAlerta = int.Parse(dr[0].ToString());
                            alert.fechaAlerta = DateTime.Parse(dr[1].ToString());
                            alert.horaAlerta = DateTime.Parse(dr[2].ToString());
                            alert.TipoAlerta = dr[3].ToString();
                            alert.nombreGenerador = dr[4].ToString();
                            alert.apellidoGenerador = dr[5].ToString();
                            alert.nombreAyuda = dr[6].ToString();
                            alert.apellidoAyuda = dr[7].ToString();
                            alert.coordenadaSospecha = dr[8].ToString();
                            alert.textoSospecha = dr[9].ToString();
                            alert.direccion = dr[10].ToString();
                            alert.organizacion = dr[11].ToString();
                            alert.nroEmergencia= dr[12].ToString();
                            alert.participantes = Int32.Parse(dr[13].ToString());
                            alert.opcionBoton = dr[15].ToString();
                            alert.idVecino = int.Parse(dr[16].ToString());
                            alert.antecedentesSalud= dr[17].ToString();
                            alertLst.Add(alert);
                        }
                    }
                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }
            return alertLst;
        }


        //metodo que lista las alertas activas en curso
        public Alerta P_AlertaById(int idAlerta, int idUsuario)
        {
            Alerta alert = new Alerta();

            String _sql = string.Format("p_AlertaLst");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Id_Alerta", SqlDbType.Int);


                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = idAlerta;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            alert.idAlerta = int.Parse(dr[0].ToString());
                            alert.fechaAlerta = DateTime.Parse(dr[1].ToString());
                            alert.horaAlerta = DateTime.Parse(dr[2].ToString());
                            alert.TipoAlerta = dr[3].ToString();
                            alert.nombreGenerador = dr[4].ToString();
                            alert.apellidoGenerador = dr[5].ToString();
                            alert.nombreAyuda = dr[6].ToString();
                            alert.apellidoAyuda = dr[7].ToString();
                            alert.coordenadaSospecha = dr[8].ToString();
                            alert.textoSospecha = dr[9].ToString();
                            alert.direccion = dr[10].ToString();
                            alert.organizacion = dr[11].ToString();
                            alert.nroEmergencia= dr[12].ToString();
                            alert.participantes = Int32.Parse(dr[13].ToString());
                            alert.foto = dr[14].ToString();
                            alert.opcionBoton = dr[15].ToString();
                            alert.idVecino = int.Parse(dr[16].ToString());
                            alert.antecedentesSalud = dr[17].ToString();
                        }
                    }
                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }
            return alert;
        }

        #endregion

        #region Organizacion

        //SP que muestra los datos de la organizacion al administrador
        public Organizacion p_OrganizacionLst(int idUsuario)
        {
            Organizacion org = new Organizacion();

            String _sql = string.Format("p_OrganizacionLst");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters[0].Value = idUsuario;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            org.nombre = dr[0].ToString();
                            org.nroEmergencia = dr[1].ToString();
                            org.cantMinAyuda = 0;
                            if (dr[2].ToString().Trim().Length!=0)
                            {
                                org.cantMinAyuda = int.Parse(dr[2].ToString());
                            }
                            org.comuna = dr[3].ToString();
                            org.ciudad = dr[4].ToString();
                            org.provincia = dr[5].ToString();
                            org.region = dr[6].ToString();
                            org.pais = dr[7].ToString();
                        }
                    }
                    else
                        org = null;

                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }

            return org;
        }


        //sp que permite editar datos de la organizacion al administrador
        public bool p_OrganizacionUpd(int idUsuario, string nombre, string nroEmergencia, int cantMinima, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_OrganizacionUpd");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Usuario", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Organizacion", SqlDbType.VarChar, 200);
                    sqlComm.Parameters.Add("@NumeroEmergencia", SqlDbType.VarChar,15);
                    sqlComm.Parameters.Add("@CantidadMinimaAyuda", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = idUsuario;
                    sqlComm.Parameters[1].Value = nombre;
                    sqlComm.Parameters[2].Value = nroEmergencia;
                    sqlComm.Parameters[3].Value = cantMinima;

                    sqlComm.ExecuteNonQuery();

                    mensaje = sqlComm.Parameters[4].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                this.Close();
                mensaje = e.Message.ToString();
                return false;
            }
        }

        #endregion

        #region VeciHelp

        public bool p_CodigoVerificacionAdministradorGenera(string correo, int idOrganizacion, out string mensaje)
        {
            mensaje = string.Empty;
            String _sql = string.Format("p_CodigoVerificacionAdministradorGenera");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Correo", SqlDbType.VarChar, 100);
                    sqlComm.Parameters.Add("@Id_Organizacion", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = correo;
                    sqlComm.Parameters[1].Value = idOrganizacion;

                    sqlComm.ExecuteNonQuery();
                    mensaje = sqlComm.Parameters[2].Value.ToString();

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }

        public List<Pais> P_PaisLst()
        {
            List<Pais> paisLst = new List<Pais>();

            String _sql = string.Format("p_PaisLst");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Pais pais = new Pais();

                            pais.idPais = int.Parse(dr[0].ToString());
                            pais.nombre = dr[1].ToString();
                            paisLst.Add(pais);
                        }
                    }
                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }
            return paisLst;
        }

        public List<RegionModel> P_RegionLst(int idPais)
        {
            List<RegionModel> regionLst = new List<RegionModel>();

            String _sql = string.Format("p_RegionLst");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Pais", SqlDbType.Int);
                    sqlComm.Parameters[0].Value = idPais;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RegionModel region = new RegionModel();

                            region.idRegion = int.Parse(dr[0].ToString());
                            region.nombre = dr[1].ToString();
                            regionLst.Add(region);
                        }
                    }
                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }
            return regionLst;
        }

        public List<Provincia> P_ProvinciaLst(int idRegion)
        {
            List<Provincia> privinciaLst = new List<Provincia>();

            String _sql = string.Format("p_ProvinciaLst");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Region", SqlDbType.Int);
                    sqlComm.Parameters[0].Value = idRegion;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Provincia provincia = new Provincia();

                            provincia.idProvincia = int.Parse(dr[0].ToString());
                            provincia.nombre = dr[1].ToString();
                            privinciaLst.Add(provincia);
                        }
                    }
                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }
            return privinciaLst;
        }

        public List<Ciudad> P_CiudadLst(int idProvincia)
        {
            List<Ciudad> ciudadLst = new List<Ciudad>();

            String _sql = string.Format("p_CiudadLst");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Provincia", SqlDbType.Int);
                    sqlComm.Parameters[0].Value = idProvincia;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Ciudad ciudad = new Ciudad();

                            ciudad.idCiudad = int.Parse(dr[0].ToString());
                            ciudad.nombre = dr[1].ToString();
                            ciudadLst.Add(ciudad);
                        }
                    }
                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }
            return ciudadLst;
        }

        public List<Comuna> P_ComunaLst(int idCiudad)
        {
            List<Comuna> comunaLst = new List<Comuna>();

            String _sql = string.Format("p_ComunaLst");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Ciudad", SqlDbType.Int);
                    sqlComm.Parameters[0].Value = idCiudad;

                    SqlDataReader dr = sqlComm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Comuna comuna = new Comuna();

                            comuna.idComuna = int.Parse(dr[0].ToString());
                            comuna.nombre = dr[1].ToString();
                            comunaLst.Add(comuna);
                        }
                    }
                    this.Close();
                }
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
            }
            return comunaLst;
        }


        public bool P_OrganizacionIns(int idComuna, string nombreOrg, out string mensaje,out int idOrganizacion)
        {
            mensaje = string.Empty;
            idOrganizacion = 0;

            String _sql = string.Format("p_OrganizacionIns");
            try
            {
                if (this.Open())
                {
                    SqlCommand sqlComm = new SqlCommand(_sql, cnn);
                    sqlComm.CommandType = CommandType.StoredProcedure;

                    sqlComm.Parameters.Add("@Id_Comuna", SqlDbType.Int);
                    sqlComm.Parameters.Add("@Organizacion", SqlDbType.VarChar,200);
                    sqlComm.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    sqlComm.Parameters.Add("@Id_Organizacion", SqlDbType.Int).Direction = ParameterDirection.Output;

                    sqlComm.Parameters[0].Value = idComuna;
                    sqlComm.Parameters[1].Value = nombreOrg;

                    sqlComm.ExecuteNonQuery();

                    mensaje = sqlComm.Parameters[2].Value.ToString();
                    idOrganizacion =int.Parse(sqlComm.Parameters[3].Value.ToString());

                    this.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException e)
            {
                //Logger.InformeErrores(maquina.ToString(), e.Message, "Insertar_Registro [BaseDatos]");
                this.Close();
                return false;
            }
        }
        #endregion
    }
}