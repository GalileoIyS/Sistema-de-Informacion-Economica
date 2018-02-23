<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        InicializaParametros();
        kpiBundle.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        // Código que se ejecuta cuando se produce un error sin procesar
        Exception excp = Server.GetLastError();

        if (excp is HttpUnhandledException)
        {
            if (excp.InnerException != null)
            {
                excp = new Exception(excp.InnerException.Message);
                Server.Transfer("~/errors/customerror.aspx?handler=Application_Error%20-%20Global.asax", true);
            }
        }
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }
    public void RegistraIncidencia(string pPagina, string pMensaje, short pError)
    {
    }

    void InicializaParametros()
    {
        // Codigo que inicializa las variables globales de la aplicacion
        // Note: Accede a la tabla ASPNET_CONFIG de la base de datos para
        // leer los parametros de inicialización y los almacena en las 
        // variables globales del servidor web
        using (Clases.cASPNET_CONFIG objConfiguracion = new Clases.cASPNET_CONFIG())
        {
            //Leemos la dirección del servidor de correos            
            objConfiguracion.campo = "SMTP_HOST";
            if (objConfiguracion.bConsultar())
                EmailUtils.smtp_host = objConfiguracion.valor_cadena;

            //Leemos el puerto del servidor de correos            
            objConfiguracion.campo = "SMTP_PORT";
            if (objConfiguracion.bConsultar())
                EmailUtils.smtp_port = Convert.ToInt32(objConfiguracion.valor_numerico);

            //Leemos el nombre del usuario de correos  
            objConfiguracion.campo = "SMTP_USER";
            if (objConfiguracion.bConsultar())
                EmailUtils.smtp_user = objConfiguracion.valor_cadena;

            //Leemos el password del usuario  de correos  
            objConfiguracion.campo = "SMTP_PASSWORD";
            if (objConfiguracion.bConsultar())
                EmailUtils.smtp_pass = objConfiguracion.valor_cadena;

            //Leemos el tiempo máximo de espera por respuesta del servidor            
            objConfiguracion.campo = "SMTP_TIMEOUT";
            if (objConfiguracion.bConsultar())
                EmailUtils.smtp_timeout = Convert.ToInt32(objConfiguracion.valor_numerico);

            //Leemos el modo de envio de correos            
            objConfiguracion.campo = "SMTP_SSLMODE";
            if (objConfiguracion.bConsultar())
                EmailUtils.smtp_sslmode = Convert.ToBoolean(objConfiguracion.valor_numerico);
        }
    }
</script>
