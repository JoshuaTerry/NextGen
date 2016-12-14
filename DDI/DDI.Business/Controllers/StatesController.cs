using System.Web.Http;

using DDI.Business.Services;
using DDI.Data.Models.Common;

namespace DDI.Business.Controllers
{
    public class StatesController : ApiController
    {
        #region Private Fields

        private GenericServiceCommonBase<State> _service;

        #endregion Private Fields

        #region Public Constructors

        public StatesController()
            : this(new GenericServiceCommonBase<State>())
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal StatesController(GenericServiceCommonBase<State> service)
        {
            _service = service;
        }

        #endregion Internal Constructors

        #region Public Methods

        [HttpGet]
        [Route("api/v1/states")]
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

        #endregion Public Methods
    }
}
