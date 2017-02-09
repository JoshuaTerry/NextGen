using DDI.Shared;
using DDI.Shared.Statics;
using System;
using System.Web.Configuration;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    public class EnvironmentController : ApiController
    {
        [HttpGet]
        [Route("api/v1/environment", Name = RouteNames.Environment)]
        public IHttpActionResult Get()
        {
            var response = new DataResponse<String>();
            string environment = string.Empty;

            try
            {
                response.Data = WebConfigurationManager.AppSettings["Environment"];
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                response.IsSuccessful = false;
            }
             
            return Ok(response);
        }
    }
}