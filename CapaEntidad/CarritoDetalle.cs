using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class CarritoDetalle
    {
        public int idCarritoDetalle { get; set; }
        public Carrito oCarrito { get; set; }
        public Producto oProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal precioNeto { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
    }
}
