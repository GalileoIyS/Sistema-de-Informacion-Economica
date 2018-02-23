<%@ WebHandler Language="C#" Class="AjaxFileuploader" %>

using System;
using System.Web;
using System.IO;

public class AjaxFileuploader : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        if (context.Request.Files.Count > 0)
        {
            System.Web.Security.MembershipUser usr = System.Web.Security.Membership.GetUser();
            if (usr == null)
            {
                string msgNoUser = "{";
                msgNoUser += string.Format("error:'{0}',\n", "No file uploaded");
                msgNoUser += string.Format("msg:'{0}'\n", string.Empty);
                msgNoUser += "}";
                context.Response.Write(msgNoUser);
            }
            
            string DirectoryPath = context.Server.MapPath("~/uploads/imports/" + usr.ProviderUserKey.ToString());
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);

            var file = context.Request.Files[0];

            string fileName;

            if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
            {
                string[] files = file.FileName.Split(new char[] { '\\' });
                fileName = files[files.Length - 1];
            }
            else
            {
                fileName = file.FileName;
            }
            string strFileName = fileName;
            fileName = Path.Combine(DirectoryPath, fileName);
            
            file.SaveAs(fileName);
            
            string msg = "{";
            msg += string.Format("error:'{0}',\n", string.Empty);
            msg += string.Format("msg:'{0}'\n", strFileName);
            msg += "}";
            context.Response.Write(msg);
        }
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

}