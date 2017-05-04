using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Data;
using DDI.Data.Helpers;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;
using DDI.WebApi.Helpers;
using DevExpress.XtraReports.Parameters;

namespace DDI.WebApi.Reports.GL
{
    public partial class PostedTransactionReport : DevExpress.XtraReports.UI.XtraReport
    {
        public PostedTransactionReport()
        {
            InitializeComponent();
            DataSource = new List<PostedTransaction>();
        }

        private void PostedTransactionReport_DataSourceDemanded(object sender, EventArgs e)
        {
            if (!ReportHelper.SetPrincipal(Parameters))
            {
                return;
            }

            BindToData();
        }
        private void BindToData()
        {
            DateTime dtStart = DateTime.MinValue;
            DateTime dtEnd = DateTime.MaxValue;
            Guid businessUnitId = BusinessUnit.Value as Guid? ?? Guid.Empty;

            // We'll need some logic to validate the business unit, and to default the business unit Id to the org. business unit if multiple business units not defined.          

            if (StartDate != null && StartDate.Value is DateTime)
            {
                dtStart = (DateTime)StartDate.Value;
            }

            if (EndDate != null && EndDate.Value is DateTime)
            {
                dtEnd = (DateTime)EndDate.Value;
            }                      

            this.DataSource = new Repository<PostedTransaction>().GetEntities(p => p.LedgerAccountYear.Account).Where(p => p.TransactionDate >= dtStart && p.TransactionDate <= dtEnd && p.PostedTransactionType == Shared.Enums.GL.PostedTransactionType.Actual && p.FiscalYear.Ledger.BusinessUnitId == businessUnitId).ToList();
        }
        
        private void xrSubtitle_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Guid unitId = BusinessUnit.Value as Guid? ?? Guid.Empty;
            if (unitId != Guid.Empty)
            {
                xrSubtitle.Text = new Repository<BusinessUnit>().GetById(unitId)?.Name ?? string.Empty;
            }
            else
            {
                xrSubtitle.Text = string.Empty;
            }

            xrParameters.Text = $"{(DateTime)StartDate.Value:d} to {(DateTime)EndDate.Value:d}";
        }

        private void PostedTransactionReport_ParametersRequestBeforeShow(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
            // Set up parameters for business units.

            if (!ReportHelper.SetPrincipal(Parameters))
            {
                return;
            }

            BusinessUnit.Description = "Entity";

            var businessUnitLogic = new BusinessUnitLogic();
            if (!businessUnitLogic.IsMultiple)
            {
                BusinessUnit.Visible = false;
            }

            else
            {
                var settings = BusinessUnit.LookUpSettings as StaticListLookUpSettings;
                settings.LookUpValues.Clear();
                User user = EntityFrameworkHelpers.GetCurrentUser();

                foreach (var entry in new Repository<BusinessUnit>().GetEntities().Where(p => p.BusinessUnitType != Shared.Enums.GL.BusinessUnitType.Organization && p.Users.Any(u => u.Id == user.Id)))
                {
                    settings.LookUpValues.Add(new LookUpValue(entry.Id, entry.Name));
                }

                BusinessUnit.Value = businessUnitLogic.GetDefaultBusinessUnit()?.Id ?? settings.LookUpValues.FirstOrDefault()?.Value;
            }

            // Start / End date values            
            StartDate.Value = new DateTime(2014, 3, 1);
            EndDate.Value = new DateTime(2014, 3, 31);
            
        }
    }
}
