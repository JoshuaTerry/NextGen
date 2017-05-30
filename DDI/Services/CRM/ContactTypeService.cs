using System;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics.CRM;

namespace DDI.Services
{
    public class ContactTypeService : ServiceBase<ContactType>
    {
        public ContactTypeService(IUnitOfWork uow) : base(uow) { }

        public override IDataResponse Delete(ContactType entity)
        {
            if (!entity.CanDelete)
            {
                return GetErrorResponse(UserMessagesCRM.ContactTypeCantDelete);
            }

            return base.Delete(entity);
        }

    }
}
