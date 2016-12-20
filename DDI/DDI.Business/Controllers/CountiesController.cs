using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DDI.Business.Services;
using DDI.Business.Services.Search;

namespace DDI.Business.Controllers
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