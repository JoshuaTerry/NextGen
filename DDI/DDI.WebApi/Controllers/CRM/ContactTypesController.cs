using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    public class ContactTypesController : GenericController<ContactType>
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
            try
            { 
                var response = Service.GetAllWhereExpression(c => c.ContactCategory.Id == categoryId);
                if(response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }

        }
    }
}