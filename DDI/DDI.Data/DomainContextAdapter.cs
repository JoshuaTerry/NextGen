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
    public class DomainContextAdapter : DbContextAdapter<ChangeSet, ApplicationUser>
    {
        private DomainContext context;

        public DomainContextAdapter(DomainContext context) : base(context)
        {
            this.context = context;
        }

        public override IQueryable<IChangeSet<ApplicationUser>> ChangeSets
        {
            get { return context.ChangeSets; }
        }
        public override IQueryable<IObjectChange<ApplicationUser>> ObjectChanges
        {
            get { return context.ObjectChanges; }
        }
        public override IQueryable<IPropertyChange<ApplicationUser>> PropertyChanges
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
