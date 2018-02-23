using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Text;

public partial class registrado_datosbydataset : BasePage
{
    #region Variables Privadas del Formulario
    private int? datasetid
    {
        get
        {
            try
            {
                return Convert.ToInt32(this.HdnDatasetId.Value);
            }
            catch
            {
                return null;
            }
        }
    }
    #endregion

    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((!Page.IsPostBack) && (User.Identity.IsAuthenticated))
        {
            if (!string.IsNullOrEmpty(Request.QueryString["dataset"]))
            {
                try
                {
                    this.HdnDatasetId.Value = Request.QueryString["dataset"];
                    RellenaIndicador();
                    RellenaDimensiones();
                }
                catch
                {
                    this.HdnDatasetId.Value = string.Empty;
                }
            }
            else
            {
                this.HdnDatasetId.Value = string.Empty;
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
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%datosbydataset.aspx",
                true);
        }
    }
    protected void lstDimensiones_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            if (this.datasetid.HasValue)
            {
                System.Data.DataRowView rowView = e.Item.DataItem as System.Data.DataRowView;
                int DimensionId = Convert.ToInt32(rowView["DIMENSIONID"]);
                Label NumValores = (Label)e.Item.FindControl("lbNumDimensionX");
                TextBox Valores = (TextBox)e.Item.FindControl("txtDimensionValues");

                StringBuilder sValores = new StringBuilder();
                string sComa = string.Empty;
                int nRecuento = 0;

                using (Clases.cKPI_DIMENSION_VALUES objDimensionValues = new Clases.cKPI_DIMENSION_VALUES())
                {
                    objDimensionValues.dimensionid = DimensionId;
                    objDimensionValues.datasetid = this.datasetid.Value;
                    DataTableReader dtrValores = objDimensionValues.ObtenerDatos().CreateDataReader();
                    while (dtrValores.Read())
                    {
                        sValores.Append(sComa + dtrValores.GetValue(2).ToString());
                        sComa = ", ";
                        nRecuento++;
                    }
                }
                NumValores.Text = nRecuento.ToString();
                Valores.Text = sValores.ToString();
            }
        }
    }
    #endregion

    #region Funciones de Relleno
    private void RellenaIndicador()
    {
        using (Clases.cKPI_DATASETS objDataSet = new Clases.cKPI_DATASETS())
        {
            objDataSet.datasetid = datasetid;
            if (objDataSet.bConsultar())
            {
                using (Clases.cKPI_INDICATORS objIndicador = new Clases.cKPI_INDICATORS())
                {
                    lbDatasetName.Text = objDataSet.nombre;
                    objIndicador.indicatorid = objDataSet.indicatorid;
                    if (objIndicador.bConsultar())
                    {
                        lbTitulo.Text = objIndicador.titulo;
                        lbIndicatorName.Text = objIndicador.titulo;
                        HdnUnidad.Value = objIndicador.unidad + "( " + objIndicador.simbolo + ") ";
                        HlnkIndicador.NavigateUrl = "~/indicator.aspx?indicatorid=" + objIndicador.indicatorid.ToString();
                    }
                }
            }
        }
    }
    private void RellenaDimensiones()
    {
        if (this.datasetid.HasValue)
        {
            using (Clases.cKPI_DATASETS objDataset = new Clases.cKPI_DATASETS())
            {
                objDataset.datasetid = this.datasetid.Value;
                if (objDataset.bConsultar())
                {
                    try
                    {
                        this.HdnTemporal.Value = objDataset.dimension;
                        if (objDataset.indicatorid.HasValue)
                        {
                            using (Clases.cKPI_DIMENSIONS objDimension = new Clases.cKPI_DIMENSIONS())
                            {
                                objDimension.indicatorid = objDataset.indicatorid;

                                lstDimensiones.DataSource = objDimension.ObtenerDatos(10, 1);
                                lstDimensiones.DataBind();
                            }
                        }
                    }
                    catch (Exception excp)
                    {
                        ExceptionUtility.LogException(excp, "Error en la función << RellenaDimensiones() >>");
                    }
                    finally
                    {
                        if (lstDimensiones.Items.Count == 0)
                            PanelDeAtributos.Visible = false;
                    }
                }
            }
        }
    }
    #endregion
}