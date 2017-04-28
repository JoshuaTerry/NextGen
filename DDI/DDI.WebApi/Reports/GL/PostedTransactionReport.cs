using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DDI.Data;
using DDI.Shared.Models.Client.GL;
using System.Linq;
using System.Collections.Generic;

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
            BindToData();
        }
        private void BindToData()
        {
            DateTime dtStart = DateTime.MinValue;
            DateTime dtEnd = DateTime.MaxValue;

            if (StartDate != null && StartDate.Value is DateTime)
            {
                dtStart = (DateTime)StartDate.Value;
            }

            if (EndDate != null && EndDate.Value is DateTime)
            {
                dtEnd = (DateTime)EndDate.Value;
            }

            this.DataSource = new Repository<PostedTransaction>().GetEntities(p => p.LedgerAccountYear.Account).Where(p => p.TransactionDate >= dtStart && p.TransactionDate <= dtEnd).ToList();
        }

        private void xrLabel3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel3.Text = $"{(DateTime)StartDate.Value:d} to {(DateTime)EndDate.Value:d}";
        }
    }
}
