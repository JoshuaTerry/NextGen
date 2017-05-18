using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using DDI.Logger;
using DDI.Shared;

namespace DDI.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(WebApiApplication));
        
        #region Protected Methods

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            InitializeLogging();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            InitializeRepoFactory();
        }

        private void InitializeLogging()
        {
            var fileInfo = new FileInfo(Path.Combine(HttpRuntime.AppDomainAppPath, "logging.config"));
            if (!fileInfo.Exists)
                throw new FileNotFoundException("Could not find file", fileInfo.Name);
            LoggerManager.LoadAndWatchConfiguration(fileInfo);
            _logger.LogInformation("Logging configuration loaded from " + fileInfo.FullName);
        }

        private void InitializeRepoFactory()
        {
            RepoFactory.RegisterTypes(typeof(Data.UnitOfWorkEF), typeof(Data.Repository<>));
        }

        #endregion Protected Methods
    }
}
