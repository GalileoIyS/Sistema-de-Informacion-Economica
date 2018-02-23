using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Drawing;
using System.Threading;

public partial class indicator : BasePage
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
        if (!Page.IsPostBack)
        {
            this.idindicator = null;

            if (!string.IsNullOrEmpty(Request.QueryString["indicatorid"]))
            {
                try
                {
                    this.idindicator = Convert.ToInt32(Request.QueryString["indicatorid"]);
                }
                catch
                {
                    this.idindicator = null;
                }
            }
            else
            {
                this.idindicator = null;
            }
            if (this.idindicator.HasValue)
            {
                RellenaIndicador();
                RellenaComentarios();
                RellenaDimensiones();
                RellenaEtiquetasPlaceHolder();
                RellenaIndicadoresRelacionados();
            }
            else
            {
                Response.Redirect("~/errors/notfound.aspx");
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
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%indicator.aspx",
                true);
        }
    }
    protected void lstComentarios_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ListView listaReplicas = (ListView)e.Item.FindControl("lstReplicas");
        HyperLink linkViewMoreReplies = (HyperLink)e.Item.FindControl("HlnkViewMoreReplies");
        int commentid = Convert.ToInt32(lstComentarios.DataKeys[e.Item.DataItemIndex].Values[0]);
        using (Clases.cKPI_COMMENTS objContestaciones = new Clases.cKPI_COMMENTS())
        {
            objContestaciones.padreid = commentid;
            int NumeroTotal = objContestaciones.nRecuento();
            if (NumeroTotal <= 3)
                linkViewMoreReplies.Visible = false;
            else
                linkViewMoreReplies.Text = "Show All Comments (" + NumeroTotal + ")";
            listaReplicas.DataSource = objContestaciones.ObtenerDatos(3, 1, "FECHA DESC");
            listaReplicas.DataBind();
        }
    }
    #endregion

    #region Funciones privadas de relleno
    private void RellenaIndicador()
    {
        if (this.idindicator.HasValue)
        {
            using (Clases.cKPI_INDICATORS objIndicador = new Clases.cKPI_INDICATORS())
            {
                objIndicador.indicatorid = this.idindicator.Value;
                hdnIndicatorID.Value = objIndicador.indicatorid.ToString();
                if (objIndicador.bConsultar())
                {
                    //********************************************
                    //  RELLENAMOS LOS DATOS BÁSICOS DEL INDICADOR
                    //********************************************
                    lbTitulo.Text = objIndicador.titulo;
                    txtNombre.Text = objIndicador.titulo;
                    txtTituloValue.Text = objIndicador.titulo;
                    txtResumen.Text = objIndicador.resumen;
                    txtResumenValue.Text = objIndicador.resumen;
                    if (!string.IsNullOrEmpty(objIndicador.descripcion))
                    {
                        txtDescripcion.Text = objIndicador.descripcion;
                        txtDescripcionValue.Text = objIndicador.descripcion;
                    }
                    else
                        txtDescripcion.Text = "We are sorry but there is currently no description available.<br/><br/>";
                    if (objIndicador.fecha_alta.HasValue)
                        lbFechaAlta.Text = objIndicador.fecha_alta.Value.ToString("dd MMMM, yyyy", new System.Globalization.CultureInfo("en-US")) + " (" + CalculaFechaDesdeCuando(objIndicador.fecha_alta.Value) + ")";
                    else
                        lbFechaAlta.Text = "--undefined--";
                    lbUnidad.Text = objIndicador.unidad + " (" + objIndicador.simbolo + ")";
                    txtUnidadValue.Text = objIndicador.unidad;
                    txtSimboloValue.Text = objIndicador.simbolo;
                    lbAgregacion.Text = objIndicador.funcion_agregada_desc;
                    cmbFuncionAgregadaValue.SelectedValue = objIndicador.funcion_agregada;
                    if (objIndicador.RatingValues.HasValue)
                        targetout.Attributes.Add("data-score", objIndicador.RatingValues.Value.ToString());
                    if (!string.IsNullOrEmpty(objIndicador.imageurl))
                        imgIndicador.ImageUrl = objIndicador.imageurl;
                    else
                        imgIndicador.ImageUrl = "~/images/indicators/no-image.jpg";
                    if (objIndicador.subcategoryid.HasValue)
                    {
                        using (Clases.cKPI_SUBCATEGORIES objSubcategorias = new Clases.cKPI_SUBCATEGORIES())
                        {
                            objSubcategorias.subcategoryid = objIndicador.subcategoryid;
                            if (objSubcategorias.bConsultar())
                                lbSubcategoria.Text = objSubcategorias.nombre;
                            else
                                lbSubcategoria.Text = "-- unspecified --";
                        }
                    }
                    else
                        lbSubcategoria.Text = "-- unspecified --";

                    if (User.Identity.IsAuthenticated)
                    {
                        MembershipUser usr = Membership.GetUser();
                        if (usr != null)
                        {
                            HyperLink lbCreatorUserName = (HyperLink)LoginViewCreatorUser.FindControl("lbCreatorUserName");
                            Panel PanelIndicadorAusente = (Panel)LoginViewIndicador.FindControl("PanelIndicadorAusente");
                            Panel PanelIndicadorPrivado = (Panel)LoginViewIndicador.FindControl("PanelIndicadorPrivado");
                            Panel PanelIndicadorCompartido = (Panel)LoginViewIndicador.FindControl("PanelIndicadorCompartido");

                            btnImportarExcel.PostBackUrl = "~/registrado/importxls.aspx?indicatorid=" + this.idindicator.Value.ToString();
                            btnImportarCsv.PostBackUrl = "~/registrado/importcsv.aspx?indicatorid=" + this.idindicator.Value.ToString();
                            btnImportarJson.PostBackUrl = "~/registrado/importjson.aspx?indicatorid=" + this.idindicator.Value.ToString();
                            btnImportarXML.PostBackUrl = "~/registrado/importxml.aspx?indicatorid=" + this.idindicator.Value.ToString();
                            btnImportarTabla.PostBackUrl = "~/registrado/importtable.aspx?indicatorid=" + this.idindicator.Value.ToString();

                            LinkButton btnCompartir = (LinkButton)LoginViewIndicador.FindControl("btnCompartir");
                            if (btnCompartir != null)
                                btnCompartir.PostBackUrl = "~/registrado/sharekpi.aspx?indicatorid=" + this.idindicator.Value.ToString();
                            LinkButton btnCompartir2 = (LinkButton)LoginViewIndicador.FindControl("btnCompartir2");
                            if (btnCompartir2 != null)
                                btnCompartir2.PostBackUrl = "~/registrado/sharekpi.aspx?indicatorid=" + this.idindicator.Value.ToString();

                            using (Clases.cKPI_INDICATOR_USERS objIndicadorusuario = new Clases.cKPI_INDICATOR_USERS())
                            {
                                objIndicadorusuario.indicatorid = this.idindicator.Value;
                                objIndicadorusuario.userid = objIndicador.userid;
                                if (objIndicadorusuario.bConsultar())
                                {
                                    if (objIndicadorusuario.anonimo)
                                    {
                                        lbCreatorUserName.Text = "-- Anonymous --";
                                    }
                                    else
                                    {
                                        using (Clases.cASPNET_INFO_USUARIO objUsuarioCreador = new Clases.cASPNET_INFO_USUARIO())
                                        {
                                            objUsuarioCreador.userid = objIndicador.userid;
                                            if (objIndicador.userid != Convert.ToInt32(usr.ProviderUserKey))
                                                lbCreatorUserName.Attributes.Add("data-userid", objIndicador.userid.Value.ToString());
                                            if (objUsuarioCreador.bConsultar())
                                                lbCreatorUserName.Text = objUsuarioCreador.apellidos + ", " + objUsuarioCreador.nombre;
                                            else
                                                lbCreatorUserName.Text = "-- unspecified --";
                                        }
                                    }
                                }
                            }
                            if (objIndicador.userid != Convert.ToInt32(usr.ProviderUserKey))
                                PanelChangeImage.Visible = false;
                            else
                                PanelChangeImage.Visible = true;

                            if (!objIndicador.compartido)
                            {
                                //El indicador aún no lo hemos compartido con el resto
                                PanelSocialMedia.Visible = false;
                                PanelIndicadorCompartido.Visible = false;
                                PanelIndicadorPrivado.Visible = true;
                                PanelComentarios.Visible = false;
                                lbSubcategoria.Text = "Private";

                                datasetsItem.Visible = true;
                                dimensionsItem.Visible = true;
                                ArtDimensions.Visible = true;
                                ArtDataSets.Visible = true;
                                PanelIndicadorAusente.Visible = false;
                                PanelEtiquetasNoEditable.Visible = false;
                                PanelEtiquetasEditable.Visible = true;
                                RellenaEtiquetasEditables();
                                RellenaAtributos();
                                RellenaDatasets();
                                RellenaImports();
                            }
                            else
                            {
                                //Se trata de un indicador compartido
                                using (Clases.cKPI_INDICATOR_USERS objIndicadorusuario = new Clases.cKPI_INDICATOR_USERS())
                                {
                                    objIndicadorusuario.indicatorid = this.idindicator.Value;
                                    objIndicadorusuario.userid = Convert.ToInt32(usr.ProviderUserKey);
                                    //LO TIENE AÑADIDO A SU BIBLIOTECA
                                    if (objIndicadorusuario.bConsultar())
                                    {
                                        othersourcesItem.Visible = true;
                                        datasetsItem.Visible = true;
                                        dimensionsItem.Visible = true;
                                        ArtOtherSources.Visible = true;
                                        ArtDimensions.Visible = true;
                                        ArtDataSets.Visible = true;
                                        PanelIndicadorAusente.Visible = false;
                                        PanelEtiquetasNoEditable.Visible = false;
                                        PanelEtiquetasEditable.Visible = true;
                                        RellenaEtiquetasEditables();
                                        RellenaOtherSources();
                                        RellenaRevisiones();
                                        RellenaAtributos();
                                        RellenaDatasets();
                                        RellenaImports();
                                        RellenaAmistades();
                                        RellenaFormulas();

                                        //Incrementamos el número de visitas
                                        objIndicadorusuario.visitas = objIndicadorusuario.visitas + 1;
                                        objIndicadorusuario.bModificarVisitas();

                                        PanelAnonimo.Visible = true;
                                        PanelSocialMedia.Visible = true;

                                        PanelIndicadorPrivado.Visible = false;
                                        PanelIndicadorCompartido.Visible = true;
                                        if (objIndicadorusuario.anonimo)
                                        {
                                            cbIsAnonymnous.Checked = true;
                                            lbIsAnonymousHelp.Text = "Only your friends would be able to see your profile";
                                        }
                                        else
                                        {
                                            cbIsAnonymnous.Checked = false;
                                            lbIsAnonymousHelp.Text = "Everybody would be able to see your profile";
                                        }


                                        using (Clases.cKPI_INDICATOR_REVISIONS objRevision = new Clases.cKPI_INDICATOR_REVISIONS())
                                        {
                                            if (objIndicador.userid == Convert.ToInt32(usr.ProviderUserKey))
                                            {
                                                //Yo soy el responsable del indicador
                                                objRevision.indicatorid = this.idindicator;
                                                PanelPendingRevision.Visible = false;
                                                if (objRevision.nRecuento() > 0)
                                                {
                                                    revisionItem.Visible = true;
                                                    ArtRevisions.Visible = true;
                                                    PanelAccetpOrCancelRevision.Visible = true;
                                                }
                                                else
                                                {
                                                    revisionItem.Visible = false;
                                                    ArtRevisions.Visible = false;
                                                    PanelAccetpOrCancelRevision.Visible = false;
                                                }
                                            }
                                            else
                                            {
                                                //El responsable del indicador es otra persona
                                                objRevision.indicatorid = this.idindicator;
                                                objRevision.userid = Convert.ToInt32(usr.ProviderUserKey);
                                                PanelAccetpOrCancelRevision.Visible = false;
                                                if (objRevision.nRecuento() > 0)
                                                    PanelPendingRevision.Visible = true;
                                                else
                                                    PanelPendingRevision.Visible = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        PanelIndicadorPrivado.Visible = false;
                                        PanelIndicadorCompartido.Visible = false;
                                        PanelIndicadorAusente.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("~/errors/notfound.aspx");
                }
            }
        }
        else
        {
            Response.Redirect("~/errors/notfound.aspx");
        }
    }
    private void RellenaAtributos()
    {
        if (this.idindicator.HasValue)
        {
            try
            {
                using (Clases.cKPI_DIMENSIONS objAtributos = new Clases.cKPI_DIMENSIONS())
                {
                    objAtributos.indicatorid = this.idindicator.Value;
                    objAtributos.nombre = txtBuscarDimension.Text;
                    lstDimensions.DataSource = objAtributos.ObtenerDatos(10, 1);
                    lstDimensions.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaIndicadoresParecidos() >>");
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
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
    private void RellenaComentarios()
    {
        if (this.idindicator.HasValue)
        {
            try
            {
                using (Clases.cKPI_COMMENTS objComentarios = new Clases.cKPI_COMMENTS())
                {
                    objComentarios.indicatorid = this.idindicator.Value;
                    objComentarios.padreid = -1;
                    lstComentarios.DataSource = objComentarios.ObtenerDatos(5, 1, " FECHA DESC");
                    lstComentarios.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaComentarios() >>");
            }
            finally
            {
                lbNumComentarios.Text = "COMMENTS (" + lstComentarios.Items.Count.ToString() + ")";
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaEtiquetasEditables()
    {
        if (this.idindicator.HasValue)
        {
            string sComa = string.Empty;
            string sKeywords = string.Empty;
            int NumEtiquetas = 0;

            try
            {
                using (Clases.cKPI_INDICATOR_ETIQUETAS objEtiquetas = new Clases.cKPI_INDICATOR_ETIQUETAS())
                {
                    objEtiquetas.indicatorid = this.idindicator.Value;
                    System.Data.DataTableReader dtrValores = objEtiquetas.ObtenerDatos().CreateDataReader();
                    while (dtrValores.Read())
                    {
                        sKeywords += sComa + dtrValores.GetValue(2).ToString();
                        sComa = ", ";
                        NumEtiquetas++;
                    }
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaEtiquetasEditables() >>");
            }
            finally
            {
                txtEtiquetasEditable.Text = sKeywords;
                Page.MetaKeywords = sKeywords;
                lbNumEtiquetasEditable.Text = NumEtiquetas.ToString();
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaEtiquetasPlaceHolder()
    {
        if (this.idindicator.HasValue)
        {
            try
            {
                using (Clases.cKPI_INDICATOR_ETIQUETAS objEtiquetas = new Clases.cKPI_INDICATOR_ETIQUETAS())
                {
                    objEtiquetas.indicatorid = this.idindicator.Value;
                    System.Data.DataTableReader dtrValores = objEtiquetas.ObtenerDatos().CreateDataReader();
                    while (dtrValores.Read())
                    {
                        HyperLink NuevoEnlace = new HyperLink();
                        NuevoEnlace.Text = dtrValores.GetValue(2).ToString();
                        NuevoEnlace.NavigateUrl = "~/search.aspx?tagstring=" + dtrValores.GetValue(2).ToString();
                        NuevoEnlace.CssClass = "tag";
                        PlaceHolderEtiquetas.Controls.Add(NuevoEnlace);
                    }
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaEtiquetasPlaceHolder() >>");
            }
            finally
            {
                if (PlaceHolderEtiquetas.Controls.Count > 0)
                {
                    lbNumEtiquetasNoEditable.Text = PlaceHolderEtiquetas.Controls.Count.ToString();
                }
                else
                {
                    PanelEtiquetasNoEditable.Visible = false;
                }
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaRevisiones()
    {
        if ((User.Identity.IsAuthenticated) && (this.idindicator.HasValue))
        {
            try
            {
                using (Clases.cKPI_INDICATOR_REVISIONS objRevision = new Clases.cKPI_INDICATOR_REVISIONS())
                {
                    objRevision.indicatorid = this.idindicator.Value;
                    lstRevisions.DataSource = objRevision.ObtenerDatos(10, 1, string.Empty);
                    lstRevisions.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaRevisiones() >>");
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaFormulas()
    {
        if (this.idindicator.HasValue)
        {
            ListView lstFormulas = (ListView)LoginViewIndicador.FindControl("lstFormulas");
            if (lstFormulas != null)
            {
                try
                {
                    using (Clases.cKPI_WIDGET_EXPRESIONS objExpresiones = new Clases.cKPI_WIDGET_EXPRESIONS())
                    {
                        objExpresiones.indicatorid = this.idindicator;
                        lstFormulas.DataSource = objExpresiones.ObtenerOtrasFormulas(15, 1, string.Empty, string.Empty);
                        lstFormulas.DataBind();
                    }
                }
                catch (Exception excp)
                {
                    ExceptionUtility.LogException(excp, "Error en la función << RellenaFormulas() >>");
                }
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaAmistades()
    {
        if ((User.Identity.IsAuthenticated) && (this.idindicator.HasValue))
        {
            int? UserId = CalculaUserId();
            if (UserId.HasValue)
            {
                ListView lstFriends = (ListView)LoginViewIndicador.FindControl("lstFriends");
                if (lstFriends != null)
                {
                    try
                    {
                        using (Clases.cASPNET_FRIENDSHIP objAmigos = new Clases.cASPNET_FRIENDSHIP())
                        {
                            objAmigos.fromuserid = UserId.Value;
                            lstFriends.DataSource = objAmigos.CommonUsers(10, 1, this.idindicator.Value, string.Empty, string.Empty);
                            lstFriends.DataBind();
                        }
                    }
                    catch (Exception excp)
                    {
                        ExceptionUtility.LogException(excp, "Error en la función << RellenaAmistades() >>");
                    }
                }
            }
            else
                Response.Redirect("~/errors/notfound.aspx");
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaIndicadoresRelacionados()
    {
        if (this.idindicator.HasValue)
        {
            try
            {
                using (Clases.cKPI_WIDGET_EXPRESIONS objExpresiones = new Clases.cKPI_WIDGET_EXPRESIONS())
                {

                    objExpresiones.indicatorid = this.idindicator;
                    lstIndicadoresRelacionados.DataSource = objExpresiones.ObtenerOtrosIndicadores();
                    lstIndicadoresRelacionados.DataBind();
                }
            }
            catch (Exception excp)
            {
                ExceptionUtility.LogException(excp, "Error en la función << RellenaIndicadoresRelacionados() >>");
            }
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaDatasets()
    {
        if ((User.Identity.IsAuthenticated) && (this.idindicator.HasValue))
        {
            int? UserId = CalculaUserId();
            if (UserId.HasValue)
            {
                try
                {
                    using (Clases.cKPI_DATASETS objDatasets = new Clases.cKPI_DATASETS())
                    {
                        objDatasets.indicatorid = this.idindicator.Value;
                        objDatasets.userid = UserId.Value;
                        lstDataSets.DataSource = objDatasets.ObtenerDatos(10, 1);
                        lstDataSets.DataBind();
                    }
                }
                catch (Exception excp)
                {
                    ExceptionUtility.LogException(excp, "Error en la función << RellenaDatasets() >>");
                }
            }
            else
                Response.Redirect("~/errors/notfound.aspx");
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaOtherSources()
    {
        if ((User.Identity.IsAuthenticated) && (this.idindicator.HasValue))
        {
            int? UserId = CalculaUserId();
            if (UserId.HasValue)
            {
                try
                {
                    using (Clases.cKPI_INDICATOR_USERS objUsuarios = new Clases.cKPI_INDICATOR_USERS())
                    {
                        objUsuarios.indicatorid = this.idindicator.Value;
                        objUsuarios.userid = UserId.Value;
                        lstOtherSources.DataSource = objUsuarios.OtherUsers(10, 1, string.Empty, string.Empty);
                        lstOtherSources.DataBind();
                    }
                }
                catch (Exception excp)
                {
                    ExceptionUtility.LogException(excp, "Error en la función << RellenaOtherSources() >>");
                }
            }
            else
                Response.Redirect("~/errors/notfound.aspx");
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    private void RellenaImports()
    {
        if ((User.Identity.IsAuthenticated) && (this.idindicator.HasValue))
        {
            int? UserId = CalculaUserId();
            if (UserId.HasValue)
            {
                try
                {
                    using (Clases.cKPI_IMPORTS objImport = new Clases.cKPI_IMPORTS())
                    {
                        objImport.indicatorid = this.idindicator.Value;
                        objImport.userid = UserId.Value;
                        lstImports.DataSource = objImport.ObtenerDatos(10, 1);
                        lstImports.DataBind();
                    }
                }
                catch (Exception excp)
                {
                    ExceptionUtility.LogException(excp, "Error en la función << RellenaImports() >>");
                }
                finally
                {
                    if (lstImports.Items.Count > 0)
                    {
                        importsItem.Visible = true;
                        ArtImports.Visible = true;
                    }
                    else
                    {
                        importsItem.Visible = false;
                        ArtImports.Visible = false;
                    }
                }
            }
            else
                Response.Redirect("~/errors/notfound.aspx");
        }
        else
            Response.Redirect("~/errors/notfound.aspx");
    }
    #endregion

    #region Botones de Accion
    protected void btnGuardarIndicador_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtTituloValue.Text))
            return;
        if (string.IsNullOrEmpty(cmbFuncionAgregadaValue.SelectedValue.ToString()))
            return;
        if (string.IsNullOrEmpty(txtUnidadValue.Text))
            return;
        if (string.IsNullOrEmpty(txtSimboloValue.Text))
            return;
        if (string.IsNullOrEmpty(txtResumenValue.Text))
            return;

        if ((User.Identity.IsAuthenticated) && (this.idindicator.HasValue))
        {
            int? UserId = CalculaUserId();
            if (UserId.HasValue)
            {
                using (Clases.cKPI_INDICATORS objIndicador = new Clases.cKPI_INDICATORS())
                {
                    objIndicador.indicatorid = this.idindicator.Value;
                    if (objIndicador.bConsultar())
                    {
                        if ((objIndicador.compartido) && (objIndicador.userid != UserId.Value))
                        {
                            using (Clases.cKPI_INDICATOR_REVISIONS objRevision = new Clases.cKPI_INDICATOR_REVISIONS())
                            {
                                objRevision.indicatorid = objIndicador.indicatorid;
                                objRevision.userid = UserId.Value;
                                objRevision.titulo = txtTituloValue.Text;
                                objRevision.funcion_agregada = cmbFuncionAgregadaValue.SelectedValue.ToString();
                                objRevision.unidad = txtUnidadValue.Text;
                                objRevision.simbolo = txtSimboloValue.Text;
                                objRevision.resumen = txtResumenValue.Text;
                                objRevision.descripcion = txtDescripcionValue.Text;
                                if (objRevision.bInsertar())
                                    RellenaIndicador();
                                else
                                    RegistraIncidencia("indicator", "Error en la función << btnGuardarIndicador_Click() >>. Motivo : No se ha podido INSERTAR la nueva revisión propuesta sobre la información general del indicador", 4);
                            }
                        }
                        else
                        {
                            objIndicador.titulo = txtTituloValue.Text;
                            objIndicador.funcion_agregada = cmbFuncionAgregadaValue.SelectedValue.ToString();
                            objIndicador.unidad = txtUnidadValue.Text;
                            objIndicador.simbolo = txtSimboloValue.Text;
                            objIndicador.resumen = txtResumenValue.Text;
                            objIndicador.descripcion = txtDescripcionValue.Text;
                            if (objIndicador.bModificar())
                                RellenaIndicador();
                            else
                                RegistraIncidencia("indicator", "Error en la función << btnGuardarIndicador_Click() >>. Motivo : No se ha podido MODIFICAR la información general del indicador", 4);
                        }
                    }
                    else
                        Response.Redirect("~/errors/notfound.aspx");
                }
            }
            else
                RegistraIncidencia("indicator", "Error en la función << btnAddIndicador_Click() >>. Motivo : No se ha encontrado un usuario autenticado para realizar esta operación", 4);
        }
        else
            RegistraIncidencia("indicator", "Error en la función << btnAddIndicador_Click() >>. Motivo : No se ha encontrado un usuario autenticado para realizar esta operación", 4);
    }
    protected void btnAddIndicador_Click(object sender, EventArgs e)
    {
        if ((User.Identity.IsAuthenticated) && (this.idindicator.HasValue))
        {
            int? UserId = CalculaUserId();
            if (UserId.HasValue)
            {
                using (Clases.cKPI_INDICATOR_USERS objIndicadorUsuario = new Clases.cKPI_INDICATOR_USERS())
                {
                    objIndicadorUsuario.indicatorid = this.idindicator.Value;
                    objIndicadorUsuario.userid = UserId.Value;
                    if (objIndicadorUsuario.bInsertar())
                        RellenaIndicador();
                    else
                        RegistraIncidencia("indicator", "Error en la función << btnAddIndicador_Click() >>. Motivo : No se ha podido INSERTAR el indicador asociado a este usuario", 4);
                }
            }
            else
                RegistraIncidencia("indicator", "Error en la función << btnAddIndicador_Click() >>. Motivo : No se ha encontrado un usuario autenticado para realizar esta operación", 4);
        }
        else
            RegistraIncidencia("indicator", "Error en la función << btnAddIndicador_Click() >>. Motivo : No se ha encontrado un usuario autenticado para realizar esta operación", 4);
    }
    protected void btnEliminarIndicador_Click(object sender, EventArgs e)
    {
        if ((User.Identity.IsAuthenticated) && (this.idindicator.HasValue))
        {
            int? UserId = CalculaUserId();
            if (UserId.HasValue)
            {
                using (Clases.cKPI_INDICATOR_USERS objIndicadorUsuario = new Clases.cKPI_INDICATOR_USERS())
                {
                    objIndicadorUsuario.indicatorid = this.idindicator.Value;
                    objIndicadorUsuario.userid = UserId.Value;
                    if (objIndicadorUsuario.bEliminar())
                        Response.Redirect("~/search.aspx");
                    else
                        RegistraIncidencia("indicator", "Error en la función << btnEliminarIndicador_Click() >>. Motivo : No se ha podido ELIMINAR el usuario asociado a este indicador", 4);
                }
            }
            else
                RegistraIncidencia("indicator", "Error en la función << btnEliminarIndicador_Click() >>. Motivo : No se ha encontrado un usuario autenticado para realizar esta operación", 4);
        }
        else
            RegistraIncidencia("indicator", "Error en la función << btnEliminarIndicador_Click() >>. Motivo : No se ha encontrado un usuario autenticado para realizar esta operación", 4);
    }
    #endregion
}