using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class registrado_dashboard : BasePage
{
    #region Variables Privadas del Formulario
    private int? currentDashboard
    {
        get
        {
            object o = ViewState["currentDashboard"];
            if (o == null)
                return null;
            else
                return (int)o;
        }
        set
        {
            ViewState["currentDashboard"] = value;
        }
    }
    #endregion

    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["iddashboard"]))
        {
            try
            {
                this.currentDashboard = Convert.ToInt32(Request.QueryString["iddashboard"]);
            }
            catch
            {
                this.currentDashboard = null;
            }
        }
        RellenaDashboards();
    }
    private void Page_Error(object sender, EventArgs e)
    {
        // Obtenemos el último error del servidor.
        Exception exc = Server.GetLastError();

        // Controlamos esta específica excepción
        if (exc is InvalidOperationException)
        {
            // Enviamos la información del error a la página de errores
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%dashboard.aspx",
                true);
        }
    }
    #endregion

    #region Funciones Privadas de Relleno
    private void RellenaDashboards()
    {
        if (User.Identity.IsAuthenticated)
        {
            RellenaDashboard();
        }
    }
    private void RellenaDashboard()
    {
        MembershipUser usr = Membership.GetUser(User.Identity.Name);
        if (User != null)
        {
            try
            {
                using (Clases.cKPI_DASHBOARDS objDashboard = new Clases.cKPI_DASHBOARDS())
                {
                    objDashboard.userid = Convert.ToInt32(usr.ProviderUserKey);

                    if (this.currentDashboard.HasValue)
                        objDashboard.iddashboard = this.currentDashboard;

                    objDashboard.bConsultar();
                    lbDashboardName.Text = objDashboard.titulo;
                    this.currentDashboard = objDashboard.iddashboard;
                    objDashboard.Inicializar();
                    objDashboard.userid = Convert.ToInt32(usr.ProviderUserKey);

                    lstDashboards.DataSource = objDashboard.ObtenerDatos();
                    lstDashboards.DataBind();

                    if (lstDashboards.Items.Count > 0)
                        RellenaWidgets();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaDashboard() >>");
            }
        }
    }
    private void RellenaWidgets()
    {
        if (currentDashboard.HasValue)
        {
            try
            {
                using (Clases.cKPI_WIDGETS objWidget = new Clases.cKPI_WIDGETS())
                {
                    objWidget.iddashboard = this.currentDashboard;
                    objWidget.idcolumn = "column1";
                    lstWidgetsColLeft.DataSource = objWidget.ObtenerDatos();
                    lstWidgetsColLeft.DataBind();

                    objWidget.iddashboard = this.currentDashboard;
                    objWidget.idcolumn = "column2";
                    lstWidgetsColMedium.DataSource = objWidget.ObtenerDatos();
                    lstWidgetsColMedium.DataBind();

                    objWidget.iddashboard = this.currentDashboard;
                    objWidget.idcolumn = "column3";
                    lstWidgetsColRight.DataSource = objWidget.ObtenerDatos();
                    lstWidgetsColRight.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaWidgets() >>");
            }
        }
    }
    #endregion
}