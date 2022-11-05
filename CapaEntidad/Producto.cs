using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Producto
    {
        public int idProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int oMarca { get; set; }
        public int oCategoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string rutaImagen { get; set; }
        public string nombreImagen { get; set; }
        public bool Activo { get; set; }
    }
}
