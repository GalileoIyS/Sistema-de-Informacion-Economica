using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class dropkey : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["widgetid"]))
            {
                    hdnWidgetID.Value = Request.QueryString["widgetid"].ToString();
            }
        }
    }
}