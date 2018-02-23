using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

namespace KPIServerControls
{
    [ToolboxData("<{0}:KPIWidgetMenuListTemporal runat=server></{0}:KPIWidgetMenuListTemporal>")]
    public class KPIWidgetMenuListTemporal : CompositeDataBoundControl
    {
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
        #endregion

        #region Funciones Protegidas
        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            int itemCount = 0;

            if (dataSource != null)
            {
                Controls.Clear();

                Panel PanelHead = new Panel();
                PanelHead.Attributes.Add("class", "widget-dimensions-bar marginleft");
                Controls.Add(PanelHead);

                Label LabelTitle = new Label();
                LabelTitle.Attributes.Add("class", "title");
                if (!string.IsNullOrEmpty(Text))
                    LabelTitle.Text = Text;
                else
                    LabelTitle.Text = "Dimensión";

                Panel PanelContent = new Panel();
                PanelContent.Attributes.Add("class", "widget-menu-list");

                PanelHead.Controls.Add(LabelTitle);
                PanelHead.Controls.Add(PanelContent);

                if (dataBinding)
                {
                    IEnumerator e = dataSource.GetEnumerator();
                    while (e.MoveNext())
                    {
                        DataRowView datarow = (DataRowView)e.Current;

                        Panel PanelItem = new Panel();
                        PanelItem.Attributes.Add("class", "widget-menu-item dimensions-item");
                        PanelItem.Attributes.Add("valor", datarow["CODIGO"].ToString());

                        Label LabelNombre = new Label();
                        LabelNombre.Text = datarow["DESCRIPCION"].ToString();

                        PanelItem.Controls.Add(LabelNombre);
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
