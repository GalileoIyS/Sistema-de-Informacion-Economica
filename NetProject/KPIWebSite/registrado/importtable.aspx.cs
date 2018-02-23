using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Collections;

public partial class registrado_importtable : BasePage
{
    #region Variables Privadas del Formulario
    private int? idindicator
    {
        get
        {
            object o = ViewState["idindicator"];
            if (o == null)
                return null;
            else
                return (int)o;
        }
        set
        {
            ViewState["idindicator"] = value;
        }
    }
    #endregion

    #region Eventos de los controles del formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((!Page.IsPostBack) && (User.Identity.IsAuthenticated))
        {
            this.idindicator = null;

            if (!string.IsNullOrEmpty(Request.QueryString["indicatorid"]))
            {
                try
                {
                    idindicator = Convert.ToInt32(Request.QueryString["indicatorid"]);
                    btnVolver.PostBackUrl = "~/indicator.aspx?indicatorid=" + Request.QueryString["indicatorid"];
                }
                catch
                {
                    idindicator = null;
                }
                finally
                {
                    RellenaIndicador();
                    RellenaDimensiones();
                    RellenaAtributos();
                }
            }
            else
            {
                this.idindicator = null;
            }      
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
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%importtable.aspx",
                true);
        }
    }
    #endregion

    #region Funciones privadas
    private void RellenaIndicador()
    {
        if (this.idindicator.HasValue)
        {
            using (Clases.cKPI_INDICATORS objIndicador = new Clases.cKPI_INDICATORS())
            {
                objIndicador.indicatorid = this.idindicator.Value;
                if (objIndicador.bConsultar())
                {
                    lbTitulo.Text = objIndicador.titulo;
                    lbIndicatorName.Text = objIndicador.titulo;
                    HlnkIndicador.NavigateUrl = "~/indicator.aspx?indicatorid=" + objIndicador.indicatorid.ToString();
                    hdnIndicatorID.Value = objIndicador.indicatorid.ToString();
                    txtNombreImport.Text = "Importación de datos manual a " + DateTime.Now.ToLongDateString();
                    txtDescripcionImport.Text = "Datos importados de forma manual a través de una hoja de datos ";
                }
            }
        }
    }
    private void RellenaDimensiones()
    {
        try
        {
            using (Clases.cKPI_TEMPORAL objTemporal = new Clases.cKPI_TEMPORAL())
            {
                cmbTemporal.DataTextField = "DESCRIPCION";
                cmbTemporal.DataValueField = "CODIGO";
                cmbTemporal.DataSource = objTemporal.ObtenerDatos();
                cmbTemporal.DataBind();
            }
        }
        catch (Exception excp)
        {
            ExceptionUtility.LogException(excp, "Error en la función << RellenaDimensiones() >>");
        }
    }
    private void RellenaAtributos()
    {
        try
        {
            using (Clases.cKPI_DIMENSIONS objAtributos = new Clases.cKPI_DIMENSIONS())
            {
                lstAtributos.DataTextField = "NOMBRE";
                lstAtributos.DataValueField = "DIMENSIONID";
                objAtributos.indicatorid = this.idindicator.Value;
                lstAtributos.DataSource = objAtributos.ObtenerDatos(10,1);
                lstAtributos.DataBind();
            }
        }
        catch (Exception excp)
        {
            ExceptionUtility.LogException(excp, "Error en la función << RellenaAtributos() >>");
        }
    }
    #endregion
}