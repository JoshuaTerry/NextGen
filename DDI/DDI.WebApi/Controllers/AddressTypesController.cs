using System.Data.Entity.Core.Common.CommandTrees;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;

namespace DDI.WebApi.Controllers
{
    public class AddressTypesController : ControllerBase
    {
        ServiceBase<AddressType> _service;

        public AddressTypesController() : this(new ServiceBase<AddressType>()) { }
        internal AddressTypesController(ServiceBase<AddressType> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/addresstypes")]
        public IHttpActionResult GetAll(int? limit = 25, int? offset = 0, string orderby = null, string fields = null)
        {
            var search = new PageableSearch()
            {
                Limit =  limit,
                Offset = offset,
                OrderBy =  orderby
            };

            try
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

                var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper(), fields);

                return Ok(dynamicResult);

            }
            catch (System.Exception)
            {
                return InternalServerError();
            }
        }
    }
}