using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using DevExpress.Data;
using DevExpress.XtraReports.Parameters;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;

namespace DDI.WebApi.Helpers
{
    public static class ReportHelper
    {
        public const string ACCESS_TOKEN_PARAMETER = "AccessToken";

        public static bool SetPrincipal(string accessToken)
        {
            try
            {
                var secureDataFormat = new TicketDataFormat(new MachineKeyProtector());
                AuthenticationTicket ticket = secureDataFormat.Unprotect(accessToken);
                Thread.CurrentPrincipal = new System.Security.Principal.GenericPrincipal(ticket.Identity, null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SetPrincipal(ParameterCollection parameters)
        {            
            IParameter tokenParameter = parameters.GetByName(ACCESS_TOKEN_PARAMETER);
            if (tokenParameter != null)
            {
                return SetPrincipal(tokenParameter.Value.ToString());
            }
            return false;
        }

        private class MachineKeyProtector : IDataProtector
        {
            private readonly string[] _purpose =
            {
                typeof(OAuthAuthorizationServerMiddleware).Namespace,
                "Access_Token",
                "v1"
            };

            public byte[] Protect(byte[] userData)
            {
                throw new NotImplementedException();
            }

            public byte[] Unprotect(byte[] protectedData)
            {
                try
                {
                    return System.Web.Security.MachineKey.Unprotect(protectedData, _purpose);
                }
                catch
                {
                    return new byte[0];
                }
            }
        }
    }
}