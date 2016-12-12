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

namespace DDI.Conversion
{
    class Program
    {



        static void Main(string[] args)
        {
            // At some point, we will want to pass in an org as the database that we load data into will be dependent upon organization. 
            //Hard coding for quick demo purposes.


            using (DomainContext context = new DomainContext())
            { 
                string organization = "NG";
                string filePath = "\\\\ddifs2\\ddi\\DDI\\Dept 00 - Common\\Projects\\NextGen\\Conversion\\Data";

                LoadDataCRM.ExecuteCRMLoad(context, organization, filePath);
                context.SaveChanges();
            }

        }

    }
    

}
