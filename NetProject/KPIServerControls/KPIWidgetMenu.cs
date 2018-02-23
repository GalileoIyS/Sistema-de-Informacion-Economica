using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KPIServerControls
{
    [ToolboxItem(false)]
    public class KPIWidgetMenu : WebControl, INamingContainer
    {
        private int _widgetid;
        private int _userid;

        public int WidgetId
        {
            get
            {
                return _widgetid;
            }
        }
        public int UserId
        {
            get
            {
                return _userid;
            }
        }

        internal KPIWidgetMenu(int widgetid, int userid): base(HtmlTextWriterTag.Div)
        {
            this._widgetid = widgetid;
            this._userid = userid;
        }
        
        #region Funciones Protegidas
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute("class", "widget-menu");
            writer.AddAttribute("ID", this.UniqueID);

            base.AddAttributesToRender(writer);
        }
        #endregion
    }
}
