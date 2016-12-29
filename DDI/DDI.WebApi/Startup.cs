using DDI.WebApi;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace DDI.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            using (var uow = new DDI.Data.UnitOfWorkEF())
            {
                var lookup = new DDI.Business.Common.ZipLookup();
                var addr = new DDI.Business.Common.ZipLookupInfo();
                addr.AddressLine1 = "4647 Shankle Drive";
                addr.AddressLine2 = "";
                addr.City = "Marianna";
                addr.State = uow.FirstOrDefault<Data.Models.Common.State>(p => p.StateCode == "FL" && p.Country.ISOCode == "US");

                string result;
                lookup.Lookup(addr, out result);
            }

        }
    }
}
