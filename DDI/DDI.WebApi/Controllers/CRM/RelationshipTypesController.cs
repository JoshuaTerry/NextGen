﻿using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class RelationshipTypesController : GenericController<RelationshipType>
    {
        public RelationshipTypesController(IService<RelationshipType> service) : base(service) { }

        private string _allFields = null;

        [HttpGet]
        [Route("api/v1/relationshiptypes")]
        public override IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/relationshiptypes/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/relationshiptypes")]
        public override IHttpActionResult Post([FromBody] RelationshipType entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/relationshiptypes/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/relationshiptypes/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        protected override Expression<Func<RelationshipType, object>>[] GetDataIncludesForSingle() =>
            new Expression<Func<RelationshipType, object>>[]
            {
                p => p.RelationshipCategory,
                p => p.ReciprocalTypeMale,
                p => p.ReciprocalTypeFemale

            };

        protected override Expression<Func<RelationshipType, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<RelationshipType, object>>[]
            {
                p => p.RelationshipCategory,
                p => p.ReciprocalTypeMale,
                p => p.ReciprocalTypeFemale
                
            };
        }
        protected override string FieldsForList => FieldLists.CodeFields;

        protected override string FieldsForAll
        {
            get
            {
                if (_allFields == null)
                {
                    // For entity types with recursive properties or large collections, we need to exclude these.
                    // This is an example of using the FieldListBuilder to create a list of fields.
                    _allFields = FieldListBuilder
                        .IncludeAll()
                        .Include(p => p.ReciprocalTypeFemale.DisplayName)
                        .Include(p => p.ReciprocalTypeMale.DisplayName)
                        .Exclude(p => p.FemaleTypes)
                        .Exclude(p => p.MaleTypes)
                        .Exclude(p => p.Relationships)
                        .Exclude(p => p.RelationshipCategory.RelationshipTypes);
                }
                return _allFields;
            }
        }
    }
}