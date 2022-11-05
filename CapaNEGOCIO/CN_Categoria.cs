using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDATOS;
using CapaEntidad;

namespace CapaNEGOCIO
{
    public class CN_Categoria
    {
        private CD_Categoria objCapaDATOS = new CD_Categoria();

        public List<Categoria> Listar()
        {
            return objCapaDATOS.Listar();
        }

        public int GuardarCategoria(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "El nombre de la categoría no puede estar vacío.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDATOS.GuardarCategoria(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool EditarCategoria(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "El nombre de la categoría no puede estar vacío.";
            }
            

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDATOS.EditarCategoria(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarCategoria(int idCategoria, out string Mensaje)
        {
            return objCapaDATOS.EliminarCategoria(idCategoria, out Mensaje);
        }
    }
}
