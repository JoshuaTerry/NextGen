﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DDI.Business.Services;
using DDI.Data.Models.Common;
using DDI.Data.Models.Client;

namespace DDI.Business.Controllers
{
    public class ClergyStatusesController : ApiController
    {
        GenericServiceBase<ClergyStatus> _service;

        public ClergyStatusesController() : this(new GenericServiceBase<ClergyStatus>()) { }
        internal ClergyStatusesController(GenericServiceBase<ClergyStatus> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/clergystatuses")]
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
    }
}