using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrito
    {
        public int idCarrito { get; set; }
        public int oCliente { get; set; }
        public int Estado { get; set; }
        public List<CarritoDetalle> oCarritoDetalle { get; set; }
    }
}
