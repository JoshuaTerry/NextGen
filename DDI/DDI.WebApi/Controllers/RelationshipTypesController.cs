using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{

    public class RelationshipTypesController : ControllerBase<RelationshipType>
    {
        private string _allFields = null;

        [HttpGet]
        [Route("api/v1/relationshiptypes", Name = RouteNames.RelationshipTypes)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.RelationshipTypes, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/relationshiptypes/{id}", Name = RouteNames.RelationshipTypes + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/relationshiptypes", Name = RouteNames.RelationshipTypes + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] RelationshipType entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/relationshiptypes/{id}", Name = RouteNames.RelationshipTypes + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/relationshiptypes/{id}", Name = RouteNames.RelationshipTypes + RouteVerbs.Delete)]
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
                    _allFields = new PathHelper.FieldListBuilder<RelationshipType>()
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