using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CapaDATOS
{
    public class CD_Producto
    {
        public List<Producto> ListarProductos()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.Conn))
                {
                    StringBuilder SB = new StringBuilder();

                    SB.AppendLine("SELECT PRD.idProducto, PRD.Nombre, PRD.Descripcion, MAR.idMarca, MAR.Descripcion Marca");
                    SB.AppendLine(", CAT.idCategoria, CAT.Descripcion Categoria, PRD.Precio, PRD.Stock, PRD.rutaImagen");
                    SB.AppendLine(", PRD.nombreImagen, PRD.Activo FROM PRODUCTO PRD");
                    SB.AppendLine(" INNER JOIN MARCA MAR ON PRD.idMarca = MAR.idMarca");
                    SB.AppendLine(" INNER JOIN CATEGORIA CAT ON CAT.idCategoria = PRD.idCategoria");

                    SqlCommand cmd = new SqlCommand(SB.ToString(), oConexion);

                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // Creamos un objeto json
                            lista.Add(
                                new Producto()
                                {
                                    idProducto = Convert.ToInt32(dr["idProducto"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    oMarca = new Marca() { idMarca = Convert.ToInt32(dr["idMarca"]), Descripcion = dr["Marca"].ToString() },
                                    oCategoria = new Categoria() { idCategoria = Convert.ToInt32(dr["idCategoria"]), Descripcion = dr["Categoria"].ToString() },
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-CL")),
                                    Stock = Convert.ToInt32(dr["Stock"]),
                                    rutaImagen = dr["rutaImagen"].ToString(),
                                    nombreImagen = dr["nombreImagen"].ToString(),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                }
                            );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Producto>();
                throw;
            }

            return lista;
        }

        public int GuardarProducto(Producto obj, out string Mensaje)
        {
            int idAutogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.Conn))
                {
                    SqlCommand cmd = new SqlCommand("sp_GuardarProducto", oConexion);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("idMarca", obj.oMarca.idMarca);
                    cmd.Parameters.AddWithValue("idCategoria", obj.oCategoria.idCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    idAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAutogenerado = 0;
                Mensaje = ex.Message;
            }

            return idAutogenerado;
        }

        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.Conn))
                {
                    string sqlSTR = "UPDATE producto SET rutaImagen = @rutaImagen, nombreImagen = @nombreImagen WHERE idProducto = @idProducto";

                    SqlCommand cmd = new SqlCommand(sqlSTR, oConexion);
                    cmd.Parameters.AddWithValue("@rutaImagen", obj.rutaImagen);
                    cmd.Parameters.AddWithValue("@nombreImagen", obj.nombreImagen);
                    cmd.Parameters.AddWithValue("@idProducto", obj.idProducto);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar la imagen";
                    }
                    
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        public bool EditarProducto(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.Conn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", oConexion);
                    cmd.Parameters.AddWithValue("idProducto", obj.Nombre);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("idMarca", obj.oMarca.idMarca);
                    cmd.Parameters.AddWithValue("idCategoria", obj.oCategoria.idCategoria);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;

        }

        public bool EliminarProducto(int idProducto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.Conn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", oConexion);
                    cmd.Parameters.AddWithValue("idProducto", idProducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
                
    }
}


