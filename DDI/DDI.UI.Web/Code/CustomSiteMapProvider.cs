using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDI.UI.Web.Code
{
    public class CustomSiteMapProvider : XmlSiteMapProvider
    {
        public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
        {
            // return base.IsAccessibleToUser(context, node);

            bool isAccessible = false;

            try
            {
                if (node.Roles != null && node.Roles.Count > 0)
                {
                    foreach (string role in node.Roles)
                    {
                        if (HttpContext.Current.User.IsInRole(role))
                        {
                            isAccessible = true;
                            break;
                        }
                    }
                }
                else
                {
                    isAccessible = true;
                }

                return isAccessible;
            }
            catch (Exception ex)
            {
                return isAccessible;
            }
        }
    }
}