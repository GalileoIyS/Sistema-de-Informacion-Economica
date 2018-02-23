using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class registrado_createkpi : BasePage
{
    #region Eventos de los controles del formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Page.IsPostBack) && (User.Identity.IsAuthenticated) && (Request.Params.Count > 0))
        {
            switch (Request.Params[0])
            {
                case "btnImportarTabla":
                    if (!string.IsNullOrEmpty(hdnIndicatorID.Value))
                        Response.Redirect("~/registrado/importtable.aspx?indicatorid=" + hdnIndicatorID.Value.ToString());
                    break;
                case "btnImportarExcel":
                    if (!string.IsNullOrEmpty(hdnIndicatorID.Value))
                        Response.Redirect("~/registrado/importxls.aspx?indicatorid=" + hdnIndicatorID.Value.ToString());
                    break;
                case "btnImportarCsv":
                    if (!string.IsNullOrEmpty(hdnIndicatorID.Value))
                        Response.Redirect("~/registrado/importcsv.aspx?indicatorid=" + hdnIndicatorID.Value.ToString());
                    break;
                case "btnImportarJson":
                    if (!string.IsNullOrEmpty(hdnIndicatorID.Value))
                        Response.Redirect("~/registrado/importjson.aspx?indicatorid=" + hdnIndicatorID.Value.ToString());
                    break;
                case "btnImportarXML":
                    if (!string.IsNullOrEmpty(hdnIndicatorID.Value))
                        Response.Redirect("~/registrado/importxml.aspx?indicatorid=" + hdnIndicatorID.Value.ToString());
                    break;
                case "btnAceptarNo":
                    if (!string.IsNullOrEmpty(hdnIndicatorID.Value))
                        Response.Redirect("~/indicator.aspx?indicatorid=" + hdnIndicatorID.Value.ToString());

                    break;
            }
        }
    }
    private void Page_Error(object sender, EventArgs e)
    {
        // Obtenemos el último error del servidor.
        Exception exc = Server.GetLastError();

        // Controlamos esta específica excepción
        if (exc is InvalidOperationException)
        {
            // Enviamos la información del error a la página de errores
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%createkpi.aspx",
                true);
        }
    }
    #endregion
}