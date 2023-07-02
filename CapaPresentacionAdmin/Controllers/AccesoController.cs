using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNEGOCIO;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult RestablecerClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Correo, string Clave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == Correo && u.Clave == CN_Recursos.ConvertToSHA256(Clave)).FirstOrDefault();

            if (oUsuario == null)
            {
                // ViewBag guarda información que vamos a compartir con la vista actual del contrlador
                // No funcionará si redireccionamos a otra vista
                ViewBag.Error = "Credenciales incorrectas! Intente de nuevo";
                return View();
            }
            else
            {
                if (oUsuario.Restablecer)
                {
                    // TempData nos permite almacenar información que vamos a compartir con múltiples
                    // vistas que están dentro del mismo controlador
                    TempData["idUsuario"] = oUsuario.idUsuario;
                    return RedirectToAction("CambiarClave");
                }
                // Le indicamos al sistema que va a usar la autenticación por formularios
                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idUsuario, string claveActual, string nuevaClave, string confirmarClave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(u => u.idUsuario == int.Parse(idUsuario)).FirstOrDefault();
            // Encriptamos la clave actual ingresada para comprarla con la que tiene el usuario
            if (oUsuario.Clave != CN_Recursos.ConvertToSHA256(claveActual))
            {
                TempData["idUsuario"] = idUsuario;
                // ViewData Nos permite almacenar valores simples, como cadenas de texto
                ViewData["claveActual"] = string.Empty;
                ViewBag.Error = "La contraseña actual no es correcta.";
                return View();
            }
            else if(nuevaClave != confirmarClave) // Verificamos que la nueva clave y la confirmación coincidan
            {
                TempData["idUsuario"] = idUsuario;
                ViewData["claveActual"] = claveActual;
                ViewBag.Error = "Las contraseñas no coinciden entre ellas.";
                return View();
            }

            ViewData["claveActual"] = string.Empty;
            nuevaClave = CN_Recursos.ConvertToSHA256(nuevaClave);
            string Mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarClaveUsuario(int.Parse(idUsuario), nuevaClave, out Mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["idUsuario"] = idUsuario;
                ViewBag.Error = Mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult RestablecerClave(string correo)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "No se encontró un usuario relacionado a ese correo.";
                return View();
            }
            else
            {
                string Mensaje = string.Empty;
                bool Respuesta = new CN_Usuarios().RestablecerClaveUsuario(oUsuario.idUsuario, correo, out Mensaje);

                if (Respuesta)
                {
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Acceso");
                }
                else
                {
                    ViewBag.Error = Mensaje;
                    return View();
                }
            }
        }

        public ActionResult CerrarSesion()
        {
            // Le indicamos al sistema que se cerró la sesión
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }

    }
}