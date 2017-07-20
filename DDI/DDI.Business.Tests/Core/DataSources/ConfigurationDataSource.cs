using System;
using System.Collections.Generic;
using DDI.Business.Core;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;

namespace DDI.Business.Tests.Core.DataSources
{
    public static class ConfigurationDataSource
    {
        public static IList<Configuration> GetDataSource(IUnitOfWork uow)
        {

            IList<Configuration> existing = uow.GetRepositoryDataSource<Configuration>();
            if (existing != null)
            {
                return existing;
            }
            
            var list = new List<Configuration>();
            uow.CreateRepositoryForDataSource(list);

            var bl = uow.GetBusinessLogic<ConfigurationLogic>();

            CreateCoreConfiguration(bl);
                       
            return list;
        }   
        
        private static void CreateCoreConfiguration(ConfigurationLogic bl)
        {
            var config = new CoreConfiguration
            {
                BusinessDate = DateTime.Now.Date,
                BusinessUnitLabel = "Entity",
                ClientCode = "TEST",
                ClientName = "Unit Test Environment"
            };

            bl.SaveConfiguration(config);
        }

    }

    
}
