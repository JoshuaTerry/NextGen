using DDI.Business.Core;
using DDI.Shared;
using DDI.Shared.Enums;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Business.CRM
{
    public class CRMConfiguration : ConfigurationBase
    {
        #region Properties

        public override ModuleType ModuleType { get; } = ModuleType.CRM;

        public bool UseRegionSecurity { get; set; }

        public bool OmitInactiveSpouse { get; set; }

        public bool AddFirstNamesToSpouses { get; set; }

        public SalutationType DefaultSalutationType { get; set; }

        public IList<AddressType> HomeAddressTypes { get; set; }

        public IList<AddressType> MailAddressTypes { get; set; }

        public AddressType DefaultAddressType { get; set; }

        public ConstituentStatus DeceasedStatus { get; set; }

        public IList<Tag> DeceasedTags { get; set; }

        public bool DisplayTagDescription { get; set; }

        public bool ApplyDeceasedTag { get; set; }

        public bool RequireVoidedCheck { get; set; }
        
        #endregion

        #region Constructors

        public CRMConfiguration()
        {
            HomeAddressTypes = new List<AddressType>();
            MailAddressTypes = new List<AddressType>();
            DeceasedTags = new List<Tag>();                          
        }

        #endregion

        #region Method Overrides

        public override string SaveProperty(string name)
        {
            switch(name)
            {
                case nameof(HomeAddressTypes):
                    return GetGuidStrings(HomeAddressTypes);
                case nameof(MailAddressTypes):
                    return GetGuidStrings(MailAddressTypes);
                case nameof(DeceasedTags):
                    return GetGuidStrings(DeceasedTags);
            }
            return null;
        }

        public override void LoadProperty(string name, string value, IUnitOfWork uow)
        {
            switch(name)
            {
                case nameof(HomeAddressTypes):
                    HomeAddressTypes = GetEntityList<AddressType>(value, uow);
                    break;
                case nameof(MailAddressTypes):
                    MailAddressTypes = GetEntityList<AddressType>(value, uow);
                    break;
                case nameof(DeceasedTags):
                    DeceasedTags = GetEntityList<Tag>(value, uow);
                    break;
            }
        }

        public override void Attach(IUnitOfWork uow)
        {
            HomeAddressTypes = HomeAddressTypes.Select(p => uow.Attach(p)).ToList();
            MailAddressTypes = MailAddressTypes.Select(p => uow.Attach(p)).ToList();
            DeceasedTags = DeceasedTags.Select(p => uow.Attach(p)).ToList();

            DeceasedStatus = uow.Attach(this.DeceasedStatus);
            DefaultAddressType = uow.Attach(this.DefaultAddressType);
            
        }

        #endregion

    }

}
