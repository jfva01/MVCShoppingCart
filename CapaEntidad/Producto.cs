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
        public Marca oMarca { get; set; }
        public Categoria oCategoria { get; set; }
        public decimal Precio { get; set; }
        public string precioTexto { get; set; }
        public int Stock { get; set; }
        public string rutaImagen { get; set; }
        public string nombreImagen { get; set; }
        public bool Activo { get; set; }
        public string Base64 { get; set; }
        public string Extension { get; set; }

        //public List<ImagenesProducto> Imagenes;
    }
}
