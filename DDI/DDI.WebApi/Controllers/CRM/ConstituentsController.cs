using DDI.Services;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Http;
using System.Web.Routing;

namespace DDI.WebApi.Controllers.CRM
{
    //[Authorize]
    public class ConstituentsController : GenericController<Constituent>
    {
        protected new IConstituentService Service => (IConstituentService) base.Service;
        public ConstituentsController()
            : base(new ConstituentService())
        {
        }

        [HttpGet]
        [Route("api/v1/constituents/test", Name = "Foodiddy" + RouteVerbs.Get)]
        public void JambaJuice()
        {
            Service.Test(); 
        }
        protected override Expression<Func<Constituent, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<Constituent, object>>[]
            {
                c => c.ConstituentAddresses,
                c => c.ClergyStatus,
                c => c.ClergyType,
                c => c.ConstituentStatus,
                c => c.ConstituentType,
                c => c.ContactInfo,
                c => c.EducationLevel,
                c => c.Gender,
                c => c.IncomeLevel,
                c => c.Language,
                c => c.MaritalStatus,
                c => c.Prefix,
                c => c.Profession,
                c => c.Tags
            };
        }

        protected override Expression<Func<Constituent, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<Constituent, object>>[]
            {
                a => a.ConstituentAddresses.First().Address
            };
        }

        [HttpGet]
        [Route("api/v1/constituents", Name = RouteNames.Constituent)]
        public IHttpActionResult GetConstituents(string quickSearch = null, 
                                                 string name = null, 
                                                 int? constituentNumber = null, 
                                                 string address = null, 
                                                 string city = null, 
                                                 string state = null,
                                                 string country = null,
                                                 string zipFrom = null,
                                                 string zipTo = null,
                                                 string altid = null,
                                                 Guid? type = null,
                                                 string region1 = null,
                                                 string region2 = null,
                                                 string region3 = null,
                                                 string region4 = null,
                                                 int? agefrom = null,
                                                 int? ageto = null,
                                                 string includetags = null,
                                                 string excludetags = null,
                                                 DateTime? createdfrom = null,
                                                 DateTime? createdto = null,
                                                 string fields = null,
                                                 int? offset = SearchParameters.OffsetDefault, 
                                                 int? limit = SearchParameters.LimitDefault, 
                                                 string orderBy = OrderByProperties.DisplayName)
        {
            var search = new ConstituentSearch()
            {
                QuickSearch = quickSearch,
                Name = name,
                ConstituentNumber = constituentNumber,
                ConstituentTypeId = type,
                Address = address,
                City = city,
                StateId = state,
                PostalCodeFrom =  zipFrom,
                PostalCodeTo = zipTo,
                RegionId1 = region1,
                RegionId2 = region2,
                RegionId3 = region3,
                RegionId4 = region4,
                AlternateId = altid,
                AgeFrom = agefrom,
                AgeTo = ageto,
                IncludeTags = includetags,
                ExcludeTags = excludetags,
                CreatedDateFrom = createdfrom,
                CreatedDateTo = createdto,

                OrderBy = orderBy,
                Offset = offset,
                Limit = limit,
                Fields = fields,
            };

            return base.GetAll(RouteNames.Constituent, search, fields);
        }

        [HttpGet]
        [Route("api/v1/constituents/lookup/{name}")]
        public IHttpActionResult ConstituentLookup(string name)
        {
            string fields = "Id,Name";

            var search = new ConstituentSearch()
            {
                QuickSearch = name,
                Offset = SearchParameters.OffsetDefault,
                Limit = 1000,
                Fields = fields,
            };

            return base.GetAll(null, search, fields);
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}", Name = RouteNames.Constituent + RouteVerbs.Get)]
        public IHttpActionResult GetConstituentById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/constituents/number/{num}")]
        public IHttpActionResult GetConstituentByConstituentNum(int num, string fields = null)
        {
            try
            {
                var constituent = Service.GetConstituentByConstituentNum(num);

                return FinalizeResponse(constituent, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/constituents/new/{constituenttypeid}")]
        public IHttpActionResult NewConstituent(Guid constituenttypeid, string fields = null)
        {
            try
            {
                var constituent = Service.NewConstituent(constituenttypeid);

                return FinalizeResponse(constituent, fields);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerError(ex);
            }
            
        }

        [HttpGet]
        [Route("api/v1/constituents/primary/{id}")]
        public IHttpActionResult GetPrimaryContactInfo(Guid id)
        {
            try
            {
                var constituent = Service.GetById(id).Data;

                if (constituent == null)
                {
                    return NotFound();
                }

                var response = Service.GetConstituentPrimaryContactInfo(constituent);

                return Ok(response);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerError(ex);
            }
        } 

        [HttpPost]
        [Route("api/v1/constituents", Name = RouteNames.Constituent + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Constituent constituent)
        {
            return base.Post(constituent);
        }

        [HttpPost]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Post(Guid id)
        {
            JambaJuice();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = Service.NewConstituent(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPatch]
        [Route("api/v1/constituents/{id}", Name = RouteNames.Constituent + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject constituentChanges)
        {
            JambaJuice();
            return base.Patch(id, constituentChanges);
        }

        [HttpDelete]
        [Route("api/v1/constituents/{id}", Name = RouteNames.Constituent + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpPost]
        [Route("api/v1/constituents/{id}/constituenttags")]
        public IHttpActionResult AddTagsToConstituent(Guid id, [FromBody] JObject tags)
        {
            try
            {
                var constituent = Service.GetById(id).Data;

                if (constituent == null)
                {
                    return NotFound();
                }

                var response = Service.AddTagsToConstituent(constituent, tags);

                return Ok(response);
            }
            catch(Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpDelete]
        [Route("api/v1/constituents/{id}/tag/{tagId}")]
        public IHttpActionResult RemoveTagFromConstituent(Guid id, Guid tagId)
        {
            try
            {
                var constituent = Service.GetById(id).Data;

                if (constituent == null)
                {
                    return NotFound();
                }

                var response = Service.RemoveTagFromConstituent(constituent, tagId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}
