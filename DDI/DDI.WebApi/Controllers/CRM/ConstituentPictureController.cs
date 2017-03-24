﻿using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    public class ConstituentPictureController : GenericController<ConstituentPicture>
    {
        [HttpGet]
        [Route("api/v1/constituentpicture/{constituentid}")]
        public IHttpActionResult GetByConstituentId(Guid constituentId)
        {
            try
            {
                return Ok(Service.GetAllWhereExpression(c => c.ConstituentId == constituentId));
            }
            catch(Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("api/v1/constituentpicture")]
        public IHttpActionResult Post([FromBody] ConstituentPicture item)
        {
            var pics = Service.GetAllWhereExpression(c => c.ConstituentId == item.ConstituentId).Data;

            foreach(var pic in pics)
            {
                base.Delete(pic.Id);
            }

            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/constituentpicture/{id}")]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }
    }
}