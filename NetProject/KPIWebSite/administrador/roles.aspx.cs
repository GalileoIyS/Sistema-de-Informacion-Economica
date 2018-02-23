using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class administrador_roles : BasePage
{
    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void DataPagerRoles_PreRender(object sender, EventArgs e)
    {
        RellenaRoles();
    }
    protected void lstRoles_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        lstRoles.EditIndex = e.NewEditIndex;
        RellenaRoles();
    }
    protected void lstRoles_CommandList(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SAVE")
        {
            if (GuardarRol(e.Item))
            {
                lstRoles.EditIndex = -1;
                RellenaRoles();
            }
            else
                lstRoles.EditIndex = -1;
        }
    }
    protected void lstRoles_UpdateList(Object sender, ListViewUpdateEventArgs e)
    {
        lstRoles.EditIndex = -1;
        RellenaRoles();
    }
    protected void lstRoles_DeleteList(Object sender, ListViewDeleteEventArgs e)
    {
        EliminaRol(e.ItemIndex);

        lstRoles.EditIndex = -1;
        RellenaRoles();
    }
    protected void lstRoles_CancelList(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstRoles.EditIndex = -1;
            RellenaRoles();
        }
    }
    #endregion

    #region Botones de Accion
    protected void btnAddRole_Click(object sender, EventArgs e)
    {
        string newRoleName = txtRoleName.Text.Trim();

        if (string.IsNullOrEmpty(newRoleName))
        {
            return;
        }
        else if (!Roles.RoleExists(newRoleName))
        {
            Roles.CreateRole(newRoleName);
            RellenaRoles();
        }

        txtRoleName.Text = string.Empty;
    }
    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        RellenaRoles();
    }
    #endregion

    #region Funciones Privadas
    private void RellenaRoles()
    {
        try
        {
            using (Clases.cASPNET_ROLES objRol = new Clases.cASPNET_ROLES())
            {
                objRol.LoweredRoleName = txtBusqueda.Text;
                lstRoles.DataSource = objRol.ObtenerDatos();
                lstRoles.DataBind();
            }
        }
        catch (Exception excp)
        {
            RegistraIncidencia("Administrador_ManageRoles", "Error en la función << RellenaRoles() >>. Motivo :" + excp.Message, 2);
        }
    }
    private Boolean GuardarRol(ListViewItem e)
    {
        using (Clases.cASPNET_ROLES objRoles = new Clases.cASPNET_ROLES())
        {
            TextBox TextNombre = e.FindControl("txtNombreEdit") as TextBox;
            TextBox TextDescripcion = e.FindControl("txtDescripcionEdit") as TextBox;
            try
            {
                objRoles.RoleId = Convert.ToInt32(lstRoles.DataKeys[e.DataItemIndex].Values[0]);

                if (objRoles.bConsultar())
                {
                    objRoles.RoleName = TextNombre.Text;
                    objRoles.Descripcion = TextDescripcion.Text;
                     return objRoles.bModificar();
                }
            }
            catch (Exception excp)
            {
                RegistraIncidencia("Administrador_ManageRoles", "Error en la función << GuardarRol() >>. Motivo :" + excp.Message, 2);
                return false;
            }
        }
        return false;
    }
    private void EliminaRol(int nItemIndex)
    {
        if (!string.IsNullOrEmpty(lstRoles.DataKeys[nItemIndex].Values[0].ToString()))
        {
            using (Clases.cASPNET_ROLES objRol = new Clases.cASPNET_ROLES())
            {
                objRol.RoleId = Convert.ToInt32(lstRoles.DataKeys[nItemIndex].Values[0]);
                if (objRol.bConsultar())
                {
                    Roles.DeleteRole(objRol.RoleName);
                    lstRoles.EditIndex = -1;
                    RellenaRoles();
                }
            }
        }
    }
    #endregion
}