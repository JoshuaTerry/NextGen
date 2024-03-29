﻿using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class ContactTypesController : GenericController<ContactType>
    {
        protected override Expression<Func<ContactType, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<ContactType, object>>[]
            {
                a => a.ContactCategory
            };
        }

        [HttpGet]
        [Route("api/v1/contacttypes", Name = RouteNames.ContactType)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ContactType, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/contacttypes/category/{categoryid}")]
        public IHttpActionResult GetByCategoryId(Guid categoryid)
        {
            try
            {
                var result = _service.GetAllWhereExpression(ct => ct.ContactCategoryId == categoryid);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/contacttypes/{id}", Name = RouteNames.ContactType + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/contacttypes", Name = RouteNames.ContactType + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ContactType entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/contacttypes/{id}", Name = RouteNames.ContactType + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/contacttypes/{id}", Name = RouteNames.ContactType + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}