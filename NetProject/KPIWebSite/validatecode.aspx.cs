using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class novalidated : System.Web.UI.Page
{
    #region Variables Privadas del Formulario
    private int? usuarioid
    {
        get
        {
            object o = ViewState["usuarioid"];
            if (o == null)
                return null;
            else
                return (int)o;
        }
        set
        {
            ViewState["usuarioid"] = value;
        }
    }
    #endregion

    #region Eventos de los controles del formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string email = string.Empty;
            string vc = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["email"]))
                email = Request.QueryString["email"].ToString();

            if (!string.IsNullOrEmpty(Request.QueryString["vc"]))
                vc = Request.QueryString["vc"].ToString();

            // Validate this email and validation code
            if (string.IsNullOrEmpty(email))
            {
                lbResultado.Text = "<div class='alert alert-danger fade in'>Sorry but the e-mail user " + email + " can't be found in our database</div>";
                btnGoWebsite.Visible = false;
            }
            else if (string.IsNullOrEmpty(vc))
            {
                // Send email for validation
                MembershipUser Usuario = Membership.GetUser(email, false);
                using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
                {
                    objUsuario.userid = Convert.ToInt32(Usuario.ProviderUserKey);
                    objUsuario.validado = true;
                    if (objUsuario.bConsultar())
                    {
                        //GREEN
                        lbResultado.Text = "<div class='alert alert-success fade in'>The e-mail user " + email + " has already been successfully validated, it is not necessary re-validate it again.</div>";
                    }
                    else
                    {
                        try
                        {
                            string validationCode = new Random().Next().ToString();

                            //1.-Destino del mensaje
                            System.Net.Mail.MailAddressCollection MisDestinos = new System.Net.Mail.MailAddressCollection();
                            MisDestinos.Add(new System.Net.Mail.MailAddress(email));

                            //2.-Cuerpo del mensaje
                            HttpServerUtility server = HttpContext.Current.Server;
                            string sMensaje = "We have received your registration request to DropKeys. To complete the subscription process, please click the following link:\r\n\r\n " + Request.Url.GetLeftPart(UriPartial.Authority) + "/validatecode.aspx?email=" +
                                server.UrlEncode(email) + "&vc=" + validationCode +
                                "\r\n\r\nThank you.";

                            if (EmailUtils.SendMessageEmail(MisDestinos, "Check your email", sMensaje))
                            {
                                objUsuario.userid = Convert.ToInt32(Usuario.ProviderUserKey);
                                objUsuario.mensaje_validacion = true;
                                objUsuario.validado = false;
                                objUsuario.codigo_validacion = validationCode;
                                objUsuario.suscrito = false;
                                objUsuario.bModificar();

                                lbResultado.Text = "<div class='alert alert-success fade in'>We have sent an e-mail to " + email + ". Please click on the link enclosed in the mail to complete the subscription process.</div>";

                            }
                            else
                            {
                                //RED
                                lbResultado.Text = "<div class='alert alert-danger fade in'>We are very sorry we were unable to send the email with the validation key <a href=\"validatecode.aspx?email=" + Server.UrlEncode(email) + "\">Press here</a> to try it again.</div>";
                            }
                        }
                        catch (Exception)
                        {
                            //RED
                            lbResultado.Text = "<div class='alert alert-danger fade in'>We are very sorry we were unable to send the email with the validation key <a href=\"validatecode.aspx?email=" + Server.UrlEncode(email) + "\">Press here</a> to try it again.</div>";
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(vc))
            {
                MembershipUser Usuario = Membership.GetUser(email, false);
                using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
                {
                    objUsuario.userid = Convert.ToInt32(Usuario.ProviderUserKey);
                    objUsuario.codigo_validacion = vc;
                    if (objUsuario.bConsultar())
                    {
                        //GREEN
                        lbResultado.Text = "<div class='alert alert-success fade in'>Congratulations, the e-mail " + email + " has been successfully validated.</div>";
                        objUsuario.validado = true;
                        objUsuario.suscrito = true;
                        objUsuario.bModificar();

                        //Logeamos
                        FormsAuthentication.SetAuthCookie(email, true);
                    }
                    else
                    {
                        //RED
                        lbResultado.Text = "<div class='alert alert-danger fade in'>Sorry but the email " + email + " could not be validated by our system. The validation code provided is not correct. " +
                                           "<a href=\"validatecode.aspx?email=" + email + "\">Click here</a>" + " to request a new validation code.</div>";
                    }
                }
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
            Server.Transfer("~/errors/customerror.aspx?handler=Page_Error%20-%novalidated.aspx",
                true);
        }
    }
    #endregion

    #region Botones de Accion
    protected void btnStartNow_Click(object sender, EventArgs e)
    {
    }
    #endregion
}