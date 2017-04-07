using System;
using System.IO;
using System.Net;
using System.Text;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web;
using DDI.Shared;
using DDI.Shared.Models.Client.Security;

namespace DDI.UI.Web
{
    public partial class Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string foo = string.Empty;
        }

        [WebMethod]
        public static void AuthorizeUser(string username, string token)
        {
            try
            {
                WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings.Get("ApiUrl") + "userbyname/" + HttpUtility.HtmlEncode(username) + "/");
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
                        
                        DataResponse<User> data = (DataResponse<User>)js.Deserialize(responseData, typeof(DataResponse<User>));

                        if (data != null && data.IsSuccessful)
                        {
                            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, data.Data.DisplayName, DateTime.Now, DateTime.Now.AddMinutes(60), false, responseData);
                            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                            HttpCookie fatCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                            HttpContext.Current.Response.Cookies.Add(fatCookie);
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                string foo = ex.ToString();
            }
        }

        [WebMethod]
        public static string GetAuthToken()
        {
            return Token.GetToken();
        }
    }
}