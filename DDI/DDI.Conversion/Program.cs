using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Core;
using DDI.Business.Helpers;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Enums.Core;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Client.GL;
using log4net;

namespace DDI.Conversion
{
    class Program
    {

        private static string _filePath;
        private static List<ConversionMethodArgs> _methodsToRun;

        static void Main(string[] args)
        {
            _methodsToRun = null;
            // At some point, we will want to pass in an org as the database that we load data into will be dependent upon organization. 
            //Hard coding for quick demo purposes.
            string organization;
            if (args.Length == 1)
            {
                organization = args[0];
            }
            else
            {
                organization = "NG";
            }

            log4net.Config.XmlConfigurator.Configure();

            Factory.RegisterRepositoryFactory<RepositoryFactoryEF>();

            _filePath = Path.Combine(DirectoryName.DataDirectory, organization);
            GuidHelper.GenerateSequentialGuids = true;

            // These can be uncommented to run individual conversions.

            //Run<Core.Initialize>();
            //Run<Core.SettingsLoader>(new ConversionMethodArgs(Core.SettingsLoader.ConversionMethod.Users));
            //Run<Core.SettingsLoader>(new ConversionMethodArgs(Core.SettingsLoader.ConversionMethod.Codes));
            //Run<Core.SettingsLoader>(new ConversionMethodArgs(Core.SettingsLoader.ConversionMethod.NoteCategories));
            //Run<Core.SettingsLoader>(new ConversionMethodArgs(Core.SettingsLoader.ConversionMethod.Configuration));
            //Run<Core.NoteConverter>(new ConversionMethodArgs(Core.NoteConverter.ConversionMethod.Notes_CRM));


            //Run<CRM.Initialize>();            
            //Run<CRM.SettingsLoader>(); // To run all conversions in SettingsLoader.
            //Run<CRM.SettingsLoader>(new ConversionMethodArgs(CRM.SettingsLoader.ConversionMethod.Codes)); // To run an individual conversion in SettingsLoader.
            //Run<CRM.SettingsLoader>(new ConversionMethodArgs(CRM.SettingsLoader.ConversionMethod.ConstituentEntityNumber)); 

            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.Individuals));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.Organizations));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.Addresses));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.ConstituentAddresses));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.DoingBusinessAs));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.Education));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.AlternateIDs));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.ContactInformation));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.Relationships));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.Tags));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.CustomFieldData));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.Notes));

            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.FileStorage));
            //Run<CRM.ConstituentConverter>(new ConversionMethodArgs(CRM.ConstituentConverter.ConversionMethod.Attachments));


            //Run<GL.SettingsLoader>(new ConversionMethodArgs(GL.SettingsLoader.ConversionMethod.BusinessUnits));
            //Run<GL.SettingsLoader>(new ConversionMethodArgs(GL.SettingsLoader.ConversionMethod.BusinessUnitUsers));
            //Run<GL.SettingsLoader>(new ConversionMethodArgs(GL.SettingsLoader.ConversionMethod.Ledgers));
            //Run<GL.SettingsLoader>(new ConversionMethodArgs(GL.SettingsLoader.ConversionMethod.FiscalYears));
            //Run<GL.SettingsLoader>(new ConversionMethodArgs(GL.SettingsLoader.ConversionMethod.FiscalPeriods));
            //Run<GL.SettingsLoader>(new ConversionMethodArgs(GL.SettingsLoader.ConversionMethod.SegmentLevels));
            //Run<GL.AccountConverter>(new ConversionMethodArgs(GL.AccountConverter.ConversionMethod.Segments));
            //Run<GL.AccountConverter>(new ConversionMethodArgs(GL.AccountConverter.ConversionMethod.AccountGroups));
            //Run<GL.AccountConverter>(new ConversionMethodArgs(GL.AccountConverter.ConversionMethod.Accounts));
            //Run<GL.AccountConverter>(new ConversionMethodArgs(GL.AccountConverter.ConversionMethod.LedgerAccounts));
            //Run<GL.AccountConverter>(new ConversionMethodArgs(GL.AccountConverter.ConversionMethod.LedgerAccountYears));
            //Run<GL.AccountConverter>(new ConversionMethodArgs(GL.AccountConverter.ConversionMethod.AccountPriorYears));
            //Run<GL.AccountConverter>(new ConversionMethodArgs(GL.AccountConverter.ConversionMethod.LedgerAccountMerges));
            //Run<GL.AccountConverter>(new ConversionMethodArgs(GL.AccountConverter.ConversionMethod.AccountBudgets));
            //Run<GL.FundConverter>(new ConversionMethodArgs(GL.FundConverter.ConversionMethod.Funds));
            //Run<GL.FundConverter>(new ConversionMethodArgs(GL.FundConverter.ConversionMethod.FundFromTos));
            //Run<GL.FundConverter>(new ConversionMethodArgs(GL.FundConverter.ConversionMethod.BusinessUnitFromTos));
            //Run<GL.PostedTransactionConverter>(new ConversionMethodArgs(GL.PostedTransactionConverter.ConversionMethod.PostedTransactions));

            //Run<GL.JournalConverter>(new ConversionMethodArgs(GL.JournalConverter.ConversionMethod.Journals));
            //Run<GL.JournalConverter>(new ConversionMethodArgs(GL.JournalConverter.ConversionMethod.JournalLines));
            //Run<GL.JournalConverter>(new ConversionMethodArgs(GL.JournalConverter.ConversionMethod.JournalTransactions));
            //Run<GL.JournalConverter>(new ConversionMethodArgs(GL.JournalConverter.ConversionMethod.JournalApprovals));
            //Run<GL.JournalConverter>(new ConversionMethodArgs(GL.JournalConverter.ConversionMethod.JournalNotes));
            //Run<GL.JournalConverter>(new ConversionMethodArgs(GL.JournalConverter.ConversionMethod.JournalEntityNumbers));

            //Run<GL.JournalConverter>(new ConversionMethodArgs(GL.JournalConverter.ConversionMethod.FileStorage));
            //Run<GL.JournalConverter>(new ConversionMethodArgs(GL.JournalConverter.ConversionMethod.Attachments));

            //Run<GL.JournalSearchIndexer>(new ConversionMethodArgs(GL.JournalSearchIndexer.ConversionMethod.IndexJournals));
            //Run<Core.NoteSearchIndexer>(new ConversionMethodArgs(Core.NoteSearchIndexer.ConversionMethod.IndexNotes));

            //Run<CP.SettingsLoader>(new ConversionMethodArgs(CP.SettingsLoader.ConversionMethod.Configuration));
            //Run<CP.SettingsLoader>(new ConversionMethodArgs(CP.SettingsLoader.ConversionMethod.EFTFormats));
            //Run<CP.SettingsLoader>(new ConversionMethodArgs(CP.SettingsLoader.ConversionMethod.BankAccounts));
            //Run<CP.SettingsLoader>(new ConversionMethodArgs(CP.SettingsLoader.ConversionMethod.ReceiptTypes));
            //Run<CP.SettingsLoader>(new ConversionMethodArgs(CP.SettingsLoader.ConversionMethod.ReceiptBatchTypes));
            Run<CP.ReceiptConverter>(new ConversionMethodArgs(CP.ReceiptConverter.ConversionMethod.ReceiptBatches));

            //Run<CP.PaymentMethodConverter>(new ConversionMethodArgs(CP.PaymentMethodConverter.ConversionMethod.PaymentMethods));

            // Post-conversion tasks

            //Run<CRM.ConstituentSearchIndexer>();



        }

        /// <summary>
        /// Run all converion methods in a conversion class.
        /// </summary>
        private static void Run<T>() where T : ConversionBase, new()
        {
            new T().Execute(_filePath, _methodsToRun);
        }

        /// <summary>
        /// Run a list of conversion methods in a conversion class.
        /// </summary>
        private static void Run<T>(IEnumerable<ConversionMethodArgs> methodsToRun) where T : ConversionBase, new()
        {
            new T().Execute(_filePath, methodsToRun);
        }

        /// <summary>
        /// Run a specific conversion method in a conversion class.
        /// </summary>
        private static void Run<T>(ConversionMethodArgs methodToRun) where T : ConversionBase, new()
        {
            new T().Execute(_filePath, new List<ConversionMethodArgs>() { methodToRun });
        }

    }


}
