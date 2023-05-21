using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace CapaNEGOCIO
{
    public class CN_Recursos
    {
        // Generador de clave con caracteres aleatorios
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 8);
            return clave;
        }

        // Encriptación de texto Método SHA256

        public static string ConvertToSHA256(string Texto)
        {
            StringBuilder SB = new StringBuilder();

            // Usar la referencia de "System.Security.Cryptography"

            using(SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(Texto));

                foreach(byte b in result)
                {
                    SB.Append(b.ToString("x2"));
                }

                return SB.ToString();
            }
        }

        // Envío de correos
        public static bool EnviarCorreo(string direccionCorreo, string Asunto, string mensaje)
        {
            bool resultado = false;
            //string mensaje = string.Empty;

            try
            {

                MailMessage mail = new MailMessage();

                mail.To.Add(direccionCorreo);
                mail.From = new MailAddress("nakama.view@gmail.com");
                mail.Subject = Asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    //Las credenciales son el correo del remitente y la clave generada en la configuración 
                    //de Contraseña de aplicaciones de Google en este caso 
                    Credentials = new NetworkCredential("nakama.view@gmail.com", "blzsishcukjjlicv"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                    
                };

                smtp.Send(mail);
                resultado = true;

            } catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }

        public static string ConvertToBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch
            {
                conversion = false;
            }

            return textoBase64;
        }
    }
}
