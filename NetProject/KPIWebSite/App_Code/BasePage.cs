using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Threading;

/// <summary>
/// Descripción breve de BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
    #region Funciones Protegidas de la Clase Base
    public BasePage()
    {
    }
    protected override void OnLoadComplete(EventArgs e)
    {
        if (Request.UrlReferrer != null)
            ViewState["RefUrl"] = Request.UrlReferrer.ToString();
        base.OnLoadComplete(e);
    }
    protected string CalculaFechaDesdeCuando(string sFechaOriginal)
    {
        if (sFechaOriginal == "never")
            return "never";

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
                    sFechaDesde = "One day ago";
                else
                    sFechaDesde = diferenciaEnDias.ToString() + " days ago";
            }
            else
            {
                int diferenciaEnHoras = ts.Hours;
                if (diferenciaEnHoras > 0)
                {
                    if (diferenciaEnHoras == 1)
                        sFechaDesde = "One hour ago";
                    else
                        sFechaDesde = diferenciaEnHoras.ToString() + " hours ago";
                }
                else
                {
                    int diferenciaEnMinutos = ts.Minutes;
                    if (diferenciaEnMinutos > 0)
                    {
                        if (diferenciaEnMinutos == 1)
                            sFechaDesde = "One minute ago";
                        else
                            sFechaDesde = diferenciaEnMinutos.ToString() + " min ago";
                    }
                    else
                    {
                        sFechaDesde = "Less than a minute";
                    }
                }
            }
        }
        catch
        {
            sFechaDesde = string.Empty;
        }

        return sFechaDesde;
    }
    protected string CalculaFechaDesdeCuando(DateTime dtFechaOriginal)
    {
        string sFechaDesde = string.Empty;
        DateTime dtFechaActual;

        try
        {
            dtFechaActual = DateTime.Now;

            //Diferencia en Dias
            TimeSpan ts = dtFechaActual - dtFechaOriginal;

            // Diferencia en días.
            int diferenciaEnDias = ts.Days;

            if (diferenciaEnDias > 0)
                if (diferenciaEnDias == 1)
                    sFechaDesde = "One day ago";
                else
                    sFechaDesde = diferenciaEnDias.ToString() + " days ago";
            else
            {
                int diferenciaEnHoras = ts.Hours;
                if (diferenciaEnHoras > 0)
                {
                    if (diferenciaEnHoras == 1)
                        sFechaDesde = "One hour ago";
                    else
                        sFechaDesde = diferenciaEnHoras.ToString() + " hours ago";
                }
                else
                {
                    int diferenciaEnMinutos = ts.Minutes;
                    if (diferenciaEnMinutos > 0)
                    {
                        if (diferenciaEnMinutos == 1)
                            sFechaDesde = "One minute ago";
                        else
                            sFechaDesde = diferenciaEnMinutos.ToString() + " minutes ago";
                    }
                    else
                    {
                        sFechaDesde = "Less than a minute";
                    }
                }
            }
        }
        catch
        {
            sFechaDesde = string.Empty;
        }

        return sFechaDesde;
    }
    protected string MostrarFechaLarga(string sFechaOriginal)
    {
        string sReturnFecha = string.Empty;

        try
        {
            DateTime dtFechaOriginal = Convert.ToDateTime(sFechaOriginal);
            sReturnFecha = dtFechaOriginal.ToString("dd MMMM, yyyy", new System.Globalization.CultureInfo("en-US"));
        }
        catch
        {
            sReturnFecha = sFechaOriginal;
        }

        return sReturnFecha;
    }
    protected string CalculaMes(string sFechaOriginal)
    {

        string sReturnFecha = string.Empty;

        try
        {
            DateTime dtFechaOriginal = Convert.ToDateTime(sFechaOriginal);
            sReturnFecha = dtFechaOriginal.ToString("MMM"); 
        }
        catch
        {
            sReturnFecha = string.Empty;
        }

        return sReturnFecha;
    }
    protected string CalculaDia(string sFechaOriginal)
    {
        string sReturnFecha = string.Empty;

        try
        {
            DateTime dtFechaOriginal = Convert.ToDateTime(sFechaOriginal);
            sReturnFecha = dtFechaOriginal.Day.ToString();
        }
        catch
        {
            sReturnFecha = string.Empty;
        }

        return sReturnFecha;
    }
    protected string IsYesNo(object o)
    {
        if (Convert.ToBoolean(o) == true)
            return "Si";
        else
            return "No";
    }
    protected string CalculaImagenPerfil(int userid)
    {
        //using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
        //{
        //    objUsuario.userid = userid;
        //    if (objUsuario.bConsultar())
        //        if (!string.IsNullOrEmpty(objUsuario.img_thumbnail))
        //        return objUsuario.img_thumbnail;
        //}
        return "~/images/pages/user-profile.png";
    }
    protected int? CalculaUserId()
    {
        if (User.Identity.IsAuthenticated)
        {
            MembershipUser usr = Membership.GetUser();
            if (usr != null)
            {
                int UserID;
                try
                {
                    UserID = Convert.ToInt32(usr.ProviderUserKey);
                    return UserID;
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }
        else
            return null;
    }
    protected void RegistraIncidencia(string pPagina, string pMensaje, short pError)
    {
        using (Clases.cASPNET_AVISOS objAviso = new Clases.cASPNET_AVISOS())
        {
            objAviso.pagina = pPagina;
            objAviso.mensaje = pMensaje;
            objAviso.codigo_error = pError;
            objAviso.bInsertar();
        }
    }
    protected string GetFullyQualifiedUrl(string url)
    {
        return string.Concat(Request.Url.GetLeftPart(UriPartial.Authority), ResolveUrl(url));
    }
    #endregion
}
