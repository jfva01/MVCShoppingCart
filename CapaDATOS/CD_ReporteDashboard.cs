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
    public class CD_ReporteDashboard
    {
        public List<Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> lista = new List<Reporte>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.Conn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", oConexion);
                    cmd.Parameters.AddWithValue("fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("fechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("idTransaccion", idTransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // Creamos un objeto json
                            lista.Add(
                                new Reporte()
                                {
                                    idVenta = Convert.ToInt32(dr["idVenta"]),
                                    fechaVenta = dr["FechaVenta"].ToString(),
                                    cliente = dr["Cliente"].ToString(),
                                    producto = dr["Producto"].ToString(),
                                    cantidad = Convert.ToInt32(dr["Cantidad"]),
                                    precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-CL")),
                                    impuesto = Convert.ToDecimal(dr["Impuesto"], new CultureInfo("es-CL")),
                                    total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-CL")),
                                    idTransaccion = dr["idTransaccion"].ToString()
                                }
                            );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Reporte>();
                throw;
            }

            return lista;
        }
        public Dashboard VerDashboard()
        {
            Dashboard objeto = new Dashboard();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.Conn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // Creamos un objeto json
                            objeto = new Dashboard()
                            {
                                TotalClientes = Convert.ToInt32(dr["TotalClientes"]),
                                TotalVentas = Convert.ToInt32(dr["TotalVentas"]),
                                TotalProductos = Convert.ToInt32(dr["TotalProductos"]),
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {
                objeto = new Dashboard();
                throw;
            }

            return objeto;
        }
    }
}
