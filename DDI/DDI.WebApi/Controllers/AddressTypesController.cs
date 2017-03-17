﻿using System;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using LinqKit;

namespace DDI.WebApi.Controllers
{
    public class AddressTypesController : ControllerBase<AddressType>
    {
        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/addresstypes", Name = RouteNames.AddressType)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.AddressType, limit, offset, orderBy, fields);
        }
    }
}