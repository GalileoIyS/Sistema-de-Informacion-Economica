using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSonClases
{
    public class ASPNET_INFO_USUARIO
    {
        public string nombre, apellidos, resumen, imageurl;

        public int userid, shared, formulas, friends, situacion;

        public List<KPI_INDICATORS> indicadores = new List<KPI_INDICATORS>();

    }
}
