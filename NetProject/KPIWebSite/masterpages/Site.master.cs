using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    #region Funciones Privadas
    private void EnviarMensajeAdministradores(string NuevoUsuario)
    {
        //1.-Destino del mensaje
        System.Net.Mail.MailAddressCollection MisDestinos = new System.Net.Mail.MailAddressCollection();
        string[] usersInRole = Roles.GetUsersInRole("administrador");
        foreach (string userid in usersInRole)
        {
            MembershipUser user = Membership.GetUser((object)userid, false);
            if (user != null) MisDestinos.Add(new System.Net.Mail.MailAddress(user.Email));
        }

        //2.-Cuerpo del mensaje
        string sMensaje = "Le informo que el usuario " + NuevoUsuario + " ha solicitado su alta web en el portal de datos DropKeys. ";

        //3.-Envio del mensaje
        EmailUtils.SendMessageEmail(MisDestinos, "New user", sMensaje);
    }
    private void EnviarMensajeUsuario(string EmailUsuario)
    {
        string validationCode = new Random().Next().ToString();

        //1.-Destino del mensaje
        System.Net.Mail.MailAddressCollection MisDestinos = new System.Net.Mail.MailAddressCollection();
        MisDestinos.Add(new System.Net.Mail.MailAddress(EmailUsuario));

        //2.-Cuerpo del mensaje
        HttpServerUtility server = HttpContext.Current.Server;
        string sMensaje = "We have sucessfully received your registration request to DropKeys. To complete the subscription process, please click the following link :\r\n\r\n " + Request.Url.GetLeftPart(UriPartial.Authority) + "/validatecode.aspx?email=" +
            server.UrlEncode(EmailUsuario) + "&vc=" + validationCode + "\r\n\r\nThank you.";

        if (EmailUtils.SendMessageEmail(MisDestinos, "Verify your email", sMensaje))
        {
            using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
            {
                MembershipUser Usuario = Membership.GetUser(EmailUsuario, false);
                objUsuario.userid = Convert.ToInt32(Usuario.ProviderUserKey);
                if (objUsuario.bConsultar())
                {
                    objUsuario.mensaje_validacion = true;
                    objUsuario.validado = false;
                    objUsuario.codigo_validacion = validationCode;
                    objUsuario.bModificar();
                }
            }
        }
    }
    #endregion

    #region Funciones del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Panel PanelRecoveryPass = (Panel)LoginUserControl.FindControl("PanelRecoveryPassword");
            if (PanelRecoveryPass != null)
                PanelRecoveryPass.Attributes.Add("style", "display:none");
        }
    }
    protected void LoginUserControl_LoginError(object sender, EventArgs e)
    {
        LoginUserControl.FailureText = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger fade in'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;The e-mail or password are not valid</div></div>";

        MembershipUser InfoUsuario = Membership.GetUser(LoginUserControl.UserName);
        if (InfoUsuario != null)
        {
            if (InfoUsuario.IsLockedOut)
            {
                LoginUserControl.FailureText = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger fade in'><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;Your account has been blocked</div></div>";
            }
            else if (!InfoUsuario.IsApproved)
            {
                LoginUserControl.FailureText = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger fade in'><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;Your account has been deactivated</div></div>";
            }
            else
            {
                LoginUserControl.FailureText = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger fade in'><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;The connection attempt was unsuccessful</div></div>";
            }
        }
        else
        {
            LoginUserControl.FailureText = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger fade in'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;The e-mail or password are not valid</div></div>";
        }
    }
    protected void LoginUserControl_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        Page.Validate("LoginUserValidation");
        if (!Page.IsValid)
            e.Cancel = true;
    }
    protected void LoginUserControl_LoggedIn(object sender, EventArgs e)
    {
        Response.Redirect("~/registrado/profile.aspx?username=" + LoginUserControl.UserName);
    }
    protected void RegistroDeUsuarioForm_CreateUserError(object sender, CreateUserErrorEventArgs e)
    {
        Literal txtError = (Literal)RegistroDeUsuarioForm.CreateUserStep.ContentTemplateContainer.FindControl("CreateUserErrorMessage");
        switch (e.CreateUserError)
        {
            case MembershipCreateStatus.DuplicateUserName:
                txtError.Text = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;Username already exists. Please enter a different user name.</div></div>";
                break;

            case MembershipCreateStatus.DuplicateEmail:
                txtError.Text = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;A username for that e-mail address already exists. Please enter a different e-mail address.</div></div>";
                break;

            case MembershipCreateStatus.InvalidPassword:
                txtError.Text = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;The password provided is invalid. Please enter a valid password value.</div></div>";
                break;

            case MembershipCreateStatus.InvalidEmail:
                txtError.Text = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;The e-mail address provided is invalid. Please check the value and try again.</div></div>";
                break;

            case MembershipCreateStatus.InvalidAnswer:
                txtError.Text = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;The password retrieval answer provided is invalid. Please check the value and try again.</div></div>";
                break;

            case MembershipCreateStatus.InvalidQuestion:
                txtError.Text = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;The password retrieval question provided is invalid. Please check the value and try again.</div></div>";
                break;

            case MembershipCreateStatus.InvalidUserName:
                txtError.Text = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;The user name provided is invalid. Please check the value and try again.</div></div>";
                break;

            case MembershipCreateStatus.ProviderError:
                txtError.Text = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.</div></div>";
                break;

            case MembershipCreateStatus.UserRejected:
                txtError.Text = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.</div></div>";
                break;

            default:
                txtError.Text = "<div class='form-group col-xs-12 col-lg-12'><div class='alert alert-danger'><button type='button' class='close' data-dismiss='alert'><span aria-hidden='true'>×</span><span class='sr-only'>Close</span></button><span class='glyphicon glyphicon-warning-sign'></span>&nbsp;An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.</div></div>";
                break;
        }
    }
    protected void RegistroDeUsuarioForm_CreatedUser(object sender, EventArgs e)
    {
        MembershipUser membershipUser = Membership.GetUser(RegistroDeUsuarioForm.Email);
        if (membershipUser != null)
        {
            //YA NO LO LOGEAMOS DE FORMA AUTOMÁTICA
            //FormsAuthentication.SetAuthCookie(RegistroDeUsuarioForm.UserName, false);
            using (Clases.cASPNET_INFO_USUARIO objUsuario = new Clases.cASPNET_INFO_USUARIO())
            {
                if (membershipUser.ProviderUserKey != null)
                {
                    objUsuario.userid = Convert.ToInt32(membershipUser.ProviderUserKey);
                    TextBox txtFirstName = (TextBox)RegistroDeUsuarioForm.CreateUserStep.ContentTemplateContainer.FindControl("txtFirstName");
                    if (txtFirstName != null)
                        objUsuario.nombre = txtFirstName.Text;
                    TextBox txtLastName = (TextBox)RegistroDeUsuarioForm.CreateUserStep.ContentTemplateContainer.FindControl("txtLastName");
                    if (txtLastName != null)
                        objUsuario.apellidos = txtLastName.Text;
                    if (objUsuario.bModificar())
                    {
                        EnviarMensajeAdministradores(RegistroDeUsuarioForm.Email);
                        EnviarMensajeUsuario(RegistroDeUsuarioForm.Email);
                    }
                    Response.Redirect("~/novalidated.aspx?userid=" + objUsuario.userid.Value.ToString());
                }
            }
        }
    }
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx");
    }
    protected void btnRecovery_Click(object sender, EventArgs e)
    {
        Panel PanelRecoveryPass = (Panel)LoginUserControl.FindControl("PanelRecoveryPassword");
        if (PanelRecoveryPass != null)
        {
            if (PanelRecoveryPass.Attributes.Count > 0)
            {
                PanelRecoveryPass.Attributes.Clear();
            }
            else
            {
                PanelRecoveryPass.Attributes.Add("style", "display:none");
            }
        }
    }
    #endregion
}
