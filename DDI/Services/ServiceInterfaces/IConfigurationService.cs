using System;
using DDI.Shared;
using DDI.Shared.Enums;

namespace DDI.Services
{
    public interface IConfigurationService : IService
    {
        IDataResponse GetConfiguration(ModuleType moduleType);
        IDataResponse<DateTime> GetBusinessDate();
    }
}