using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Enums;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Core
{
    public class CoreConfiguration : ConfigurationBase
    {
        public string ClientName { get; set; }

        public string ClientCode { get; set; }

        public bool UseBusinessDate => BusinessDate.HasValue;

        public DateTime? BusinessDate { get; set; }

        public string BusinessUnitLabel { get; set; }

        public bool MultipleBusinessUnits { get; private set; }

        public override ModuleType ModuleType => ModuleType.Core;

        public override void AfterLoad(IUnitOfWork uow)
        {
            // Set non-persistent values.
            MultipleBusinessUnits = uow.Any<BusinessUnit>(p => p.BusinessUnitType != Shared.Enums.GL.BusinessUnitType.Organization);
        }

    }
}
