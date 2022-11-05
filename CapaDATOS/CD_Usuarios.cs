using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDATOS
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.Conn))
                {
                    string query = "SELECT idUsuario, Nombres, Apellidos, Correo, Clave, Restablecer, Activo, fechaRegistro FROM USUARIO";

                    SqlCommand cmd = new SqlCommand(query, oConexion);

                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) { 
                        while (dr.Read())
                        {
                            // Creamos un objeto json
                            lista.Add(
                                new Usuario()
                                {
                                    idUsuario = Convert.ToInt32(dr["idUsuario"]),
                                    Nombres = Convert.ToString(dr["Nombres"]),
                                    Apellidos = Convert.ToString(dr["Apellidos"]),
                                    Correo = Convert.ToString(dr["Correo"]),
                                    Clave = Convert.ToString(dr["Clave"]),
                                    Restablecer = Convert.ToBoolean(dr["Restablecer"]),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                }    
                            );
                        } 
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Usuario>();
                throw;
            }

            return lista;
        }

        public int GuardarUsuario(Usuario obj, out string Mensaje)
        {
            int idAutogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using(SqlConnection oConexion = new SqlConnection(Conexion.Conn))
                {
                    SqlCommand cmd = new SqlCommand("sp_GuardarUsuario", oConexion);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    idAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch(Exception ex)
            {
                idAutogenerado = 0;
                Mensaje = ex.Message;
            }

            return idAutogenerado;
        }

        public bool EditarUsuario(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.Conn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.idUsuario);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }catch(Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;

        }

        public bool EliminarUsuario(int idUsuario, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try {
                using (SqlConnection oConexion = new SqlConnection(Conexion.Conn))
                {
                    SqlCommand cmd = new SqlCommand("DELETE TOP(1) FROM USUARIO WHERE IdUsuario = @idUsuario", oConexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.CommandType = CommandType.Text;
                    oConexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false; // Si el total de filas afectadas es mayor a 0 asigna valor true
                }
            } catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
    }
}
