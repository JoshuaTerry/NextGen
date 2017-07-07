using DDI.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace DDI.UI.Web.Code
{
    public class ApiRoleProvider : RoleProvider
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(ApiRoleProvider));
        #region Properties
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Unsupported Methods
        
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Supported Methods

        public override bool IsUserInRole(string username, string roleName)
        {
            bool isInRole = false;
            string token = string.Empty;

            if (HttpContext.Current.Session[Token.DDI_User_Token] != null)
            {
                token = HttpContext.Current.Session[Token.DDI_User_Token].ToString();
            }

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(username);

                WebRequest request = WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings.Get("ApiUrl") + "users");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                request.PreAuthenticate = true;
                request.Headers.Add("Authorization", "Bearer " + token);

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

                        isInRole = (bool)js.Deserialize(responseData, typeof(bool));
                    }
                }

                return isInRole;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                return isInRole;
            }
        }

        #endregion

    }
}