using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Audit;
using DDI.Shared.Models.Client.Core;
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
        private readonly IReadOnlyRepository<ChangeSet> _repository;
        private readonly IUnitOfWork _uow;
        public AuditService() : this(new UnitOfWorkEF())
        { }
        public AuditService(IUnitOfWork uow)
        {
            this._uow = uow;
        }

        // JLT - Can't find a way when calling to refer to the sub-types on ChangeSet in the expression
        public IDataResponse<List<dynamic>> GetAllWhereExpression(Expression<Func<ChangeSet, bool>> expression, IPageable search = null)
        {
            var response = new DataResponse<List<dynamic>>();
            try
            {
                var results = _uow.GetRepository<ChangeSet>().Entities
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
                                         }).ToList<dynamic>();

                response.Data = results;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        public IDataResponse<List<dynamic>> GetAll(Guid id, DateTime start, DateTime end, IPageable search = null)
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
