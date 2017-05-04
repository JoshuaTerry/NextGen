using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDI.UI.Web
{
    public class BasePage : System.Web.UI.Page
    {
        #region Properties

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        private string PageName
        {
            get
            {
                return Path.GetFileName(Page.AppRelativeVirtualPath).ToLower();
            }
        }

        #endregion

        #region Methods

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (!IsAuthenticated && PageName != "login.aspx")
                Response.Redirect("~/Login.aspx", false);
        }

        #endregion
    }
}