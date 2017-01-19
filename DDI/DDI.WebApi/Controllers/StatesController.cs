using System;
using System.Web.Http;
using DDI.Services;
using DDI.Services.Search;

namespace DDI.WebApi.Controllers
{
    public class StatesController : ApiController
    {
        #region Private Fields

        private StateService _service;

        #endregion Private Fields

        #region Public Constructors

        public StatesController()
            : this(new StateService())
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal StatesController(StateService service)
        {
            _service = service;
        }

        #endregion Internal Constructors

        #region Public Methods

        [HttpGet]
        [Route("api/v1/states")]
        public IHttpActionResult GetAll(Guid? countryId = null)
        {
            var search = new ForeignKeySearch()
            {
                Id = countryId
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
