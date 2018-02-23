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
    [ToolboxData("<{0}:KPIWidgetMenuListIndicators runat=server></{0}:KPIWidgetMenuListIndicators>")]
    public class KPIWidgetMenuListIndicators : CompositeDataBoundControl
    {
        #region Funciones Privadas

        #endregion

        #region Funciones Protegidas
        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            int itemCount = 0;

            if (dataSource != null)
            {
                Controls.Clear();

                Panel PanelHead = new Panel();
                PanelHead.Attributes.Add("class", "widget-indicators-bar");
                Controls.Add(PanelHead);

                HyperLink HLnkTitulo = new HyperLink();
                HLnkTitulo.Attributes.Add("title", "Opciones");
                HLnkTitulo.Attributes.Add("class", "widget-indicators-trigger");
                HLnkTitulo.Text = "Indicadores";

                PanelHead.Controls.Add(HLnkTitulo);

                Panel PanelContent = new Panel();
                PanelContent.Attributes.Add("class", "widget-indicators-content");
                Controls.Add(PanelContent);

                HtmlGenericControl HtmlList = new HtmlGenericControl("ul");
                HtmlList.Attributes.Add("class", "widget-indicators-main-list");
                PanelContent.Controls.Add(HtmlList);

                if (dataBinding)
                {
                    IEnumerator e = dataSource.GetEnumerator();
                    while (e.MoveNext())
                    {
                        DataRowView datarow = (DataRowView)e.Current;

                        HtmlGenericControl HtmlItem = new HtmlGenericControl("li");
                        HtmlItem.Attributes.Add("class", "widget-indicator-item True");
                        HtmlItem.Attributes.Add("indicatorid", datarow["INDICATORID"].ToString());

                        Image imgDelete = new Image();
                        imgDelete.ImageUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), "KPIServerControls.images.notification_error.png");
                        imgDelete.Attributes.Add("class", "delKpi");

                        Label lbNombre = new Label();
                        lbNombre.Text = datarow["TITULO"].ToString();
                        lbNombre.Attributes.Add("class", "item");

                        HtmlItem.Controls.Add(imgDelete);
                        HtmlItem.Controls.Add(lbNombre);

                        HtmlList.Controls.Add(HtmlItem);
                        itemCount++;
                    }
                }
            }
            return itemCount;
        }
        #endregion
    }
}
