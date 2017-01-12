using DDI.Services;
using DDI.Services.Search;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    public class CountiesController : ApiController
    {
        #region Private Fields

        private CountyService _service;

        #endregion Private Fields

        #region Public Constructors

        public CountiesController()
            : this(new CountyService())
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal CountiesController(CountyService service)
        {
            _service = service;
        }

        #endregion Internal Constructors

        #region Public Methods

        [HttpGet]
        [Route("api/v1/counties")]
        public IHttpActionResult GetAll(Guid? stateId = null,
                                        string orderBy = "Description")
        {
            var search = new CountySearch()
            {
                StateId = stateId,
                OrderBy = orderBy,
            };
            var result = _service.GetAll(search);

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

        #endregion Public Methods
    }

}