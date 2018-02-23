using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_KPILibrary : System.Web.UI.UserControl
{
    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            RellenaIndicadores();
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
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%KPILibrary.ascx",
                true);
        }
    }
    #endregion

    #region Funciones Privadas
    private void RellenaIndicadores()
    {
        System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
        if (usr != null)
        {
            try
            {
                using (Clases.cKPI_INDICATOR_USERS objIndicador = new Clases.cKPI_INDICATOR_USERS())
                {
                    objIndicador.userid = Convert.ToInt32(usr.ProviderUserKey);
                    lstIndicadores.DataSource = objIndicador.TopXUltimos(8, string.Empty);
                    lstIndicadores.DataBind();
                }
            }
            finally
            {
                if (lstIndicadores.Items.Count < 8)
                    PanelMas.Visible = false;
            }
        }
        else
        {
            PanelMiLibreria.Visible = false;
        }
    }
    #endregion
}