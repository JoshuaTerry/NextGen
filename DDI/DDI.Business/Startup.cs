using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.Domain;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DDI.Business.Startup))]

namespace DDI.Business
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Test();
        }

        public void Test()
        {
            var domain = new ConstituentDomain();
            var name = domain.Repository.Entities.FirstOrDefault(p => p.LastName == "Byers");
            name.FirstName = "Elizabeth";

            domain.Validate(name);

        }
    }
}
