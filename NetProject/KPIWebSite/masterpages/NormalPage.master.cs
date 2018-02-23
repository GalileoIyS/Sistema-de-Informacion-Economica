using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class masterpages_normalpage : System.Web.UI.MasterPage
{
    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion

    #region Funciones protegidas del masterpage
    protected string CalculaFechaDesdeCuando(string sFechaOriginal)
    {
        if (sFechaOriginal == "nunca")
            return "nunca";

        string sFechaDesde = string.Empty;
        DateTime dtFechaOriginal, dtFechaActual;

        try
        {
            dtFechaOriginal = Convert.ToDateTime(sFechaOriginal);
            dtFechaActual = DateTime.Now;

            //Diferencia en Dias
            TimeSpan ts = dtFechaActual - dtFechaOriginal;

            // Diferencia en días.
            int diferenciaEnDias = ts.Days;

            if (diferenciaEnDias > 0)
            {
                if (diferenciaEnDias == 1)
                    sFechaDesde = "Hace 1 día";
                else
                    sFechaDesde = "Hace " + diferenciaEnDias.ToString() + " días";
            }
            else
            {
                int diferenciaEnHoras = ts.Hours;
                if (diferenciaEnHoras > 0)
                {
                    if (diferenciaEnHoras == 1)
                        sFechaDesde = "Hace 1 hora";
                    else
                        sFechaDesde = "Hace " + diferenciaEnHoras.ToString() + " horas";
                }
                else
                {
                    int diferenciaEnMinutos = ts.Minutes;
                    if (diferenciaEnMinutos > 0)
                    {
                        if (diferenciaEnMinutos == 1)
                            sFechaDesde = "Hace 1 minuto";
                        else
                            sFechaDesde = "Hace " + diferenciaEnMinutos.ToString() + " minutos";
                    }
                    else
                        sFechaDesde = "Hace menos de 1 minuto";
                }
            }
        }
        catch
        {
            sFechaDesde = string.Empty;
        }

        return sFechaDesde;
    }
    #endregion
}
