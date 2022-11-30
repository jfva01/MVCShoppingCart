using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNEGOCIO;
using Newtonsoft.Json;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenimientoController : Controller
    {
        // GET: Mantenimiento
        public ActionResult Categoria()
        {
            return View();
        }

        public ActionResult Marca()
        {
            return View();
        }

        public ActionResult Producto()
        {
            return View();
        }

        #region CATEGORIA SECTION

        [HttpGet]
        public JsonResult ListarCategorias()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.idCategoria == 0)
            {
                resultado = new CN_Categoria().GuardarCategoria(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().EditarCategoria(obj, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int idCategoria)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Categoria().EliminarCategoria(idCategoria, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region MARCA SECTION
        [HttpGet]
        public JsonResult ListarMarcas()
        {
            List<Marca> oLista = new List<Marca>();
            oLista = new CN_Marca().ListarMarcas();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.idMarca == 0)
            {
                resultado = new CN_Marca().GuardarMarca(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().EditarMarca(obj, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int idMarca)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Marca().EliminarMarca(idMarca, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PRODUCTO SECTION

        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();
            oLista = new CN_Producto().ListarProductos();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal Precio;

            if (decimal.TryParse(oProducto.precioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-CL"), out Precio))
            {
                oProducto.Precio = Precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ####,##" }, JsonRequestBehavior.AllowGet);
            }


            if (oProducto.idProducto == 0)
            {
                int idProductoGenerado = new CN_Producto().GuardarProducto(oProducto, out mensaje);

                if (idProductoGenerado != 0)
                {
                    oProducto.idProducto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Producto().EditarProducto(oProducto, out mensaje);
            }
            // Procedimiento para guardar la imagen del producto
            if (operacion_exitosa)
            {
                if(archivoImagen != null)
                {
                    string rutaImagenes = ConfigurationManager.AppSettings["ServidordeFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.idProducto.ToString(),extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(rutaImagenes, nombre_imagen));
                    }
                    catch(Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }

                    if (guardar_imagen_exito)
                    {
                        oProducto.rutaImagen = rutaImagenes;
                        oProducto.nombreImagen = nombre_imagen;
                        bool resp = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Producto guardado con exito, pero se produjo un error al guardar la imagen.";
                    }
                }
            }

            return Json(new { operacionExitosa = operacion_exitosa,idGenerado = oProducto.idProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(int Id)
        {
            bool conversion;

            Producto oProducto = new CN_Producto().ListarProductos().Where(p => p.idProducto == Id).FirstOrDefault();
            string textoBase64 = CN_Recursos.ConvertToBase64(Path.Combine(oProducto.rutaImagen, oProducto.nombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oProducto.nombreImagen)
            },
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public JsonResult EliminarProducto(int idProducto)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Producto().EliminarProducto(idProducto, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}