using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class registrado_desktop : BasePage
{
    #region Eventos de los controles del formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((!Page.IsPostBack) && (User.Identity.IsAuthenticated))
        {
            RellenaInfoUsuario();
            RellenaDashboards();
            RellenaAmistades();
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
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%desktop.aspx",
                true);
        }
    }
    protected void lstDashboards_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ListView lstWidgets = (ListView)e.Item.FindControl("lstWidgets");
        int iddashboard = Convert.ToInt32(lstDashboards.DataKeys[e.Item.DataItemIndex].Values[0]);
        using (Clases.cKPI_WIDGETS objWidget = new Clases.cKPI_WIDGETS())
        {
            objWidget.iddashboard = iddashboard;
            lstWidgets.DataSource = objWidget.ObtenerDatos();
            lstWidgets.DataBind();
        }
    }
    #endregion

    #region Funciones Privadas de Relleno
    private void RellenaInfoUsuario()
    {
        MembershipUser usr = Membership.GetUser();
        if (usr != null)
        {
            try
            {
                using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
                {
                    objUsuario.userid = Convert.ToInt32(usr.ProviderUserKey);
                    if (objUsuario.bConsultar())
                    {
                        if ((!string.IsNullOrEmpty(objUsuario.apellidos)) && (!string.IsNullOrEmpty(objUsuario.nombre)))
                            lbNombreUsuario.Text = objUsuario.apellidos + ", " + objUsuario.nombre;
                        else if (!string.IsNullOrEmpty(objUsuario.apellidos))
                            lbNombreUsuario.Text = objUsuario.apellidos;
                        else if (!string.IsNullOrEmpty(objUsuario.nombre))
                            lbNombreUsuario.Text = objUsuario.nombre;
                        imgProfileUser.ImageUrl = objUsuario.imageurl;
                    }
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaDimensiones() >>");
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaDashboards()
    {
        try
        {
            using (Clases.cKPI_DASHBOARDS objDashboard = new Clases.cKPI_DASHBOARDS())
            {
                objDashboard.userid = CalculaUserId();
                lstDashboards.DataSource = objDashboard.ObtenerDatos();
                lstDashboards.DataBind();
            }
        }
        catch (Exception excp)
        {
            ExceptionUtility.LogException(excp, "Error en la función << RellenaDashboards() >>");
        }
    }
    private void RellenaAmistades()
    {
        if (User.Identity.IsAuthenticated)
        {
            MembershipUser usr = Membership.GetUser();
            if (usr != null)
            {
                try
                {
                    using (Clases.cASPNET_FRIENDSHIP objAmigos = new Clases.cASPNET_FRIENDSHIP())
                    {
                        objAmigos.aceptado = true;
                        objAmigos.fromuserid = Convert.ToInt32(usr.ProviderUserKey);
                        lstFriends.DataSource = objAmigos.CommonUsers(10, 1, -1, string.Empty, string.Empty);
                        lstFriends.DataBind();
                    }
                }
                catch (Exception excp)
                {
                    ExceptionUtility.LogException(excp, "Error en la función << RellenaAmistades() >>");
                }
            }
        }
    }
    #endregion
}