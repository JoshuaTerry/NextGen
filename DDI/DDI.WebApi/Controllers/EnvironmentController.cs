﻿using DDI.Shared;
using DDI.Shared.Statics;
using System;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    public class EnvironmentController : ApiController
    {
        private const string ENVIRONMENTKEY = "Environment";
        [HttpGet]
        [Route("api/v1/environment", Name = RouteNames.Environment)]
        public IHttpActionResult Get()
        {
            var response = new DataResponse<String>();
            string environment = string.Empty;

            if (HttpContext.Current.Cache[ENVIRONMENTKEY] == null)
            {
                try
                {
                    HttpContext.Current.Cache[ENVIRONMENTKEY] = WebConfigurationManager.AppSettings[ENVIRONMENTKEY];
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                    response.IsSuccessful = false;
                }
            }
             
            response.Data = (string)HttpContext.Current.Cache[ENVIRONMENTKEY];
         
            return Ok(response);
        }
    }
}