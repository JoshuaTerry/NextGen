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
    public class ConstituentsController : ApiController
    {
        private IConstituentService _service;
        private IPagination _pagination;
        private DynamicTransmogrifier _dynamicTransmogrifier;

        public ConstituentsController()
            :this(new ConstituentService(), new Pagination(), new DynamicTransmogrifier())
        {
        }

        internal ConstituentsController(IConstituentService service, IPagination pagination, DynamicTransmogrifier dynamicTransmogrifier)
        {
            _service = service;
            _pagination = pagination;
            _dynamicTransmogrifier = dynamicTransmogrifier;
        }

        [HttpGet]
        [Route("api/v1/constituents", Name = RouteNames.Constituents)]
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
                                                 string orderby = null)
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
            var urlHelper = new UrlHelper(Request);

            _pagination.AddPaginationHeaderToResponse(urlHelper, search, totalCount, RouteNames.Constituents);

            var dynamicConstituents = _dynamicTransmogrifier.ToDynamicResponse(constituents);
            return Ok(dynamicConstituents);
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}")]
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

            return Ok(constituent);
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

                return Ok(constituent);

            }
        }

        [HttpPost]
        [Route("api/v1/constituents")]
        public IHttpActionResult Post([FromBody] Constituent constituent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response =_service.AddConstituent(constituent);
            return Ok(response);
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



        [HttpPut]
        [Route("api/v1/constituents/")]
        public IHttpActionResult Put(Constituent constituentChanges)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //var updatedConstituent = _service.UpdateConstituent(constituentChanges);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPatch]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Patch(Guid id, JObject constituentChanges)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.UpdateConstituent(id, constituentChanges);

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Delete(Guid id)
        {
            return Ok();
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}/dbas")]
        public IHttpActionResult GetConstituentDBAs(Guid constituentId)
        {
            var result = _service.GetConstituentDBAs(constituentId);

            if (result == null)
            {
                return NotFound();
            }
            if (!result.IsSuccessful)
            {
                return InternalServerError();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}/educationlevel")]
        public IHttpActionResult GetEducationLevels(Guid constituentId)
        {
            var result = _service.GetEducationLevels(constituentId);

            if (result == null)
            {
                return NotFound();
            }
            if (!result.IsSuccessful)
            {
                return InternalServerError();
            }

            return Ok(result);
        }
    }
}
