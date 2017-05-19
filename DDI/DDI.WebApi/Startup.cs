using DDI.Shared;
using DDI.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;


[assembly: OwinStartup(typeof(Startup))]

namespace DDI.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            ConfigureAuth(app);

            // Set the CacheHelper's cache provider
            Shared.Caching.CacheHelper.CacheProvider = new Providers.HttpCacheProvider();

            // Initialize the repository factory.
            Factory.RegisterRepositoryFactory(new DDI.Data.RepositoryFactoryEF());
        }
    }
}
