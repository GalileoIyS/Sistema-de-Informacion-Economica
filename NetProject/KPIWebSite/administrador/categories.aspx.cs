using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Web.Services;

public partial class administrador_categories : BasePage
{
    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void DataPagerCategorias_PreRender(object sender, EventArgs e)
    {
        RellenaCategorias();
    }
    protected void lstCategorias_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        lstCategorias.EditIndex = e.NewEditIndex;
        RellenaCategorias();
    }
    protected void lstCategorias_CommandList(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SAVE")
        {
            GuardaCategoria(e.Item);
            lstCategorias.EditIndex = -1;
            RellenaCategorias();
        }
    }
    protected void lstCategorias_UpdateList(Object sender, ListViewUpdateEventArgs e)
    {
        lstCategorias.EditIndex = -1;
        RellenaCategorias();
    }
    protected void lstCategorias_DeleteList(Object sender, ListViewDeleteEventArgs e)
    {
        EliminaCategoria(e.ItemIndex);

        lstCategorias.EditIndex = -1;
        RellenaCategorias();
    }
    protected void lstCategorias_CancelList(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstCategorias.EditIndex = -1;
            RellenaCategorias();
        }
    }
    #endregion

    #region Funciones Privadas
    private void RellenaCategorias()
    {
        try
        {
            using (Clases.cKPI_CATEGORIES objCategorias = new Clases.cKPI_CATEGORIES())
            {
                objCategorias.nombre = txtBusqueda.Text;
                lstCategorias.DataSource = objCategorias.ObtenerDatos();
                lstCategorias.DataBind();
            }
        }
        catch (Exception excp)
        {
            RegistraIncidencia("administrador_categories", "Error en la función << RellenaCategorias() >>. Motivo :" + excp.Message, 2);
        }
    }
    private void GuardaCategoria(ListViewItem e)
    {
        using (Clases.cKPI_CATEGORIES objCategoria = new Clases.cKPI_CATEGORIES())
        {
            TextBox TextNombre = e.FindControl("txtNombreEdit") as TextBox;
            TextBox TextDescripcion = e.FindControl("txtDescripcionEdit") as TextBox;
            TextBox TextEstilo = e.FindControl("txtEstiloEdit") as TextBox;
            objCategoria.categoryid = Convert.ToInt32(lstCategorias.DataKeys[e.DisplayIndex].Values[0]);

            if (objCategoria.bConsultar())
            {
                objCategoria.nombre = TextNombre.Text;
                objCategoria.descripcion = TextDescripcion.Text;
                objCategoria.estilo = TextEstilo.Text;
                objCategoria.bModificar();
            }
        }
    }
    private void EliminaCategoria(int nItemIndex)
    {
        if (!string.IsNullOrEmpty(lstCategorias.DataKeys[nItemIndex].Values[0].ToString()))
        {
            using (Clases.cKPI_CATEGORIES objCategoria = new Clases.cKPI_CATEGORIES())
            {
                objCategoria.categoryid = Convert.ToInt32(lstCategorias.DataKeys[nItemIndex].Values[0]);
                if (objCategoria.bEliminar())
                {
                    lstCategorias.EditIndex = -1;
                    RellenaCategorias();
                }
            }
        }
    }
    #endregion

    #region Botones de Accion
    protected void btnCrearCategoria_Click(object sender, EventArgs e)
    {
        string newCategoria = txtCategoria.Text.Trim();

        if (string.IsNullOrEmpty(newCategoria))
            return;

        using (Clases.cKPI_CATEGORIES objCategoria = new Clases.cKPI_CATEGORIES())
        {
            objCategoria.nombre = newCategoria;
            objCategoria.descripcion = "-sin descripción-";
            objCategoria.bInsertar();
        }

        txtCategoria.Text = string.Empty;
    }
    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        RellenaCategorias();
    }
    #endregion
}