using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;

/// <summary>
/// Summary description for EmailUtils
/// </summary>
public class EmailUtils
{
    #region variables públicas
    public static bool smtp_sslmode;
    public static int smtp_timeout;
    public static string smtp_host;
    public static int smtp_port;
    public static string smtp_user;
    public static string smtp_pass;
    #endregion

    #region Funciones públicas
    public static SmtpClient GetSmtpClient()
    {
        SmtpClient client = new SmtpClient();

        client.EnableSsl = smtp_sslmode;
        client.Timeout *= smtp_timeout;
        client.UseDefaultCredentials = false;
        client.Host = smtp_host;
        client.Port = smtp_port;
        client.Credentials = new System.Net.NetworkCredential(smtp_user, smtp_pass);

        return client;
    }
    public static Boolean SendMessageEmail(MailAddressCollection destinyEmails, string sujeto, string body)
    {
        Boolean bTodoOk = true;
        MailMessage msg = new MailMessage();
        SmtpClient client = EmailUtils.GetSmtpClient();

        //1.-Destino del mensaje
        msg.From = new MailAddress(smtp_user);
        foreach (MailAddress email in destinyEmails)
            msg.To.Add(email);

        //2.-Sujeto del mensaje
        msg.Subject = sujeto;

        //2.-Cuerpo del mensaje
        msg.Body = body;

        try
        {
            client.Send(msg);
        }
        catch (Exception excp)
        {
            bTodoOk = false;
        }
        finally
        {
            msg.Dispose();
            client.Dispose();
        }
        return bTodoOk;
    }
    #endregion
}