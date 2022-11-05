using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDATOS;
using CapaEntidad;

namespace CapaNEGOCIO
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDATOS = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDATOS.Listar();
        }

        public int GuardarUsuario(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres)) {
                Mensaje = "El nombre no puede estar vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos)) { 
                Mensaje = "El apellido no puede estar vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo)) {
                Mensaje = "El correo electrónico no puede estar vacío.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string Clave = CN_Recursos.GenerarClave();
                obj.Clave = CN_Recursos.ConvertToSHA256(Clave);
                int resultado = objCapaDATOS.GuardarUsuario(obj, out Mensaje);

                if(Mensaje == "")
                {
                    string asunto = "Hola " + obj.Nombres + " " + obj.Apellidos + ". Bienvenido a VM Sistema de Ventas";
                    string mensaje_correo = "<h3>Tu cuenta ha sido creada correctamente.</h3><p>Para acceder, ingresa con tu correo electrónico y la siguiente contraseña: <strong style='font-size:16px'>" + Clave + "</strong></p>";
                    bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo);

                    return resultado;
                }else
                {
                    Mensaje = "Ha ocurrido un error. No se pudo enviar el correo.";
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public bool EditarUsuario(Usuario obj, out string Mensaje)
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
                return objCapaDATOS.EditarUsuario(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarUsuario(int idUsuario, out string Mensaje)
        {
            return objCapaDATOS.EliminarUsuario(idUsuario, out Mensaje);
        }
    }
}
