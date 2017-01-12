using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Data;
using DDI.Conversion;
using DDI.Data.Models.Client;
using System.Data.Entity.Migrations;
using log4net;

namespace DDI.Conversion
{
    class Program
    {



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
                minCount = 71700;
                maxCount = int.MaxValue;
            }
            
			log4net.Config.XmlConfigurator.Configure();

            string filePath = Path.Combine(@"\\ddifs2\ddi\DDI\Dept 00 - Common\Projects\NextGen\Conversion\Data", organization);
            ConversionArgs conversionArgs = new ConversionArgs(organization, filePath, minCount, maxCount, false);

            //new Core.Initialize().Execute(conversionArgs);
            //new CRM.Initialize().Execute(conversionArgs);
            //new CRM.SettingsLoader().Execute(conversionArgs);
            new CRM.ConstituentLoader().Execute(conversionArgs);

        }

    }
    

}
