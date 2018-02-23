using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Text;

public partial class registrado_revision : BasePage
{
    #region Variables Privadas del Formulario
    private int? revisionid
    {
        get
        {
            object o = ViewState["revisionid"];
            if (o == null)
                return null;
            else
                return (int)o;
        }
        set
        {
            ViewState["revisionid"] = value;
        }
    }
    private int? indicatorid
    {
        get
        {
            object o = ViewState["indicatorid"];
            if (o == null)
                return null;
            else
                return (int)o;
        }
        set
        {
            ViewState["indicatorid"] = value;
        }
    }
    #endregion

    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((!Page.IsPostBack) && (User.Identity.IsAuthenticated))
        {
            if (!string.IsNullOrEmpty(Request.QueryString["idrevision"]))
            {
                try
                {
                    this.revisionid = Convert.ToInt32(Request.QueryString["idrevision"]);
                    RellenaIndicador();
                }
                catch
                {
                    this.revisionid = null;
                }
            }
            else
            {
                this.revisionid = null;
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
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%revision.aspx",
                true);
        }
    }
    #endregion

    #region Funciones de Relleno
    private void RellenaIndicador()
    {
        using (Clases.cKPI_INDICATOR_REVISIONS objRevision = new Clases.cKPI_INDICATOR_REVISIONS())
        {
            objRevision.revisionid = this.revisionid;
            if (objRevision.bConsultar())
            {
                this.indicatorid = objRevision.indicatorid;
                txtTituloValue.Text = objRevision.titulo;
                lbFechaRevision.Text = objRevision.fecha.Value.ToLongDateString();
                cmbFuncionAgregadaValue.SelectedValue = objRevision.funcion_agregada;
                txtUnidadValue.Text = objRevision.unidad;
                txtSimboloValue.Text = objRevision.simbolo;
                txtResumenValue.Text = objRevision.resumen;
                txtDescripcionValue.Text = objRevision.descripcion;

                using (Clases.cKPI_INDICATORS objIndicator = new Clases.cKPI_INDICATORS())
                {
                    objIndicator.indicatorid = objRevision.indicatorid;
                    if (objIndicator.bConsultar())
                    {
                        lbTitulo.Text = objIndicator.titulo;
                        lbTitle.Text = objIndicator.titulo;
                        lbIndicatorName.Text = objIndicator.titulo;
                        lbFechaAlta.Text = objIndicator.fecha_alta.Value.ToLongDateString();
                        lbAgregacion.Text = objIndicator.funcion_agregada_desc;
                        lbUnidad.Text = objIndicator.unidad;
                        lbSimbolo.Text = objIndicator.simbolo;
                        txtResumen.Text = objIndicator.resumen;
                        txtDescripcion.Text = objIndicator.descripcion;
                    }
                }
            }
        }
    }
    #endregion

    #region Botones de accion
    protected void HlnkRejectAndForget_Click(object sender, EventArgs e)
    {
        if ((this.indicatorid.HasValue) && (this.revisionid.HasValue))
        {
            using (Clases.cKPI_INDICATOR_REVISIONS objRevision = new Clases.cKPI_INDICATOR_REVISIONS())
            {
                objRevision.revisionid = this.revisionid;
                if (objRevision.bEliminar())
                {
                    Response.Redirect("~/indicator.aspx?indicatorid=" + this.indicatorid.Value.ToString());
                }
            }
        }
    }
    protected void HlnkAcceptAndChange_Click(object sender, EventArgs e)
    {
        if ((this.indicatorid.HasValue) && (this.revisionid.HasValue))
        {
            if (string.IsNullOrEmpty(txtTituloValue.Text))
                return;
            if (string.IsNullOrEmpty(txtUnidadValue.Text))
                return;
            if (string.IsNullOrEmpty(txtSimboloValue.Text))
                return;
            if (string.IsNullOrEmpty(txtResumenValue.Text))
                return;
            if (string.IsNullOrEmpty(txtDescripcionValue.Text))
                return;

            using (Clases.cKPI_INDICATORS objIndicador = new Clases.cKPI_INDICATORS())
            {
                objIndicador.indicatorid = this.indicatorid;
                if (objIndicador.bConsultar())
                {
                    objIndicador.titulo = txtTituloValue.Text;
                    objIndicador.funcion_agregada = cmbFuncionAgregadaValue.SelectedValue.ToString();
                    objIndicador.unidad = txtUnidadValue.Text;
                    objIndicador.simbolo = txtSimboloValue.Text;
                    objIndicador.resumen = txtResumenValue.Text;
                    objIndicador.descripcion = txtDescripcionValue.Text;
                    if (objIndicador.bModificar())
                    {
                        using (Clases.cKPI_INDICATOR_REVISIONS objRevision = new Clases.cKPI_INDICATOR_REVISIONS())
                        {
                            objRevision.revisionid = this.revisionid;
                            if (objRevision.bEliminar())
                            {
                                Response.Redirect("~/indicator.aspx?indicatorid=" + this.indicatorid.Value.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion
}