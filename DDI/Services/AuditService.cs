using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Audit;
using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services
{
    public class AuditService
    {
        private readonly IReadOnlyRepository<ChangeSet> _repository;
        private readonly IUnitOfWork _uow;
        public AuditService() : this(new UnitOfWorkEF())
        { }
        public AuditService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        public void GetAllWhereExpression(Expression<Func<ChangeSet, bool>> expression, IPageable search = null)
        {
            var a = _uow.GetRepository<ChangeSet>().Entities
                             .Where(expression)
                             .Join(_uow.GetRepository<ObjectChange>().Entities,
                                     cs => cs.Id,
                                     oc => oc.ChangeSetId,
                                     (cs, oc) => new { cs, oc })
                             .GroupJoin(_uow.GetRepository<PropertyChange>().Entities,
                                     outer => outer.oc.Id,
                                     pc => pc.ObjectChangeId,
                                     (outer, pc) => new { cs = outer.cs, oc = outer.oc, pc })
                             .SelectMany(x => x.pc.DefaultIfEmpty(),
                                     (outer, pc) => new { cs = outer.cs, oc = outer.oc, pc })
                             .Join(_uow.GetRepository<DDIUser>().Entities,
                                     outer => outer.cs.UserId,
                                     u => u.Id,
                                     (outer, u) => new
                                     {
                                         ChangeSetId = outer.cs.Id,
                                         Timestamp = outer.cs.Timestamp,
                                         User = u.DisplayName,
                                         EntityType = outer.oc.TypeName,
                                         EntityValue = outer.oc.DisplayName,
                                         Property = outer.pc.PropertyName,
                                         OldValue = outer.pc.OriginalValue,
                                         NewValue = outer.pc.Value
                                     });
 
            //return GetPagedResults(queryable, search);
        }
    }
}
