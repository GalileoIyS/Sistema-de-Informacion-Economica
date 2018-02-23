using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSonClases
{
    public class KPI_RESUMEN
    {
        public int usuariosV, usuariosP, usuariosAll;
        public int datasetsV, datasetsP, datasetsAll;
        public int datosV, datosP, datosAll;

        public KPI_RESUMEN()
        {
            usuariosV = 0;
            datasetsV = 0;
            datosV = 0;
            usuariosAll = 0;
            datasetsAll = 0;
            datosAll = 0;
        }
    }
}
