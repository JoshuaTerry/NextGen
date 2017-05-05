﻿using DDI.Services.GL;
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
                var search = new PageableSearch(SearchParameters.OffsetDefault,10000000, OrderByProperties.TransactionDate);
                //DataSourceLoadOptions loadOptions = new DataSourceLoadOptions();
                var ledgerAccountYear = _ledgerAccountYear.GetAllWhereExpression(ly=> ly.AccountId == id).Data.FirstOrDefault();
                var result = _service.GetAllWhereExpression(pt => pt.LedgerAccountYearId == ledgerAccountYear.Id,search);
                var gridResults = Json(DataSourceLoader.Load(result.Data.ToList(), loadOptions));
                return Request.CreateResponse(HttpStatusCode.OK, DataSourceLoader.Load(result.Data.ToList(), loadOptions)); 
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        [HttpGet]
        [Route("api/v1/posttransactions/totalcount/accountId/{Id}")]
        public HttpResponseMessage GetTotalCountPTGridByAccountId(Guid id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                //DataSourceLoadOptions loadOptions = new DataSourceLoadOptions();
                var ledgerAccountYear = _ledgerAccountYear.GetAllWhereExpression(ly => ly.AccountId == id).Data.FirstOrDefault();
                var result = _service.GetAllWhereExpression(pt => pt.LedgerAccountYearId == ledgerAccountYear.Id);
                var count = Json( DataSourceLoader.Load(result.Data.ToList(), loadOptions));
               
                return Request.CreateResponse(HttpStatusCode.OK, count);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        [HttpGet]
        [Route("api/v1/posttransactions/groupcount/accountId/{Id}")]
        public HttpResponseMessage GetGroupCountPTGridByAccountId(Guid id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                //DataSourceLoadOptions loadOptions = new DataSourceLoadOptions();
                var ledgerAccountYear = _ledgerAccountYear.GetAllWhereExpression(ly => ly.AccountId == id).Data.FirstOrDefault();
                var result = _service.GetAllWhereExpression(pt => pt.LedgerAccountYearId == ledgerAccountYear.Id);
                var count = Json(DataSourceLoader.Load(result.Data.ToList(), loadOptions));

                return Request.CreateResponse(HttpStatusCode.OK, count);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
