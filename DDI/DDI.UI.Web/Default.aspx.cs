using System;
using System.Web.Services;

namespace DDI.UI.Web
{
    public partial class Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetAuthToken()
        {
            return Token.GetToken();
        }
    }
}