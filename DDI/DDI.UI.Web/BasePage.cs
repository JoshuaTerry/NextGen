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
        
        private string PageName
        {
            get
            {
                return Path.GetFileName(Page.AppRelativeVirtualPath).ToLower();
            }
        }

        #endregion        
    }
}