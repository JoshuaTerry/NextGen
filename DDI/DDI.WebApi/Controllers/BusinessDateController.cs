using DDI.Shared;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    public class BusinessDateController : ApiController
    { 
        [HttpGet]
        [Route("api/v1/businessdate", Name = RouteNames.BusinessDate)]
        public IHttpActionResult Get()
        {
            var response = new DataResponse<DateTime>() { Data = DateTime.Now }; 
            return Ok(response);
        }         
    }
}