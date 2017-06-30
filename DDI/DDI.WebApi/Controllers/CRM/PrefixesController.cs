﻿using System;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class PrefixesController : GenericController<Prefix>
    {
        public PrefixesController(IService<Prefix> service) : base(service) { }

        protected override Expression<Func<Prefix, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<Prefix, object>>[]
            {
                e => e.Gender
            };
        }

        protected override Expression<Func<Prefix, object>>[] GetDataIncludesForSingle() => GetDataIncludesForList();

        protected override string FieldsForList => $"{FieldLists.CodeFields},{nameof(Prefix.ShowOnline)}";

        [HttpGet]
        [Route("api/v1/prefixes")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.Prefix, string fields = null)
        {            
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/prefixes/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/prefixes")]
        public IHttpActionResult Post([FromBody] Prefix entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/prefixes/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/prefixes/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

    }
}