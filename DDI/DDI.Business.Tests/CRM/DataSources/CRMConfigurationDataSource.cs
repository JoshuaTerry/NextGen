using System.Collections.Generic;
using System.Linq;
using DDI.Business.Core;
using DDI.Business.CRM;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class CRMConfigurationDataSource
    {
        public static CRMConfiguration GetDataSource(IUnitOfWork uow)
        { 
            uow.CreateRepositoryForDataSource(new List<Configuration>());
            var logic = uow.GetBusinessLogic<ConfigurationLogic>();

            var addressTypes = AddressTypeDataSource.GetDataSource(uow);

            var configuration = logic.GetConfiguration<CRMConfiguration>();
            configuration.DefaultSalutationType = Shared.Enums.CRM.SalutationType.Formal;
            configuration.AddFirstNamesToSpouses = false;
            configuration.OmitInactiveSpouse = true;

            configuration.DefaultAddressType = addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Home);

            configuration.HomeAddressTypes = new AddressType[]
            {
                addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Home),
                addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Location),
                addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Work)
            };

            configuration.MailAddressTypes = new AddressType[]
            {
                addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Mailing),
                addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Home),
                addressTypes.FirstOrDefault(p => p.Code == AddressTypeCodes.Work),
            };

            logic.SaveConfiguration(configuration);

            return configuration;
        }    

    }

    
}
