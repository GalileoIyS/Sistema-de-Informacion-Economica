using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;

namespace KPIServerControls
{
    [ToolboxData("<{0}:KPIWidgetMenuListGraphics runat=server></{0}:KPIWidgetMenuListGraphics>")]
    public class KPIWidgetMenuListGraphics : CompositeDataBoundControl
    {
        #region Propiedades publicas
        public string Valor
        {
            get
            {
                String s = (String)ViewState["Valor"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Valor"] = value;
            }
        }
        #endregion

        #region Funciones Privadas
        private string GetImageUrl(string psValor)
        {
            switch (psValor)
            {
                case "B": return Page.ClientScript.GetWebResourceUrl(this.GetType(), "KPIServerControls.images.BarChart.png");
                case "Q": return Page.ClientScript.GetWebResourceUrl(this.GetType(), "KPIServerControls.images.PieChart.png");
                case "L": return Page.ClientScript.GetWebResourceUrl(this.GetType(), "KPIServerControls.images.LineChart.png");
                case "A": return Page.ClientScript.GetWebResourceUrl(this.GetType(), "KPIServerControls.images.AreaChart.png");
                case "H": return Page.ClientScript.GetWebResourceUrl(this.GetType(), "KPIServerControls.images.HistoChart.png");
                case "T": return Page.ClientScript.GetWebResourceUrl(this.GetType(), "KPIServerControls.images.TableChart.png");
                case "G": return Page.ClientScript.GetWebResourceUrl(this.GetType(), "KPIServerControls.images.Gauge.png");
            }
            return string.Empty;
        }
        #endregion

        #region Funciones Protegidas
        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            int itemCount = 0;

            if (dataSource != null)
            {
                Controls.Clear();

                Panel PanelHead = new Panel();
                PanelHead.Attributes.Add("class", "widget-graphics-bar marginright");
                Controls.Add(PanelHead);

                Image ImgTitle = new Image();
                ImgTitle.Attributes.Add("class", "principal");
                ImgTitle.ImageUrl = GetImageUrl(Valor);

                Panel PanelContent = new Panel();
                PanelContent.Attributes.Add("class", "widget-menu-list graphics");

                PanelHead.Controls.Add(ImgTitle);
                PanelHead.Controls.Add(PanelContent);

                if (dataBinding)
                {
                    IEnumerator e = dataSource.GetEnumerator();
                    while (e.MoveNext())
                    {
                        DataRowView datarow = (DataRowView)e.Current;

                        Panel PanelItem = new Panel();
                        PanelItem.Attributes.Add("class", "widget-menu-item graphics-item");
                        PanelItem.Attributes.Add("valor", datarow["CODIGO"].ToString());

                        Image imgItem = new Image();
                        imgItem.ImageUrl = GetImageUrl(datarow["CODIGO"].ToString());

                        HtmlGenericControl HtmlSpan = new HtmlGenericControl("span");
                        HtmlSpan.InnerText = datarow["DESCRIPCION"].ToString();

                        PanelItem.Controls.Add(imgItem);
                        PanelItem.Controls.Add(HtmlSpan);
                        PanelContent.Controls.Add(PanelItem);

                        itemCount++;
                    }
                }
            }
            return itemCount;
        }
        #endregion
    }
}
