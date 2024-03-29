﻿using DDI.Shared;
using DDI.Shared.Statics;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    //When model is created for this controller change inheritance from APIController to ControllerBase<BusinessData>
    public class BusinessDateController : ApiController
    { 
        [HttpGet]
        [Route("api/v1/businessdate", Name = RouteNames.BusinessDate)]
        public IHttpActionResult Get()
        {
            try
            {
                var response = new DataResponse<DateTime>() { Data = DateTime.Now }; 
                return Ok(response);
            }
            catch (Exception ex)
            {
                //base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }         
    }
}