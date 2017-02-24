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
        private readonly IReadOnlyRepository<ChangeSet> _csRepo;
        private readonly IReadOnlyRepository<ObjectChange> _ocRepo;
        private readonly IReadOnlyRepository<PropertyChange> _pcRepo;
        private readonly IRepository<DDIUser> _uRepo;
        private readonly IUnitOfWork _uow;
        public AuditService() : this(new ReadOnlyRepository<ChangeSet>(), new ReadOnlyRepository<ObjectChange>(), new ReadOnlyRepository<PropertyChange>(), new Repository<DDIUser>())
        { }
        public AuditService(IReadOnlyRepository<ChangeSet> crepo, IReadOnlyRepository<ObjectChange> orepo, IReadOnlyRepository<PropertyChange> prepo, IRepository<DDIUser> urepo)
        {
            this._csRepo = crepo;
            this._ocRepo = orepo;
            this._pcRepo = prepo;
            this._uRepo = urepo;
        }

        public IDataResponse<List<dynamic>> GetAll(Guid id, DateTime start, DateTime end, IPageable search = null)
        {
            var response = new DataResponse<List<dynamic>>();
            try
            {
                var results = _csRepo.Entities
                                 .Where(c => c.Timestamp >= start && c.Timestamp <= end)
                                 .Join(_ocRepo.Entities.Where(o => o.EntityId == id.ToString()),
                                         cs => cs.Id,
                                         oc => oc.ChangeSetId,
                                         (cs, oc) => new { cs, oc })
                                 .GroupJoin(_pcRepo.Entities,
                                         outer => outer.oc.Id,
                                         pc => pc.ObjectChangeId,
                                         (outer, pc) => new { cs = outer.cs, oc = outer.oc, pc })
                                 .SelectMany(x => x.pc.DefaultIfEmpty(),
                                         (outer, pc) => new { cs = outer.cs, oc = outer.oc, pc })
                                 .Join(_uRepo.Entities,
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
