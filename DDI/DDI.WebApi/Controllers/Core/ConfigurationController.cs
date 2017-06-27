using DDI.Logger;
using DDI.Services;
using DDI.Shared.Enums;
using DDI.Shared.Helpers;
using DDI.Shared.Statics;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class ConfigurationController : ApiController
    {
        #region Public Properties

        private IConfigurationService _service;
        private ILogger _logger;

        #endregion Public Properties

        #region Public Constructors

        public ConfigurationController(IConfigurationService service)          
        {
            _service = service;
            _logger = LoggerManager.GetLogger(typeof(ConfigurationController));
        }
        
        #endregion Public Constructors

        [HttpGet]
        [Route("api/v1/configurations")]
        public IHttpActionResult GetConfigurations(string module = null)
        {
            try
            {
                ModuleType moduleType = ModuleType.None;
                if (!string.IsNullOrWhiteSpace(module))
                {
                    moduleType = EnumHelper.ConvertToEnum<ModuleType>(module, ModuleType.None);
                }

                return Ok(_service.GetConfiguration(moduleType));
            }

            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/businessdate", Name = RouteNames.BusinessDate)]
        public IHttpActionResult Get()
        {
            try
            {
                var response = _service.GetBusinessDate();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}