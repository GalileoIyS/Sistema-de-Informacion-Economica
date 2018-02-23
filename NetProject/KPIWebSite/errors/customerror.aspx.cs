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
        string generalErrorMsg = "Ha ocurrido un error en la página. Por favor inténtelo de nuevo. Si el error continúa, contacte con el soporte.";
        string httpErrorMsg = "Ha ocurrido un error HTTP. La página que pretende buscar no existe. Por favor, inténtelo de nuevo.";
        string unhandledErrorMsg = "Se ha producido un error no controlado por la aplicación Web.";

        // Display safe error message.
        FriendlyErrorMsg.Text = generalErrorMsg;

        // Determine where error was handled.
        string errorHandler = Request.QueryString["handler"];
        if (errorHandler == null)
        {
            errorHandler = "Página de error";
        }

        // Get the last error from the server.
        Exception ex = Server.GetLastError();

        // Get the error number passed as a querystring value.
        string errorMsg = Request.QueryString["msg"];
        if (errorMsg == "404")
        {
            ex = new HttpException(404, httpErrorMsg, ex);
            FriendlyErrorMsg.Text = ex.Message;
        }

        // If the exception no longer exists, create a generic exception.
        if (ex == null)
        {
            ex = new Exception(unhandledErrorMsg);
        }

        // Show error details to only you (developer). LOCAL ACCESS ONLY.
        if (Request.IsLocal)
        {
            // Detailed Error Message.
            ErrorDetailedMsg.Text = ex.Message;

            // Show where the error was handled.
            ErrorHandler.Text = errorHandler;

            // Show local access details.
            DetailedErrorPanel.Visible = true;

            if (ex.InnerException != null)
            {
                InnerMessage.Text = ex.GetType().ToString() + "<br/>" +
                    ex.InnerException.Message;
                InnerTrace.Text = ex.InnerException.StackTrace;
            }
            else
            {
                InnerMessage.Text = ex.GetType().ToString();
                if (ex.StackTrace != null)
                {
                    InnerTrace.Text = ex.StackTrace.ToString().TrimStart();
                }
            }
        }

        // Log the exception.
        ExceptionUtility.LogException(ex, errorHandler);

        // Clear the error from the server.
        Server.ClearError();
    }
    #endregion

}