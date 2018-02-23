using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class administrador_configuracion : System.Web.UI.Page
{
    #region Eventos del Formulario
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            RellenaParametros();
        }
    }
    #endregion

    #region Funciones Privadas
    private void RellenaParametros()
    {
        using (Clases.cASPNET_CONFIG objParametro = new Clases.cASPNET_CONFIG())
        {
            // TAB CORREO
            //Leemos la dirección del servidor de correos  
            objParametro.campo = "SMTP_HOST";
            if (objParametro.bConsultar())
            {
                txtSmtpHost.Text = objParametro.valor_cadena;
                lbSmtpHostDesc.Text = objParametro.descripcion;
            }

            //Leemos el puerto del servidor de correos  
            objParametro.campo = "SMTP_PORT";
            if (objParametro.bConsultar())
            {
                txtSmtpPort.Text = objParametro.valor_numerico.ToString();
                lbSmtpPortDesc.Text = objParametro.descripcion;
            }

            //Leemos el nombre del usuario de correos  
            objParametro.campo = "SMTP_USER";
            if (objParametro.bConsultar())
            {
                txtSmtpUser.Text = objParametro.valor_cadena;
                lbSmtpUserDesc.Text = objParametro.descripcion;
            }

            //Leemos el password del usuario  de correos 
            objParametro.campo = "SMTP_PASSWORD";
            if (objParametro.bConsultar())
            {
                txtSmtpPassword.Text = objParametro.valor_cadena;
                lbSmtpPasswordDesc.Text = objParametro.descripcion;
            }

            //Leemos el tiempo máximo de espera por respuesta del servidor     
            objParametro.campo = "SMTP_TIMEOUT";
            if (objParametro.bConsultar())
            {
                txtSmtpTimeOut.Text = objParametro.valor_numerico.ToString();
                lbSmtpTimeOutDesc.Text = objParametro.descripcion;
            }

            //Leemos el modo de envio de correos  
            objParametro.campo = "SMTP_SSLMODE";
            if (objParametro.bConsultar())
            {
                txtSmtpSslMode.Text = objParametro.valor_numerico.ToString();
                lbSmtpSslModeDesc.Text = objParametro.descripcion;
            }
        }
    }
    #endregion

    #region Botones de accion
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        using (Clases.cASPNET_CONFIG objParametro = new Clases.cASPNET_CONFIG())
        {
            //TAB CORREO
            //Leemos la dirección del servidor de correos 
            objParametro.campo = "SMTP_HOST";
            objParametro.valor_cadena = txtSmtpHost.Text;
            if (objParametro.bModificar())
                EmailUtils.smtp_host = txtSmtpHost.Text;

            //Leemos el puerto del servidor de correos      
            objParametro.campo = "SMTP_PORT";
            try
            {
                objParametro.valor_numerico = Convert.ToInt32(txtSmtpPort.Text);
            }
            catch
            {
                objParametro.valor_numerico = 0;
            }
            if (objParametro.bModificar())
                EmailUtils.smtp_port = Convert.ToInt32(objParametro.valor_numerico);

            //Leemos el nombre del usuario de correos 
            objParametro.campo = "SMTP_USER";
            objParametro.valor_cadena = txtSmtpUser.Text;
            if (objParametro.bModificar())
                EmailUtils.smtp_user = txtSmtpUser.Text;

            //Leemos el password del usuario  de correos
            objParametro.campo = "SMTP_PASSWORD";
            objParametro.valor_cadena = txtSmtpPassword.Text;
            if (objParametro.bModificar())
                EmailUtils.smtp_pass = txtSmtpPassword.Text;

            //Leemos el tiempo máximo de espera por respuesta del servidor
            objParametro.campo = "SMTP_TIMEOUT";
            try
            {
                objParametro.valor_numerico = Convert.ToInt32(txtSmtpTimeOut.Text);
            }
            catch
            {
                objParametro.valor_numerico = 0;
            }
            if (objParametro.bModificar())
                EmailUtils.smtp_timeout = Convert.ToInt32(objParametro.valor_numerico);

            //Leemos el modo de envio de correos  
            objParametro.campo = "SMTP_SSLMODE";
            try
            {
                objParametro.valor_numerico = Convert.ToInt32(txtSmtpSslMode.Text);
            }
            catch
            {
                objParametro.valor_numerico = 0;
            }
            if (objParametro.bModificar())
                EmailUtils.smtp_sslmode = Convert.ToBoolean(objParametro.valor_numerico);
        }
    }
    #endregion
}