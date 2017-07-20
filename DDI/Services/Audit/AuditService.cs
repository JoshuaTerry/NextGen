﻿using DDI.Shared;
using DDI.Shared.Enums.Core;
using DDI.Shared.Models.Client.Audit;
using DDI.Shared.Models.Client.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DDI.Services
{
    public class AuditService : IService
    {
        private readonly IUnitOfWork _uow;

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

        public IDataResponse<List<ObjectChange>> GetChanges(Guid id, DateTime start, DateTime end, IPageable search = null)
        {
            var changes = _uow.GetEntities<ObjectChange>(o => o.ChangeSet, o => o.PropertyChanges)
                              .Where(o => o.EntityId == id.ToString() &&
                                          o.ChangeSet.Timestamp > start &&
                                          o.ChangeSet.Timestamp <= end).ToList();
            var response = new DataResponse<List<ObjectChange>>();
            response.Data = changes;
            return response;
        }

        public IDataResponse<List<dynamic>> GetAllFlat(Guid[] ids, DateTime start, DateTime end, IPageable search = null)
        {
            var response = new DataResponse<List<dynamic>>();
            var stringArray = Array.ConvertAll(ids, x => x.ToString());
            try
            {
                var results = _uow.GetRepository<ChangeSet>().Entities
                                 .Where(c => c.Timestamp >= start && c.Timestamp <= end)
                                 .Join(_uow.GetRepository<ObjectChange>().Entities.Where(o => stringArray.Contains(o.EntityId)),
                                         cs => cs.Id,
                                         oc => oc.ChangeSetId,
                                         (cs, oc) => new { cs, oc })
                                 .GroupJoin(_uow.GetRepository<PropertyChange>().Entities,
                                         outer => outer.oc.Id,
                                         pc => pc.ObjectChangeId,
                                         (outer, pc) => new { cs = outer.cs, oc = outer.oc, pc })
                                 .SelectMany(x => x.pc.DefaultIfEmpty(),
                                         (outer, pc) => new
                                         {
                                             ChangeSetId = outer.cs.Id,
                                             Timestamp = outer.cs.Timestamp,
                                             User = outer.cs.UserName,
                                             ChangeType = outer.oc.ChangeType,
                                             EntityType = outer.oc.TypeName,
                                             EntityValue = outer.oc.DisplayName,
                                             Property = pc.PropertyName,
                                             PropertyChangeType = pc.ChangeType,
                                             OldDisplayName = pc.OriginalDisplayName,
                                             OldValue = pc.OriginalValue,
                                             NewDisplayName = pc.NewDisplayName,
                                             NewValue = pc.NewValue
                                         }).Distinct().OrderBy(c => c.EntityType).OrderByDescending(c => c.Timestamp).ToList<dynamic>();

                response.Data = results;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public IDataResponse<List<dynamic>> GetByEntityType(EntityType entityType, DateTime start, DateTime end, IPageable search = null)
        {
            var response = new DataResponse<List<dynamic>>();
            try
            {
                var results = _uow.GetRepository<ChangeSet>().Entities
                                 .Where(c => c.Timestamp >= start && c.Timestamp <= end)
                                 .Join(_uow.GetRepository<ObjectChange>().Entities.Where(o => o.TypeName == entityType.ToString()),
                                         cs => cs.Id,
                                         oc => oc.ChangeSetId,
                                         (cs, oc) => new { cs, oc })
                                 .GroupJoin(_uow.GetRepository<PropertyChange>().Entities,
                                         outer => outer.oc.Id,
                                         pc => pc.ObjectChangeId,
                                         (outer, pc) => new { cs = outer.cs, oc = outer.oc, pc })
                                 .SelectMany(x => x.pc.DefaultIfEmpty(),
                                         (outer, pc) => new
                                         {
                                             ChangeSetId = outer.cs.Id,
                                             Timestamp = outer.cs.Timestamp,
                                             User = outer.cs.UserName,
                                             ChangeType = outer.oc.ChangeType,
                                             EntityType = outer.oc.TypeName,
                                             EntityValue = outer.oc.DisplayName,
                                             Property = pc.PropertyName,
                                             PropertyChangeType = pc.ChangeType,
                                             OldDisplayName = pc.OriginalDisplayName,
                                             OldValue = pc.OriginalValue,
                                             NewDisplayName = pc.NewDisplayName,
                                             NewValue = pc.NewValue
                                         }).Distinct().OrderBy(c => c.EntityType).OrderByDescending(c => c.Timestamp).ToList<dynamic>();
               
                response.Data = results;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
    }
}
