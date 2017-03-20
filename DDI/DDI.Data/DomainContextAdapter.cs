using DDI.EFAudit.Contexts;
using DDI.Shared.Models.Client.Audit;
using DDI.Shared.Models.Client.Security;
using System;
using System.Linq;

namespace DDI.Data
{
    public class DomainContextAdapter : DbContextAdapter<ChangeSet, User>
    {
        private DomainContext context;

        public DomainContextAdapter(DomainContext context) : base(context)
        {
            this.context = context;
        }

        public override IQueryable<IChangeSet<User>> ChangeSets
        {
            get { return context.ChangeSets; }
        }
        public override IQueryable<IObjectChange<User>> ObjectChanges
        {
            get { return context.ObjectChanges; }
        }
        public override IQueryable<IPropertyChange<User>> PropertyChanges
        {
            get { return context.PropertyChanges; }
        }
        public override void AddChangeSet(ChangeSet changeSet)
        {
            context.ChangeSets.Add(changeSet);
        }

        public override Type UnderlyingContextType
        {
            get { return typeof(DomainContext); }
        }
    }
}
