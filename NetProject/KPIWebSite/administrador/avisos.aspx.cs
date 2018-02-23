using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class administrador_avisos : BasePage
{
    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void DataPagerAvisos_PreRender(object sender, EventArgs e)
    {
        RellenaAvisos();
    }
    #endregion

    #region Funciones Privadas
    private void RellenaAvisos()
    {
        try
        {
            using (Clases.cASPNET_AVISOS objAvisos = new Clases.cASPNET_AVISOS())
            {
                objAvisos.pagina = txtBusqueda.Text;
                lstAvisos.DataSource = objAvisos.ObtenerDatos(0, 0);
                lstAvisos.DataBind();
            }
        }
        catch (Exception excp)
        {
            RegistraIncidencia("administrador_avisos", "Error en la función << RellenaAvisos() >>. Motivo :" + excp.Message, 2);
        }
    }
    #endregion

    #region Botones de Accion
    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        RellenaAvisos();
    }
    protected void btnAccion_Click(object sender, EventArgs e)
    {
        using (Clases.cASPNET_AVISOS objAvisos = new Clases.cASPNET_AVISOS())
        {
            try
            {
                objAvisos.codigo_error = Convert.ToInt32(dplstAcciones.SelectedValue);
            }
            catch
            {
                return;
            }
            objAvisos.bEliminar();
            RellenaAvisos();
        }
    }
    #endregion
}