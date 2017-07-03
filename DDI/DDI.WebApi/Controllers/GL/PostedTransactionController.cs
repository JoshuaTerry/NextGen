using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DDI.Services.GL;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class PostedTransactionController : GenericController<PostedTransaction>
    {

        public PostedTransactionController(IPostedTransactionService service)
            : base(service)
        {
        }

        [HttpGet]
        [Route("api/v1/postedtransactions/accountId/{Id}")]
        public HttpResponseMessage GetAllPTGridByAccountId(Guid id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var results = ((IPostedTransactionService)Service).GetPostedTransactionsForAccountId(id);
                return Request.CreateResponse(HttpStatusCode.OK, DataSourceLoader.Load(results, loadOptions)); 
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, InternalServerError(new Exception(ex.Message)));
            }
        }
    }
}
