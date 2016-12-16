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
    public class ConstituentStatusesController : ApiController
    {
        GenericServiceBase<ConstituentStatus> _service;

        public ConstituentStatusesController() : this(new GenericServiceBase<ConstituentStatus>()) { }
        internal ConstituentStatusesController(GenericServiceBase<ConstituentStatus> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/constituentstatues")]
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