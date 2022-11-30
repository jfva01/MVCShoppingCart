using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDATOS;
using CapaEntidad;

namespace CapaNEGOCIO
{
    public class CN_Producto
    {
        private CD_Producto objCapaDATOS = new CD_Producto();

        public List<Producto> ListarProductos()
        {
            return objCapaDATOS.ListarProductos();
        }

        public int GuardarProducto(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre de la Producto no puede estar vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción del Producto no puede estar vacía.";
            }
            else if(obj.oMarca.idMarca == 0)
            {
                Mensaje = "Debe seleccionar una marca.";
            }
            else if(obj.oCategoria.idCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoría.";
            }
            else if(obj.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del producto.";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar la cantidad en stock del producto.";
            }

            if(string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDATOS.GuardarProducto(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool EditarProducto(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre de la Producto no puede estar vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción del Producto no puede estar vacía.";
            }
            else if (obj.oMarca.idMarca == 0)
            {
                Mensaje = "Debe seleccionar una marca.";
            }
            else if (obj.oCategoria.idCategoria == 0)
            {
                Mensaje = "Debe seleccionar una categoría.";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del producto.";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Debe ingresar la cantidad en stock del producto.";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDATOS.EditarProducto(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            return objCapaDATOS.GuardarDatosImagen(obj, out Mensaje);
        }

        public bool EliminarProducto(int idProducto, out string Mensaje)
        {
            return objCapaDATOS.EliminarProducto(idProducto, out Mensaje);
        }
    }
}
