using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Data;
using DDI.Conversion;
using System.Data.Entity.Migrations;
using log4net;
using DDI.Conversion.Statics;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Conversion
{
    class Program
    {

        private static string _filePath;
        private static List<ConversionMethodArgs> _methodsToRun;

        static void Main(string[] args)
        {
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

            Run<CRM.SettingsLoader>(new ConversionMethodArgs(CRM.SettingsLoader.ConversionMethod.Prefixes));

            var bl = new Business.CRM.NameFormatter();
            var name = bl.UnitOfWork.FirstOrDefault<Constituent>(p => p.ConstituentNumber == 1000048);
            //name.SalutationType = Shared.Enums.CRM.SalutationType.Custom;
            //name.Salutation = "Dear Mr. Awesome";

            var name2 = bl.UnitOfWork.FirstOrDefault<Constituent>(p => p.ConstituentNumber == 1049925);
            //name2.SalutationType = Shared.Enums.CRM.SalutationType.Custom;
            //name2.Salutation = "Dear Mrs. Awesome";

            //name.LastName = "Jones";
            //name.Prefix = bl.UnitOfWork.FirstOrDefault<Prefix>(p => p.Code == "Rep");
            string result = bl.BuildSalutation(name, new Business.CRM.SalutationFormattingOptions()
            {
                PreferredType = Shared.Enums.CRM.SalutationType.Formal ,
                ForcePreferredtype = true,
                KeepSeparate = false,
                Recipient = Business.CRM.LabelRecipient.Both,
                AddFirstNames = false
                , OmitPrefix = false
                
               

            });
            Console.WriteLine(result);
            Console.ReadLine();
            return;

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
