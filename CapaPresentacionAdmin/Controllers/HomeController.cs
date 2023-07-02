using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNEGOCIO;
using ClosedXML.Excel;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista = new List<Usuario>();

            oLista = new CN_Usuarios().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.idUsuario == 0) {
                resultado = new CN_Usuarios().GuardarUsuario(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().EditarUsuario(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int idUsuario)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Usuarios().EliminarUsuario(idUsuario, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListaReporte(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_ReporteDashboard().Ventas(fechaInicio, fechaFin, idTransaccion);

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult VistaDashboard()
        {
            Dashboard objeto = new CN_ReporteDashboard().VerDashboard();
            return Json(new { resultado = objeto}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVentas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_ReporteDashboard().Ventas(fechaInicio, fechaFin, idTransaccion);
            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-CL");
            dt.Columns.Add("Fecha Venta",typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Impuesto", typeof(decimal));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach (Reporte rp in oLista)
            {
                dt.Rows.Add(new object[] { 
                    rp.fechaVenta,
                    rp.cliente,
                    rp.producto,
                    rp.cantidad,
                    rp.precio,
                    rp.impuesto,
                    rp.total,
                    rp.idTransaccion
                });
            }

            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using(MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vmd.openxmlformats-officedocument.spreadsheet.sheet","ReporteVenta_" + DateTime.Now.ToShortDateString() + ".xlsx");
                }
            }
        }
    }
}