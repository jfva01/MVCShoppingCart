using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDATOS;
using CapaEntidad;

namespace CapaNEGOCIO
{
    public class CN_Marca
    {
        private CD_Marca objCapaDATOS = new CD_Marca();

        public List<Marca> ListarMarcas()
        {
            return objCapaDATOS.ListarMarcas();
        }

        public int GuardarMarca(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "El nombre de la marca no puede estar vacío.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDATOS.GuardarMarca(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public bool EditarMarca(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "El nombre de la marca no puede estar vacío.";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDATOS.EditarMarca(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarMarca(int idMarca, out string Mensaje)
        {
            return objCapaDATOS.EliminarMarca(idMarca, out Mensaje);
        }
    }
}
