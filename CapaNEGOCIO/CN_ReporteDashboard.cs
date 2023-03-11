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

        public Dashboard VerDashboard()
        {
            return objCapaDATOS.VerDashboard();
        }
    }
}
