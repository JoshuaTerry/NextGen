using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using DDI.Shared.Models.Client.Security;
using System.Security.Principal;

namespace DDI.UI.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                JavaScriptSerializer js = new JavaScriptSerializer();

                FormsIdentity fi = new FormsIdentity(ticket);

                HttpContext.Current.User = new GenericPrincipal(fi, null);
            }
        }
    }
}