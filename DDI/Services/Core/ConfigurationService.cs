using System;
using System.Collections.Generic;
using System.Dynamic;
using DDI.Business.Core;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Enums;
using DDI.Shared.Models.Client.Core;

namespace DDI.Services
{
    public class ConfigurationService : ServiceBase<Configuration>, IConfigurationService
    {
        #region Private Fields

        private ConfigurationLogic _logic;

        #endregion Private Fields

        #region Public Methods

        public IDataResponse GetConfiguration(ModuleType moduleType)
        {
            // Get the dictionary of known configuration types.
            Dictionary<ModuleType, Type> configurations = _logic.GetConfigurationTypes();

            if (moduleType != ModuleType.None && configurations.ContainsKey(moduleType))
            {
                // A specific module was specified              
                return new DataResponse<ConfigurationBase>
                {
                    Data = _logic.GetConfiguration(configurations[moduleType])
                };
            }

            // No module was specified, so build a dynamic response containing all configurations.

            var response = new DataResponse<dynamic>()
            {
                Data = new ExpandoObject()
            };
            
            foreach (var entry in configurations)
            {
                ConfigurationBase config = _logic.GetConfiguration(entry.Value);
                ((IDictionary<string, object>)response.Data)[entry.Key.ToString()] = config;
            }

            return response;
        }

        /// <summary>
        /// Get the business date for the client.
        /// </summary>
        public IDataResponse<DateTime> GetBusinessDate()
        {
            DateTime businessDate = DateTime.Now.Date;

            CoreConfiguration config = _logic.GetConfiguration<CoreConfiguration>();

            if (config != null && config.UseBusinessDate && config.BusinessDate.HasValue)
            {
                businessDate = config.BusinessDate.Value;
            }

            return new DataResponse<DateTime>() { Data = businessDate };
        }


        #endregion Public Methods

        #region Private Methods
        protected override void Initialize()
        {
            _logic = UnitOfWork.GetBusinessLogic<ConfigurationLogic>();
        }

        #endregion Private Methods
    }
}
