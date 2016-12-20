using System.Web.Http;

using DDI.Business.Services;
using DDI.Business.Services.Search;
using DDI.Data.Models.Common;

namespace DDI.Business.Controllers
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
        public IHttpActionResult GetAll(string orderBy = "Description")
        {
            var result = _service.GetAll(new PageableSearch { OrderBy = orderBy });

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
