using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using DDI.WebApi.Helpers;
using DDI.WebApi.Reports.GL;
using DevExpress.XtraReports.Service.Extensions;
using DevExpress.XtraReports.UI;

namespace DDI.WebApi.Reports
{
    [Export(typeof(IReportResolver))]
    public class CustomReportResolver : IReportResolver
    {
        public XtraReport Resolve(string reportName, bool getParameters)
        {
            // Report type should include the type of report, a comma, and a bearer access token.
            string[] parameters = reportName.Split(',');
            string reportType = parameters.Length > 0 ? parameters[0].Trim() : string.Empty;
            string token = parameters.Length > 1 ? parameters[1].Trim() : string.Empty;

            XtraReport report = null;

            if (ReportHelper.SetPrincipal(token))
            { 

                // Really dumb way to resolve report types.
                if (reportType == "PostedTransactionReport")
                {
                    report = new PostedTransactionReport();
                }

                if (!string.IsNullOrWhiteSpace(token))
                {
                    report.Parameters.Add(new DevExpress.XtraReports.Parameters.Parameter()
                    {
                        Name = ReportHelper.ACCESS_TOKEN_PARAMETER,
                        Value = token,
                        Type = typeof(string),
                        Visible = false
                    });
                }
            }

            return report;
        }
    }


}