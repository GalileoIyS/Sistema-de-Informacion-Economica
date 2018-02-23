using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class administrador_users : BasePage
{
    #region Variables Privadas del Formulario
    private int PageIndex
    {
        get
        {
            object o = ViewState["PageIndex"];
            if (o == null)
                return 0;
            else
                return (int)o;
        }
        set
        {
            ViewState["PageIndex"] = value;
        }
    }
    private int PageSize
    {
        get
        {
            return Convert.ToInt16(30);
        }
    }
    #endregion

    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void DataPagerUsuarios_PreRender(object sender, EventArgs e)
    {
        if ((User.Identity.IsAuthenticated) && (User.IsInRole("administrador")))
            RellenaUsuarios();
    }
    protected void lstUsuarios_DeleteList(Object sender, ListViewDeleteEventArgs e)
    {
        if (!string.IsNullOrEmpty(lstUsuarios.DataKeys[e.ItemIndex].Values[0].ToString()))
        {
            if (Membership.DeleteUser(lstUsuarios.DataKeys[e.ItemIndex].Values[0].ToString(), true))
            {
                RellenaUsuarios();
                lstUsuarios.EditIndex = -1;
            }
        }
    }
    #endregion

    #region Botones de Accion
    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        RellenaUsuarios();
    }
    #endregion

    #region Funciones Privadas
    private void RellenaUsuarios()
    {
        try
        {
            int totalRecords;
            MembershipUserCollection Usuarios;
            if (string.IsNullOrEmpty(txtBusqueda.Text))
                Usuarios = Membership.FindUsersByName(" 1=1 ", this.PageIndex, this.PageSize, out totalRecords);
            else
                Usuarios = Membership.FindUsersByName(" LOWEREDUSERNAME LIKE '%" + txtBusqueda.Text.ToLower() + "%' ", this.PageIndex, this.PageSize, out totalRecords);
            lstUsuarios.DataSource = Usuarios;
            lstUsuarios.DataBind();
        }
        catch (Exception excp)
        {
            RegistraIncidencia("Administrador_Users", "Error en la función << RellenaUsuarios() >>. Motivo :" + excp.Message, 2);
        }
    }
    #endregion
}
