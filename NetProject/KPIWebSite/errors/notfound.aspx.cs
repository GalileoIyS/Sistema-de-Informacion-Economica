using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class errors_notfound : System.Web.UI.Page
{
    #region Eventos de los controles del formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            
        }
    }
    protected void txtSearchError_TextChanged(object sender, EventArgs e)
    {
        Response.Redirect("~/search.aspx?tagstring=" + txtSearchError.Text);
    }
    #endregion

}