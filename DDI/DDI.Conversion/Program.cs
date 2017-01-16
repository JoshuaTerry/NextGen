﻿using System;
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
            if (args.Length == 1)
            {
                organization = args[0];
            }
            else
            {
                organization = "NG";
            }

            log4net.Config.XmlConfigurator.Configure();

            _filePath = Path.Combine(@"\\ddifs2\ddi\DDI\Dept 00 - Common\Projects\NextGen\Conversion\Data", organization);
            _methodsToRun = new List<ConversionMethodArgs>()
            {
               
            };

            //Run<Core.Initialize>();

            //Run<CRM.Initialize>();
            //Run<CRM.SettingsLoader>();
            Run<CRM.SettingsLoader>(new ConversionMethodArgs(CRM.SettingsLoader.ConversionMethod.Codes));

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

        }

        private static void Run<T>() where T : ConversionBase, new()
        {
            new T().Execute(_filePath, _methodsToRun);
        }

        private static void Run<T>(IEnumerable<ConversionMethodArgs> methodsToRun) where T : ConversionBase, new()
        {
            new T().Execute(_filePath, methodsToRun);
        }

        private static void Run<T>(ConversionMethodArgs methodToRun) where T : ConversionBase, new()
        {
            new T().Execute(_filePath, new List<ConversionMethodArgs>() { methodToRun });
        }

    }


}
