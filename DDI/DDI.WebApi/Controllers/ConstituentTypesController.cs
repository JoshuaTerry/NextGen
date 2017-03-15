using System;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Routing;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class ConstituentTypesController : ControllerBase<ConstituentType>
    {
        [HttpGet]
        [Route("api/v1/constituenttypes", Name = RouteNames.ConstituentType)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ConstituentType, limit, offset, orderBy, fields);
        }

        [HttpPost]
        [Route("api/v1/constituenttypes", Name = RouteNames.ConstituentType + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ConstituentType item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/constituenttypes/{id}", Name = RouteNames.ConstituentType + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/constituenttypes/{id}", Name = RouteNames.ConstituentType + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        protected override Expression<Func<ConstituentType, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<ConstituentType, object>>[]
            {
                a => a.Tags
            };
        }

        protected override string FieldsForAll
        {
            get
            {
                if (_allFields == null)
                {
                    //// For entity types with recursive properties or large collections, we need to exclude these.
                    //// This is an example of using the FieldListBuilder to create a list of fields.
                    _allFields = new PathHelper.FieldListBuilder<ConstituentType>()
                        .Include(p => p.Tags.ToString());
                    //    .Exclude(p => p.ReciprocalTypeMale)
                    //    .Exclude(p => p.FemaleTypes)
                    //    .Exclude(p => p.MaleTypes)
                    //    .Exclude(p => p.Relationships)
                    //    .Exclude(p => p.RelationshipCategory.RelationshipTypes);
                }
                return _allFields;
            }
        }

        [HttpPost]
        [Route("api/v1/constituentstypes/{id}/constituenttypetags")]
        public IHttpActionResult AddTagsToConstituentType(Guid id, [FromBody] JObject tags)
        {
            try
            {
                var constituentType = Service.GetById(id).Data;

                if (constituentType == null)
                {
                    return NotFound();
                }

                var response = Service.AddTagsToConstituentType(constituentType, tags);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("api/v1/constituenttypes/{id}/tag/{tagId}")]
        public IHttpActionResult RemoveTagFromConstituentType(Guid id, Guid tagId)
        {
            try
            {
                var constituentType = Service.GetById(id).Data;

                if (constituentType == null)
                {
                    return NotFound();
                }

                var response = Service.RemoveTagFromConstituentType(constituentType, tagId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(ex);
            }
        }

        protected new IConstituentTypeService Service => (IConstituentTypeService)base.Service;
        public ConstituentTypesController()
            : base(new ConstituentTypeService())
        {
        }
    }
}