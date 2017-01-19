using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Core;
using DDI.Business.CRM;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class CRMConfigurationDataSource
    {
        public static CRMConfiguration GetDataSource(UnitOfWorkNoDb uow)
        {
            uow.CreateRepositoryForDataSource(new List<Configuration>());
            var logic = uow.GetBusinessLogic<ConfigurationLogic>();

            var configuration = logic.GetConfiguration<CRMConfiguration>();
            configuration.DefaultSalutationType = Shared.Enums.CRM.SalutationType.Formal;
            configuration.AddFirstNamesToSpouses = false;
            configuration.OmitInactiveSpouse = true;

            logic.SaveConfiguration(configuration);

            return configuration;
        }    

    }

    
}
