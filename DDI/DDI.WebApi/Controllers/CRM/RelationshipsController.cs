using DDI.Services;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class RelationshipsController : GenericController<Relationship>
    {
        protected new IRelationshipService Service => (IRelationshipService)base.Service;

        public RelationshipsController()
            : base(new RelationshipService())
        {
        }

        private string DefaultFields =>
                string.Join(",", new string[]
                {
                    "Id",
                    $"{nameof(Relationship.RelationshipType)}Id",
                    $"{nameof(Relationship.RelationshipType)}.Id",
                    $"{nameof(Relationship.RelationshipType)}.{nameof(RelationshipType.Name)}",
                    $"{nameof(Relationship.RelationshipType)}.{nameof(RelationshipType.RelationshipCategory)}.{nameof(RelationshipCategory.Name)}",
                    $"{nameof(Relationship.RelationshipType)}.{nameof(RelationshipType.RelationshipCategory)}.{nameof(RelationshipCategory.IsShownInQuickView)}",
                    $"{nameof(Relationship.Constituent1)}Id",
                    $"{nameof(Relationship.Constituent1)}.Id",
                    $"{nameof(Relationship.Constituent1Name)}",
                    $"{nameof(Relationship.Constituent1)}.{nameof(Constituent.ConstituentNumber)}",
                    $"{nameof(Relationship.Constituent1)}.{nameof(Constituent.FormattedName)}",
                    $"{nameof(Relationship.Constituent2)}Id",
                    $"{nameof(Relationship.Constituent2)}.Id",
                    $"{nameof(Relationship.Constituent2Name)}",
                    $"{nameof(Relationship.Constituent2)}.{nameof(Constituent.ConstituentNumber)}",
                    $"{nameof(Relationship.Constituent2)}.{nameof(Constituent.FormattedName)}",
                    $"{nameof(Relationship.IsSwapped)}"
                });

        protected override Expression<Func<Relationship, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<Relationship, object>>[]
            {
                r => r.RelationshipType,
                r => r.RelationshipType.RelationshipCategory,
                c => c.Constituent1,
                c => c.Constituent2
            };
        }
        
        protected override Expression<Func<Relationship, object>>[] GetDataIncludesForSingle()
        {
            return GetDataIncludesForList();
        }
        
        [HttpGet]
        [Route("api/v1/relationships", Name = RouteNames.Relationship)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitDefault, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = DefaultFields;
            }

            return base.GetAll(RouteNames.Relationship, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/relationships/{id}", Name = RouteNames.Relationship + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, Guid? constituentId = null, string fields = null)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = DefaultFields;
            }

            if (constituentId != null)
            {
                Service.TargetConstituentId = constituentId;
            }

            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/relationships", Name = RouteNames.Relationship + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Relationship entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_ReadWrite)]
        [HttpPost]
        [Route("api/v1/constituents/{constituentId}/relationships", Name = RouteNames.Constituent + RouteNames.Relationship + RouteVerbs.Post)]
        public IHttpActionResult Post(Guid constituentId, [FromBody] Relationship entityToSave)
        {
            Service.TargetConstituentId = constituentId;

            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/relationships/{id}", Name = RouteNames.Relationship + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/constituents/{constituentId}/relationships/{id}", Name = RouteNames.Constituent + RouteNames.Relationship + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid constituentId, Guid id, JObject entityChanges)
        {
            Service.TargetConstituentId = constituentId;

            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/relationships/{id}", Name = RouteNames.Relationship + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [Authorize(Roles = Permissions.CRM_Read)]
        [HttpGet]
        [Route("api/v1/constituents/{constituentId}/relationships/{id}", Name = RouteNames.Constituent + RouteNames.Relationship + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid constituentId, Guid id, string fields = null)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = DefaultFields;
            }

            Service.TargetConstituentId = constituentId;

            return base.GetById(id, fields);
        }


        [Authorize(Roles = Permissions.CRM_Read)]
        [HttpGet]
        [Route("api/v1/relationships/constituents/{id}")]
        [Route("api/v1/constituents/{id}/relationships", Name = RouteNames.Constituent + RouteNames.Relationship)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = "", int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fields))
                {
                    fields = DefaultFields;
                }
                Service.TargetConstituentId = id;
                var search = new PageableSearch(offset, limit, orderBy);
                var response1 = Service.GetAllWhereExpression(a => a.Constituent1Id == id, search);
                var response2 = Service.GetAllWhereExpression(a => a.Constituent2Id == id, search);
                response1.Data = response1.Data.Union(response2.Data).ToList();
                return FinalizeResponse(response1, RouteNames.Constituent + RouteNames.Relationship, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }
    }
}