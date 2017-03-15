using System;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Routing;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;


namespace DDI.WebApi.Controllers
{
    public class ConstituentTypesController : ControllerBase<ConstituentType>
    {
        private string _allFields = null;

        protected override Expression<Func<ConstituentType, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<ConstituentType, object>>[]
            {
                a => a.Tags
            };
        }
        
        protected override Expression<Func<ConstituentType, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<ConstituentType, object>>[]
            {
                c => c.Tags
            };
        }

        [HttpGet]
        [Route("api/v1/constituenttypes", Name = RouteNames.ConstituentType)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ConstituentType, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/constituenttypes/{id}", Name = RouteNames.ConstituentType + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
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

        //protected override string FieldsForAll
        //{
        //    get
        //    {
        //        if (_allFields == null)
        //        {
        //            //// For entity types with recursive properties or large collections, we need to exclude these.
        //            //// This is an example of using the FieldListBuilder to create a list of fields.
        //            _allFields = new PathHelper.FieldListBuilder<ConstituentType>()
        //                .Include(p => p.Code)
        //                .Include(p => p.Category)
        //                .Include(p => p.DisplayName)
        //                .Include(p => p.Id)
        //                .Include(p => p.IsActive)
        //                .Include(p => p.IsRequired)
        //                .Include(p => p.Name)
        //                .Include(p => p.NameFormat)
        //                .Include(p => p.SalutationFormal)
        //                .Include(p => p.SalutationInformal)
        //                .Include(p => p.Code)
        //                .Include(p => p.Tags.Count);

        //            //    .Exclude(p => p.ReciprocalTypeMale)
        //            //    .Exclude(p => p.FemaleTypes)
        //            //    .Exclude(p => p.MaleTypes)
        //            //    .Exclude(p => p.Relationships)
        //            //    .Exclude(p => p.RelationshipCategory.RelationshipTypes);
        //        }
        //        return _allFields;
        //    }
        //}

        [HttpPost]
        [Route("api/v1/constituenttypes/{id}/constituenttypetags")]
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