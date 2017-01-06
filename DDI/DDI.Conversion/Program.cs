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
using System.Data.Entity;

namespace DDI.Conversion
{
    class Program
    {



        static void Main(string[] args)
        {
            // At some point, we will want to pass in an org as the database that we load data into will be dependent upon organization. 
            //Hard coding for quick demo purposes.
            int minCount;
            int maxCount;
            if (args.Length == 2)
            {
                int.TryParse(args[0], out minCount);
                int.TryParse(args[1], out maxCount);
            }
            else
            {
                minCount = 0;
                maxCount = 999999;
            }



			log4net.Config.XmlConfigurator.Configure();

            var bl = new Business.CRM.NameFormatter();
            var name1 = bl.UnitOfWork.FirstOrDefault<Data.Models.Client.CRM.Constituent>(p => p.ConstituentNumber == 1509818);
            var name2 = bl.UnitOfWork.FirstOrDefault<Data.Models.Client.CRM.Constituent>(p => p.LastName.StartsWith("Byers"));

            name1.MiddleName = "Henry";
            name1.Prefix = bl.UnitOfWork.FirstOrDefault<Data.Models.Client.CRM.Prefix>(p => p.Code == "Law");

            name2.MiddleName = "Elizabeth";
            name2.Prefix = bl.UnitOfWork.FirstOrDefault<Data.Models.Client.CRM.Prefix>(p => p.Code == "Dr");

            string line1, line2;

            bl.BuildNameLines(name1, name2, new Business.CRM.LabelFormattingOptions() { Recipient = Business.CRM.LabelRecipient.Both }, out line1, out line2);

            Console.WriteLine(line1);
            Console.WriteLine(line2);
            Console.ReadLine();
            return;

            using (DomainContext context = new DomainContext())
            { 
                var common = new CommonContext();
                string organization = "NG";
                string filePath = @"\\ddifs2\ddi\DDI\Dept 00 - Common\Projects\NextGen\Conversion\Data";
											
                LoadDataCRM.ExecuteCRMLoad(context, common, organization, filePath, minCount, maxCount);
                context.SaveChanges();
            }

        }

    }
    

}
