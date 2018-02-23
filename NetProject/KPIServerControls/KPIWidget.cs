using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: WebResource("KPIServerControls.scripts.Widget.js", "text/javascript")]

namespace KPIServerControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:KPIWidget runat=server></{0}:KPIWidget>")]
    public class KPIWidget : WebControl, INamingContainer
    {
        public KPIWidget(): base(HtmlTextWriterTag.Div)
        {
        }

        #region Enumerables
        enum WidgetColors { color_violeta, color_rojo, color_azul, color_amarillo, color_verde, color_tierra };
        #endregion

        #region Variables Privadas
        private ImageButton btnRemove;
        private ImageButton btnConfig;
        private HyperLink btnCollapse;
        private Panel btnEditar;
        private ITemplate widgetMenuTemplate = null;
        private ITemplate widgetContentTemplate = null;
        #endregion

        #region Propiedades publicas
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }
        public string ClassStyle
        {
            get
            {
                String s = (String)ViewState["ClassStyle"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["ClassStyle"] = value;
            }
        }

        [DescriptionAttribute("Identificador del usuario propietario del widget"), DefaultValue(null)]
        public string UserId
        {
            get
            {
                String s = (String)ViewState["UserId"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["UserId"] = value;
            }
        }

        [DescriptionAttribute("Indica si el widget está colpasado o no"), DefaultValue(null)]
        public string Colapsed
        {
            get
            {
                String s = (String)ViewState["Colapsed"];
                return ((s == null) ? String.Empty : s);
            }
            set
            {
                ViewState["Colapsed"] = value;
            }
        }

        [DescriptionAttribute("Identificador del Widget en la Base de Datos"), DefaultValue(null)]
        public string IdWidget
        {
            get
            {
                Object s = ViewState["IdWidget"];
                if (s == null)
                    return string.Empty;
                else
                    return s.ToString();
            }

            set
            {
                ViewState["IdWidget"] = value;
            }
        }

        [DescriptionAttribute("Tipo de la gráfica"), DefaultValue(null)]
        public string Tipo
        {
            get
            {
                Object s = ViewState["Tipo"];
                if (s == null)
                    return string.Empty;
                else
                    return s.ToString();
            }
            set
            {
                ViewState["Tipo"] = value;
            }
        }

        [DescriptionAttribute("Dimension temporal"), DefaultValue(null)]
        public string Dimension
        {
            get
            {
                Object s = ViewState["Dimension"];
                if (s == null)
                    return string.Empty;
                else
                    return s.ToString();
            }
            set
            {
                ViewState["Dimension"] = value;
            }
        }

        [DescriptionAttribute("Fecha Inicio de la gráfica para consultar valores"), DefaultValue(null)]
        public string FechaInicio
        {
            get
            {
                Object s = ViewState["FechaInicio"];
                if (s == null)
                    return string.Empty;
                else
                    return s.ToString();
            }
            set
            {
                ViewState["FechaInicio"] = value;
            }
        }

        [DescriptionAttribute("Fecha Final de la gráfica para consultar valores"), DefaultValue(null)]
        public string FechaFin
        {
            get
            {
                Object s = ViewState["FechaFin"];
                if (s == null)
                    return string.Empty;
                else
                    return s.ToString();
            }
            set
            {
                ViewState["FechaFin"] = value;
            }
        }

        [DescriptionAttribute("Marca el componente como colapsable"), DefaultValue(true)]
        public Boolean Colapsable
        {
            get
            {
                object o = ViewState["colapsable"];
                if (o == null)
                    return false;
                else
                    return (Boolean)o;
            }
            set
            {
                ViewState["colapsable"] = value;
            }
        }

        [DescriptionAttribute("Permite que el usuario pueda editar las propiedades del componente"), DefaultValue(true)]
        public Boolean Editable
        {
            get
            {
                object o = ViewState["editable"];
                if (o == null)
                    return false;
                else
                    return (Boolean)o;
            }
            set
            {
                ViewState["editable"] = value;
            }
        }

        [DescriptionAttribute("Permite que el usuario pueda eliminar este componente"), DefaultValue(true)]
        public Boolean Eliminable
        {
            get
            {
                object o = ViewState["eliminable"];
                if (o == null)
                    return false;
                else
                    return (Boolean)o;
            }
            set
            {
                ViewState["eliminable"] = value;
            }
        }

        [DescriptionAttribute("Permite que el usuario pueda configurar las propiedades del componente"), DefaultValue(true)]
        public Boolean Configurable
        {
            get
            {
                object o = ViewState["configurable"];
                if (o == null)
                    return false;
                else
                    return (Boolean)o;
            }
            set
            {
                ViewState["configurable"] = value;
            }
        }

        [Browsable(false), DefaultValue(null), Description("The Widget Menu Template"), TemplateContainer(typeof(KPIWidgetMenu)), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate WidgetMenuTemplate
        {
            get
            {
                return widgetMenuTemplate;
            }
            set
            {
                widgetMenuTemplate = value;
            }
        }

        [Browsable(false), DefaultValue(null), Description("The Widget Content Template"), TemplateContainer(typeof(KPIWidgetContent)), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate WidgetContentTemplate
        {
            get
            {
                return widgetContentTemplate;
            }
            set
            {
                widgetContentTemplate = value;
            }
        }

        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }
        #endregion

        #region Eventos Públicos
        public event ImageClickEventHandler btnConfig_Click;
        #endregion

        #region Funciones Privadas
        private void CreateHeaderContent()
        {
            Panel PanelHead = new Panel();
            PanelHead.Attributes.Add("class", "widget-head");
            Controls.Add(PanelHead);

            if (this.Eliminable)
            {
                btnRemove = new ImageButton();
                btnRemove.Attributes.Add("class", "remove");
                PanelHead.Controls.Add(btnRemove);
            }
            if (this.Colapsable)
            {
                btnCollapse = new HyperLink();
                btnCollapse.Attributes.Add("href", "#");
                btnCollapse.Attributes.Add("class", "collapse");
                PanelHead.Controls.Add(btnCollapse);
            }
            if (this.Configurable)
            {
                btnConfig = new ImageButton();
                btnConfig.Attributes.Add("class", "maximize");
                btnConfig.Click += new ImageClickEventHandler(OnConfigClick);
                btnConfig.CommandArgument = this.IdWidget;
                PanelHead.Controls.Add(btnConfig);
            }
            if (this.Editable)
            {
                btnEditar = new Panel();
                btnEditar.Attributes.Add("href", "#");
                btnEditar.Attributes.Add("class", "edit");
                PanelHead.Controls.Add(btnEditar);

                Panel PanelEdit = new Panel();
                PanelEdit.Attributes.Add("style", "display:none;");
                PanelEdit.Attributes.Add("class", "edit-box");
                Controls.Add(PanelEdit);

                StringBuilder EditPanel = new StringBuilder();
                EditPanel.AppendLine("<ul>");
                EditPanel.AppendLine("    <li class='item'><label>Titulo:</label><input class='nuevo-titulo' value='" + this.Text.Replace(" ", "&nbsp;") + "' /></li>");
                EditPanel.AppendLine("    <li class='item'><label>Colores:</label>");
                foreach (WidgetColors KPIColor in Enum.GetValues(typeof(WidgetColors)))
                {
                    EditPanel.AppendLine("<span class='pickcolor " + KPIColor.ToString() + "'/>&nbsp;</span>");
                }
                EditPanel.AppendLine("    </li>");
                EditPanel.AppendLine("</ul>");
                LiteralControl EditColors = new LiteralControl(EditPanel.ToString());
                PanelEdit.Controls.Add(EditColors);
            }

            LiteralControl titulo = new LiteralControl("<h3>" + this.Text + "</h3>");
            PanelHead.Controls.Add(titulo);
        }
        private void CreateBodyContent()
        {
            Image imgLoadingContent = new Image();
            imgLoadingContent.ImageUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "KPIServerControls.images.loading.gif");

            Panel PanelLoadingContent = new Panel();
            PanelLoadingContent.Attributes.Add("class", "widget-loading-content");
            PanelLoadingContent.Controls.Add(imgLoadingContent);

            Panel PanelLoading = new Panel();
            PanelLoading.Attributes.Add("class", "widget-loading");
            PanelLoading.Controls.Add(PanelLoadingContent);

            Panel PanelContent = new Panel();
            PanelContent.ID = "widget" + this.IdWidget.ToString();
            PanelContent.Attributes.Add("class", "widget-content");
            if (this.Colapsed == "S")
                PanelContent.Attributes.Add("style", "display: none;");
            else
                PanelContent.Attributes.Add("style", "display: block;");
            PanelContent.Controls.Add(PanelLoading);

            Controls.Add(PanelContent);

            if (widgetMenuTemplate != null)
            {
                KPIWidgetMenu widgetMenu = new KPIServerControls.KPIWidgetMenu(Convert.ToInt32(this.IdWidget), Convert.ToInt32(UserId));
                widgetMenuTemplate.InstantiateIn(widgetMenu);
                PanelContent.Controls.Add(widgetMenu);
            }
            if (widgetContentTemplate != null)
            {
                KPIWidgetContent widgetContent = new KPIServerControls.KPIWidgetContent(Convert.ToInt32(this.IdWidget), Convert.ToInt32(UserId));
                widgetContentTemplate.InstantiateIn(widgetContent);
                PanelContent.Controls.Add(widgetContent);
            }
        }
        #endregion

        #region Eventos
        private void OnConfigClick(object sender, ImageClickEventArgs e)
        {
            if (btnConfig_Click != null)
            {
                btnConfig_Click(this, e);
            }
        }
        #endregion

        #region Funciones Protegidas
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute("ID", this.UniqueID);

            if (!string.IsNullOrEmpty(this.ClassStyle))
                writer.AddAttribute("class", ClassStyle);

            if (!string.IsNullOrEmpty(this.IdWidget))
                writer.AddAttribute("idwidget", this.IdWidget);

            if (!string.IsNullOrEmpty(this.Colapsed))
                writer.AddAttribute("colapsado", this.Colapsed);

            if (!string.IsNullOrEmpty(this.Tipo))
                writer.AddAttribute("tipo", this.Tipo);

            if (!string.IsNullOrEmpty(this.Dimension))
                writer.AddAttribute("dimension", this.Dimension);

            if ((!string.IsNullOrEmpty(this.FechaInicio)) && (this.FechaInicio.Length >= 8) )
                writer.AddAttribute("FechaInicio", this.FechaInicio.Substring(0, 10));

            if ((!string.IsNullOrEmpty(this.FechaFin)) && (this.FechaFin.Length >= 8))
                writer.AddAttribute("FechaFin", this.FechaFin.Substring(0, 10));

            base.AddAttributesToRender(writer);
        }
        protected override void CreateChildControls()
        {
            //LIMPIAMOS TODO
            Controls.Clear();
            //CREAR LA CABECERA
            CreateHeaderContent();
            //FUNCION PARA EL BODY
            CreateBodyContent();   
        }
        protected override void Render(HtmlTextWriter writer)
        {
            Page.VerifyRenderingInServerForm(this);

            base.Render(writer);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.ClientScript.RegisterClientScriptResource(typeof(KPIWidget), "KPIServerControls.scripts.Widget.js");
        }
        #endregion
    }
}
