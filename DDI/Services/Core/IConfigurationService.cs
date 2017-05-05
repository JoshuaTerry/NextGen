using DDI.Shared;
using DDI.Shared.Enums;

namespace DDI.Services
{
    public interface IConfigurationService
    {
        IDataResponse GetConfiguration(ModuleType moduleType);
    }
}