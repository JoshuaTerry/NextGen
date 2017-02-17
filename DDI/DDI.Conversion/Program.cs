using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Helpers;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Models.Client.CRM;
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

            _filePath = Path.Combine(DirectoryName.DataDirectory, organization);

            // These can be uncommented to run individual conversions.

            //Run<Core.Initialize>();
            //Run<CRM.Initialize>();

            //Run<CRM.SettingsLoader>(); // To run all conversions in SettingsLoader.
            //Run<CRM.SettingsLoader>(new ConversionMethodArgs(CRM.SettingsLoader.ConversionMethod.Codes)); // To run an individual conversion in SettingsLoader.

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

            //Run<CP.SettingsLoader>();
            //Run<CP.PaymentMethodConverter>(new ConversionMethodArgs(CP.PaymentMethodConverter.ConversionMethod.PaymentMethods));

            // Post-conversion tasks

            //Run<CRM.ConstituentSearchIndexer>();
            Console.WriteLine("Start");
            Tester();
            Console.WriteLine("Done");
            Console.ReadLine();

        }

        private static void Tester()
        {
            using (var uow = new UnitOfWorkEF())
            {
                var addr = uow.GetById<Address>(new Guid("aba05c81-324d-4a25-a4bd-3a4f7930e6af"));
                //addr.AddressLine1 = "1253 Elmwood Ave";
                addr.AddressLine1 = "31415926535 Schleicher Ave";
                BusinessLogicHelper.GetBusinessLogic<Address>(uow).Validate(addr);
                uow.SaveChanges();          
            }
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
