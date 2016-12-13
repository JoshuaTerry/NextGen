using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DDI.Business.Services;
using DDI.Data.Models.Common;

namespace DDI.Business.Controllers
{
    public class CountriesController
    {
        IGenericService<Country> _service;

        public CountriesController() : this(new CountriesService()) { }
        internal CountriesController(IGenericService<Country> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/countries")]
        public IHttpActionResult GetCountries()
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
    }
}