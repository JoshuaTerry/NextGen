using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Text;
using System.Web.Script.Serialization;

namespace DDI.UI.Web
{
    public class Token
    {
        private const string DDI_User_Token = "DDI_Authenticated_User_Token";

        #region Properties

        public string Access_Token { get; set; }     
        public string UserName { get; set; }

        #endregion

        #region Methods
        
        public static string GetToken()
        {
            try
            {
                if (HttpContext.Current.Session[DDI_User_Token] == null)
                {
                    Configuration config = GetConfiguration();

                    string username = config.AppSettings.Settings["username"].Value;
                    string password = config.AppSettings.Settings["password"].Value;

                    AuthenticateUser(username, password);
                }

                return HttpContext.Current.Session[DDI_User_Token].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static Configuration GetConfiguration()
        {
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = HttpContext.Current.Server.MapPath("~/auth.config");
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            return config;
        }

        private static void AuthenticateUser(string username, string password)
        {
            string data = "grant_type=password&username=" + username + "&password=" + password;
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            WebRequest request = WebRequest.Create("http://localhost:49490/api/v1/Login");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string responseData = reader.ReadToEnd();
                    Token token = (Token)js.Deserialize(responseData, typeof(Token));

                    if (token != null && !string.IsNullOrWhiteSpace(token.Access_Token))
                    {
                        HttpContext.Current.Session[DDI_User_Token] = token.Access_Token;
                    }
                }
            }
        }

        #endregion
    }
}