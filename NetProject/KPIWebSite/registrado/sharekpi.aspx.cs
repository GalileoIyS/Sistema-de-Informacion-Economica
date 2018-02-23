using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class registrado_sharekpi : BasePage
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
                }
                catch
                {
                    idindicator = null;
                }
                finally
                {
                    RellenaIndicador();
                    RellenaCategorias();
                }
            }
            else
            {
                this.idindicator = null;
            }
        }
        else if (Request.Params.Count > 0)
        {
            switch (Request.Params[0])
            {
                case "btnTerminar":
                    Response.Redirect("~/indicator.aspx?indicatorid=" + this.idindicator.ToString());
                    break;
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
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%sharekpi.aspx",
                true);
        }
    }
    protected void DataPagerIndicadores_PreRender(object sender, EventArgs e)
    {
        RellenaIndicadoresParecidos();
    }
    protected void cmbCategorias_SelectedIndexChanged(object sender, EventArgs e)
    {
        RellenaSubcategorias();
    }
    #endregion

    #region Funciones Privadas de Relleno
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
                }
            }
        }
    }
    private void RellenaIndicadoresParecidos()
    {
        if (this.idindicator.HasValue)
        {
            using (Clases.cKPI_INDICATORS objIndicadores = new Clases.cKPI_INDICATORS())
            {
                objIndicadores.indicatorid = this.idindicator;
                if (objIndicadores.bConsultar())
                {
                    int TotalDatos = 0;
                    hdnIndicatorID.Value = objIndicadores.indicatorid.ToString();
                    if (!string.IsNullOrEmpty(objIndicadores.imageurl))
                        imgIndicador.ImageUrl = objIndicadores.imageurl;
                    try
                    {
                        DataTable dtDatos = objIndicadores.ObtenerParecidos(objIndicadores.titulo);
                        TotalDatos = dtDatos.Rows.Count;
                        lstIndicadores.DataSource = dtDatos;
                        lstIndicadores.DataBind();
                    }
                    catch (Exception excp)
                    {
                        ExceptionUtility.LogException(excp, "Error en la función << RellenaIndicadoresParecidos() >>");
                    }
                    finally
                    {
                        if (TotalDatos == 0)
                        {
                            PanelResultadosBusqueda.Visible = true;
                            lbNumIndicadores.Text = "No related indicator has been founded";
                            lbTextoBusqueda.Text = "Congratulations!";
                            lbResultadosBusqueda.Text = "The indicator that you want to share with the rest of the community is the first of its kind. Please continue with the wizard to complete the process and enjoy the benefits about publishing it";
                        }
                        else
                        {
                            PanelResultadosBusqueda.Visible = false;
                            lbNumIndicadores.Text = "<span class='color-terciario-foreground'><strong>" + TotalDatos.ToString() + " possible related indicators</strong></span> have been founded ";
                        }
                    }
                }
            }
        }
    }
    private void RellenaCategorias()
    {
        try
        {
            using (Clases.cKPI_CATEGORIES objCategorias = new Clases.cKPI_CATEGORIES())
            {
                cmbCategorias.DataTextField = "NOMBRE";
                cmbCategorias.DataValueField = "CATEGORYID";
                cmbCategorias.DataSource = objCategorias.ObtenerDatos(" AND A.CATEGORYID > 1");
                cmbCategorias.DataBind();
            }
        }
        catch (Exception excp)
        {
            ExceptionUtility.LogException(excp, "Error en la función << RellenaCategorias() >>");
        }
        finally
        {
            if (cmbCategorias.Items.Count > 0)
                RellenaSubcategorias();
        }
    }
    private void RellenaSubcategorias()
    {
        if (cmbCategorias.SelectedValue != null)
        {
            try
            {
                using (Clases.cKPI_SUBCATEGORIES objSubcategorias = new Clases.cKPI_SUBCATEGORIES())
                {
                    objSubcategorias.categoryid = Convert.ToInt32(cmbCategorias.SelectedValue);
                    lstSubcategorias.DataSource = objSubcategorias.ObtenerDatos(10, 1, string.Empty, string.Empty);
                    lstSubcategorias.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaSubcategorias() >>");
            }
        }
    }
    #endregion
}