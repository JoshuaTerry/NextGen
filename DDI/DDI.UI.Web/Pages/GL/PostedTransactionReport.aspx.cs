using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace DDI.UI.Web
{
    public partial class PostedTransactionReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportDocumentViewer.SettingsRemoteSource.ReportTypeName = "PostedTransactionReport," + Token.GetToken().Access_Token;
            ReportDocumentViewer.SettingsRemoteSource.ServerUri = Regex.Replace(ConfigurationManager.AppSettings["ApiUrl"], @"/api/v.*/", "/DXReportService.svc");
        }
    }
}