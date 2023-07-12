using CapaEntidad;
using CapaNEGOCIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult GuardarCliente()
        {
            return View();
        }

        public ActionResult RestablecerClave()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuardarCliente(Cliente objeto)
        {
            int resultado;
            string Mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            if(objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            resultado = new CN_Cliente().GuardarCliente(objeto, out Mensaje);

            if(resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                ViewBag.Error = Mensaje;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Login(string Correo, string Clave)
        {
            Cliente oCliente = null;

            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == Correo && item.Clave == CN_Recursos.ConvertToSHA256(Clave)).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "Correo o Contraseña no son correctos.";
                return View();
            }
            else
            {
                if (oCliente.Restablecer)
                {
                    TempData["idCliente"] = oCliente.idCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo,false);
                    Session["Cliente"] = oCliente;

                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }
        }
        [HttpPost]
        public ActionResult RestablecerClave(string correo)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "No se encontró un cliente relacionado a ese correo.";
                return View();
            }
            else
            {
                string Mensaje = string.Empty;
                bool Respuesta = new CN_Cliente().RestablecerClaveCliente(oCliente.idCliente, correo, out Mensaje);

                if (Respuesta)
                {
                    ViewBag.Error = null;
                    return RedirectToAction("Login", "Acceso");
                }
                else
                {
                    ViewBag.Error = Mensaje;
                    return View();
                }
            }
        }
        [HttpPost]
        public ActionResult CambiarClave(string idCliente, string claveActual, string nuevaClave, string confirmarClave)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(u => u.idCliente == int.Parse(idCliente)).FirstOrDefault();
            // Encriptamos la clave actual ingresada para comprarla con la que tiene el usuario
            if (oCliente.Clave != CN_Recursos.ConvertToSHA256(claveActual))
            {
                TempData["idCliente"] = idCliente;
                // ViewData Nos permite almacenar valores simples, como cadenas de texto
                ViewData["claveActual"] = string.Empty;
                ViewBag.Error = "La contraseña actual no es correcta.";
                return View();
            }
            else if (nuevaClave != confirmarClave) // Verificamos que la nueva clave y la confirmación coincidan
            {
                TempData["idCliente"] = idCliente;
                ViewData["claveActual"] = claveActual;
                ViewBag.Error = "Las contraseñas no coinciden entre ellas.";
                return View();
            }

            ViewData["claveActual"] = string.Empty;
            nuevaClave = CN_Recursos.ConvertToSHA256(nuevaClave);
            string Mensaje = string.Empty;

            bool respuesta = new CN_Cliente().CambiarClaveCliente(int.Parse(idCliente), nuevaClave, out Mensaje);

            if (respuesta)
            {
                return RedirectToAction("Login");
            }
            else
            {
                TempData["idCliente"] = idCliente;
                ViewBag.Error = Mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            // Le indicamos al sistema que se cerró la sesión
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Acceso");
        }
    }
}