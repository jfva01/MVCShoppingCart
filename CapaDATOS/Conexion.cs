using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CapaDATOS
{
	public class Conexion
	{

		public static string Conn = ConfigurationManager.ConnectionStrings["Conn"].ToString();

	}
}