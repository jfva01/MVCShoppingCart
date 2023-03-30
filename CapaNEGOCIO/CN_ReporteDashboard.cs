using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDATOS;
using CapaEntidad;

namespace CapaNEGOCIO
{
    public class CN_ReporteDashboard
    {
        private CD_ReporteDashboard objCapaDATOS = new CD_ReporteDashboard();

        public List<Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            return objCapaDATOS.Ventas(fechaInicio, fechaFin, idTransaccion);
        }

        public Dashboard VerDashboard()
        {
            return objCapaDATOS.VerDashboard();
        }
    }
}
