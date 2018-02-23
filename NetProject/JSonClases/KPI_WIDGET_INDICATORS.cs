using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSonClases
{
    public class KPI_WIDGET_INDICATORS
    {
        public string titulo, asignado;

        public int widgetid, indicatorid;

        public DateTime fecha;

        public List<KPI_DASHBOARD> valores = new List<KPI_DASHBOARD>();

    }
}
