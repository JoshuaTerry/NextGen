using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Data;
using DDI.Conversion.LoadDataCRM;

namespace DDI.Conversion
{
    class Program
    {

        

        static void Main(string[] args)
        {
            // At some point, we will want to pass in an org as the database that we load data into will be dependent upon organization. 
            //Hard coding for quick demo purposes.


            DomainContext context;
            string organization = "NG";
            string filePath = "J:\\DDI\\Dept 00 - Common\\Projects\\NextGen\\Conversion\\Data";

            ExecuteCRMLoad(context, organization, filePath);

            }
        }
        

    }
    

}
