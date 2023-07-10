using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDATOS;
using CapaEntidad;

namespace CapaNEGOCIO
{
    public class CN_Cliente
    {

        private CD_Cliente objCapaDATOS = new CD_Cliente();

        public int GuardarCliente(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre no puede estar vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido no puede estar vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo electrónico no puede estar vacío.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                obj.Clave = CN_Recursos.ConvertToSHA256(obj.Clave);
                return objCapaDATOS.GuardarCliente(obj, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        public List<Cliente> Listar()
        {
            return objCapaDATOS.ListarCliente();
        }

        public bool CambiarClaveCliente(int idCliente, string nuevaClave, out string Mensaje)
        {
            return objCapaDATOS.CambiarClaveCliente(idCliente, nuevaClave, out Mensaje);
        }

        public bool RestablecerClaveCliente(int idCliente, string Correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDATOS.RestablecerClaveCliente(idCliente, CN_Recursos.ConvertToSHA256(nuevaClave), out Mensaje);

            if (resultado)
            {
                string asunto = "Restablecer contraseña. VM Sistema de Ventas";
                string mensaje_correo = "<h3>Tu cuenta ha sido restablecida correctamente.</h3><p>Para acceder, ingresa con tu correo electrónico y la siguiente contraseña: <strong style='font-size:16px'>" + nuevaClave + "</strong></p>";
                bool respuesta = CN_Recursos.EnviarCorreo(Correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo.";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo restablecer la contraseña.";
                return false;
            }
        }
    }
}
