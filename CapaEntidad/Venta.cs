using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int idVenta { get; set; }
        public int idCliente { get; set; }
        public int idCarrito { get; set; }
        public int totalProductos { get; set; }
        public decimal montoTotal { get; set; }
        public string Contacto { get; set; }
        public int idEstado { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public int idTransaccion { get; set; }
    }
}
