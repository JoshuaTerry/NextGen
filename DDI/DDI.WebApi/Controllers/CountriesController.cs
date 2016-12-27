using System;
using System.Web.Http;
using DDI.Data.Models.Common;
using DDI.WebApi.Services;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class CountriesController : ApiController
    {
        #region Private Fields

        private GenericServiceCommonBase<Country> _service;

        #endregion Private Fields

        #region Public Constructors

        public CountriesController()
            : this(new GenericServiceCommonBase<Country>())
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal CountriesController(GenericServiceCommonBase<Country> service)
        {
            _service = service;
        }

        #endregion Internal Constructors

        #region Public Methods

        [HttpGet]
        [Route("api/v1/countries")]
        public IHttpActionResult GetAll()
        {
            var result = _service.GetAll();

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

        [HttpPost]
        [Route("api/v1/countries")]
        public IHttpActionResult Post([FromBody] Country item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.Add(item);
            return Ok();
        }

        [HttpPatch]
        [Route("api/v1/countries/{id}")]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Update(id, changes);

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        #endregion Public Methods
    }
}
