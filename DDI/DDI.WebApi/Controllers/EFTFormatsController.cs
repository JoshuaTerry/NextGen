using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class EFTFormatsController : ControllerBase<EFTFormat>
    {
        [HttpGet]
        [Route("api/v1/eftformats")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll("", limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/eftformats/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }
    }
}
