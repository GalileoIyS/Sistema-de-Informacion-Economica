using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

public partial class administrador_etiquetas : BasePage
{
    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void DataPagerEtiquetas_PreRender(object sender, EventArgs e)
    {
        RellenaEtiquetas();
    }
    protected void lstEtiquetas_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        lstEtiquetas.EditIndex = e.NewEditIndex;
        RellenaEtiquetas();
    }
    protected void lstEtiquetas_CommandList(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SAVE")
        {
            GuardarTipo(e.Item);
            lstEtiquetas.EditIndex = -1;
            RellenaEtiquetas();
        }
    }
    protected void lstEtiquetas_UpdateList(Object sender, ListViewUpdateEventArgs e)
    {
        lstEtiquetas.EditIndex = -1;
        RellenaEtiquetas();
    }
    protected void lstEtiquetas_DeleteList(Object sender, ListViewDeleteEventArgs e)
    {
        EliminaTipo(e.ItemIndex);

        lstEtiquetas.EditIndex = -1;
        RellenaEtiquetas();
    }
    protected void lstEtiquetas_CancelList(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstEtiquetas.EditIndex = -1;
            RellenaEtiquetas();
        }
    }
    #endregion

    #region Funciones Privadas
    private void RellenaEtiquetas()
    {
        try
        {
            using (Clases.cKPI_ETIQUETAS objEtiquetas = new Clases.cKPI_ETIQUETAS())
            {
                objEtiquetas.nombre = txtBusqueda.Text;
                lstEtiquetas.DataSource = objEtiquetas.ObtenerDatos();
                lstEtiquetas.DataBind();
            }
        }
        catch (Exception excp)
        {
            RegistraIncidencia("administrador_etiquetas", "Error en la función << RellenaEtiquetas() >>. Motivo :" + excp.Message, 2);
        }
    }
    private void GuardarTipo(ListViewItem e)
    {
        using (Clases.cKPI_ETIQUETAS objEtiquetas = new Clases.cKPI_ETIQUETAS())
        {
            TextBox TextNombre = e.FindControl("txtNombreEdit") as TextBox;
            TextBox TextTamano = e.FindControl("txtTamanoEdit") as TextBox;
            TextBox TextOrden = e.FindControl("txtOrdenEdit") as TextBox;
            objEtiquetas.etiquetaid = Convert.ToInt32(lstEtiquetas.DataKeys[e.DisplayIndex].Values[0]);

            if (objEtiquetas.bConsultar())
            {
                objEtiquetas.nombre = TextNombre.Text;
                objEtiquetas.bModificar();
            }
        }
    }
    private void EliminaTipo(int nItemIndex)
    {
        if (!string.IsNullOrEmpty(lstEtiquetas.DataKeys[nItemIndex].Values[0].ToString()))
        {
            using (Clases.cKPI_ETIQUETAS objEtiquetas = new Clases.cKPI_ETIQUETAS())
            {
                objEtiquetas.etiquetaid = Convert.ToInt32(lstEtiquetas.DataKeys[nItemIndex].Values[0]);
                if (objEtiquetas.bEliminar())
                {
                    lstEtiquetas.EditIndex = -1;
                    RellenaEtiquetas();
                }
            }
        }
    }
    #endregion

    #region Botones de Accion
    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        RellenaEtiquetas();
    }
    #endregion
}