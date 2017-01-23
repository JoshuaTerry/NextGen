using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using DDI.Services;
using DDI.Services.Extensions;
using Newtonsoft.Json.Linq;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;

namespace DDI.WebApi.Controllers
{
    //[Authorize]
    public class ConstituentsController : ControllerBase
    {
        private IConstituentService _service;

        public ConstituentsController()
            :this(new ConstituentService())
        {
        }

        internal ConstituentsController(IConstituentService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("api/v1/constituents", Name = RouteNames.Constituent)]
        public IHttpActionResult GetConstituents(string quickSearch = null, 
                                                 string name = null, 
                                                 int? constituentNumber = null, 
                                                 string address = null, 
                                                 string city = null, 
                                                 string state = null,
                                                 string zipFrom = null,
                                                 string zipTo = null,
                                                 string alternateId = null,
                                                 Guid? constituentTypeId = null,
                                                 string fields = null,
                                                 int? offset = null, 
                                                 int? limit = 25, 
                                                 string orderby = "DisplayName")
        {
            var search = new ConstituentSearch()
            {
                QuickSearch = quickSearch,
                Name = name,
                ConstituentNumber = constituentNumber,
                Address = address,
                City = city,
                State = state,
                Offset = offset,
                Limit = limit,
                OrderBy = orderby,
                AlternateId = alternateId,
                ZipFrom =  zipFrom,
                ZipTo = zipTo,
                Fields = fields,
                ConstituentTypeId = constituentTypeId
            };

            var constituents = _service.GetConstituents(search);

            if (constituents == null)
            {
                return NotFound();
            }
            if (!constituents.IsSuccessful)
            {
                return InternalServerError();
            }

            var totalCount = constituents.TotalResults;
            var urlHelper = GetUrlHelper();

            Pagination.AddPaginationHeaderToResponse(urlHelper, search, totalCount, RouteNames.Constituent);
            var dynamicConstituents = DynamicTransmogrifier.ToDynamicResponse(constituents, urlHelper, fields);

            return Ok(dynamicConstituents);
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}", Name = RouteNames.Constituent + RouteVerbs.Get)]
        public IHttpActionResult GetConstituentById(Guid id, string fields = null)
        {
            var constituent = _service.GetConstituentById(id);

            if (constituent == null)
            {
                return NotFound();
            }
            if (!constituent.IsSuccessful)
            {
                return InternalServerError();
            }

            var dynamicConstituent = DynamicTransmogrifier.ToDynamicResponse(constituent, GetUrlHelper(), fields);
            return Ok(dynamicConstituent);
        }

        [HttpGet]
        [Route("api/v1/constituents/number/{num}")]
        public IHttpActionResult GetConstituentByConstituentNum(int num, string fields = null)
        {
            {
                var constituent = _service.GetConstituentByConstituentNum(num);

                if (constituent == null)
                {
                    return NotFound();
                }
                if (!constituent.IsSuccessful)
                {
                    return InternalServerError();
                }

                var dynamicConstituent = DynamicTransmogrifier.ToDynamicResponse(constituent, GetUrlHelper(), fields);

                return Ok(dynamicConstituent);
            }
        }

        [HttpPost]
        [Route("api/v1/constituents", Name = RouteNames.Constituent + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Constituent constituent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.AddConstituent(constituent);
            var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, GetUrlHelper());

            return Ok(dynamicResponse);
        }

        [HttpPost]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Post(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.NewConstituent(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPatch]
        [Route("api/v1/constituents/{id}", Name = RouteNames.Constituent + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject constituentChanges)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var constituent = _service.UpdateConstituent(id, constituentChanges);
                var dynamicConstituent = DynamicTransmogrifier.ToDynamicResponse(constituent, GetUrlHelper());

                return Ok(dynamicConstituent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("api/v1/constituents/{id}", Name = RouteNames.Constituent + RouteVerbs.Delete)]
        public IHttpActionResult Delete(Guid id)
        {
            return Ok();
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}/constituentaddresses", Name = RouteNames.Constituent + RouteNames.ConstituentAddress)]
        public IHttpActionResult GetConstituentConstituentAddresses(Guid id, string fields = null)
        {
            var result = _service.GetConstituentAddresses(id);

            if (result == null)
            {
                return NotFound();
            }
            if (!result.IsSuccessful)
            {
                return InternalServerError();
            }

            var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper(), fields);
            return Ok(dynamicResult);
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}/dbas")]
        public IHttpActionResult GetConstituentDBAs(Guid id, string fields = null)
        {
            var result = _service.GetConstituentDBAs(id);

            if (result == null)
            {
                return NotFound();
            }
            if (!result.IsSuccessful)
            {
                return InternalServerError();
            }

            var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper(), fields);
            return Ok(dynamicResult);
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}/educationlevel")]
        public IHttpActionResult GetEducationLevel(Guid id, string fields = null)
        {
            var result = _service.GetEducationLevel(id);

            if (result == null)
            {
                return NotFound();
            }
            if (!result.IsSuccessful)
            {
                return InternalServerError();
            }

            var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper(), fields);
            return Ok(dynamicResult);
        }
    }
}
