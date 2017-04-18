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
        public const string DDI_User_Token = "DDI_Authenticated_User_Token";

        #region Properties

        public string Access_Token { get; set; }     
        public string UserName { get; set; }
        public string Password { get; set; }

        #endregion

        #region Methods
        
        public static Token GetToken()
        {
            var token = new Token();

            try
            {
                if (HttpContext.Current.Session[DDI_User_Token] == null)
                {
                    Configuration config = GetConfiguration();

                    token.UserName = config.AppSettings.Settings["username"].Value;
                    token.Password = config.AppSettings.Settings["password"].Value;

                    AuthenticateUser(token);
                }

                token.Access_Token = HttpContext.Current.Session[DDI_User_Token].ToString();

                return token;
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

        private static void AuthenticateUser(Token token)
        {
            string data = "grant_type=password&username=" + token.UserName + "&password=" + token.Password;
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings.Get("ApiUrl") + "Login");
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
                    Token t = (Token)js.Deserialize(responseData, typeof(Token));

                    if (t != null && !string.IsNullOrWhiteSpace(t.Access_Token))
                    {
                        HttpContext.Current.Session[DDI_User_Token] = t.Access_Token;
                    }
                }
            }
        }

        #endregion
    }
}