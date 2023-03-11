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
    public class CD_ReporteDashboard
    {
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
