using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class registrado_profile : BasePage
{
    #region Variables Privadas del Formulario
    private int? iduser
    {
        get
        {
            object o = ViewState["iduser"];
            if (o == null)
                return null;
            else
                return (int)o;
        }
        set
        {
            ViewState["iduser"] = value;
        }
    }
    #endregion

    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["userid"]))
                    try
                    {
                        this.iduser = Convert.ToInt32(Request.QueryString["userid"].ToString());
                    }
                    catch
                    {
                        this.iduser = CalculaUserId();
                    }
                else
                    this.iduser = CalculaUserId();

                if (this.iduser.HasValue)
                {
                    RellenaMuroActividades();
                    RellenaDatosPerfil();
                    RellenaDatosGenerales();
                    RellenaFriends();
                    RellenaComentarios();
                    RellenaTopSeisIndicadoresPublicos();
                    RellenaUltimosAmigos();
                    RellenaIndicadores();
                    RellenaDashboards();
                }
                else
                {
                    Response.Redirect("~/errors/notfound.aspx");
                }
            }
            else
                RegistraIncidencia("registrado_profile", "Intento de acceso a la página del perfil del usuario sin estar autenticado", 2);
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
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%profile.aspx",
                true);
        }
    }
    #endregion

    #region Funciones Privadas de Relleno
    private void RellenaDatosPerfil()
    {
        if (this.iduser.HasValue)
        {
            MembershipUser usr = Membership.GetUser(this.iduser);
            try
            {
                lbFechaDeAltaUsuario.Text = CalculaFechaDesdeCuando(usr.CreationDate);
                lbUltimoAccesoUsuario.Text = CalculaFechaDesdeCuando(usr.LastActivityDate);
                lbEmailUsuario.Text = usr.Email;
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaDatosPerfil() >>");
            }

            using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
            {
                objUsuario.userid = this.iduser;
                if (objUsuario.bConsultar())
                {
                    txtFirstName.Text = objUsuario.nombre;
                    txtLastName.Text = objUsuario.apellidos;
                    txtResume.Text = objUsuario.resumen;
                    if ((!string.IsNullOrEmpty(objUsuario.apellidos)) && (!string.IsNullOrEmpty(objUsuario.nombre)))
                        lbNombreUsuario.Text = objUsuario.apellidos + ", " + objUsuario.nombre;
                    else if (!string.IsNullOrEmpty(objUsuario.apellidos))
                        lbNombreUsuario.Text = objUsuario.apellidos;
                    else if (!string.IsNullOrEmpty(objUsuario.nombre))
                        lbNombreUsuario.Text = objUsuario.nombre;
                    if (!string.IsNullOrEmpty(objUsuario.imageurl))
                        imagePerfilUsuario.ImageUrl = objUsuario.imageurl;
                    else
                        imagePerfilUsuario.ImageUrl = "~/images/noavatar.png";
                    if (string.IsNullOrEmpty(objUsuario.resumen))
                        lbResumenUsuario.Text = "You haven't provided a brief resume about you";
                    else
                        lbResumenUsuario.Text = objUsuario.resumen;
                }
                else
                    Response.Redirect("~/errors/notfound.aspx");
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaDatosGenerales()
    {
        if (this.iduser.HasValue)
        {
            using (Clases.cKPI_INDICATOR_USERS objIndicadorUsuario = new Clases.cKPI_INDICATOR_USERS())
            {
                objIndicadorUsuario.userid = this.iduser;
                lbNumberOfFormulas.Text = objIndicadorUsuario.nRecuentoFormulas().ToString();
                lbNumberOfWidgets.Text = objIndicadorUsuario.nRecuentoWidgets().ToString();
                lbNumberOfDashboards.Text = objIndicadorUsuario.nRecuentoDashboards().ToString();
            }
            using (Clases.cKPI_INDICATORS objIndicadores = new Clases.cKPI_INDICATORS())
            {
                objIndicadores.userid = this.iduser;
                objIndicadores.compartido = true;
                lbIndicadoresCompartidos.Text = objIndicadores.NumeroIndicadores().ToString();
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaMuroActividades()
    {
        if (this.iduser.HasValue)
        {
            try
            {
                using (Clases.cKPI_DAILY objDiario = new Clases.cKPI_DAILY())
                {
                    objDiario.userid = this.iduser;
                    lstActivity.DataSource = objDiario.ObtenerDiario(10, 1);
                    lstActivity.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaMuroActividades() >>");
            }
            finally
            {
                numTabActivity.Text = lstActivity.Items.Count.ToString();
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaFriends()
    {
        if (this.iduser.HasValue)
        {
            try
            {
                using (Clases.cASPNET_FRIENDSHIP objUsuarios = new Clases.cASPNET_FRIENDSHIP())
                {
                    objUsuarios.fromuserid = this.iduser;
                    objUsuarios.aceptado = true;
                    lstFriends.DataSource = objUsuarios.ObtenerDatos(string.Empty, 15, 1, string.Empty);
                    lstFriends.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaFriends() >>");
            }
            finally
            {
                numTabFriends.Text = lstFriends.Items.Count.ToString();
                if (lstFriends.Items.Count == 0)
                {
                    PanelMasFriends.Visible = true;
                    lbMasFriends.Text = "No friends present";
                }
                else if (lstFriends.Items.Count <= 6)
                    PanelMasFriends.Visible = false;
                else
                {
                    PanelMasFriends.Visible = true;
                    lbMasFriends.Text = Convert.ToString(lstFriends.Items.Count - 6) + " more";
                }
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaIndicadores()
    {
        if (this.iduser.HasValue)
        {
            try
            {
                using (Clases.cKPI_INDICATOR_USERS objIndicadores = new Clases.cKPI_INDICATOR_USERS())
                {
                    objIndicadores.userid = this.iduser;
                    lstIndicators.DataSource = objIndicadores.ObtenerDatos(string.Empty);
                    lstIndicators.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaIndicadores() >>");
            }
            finally
            {
                numTabIndicators.Text = lstIndicators.Items.Count.ToString();
                if (lstIndicators.Items.Count == 0)
                {
                    PanelMasIndicators.Visible = true;
                    lbMasIndicators.Text = "No dropkeys present";
                }
                else if (lstIndicators.Items.Count <= 6)
                    PanelMasIndicators.Visible = false;
                else
                {
                    PanelMasIndicators.Visible = true;
                    lbMasIndicators.Text = Convert.ToString(lstIndicators.Items.Count - 6) + " more";
                }
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaTopSeisIndicadoresPublicos()
    {
        if (this.iduser.HasValue)
        {
            try
            {
                using (Clases.cKPI_INDICATOR_USERS objIndicador = new Clases.cKPI_INDICATOR_USERS())
                {
                    objIndicador.userid = this.iduser;
                    lstTopSixPublicIndicators.DataSource = objIndicador.TopXUltimos(6, "S");
                    lstTopSixPublicIndicators.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaTopSeisIndicadoresPublicos() >>");
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaUltimosAmigos()
    {
        if (this.iduser.HasValue)
        {
            try
            {
                using (Clases.cASPNET_FRIENDSHIP objAmigos = new Clases.cASPNET_FRIENDSHIP())
                {
                    objAmigos.fromuserid = this.iduser;
                    objAmigos.aceptado = true;
                    lstLastSixFriends.DataSource = objAmigos.ObtenerDatos(6, 1, " FECHA DESC ");
                    lstLastSixFriends.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaUltimosAmigos() >>");
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaDashboards()
    {
        if (this.iduser.HasValue)
        {
            try
            {
                using (Clases.cKPI_DASHBOARDS objDashboard = new Clases.cKPI_DASHBOARDS())
                {
                    objDashboard.userid = this.iduser;
                    lstDashboards.DataSource = objDashboard.ObtenerDatos(string.Empty);
                    lstDashboards.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaDashboards() >>");
            }
            finally
            {
                numTabDashboards.Text = lstDashboards.Items.Count.ToString();
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaComentarios()
    {
        if (this.iduser.HasValue)
        {
            try
            {
                using (Clases.cKPI_COMMENTS objComentarios = new Clases.cKPI_COMMENTS())
                {
                    objComentarios.userid = this.iduser;
                    lstComments.DataSource = objComentarios.ObtenerDatos(15, 1, "FECHA DESC");
                    lstComments.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaComentarios() >>");
            }
            finally
            {
                numTabComments.Text = lstComments.Items.Count.ToString();
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    #endregion

    #region Botones de Accion
    protected void btnSaveDataProfile_Click(object sender, EventArgs e)
    {
        if (this.iduser.HasValue)
        {
            using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
            {
                objUsuario.userid = this.iduser;
                if (objUsuario.bConsultar())
                {
                    if (!string.IsNullOrEmpty(txtFirstName.Text))
                        objUsuario.nombre = txtFirstName.Text;
                    if (!string.IsNullOrEmpty(txtLastName.Text))
                        objUsuario.apellidos = txtLastName.Text;
                    if (!string.IsNullOrEmpty(txtResume.Text))
                        objUsuario.resumen = txtResume.Text;
                    if (objUsuario.bModificar())
                        RellenaDatosPerfil();
                    else
                        RegistraIncidencia("registrado_profile", "Error en la función << btnSaveDataProfile_Click() >>. Motivo : No se ha podido MODIFICAR el perfil del usuario", 4);
                }
                else
                    Response.Redirect("~/errors/notfound.aspx");
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    #endregion
}