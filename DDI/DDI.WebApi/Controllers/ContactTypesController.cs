using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using System;

namespace DDI.WebApi.Controllers
{
    public class ContactTypesController : ControllerBase<ContactType>
    {
        [HttpGet]
        [Route("api/v1/contacttypes", Name = RouteNames.ContactType)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ContactType, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/contacttypes/{categoryid}", Name = RouteNames.ContactType + RouteNames.ContactCategory)]
        public IHttpActionResult GetContactTypesByContactCategory(Guid categoryId)
        {
            var response = Service.GetAllWhereExpression(c => c.ContactCategory.Id == categoryId);
            if(response == null)
            {
                return NotFound();
            }

            if(!response.IsSuccessful)
            {
                return InternalServerError();
            }

            return Ok(response);
            
        }
    }
}