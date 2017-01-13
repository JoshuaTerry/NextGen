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
            int minCount;
            int maxCount;
            if (args.Length == 3)
            {
                organization = args[0];
                int.TryParse(args[1], out minCount);
                int.TryParse(args[2], out maxCount);
            }
            if (args.Length == 2)
            {
                organization = "NG";
                int.TryParse(args[0], out minCount);
                int.TryParse(args[1], out maxCount);
            }
            else
            {
                organization = "NG";
                minCount = 0;
                maxCount = int.MaxValue;
            }

            log4net.Config.XmlConfigurator.Configure();

            _filePath = Path.Combine(@"\\ddifs2\ddi\DDI\Dept 00 - Common\Projects\NextGen\Conversion\Data", organization);
            _methodsToRun = new List<ConversionMethodArgs>()
            {
               
            };

            //Run<Core.Initialize>();

            //Run<CRM.Initialize>();
            //Run<CRM.SettingsLoader>();

            Run<CRM.ConstituentLoader>(new ConversionMethodArgs(CRM.ConstituentLoader.ConversionMethod.Organizations_FW, 0, 0, true));
        }

        private static void Run<T>() where T : ConversionBase, new()
        {
            new T().Execute(_filePath, _methodsToRun);
        }

        private static void Run<T>(IEnumerable<ConversionMethodArgs> methodsToRun) where T : ConversionBase, new()
        {
            new T().Execute(_filePath, methodsToRun);
        }

        private static void Run<T>(ConversionMethodArgs methodsToRun) where T : ConversionBase, new()
        {
            new T().Execute(_filePath, new List<ConversionMethodArgs>() { methodsToRun });
        }

    }


}
