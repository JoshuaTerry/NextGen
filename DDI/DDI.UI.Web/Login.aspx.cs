using System;
using System.IO;
using System.Net;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web;
using DDI.Shared;
using DDI.Shared.Models.Client.Security;

namespace DDI.UI.Web
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static void AuthorizeUser(string username, string token)
        {
            try
            {
                //api/v1/roles/user/{username}
                WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings.Get("ApiUrl") + "roles/user/" + HttpUtility.HtmlEncode(username) + "/");
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = 0;
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (WebResponse response = request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string responseData = reader.ReadToEnd();

                        List<string> data = (List<string>)js.Deserialize(responseData, typeof(List<string>));
                        string roles = string.Join(",", data.ToArray());

                        //if (data != null && data.IsSuccessful)
                        //{
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddMinutes(60), false, roles);
                        string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                        HttpCookie fatCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                        HttpContext.Current.Response.Cookies.Add(fatCookie);
                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                string foo = ex.ToString();
            }
        }

        [WebMethod]
        public static string GetAuthToken()
        {
            Token token = Token.GetToken();

            Login.AuthorizeUser(token.UserName, token.Access_Token);

            return token.Access_Token;
        }
    }
}