using DDI.EFAudit.Helpers;
using DDI.Shared.Models.Client.Audit;
using System;
using System.Data.Entity;

namespace DDI.EFAudit.Contexts
{
    public abstract class DbContextAdapter<TChangeSet, TPrincipal> : ObjectContextAdapter<TChangeSet, TPrincipal> where TChangeSet : IChangeSet<TPrincipal>
    {
        private readonly DbContext _context;

        public DbContextAdapter(DbContext context)
            : base(((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext)
        {
            this._context = context;
        }

        public override int SaveAndAcceptChanges(EventHandler onSavingChanges = null)
        {
            // Save is wrapped in disposable listener for SaveChanges.  
            // Handler is invoked AFTER saving but BEFORE accepting changes!!!
            using (new DisposableSavingChangesListener(_context, onSavingChanges))
            {
                return _context.SaveChanges();
            }
        }

        
    }
}
