using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class administrador_indicadores : BasePage
{
    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
       if ((!Page.IsPostBack) && (User.Identity.IsAuthenticated) && ((User.IsInRole("kpi")) || (User.IsInRole("administrador"))))
        {

        }
    }
    protected void DataPagerKpis_PreRender(object sender, EventArgs e)
    {
        RellenaIndicadores();
    }
    #endregion

    #region Funciones Privadas
    private void RellenaIndicadores()
    {
        try
        {
            using (Clases.cKPI_INDICATORS objIndicadores = new Clases.cKPI_INDICATORS())
            {
                objIndicadores.titulo = txtBusqueda.Text;
                lstIndicadores.DataSource = objIndicadores.ObtenerDatosBuscador(ConstruyeFiltro());
                lstIndicadores.DataBind();
            }
        }
        catch (Exception excp)
        {
            RegistraIncidencia("administrador_indicadores", "Error en la función << RellenaIndicadores() >>. Motivo :" + excp.Message, 2);
        }
    }
    private string ConstruyeFiltro()
    {
        string sFiltro = string.Empty;

        if (!string.IsNullOrEmpty(txtBusqueda.Text))
            sFiltro += " AND LOWER(TITULO) LIKE '%" + txtBusqueda.Text.ToLower() + "%' ";
       
        return sFiltro;
    }
    #endregion

    #region Botones de Accion
    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        RellenaIndicadores();
    }
    #endregion
}