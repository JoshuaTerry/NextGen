using DDI.EFAudit.Contexts; 
using DDI.Shared.Models.Client.Audit;
using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Data
{
    public class DomainContextAdapter : DbContextAdapter<ChangeSet, UserLogin>
    {
        private DomainContext context;

        public DomainContextAdapter(DomainContext context) : base(context)
        {
            this.context = context;
        }

        public override IQueryable<IChangeSet<UserLogin>> ChangeSets
        {
            get { return context.ChangeSets; }
        }
        public override IQueryable<IObjectChange<UserLogin>> ObjectChanges
        {
            get { return context.ObjectChanges; }
        }
        public override IQueryable<IPropertyChange<UserLogin>> PropertyChanges
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
