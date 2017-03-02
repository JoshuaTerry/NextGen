using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Audit;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services
{
    public class AuditService
    { 
        private readonly IUnitOfWork _uow;
        public AuditService() : this(new UnitOfWorkEF())
        { }
        public AuditService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        private Expression<Func<ObjectChange, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<ObjectChange, object>>[]
            {
                o => o.ChangeSet,
                o => o.PropertyChanges
            };
        }
        private Expression<Func<ObjectChange, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<ObjectChange, object>>[]
            {
                o => o.ChangeSet,
                o => o.ChangeSet.User,
                o => o.PropertyChanges
            };
        }
        public IDataResponse<List<ChangeSet>> GetAllWhereExpression(Expression<Func<ObjectChange, bool>> expression, IPageable search = null)
        {
          
            var result = _uow.GetRepository<ObjectChange>().GetEntities(GetDataIncludesForList()).Where(expression).Select(o => o.ChangeSet).ToList();  //.GetEntities(_includesForList).Where(expression);
            return null;
        }
        public IDataResponse<List<dynamic>> GetAllFlat(Guid id, DateTime start, DateTime end, IPageable search = null)
        {
            var response = new DataResponse<List<dynamic>>();
            try
            {
                var results = _uow.GetRepository<ChangeSet>().Entities
                                 .Where(c => c.Timestamp >= start && c.Timestamp <= end)
                                 .Join(_uow.GetRepository<ObjectChange>().Entities.Where(o => o.EntityId == id.ToString()),
                                         cs => cs.Id,
                                         oc => oc.ChangeSetId,
                                         (cs, oc) => new { cs, oc })
                                 .GroupJoin(_uow.GetRepository<PropertyChange>().Entities,
                                         outer => outer.oc.Id,
                                         pc => pc.ObjectChangeId,
                                         (outer, pc) => new { cs = outer.cs, oc = outer.oc, pc })
                                 .SelectMany(x => x.pc.DefaultIfEmpty(),
                                         (outer, pc) => new { cs = outer.cs, oc = outer.oc, pc })
                                 .Join(_uow.GetRepository<User>().Entities,
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
                                         }).ToList<dynamic>();

                response.Data = results;
            }
            catch(Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }       
    }
}
