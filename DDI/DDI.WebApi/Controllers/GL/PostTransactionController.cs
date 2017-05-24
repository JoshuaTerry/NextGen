using DDI.Services.GL;
using DDI.Services.Search;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using DDI.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using DevExtreme.AspNet.Data;
using DDI.WebApi.Models.BindingModels;

namespace DDI.WebApi.Controllers.GL
{
    public class PostTransactionController : GenericController<PostedTransaction>
    {
        private ServiceBase<LedgerAccountYear> _ledgerAccountYear = new ServiceBase<LedgerAccountYear>();


        [HttpGet]
        [Route("api/v1/posttransactions/accountId/{Id}")]
        public HttpResponseMessage GetAllPTGridByAccountId(Guid id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var ledgerAccountYear = _ledgerAccountYear.GetAllWhereExpression(ly=> ly.AccountId == id).Data.FirstOrDefault();
                var results = new PostedTransactionService().GetPostedTransactionsForLedgerAccountYearId(ledgerAccountYear.Id);
               
                //var gridResults = DataSourceLoader.Load(results, loadOptions);
                
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
