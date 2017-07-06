using DDI.Shared;

namespace DDI.WebApi
{
    public class FactoryConfig
    {
        /// <summary>
        /// Configure the Factory class.
        /// </summary>
        public static void Configure()
        {
            Factory.RegisterRepositoryFactory<DDI.Data.RepositoryFactoryEF>();
            Factory.RegisterServiceFactory<DDI.Services.ServiceFactory>();
            Factory.RegisterFactoryProvider<IoC.HttpFactoryScopeProvider>();
        }

    }
}