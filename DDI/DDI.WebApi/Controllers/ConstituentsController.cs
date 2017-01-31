﻿using DDI.Services;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Logger;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    //[Authorize]
    public class ConstituentsController : ControllerBase<Constituent>
    {
        protected new IConstituentService Service => (IConstituentService) base.Service;
        public ConstituentsController()
            : base(new ConstituentService())
        {
        }

        [HttpGet]
        [Route("api/v1/constituents", Name = RouteNames.Constituent)]
        public IHttpActionResult GetConstituents(string quickSearch = null, 
                                                 string name = null, 
                                                 int? constituentNumber = null, 
                                                 string address = null, 
                                                 string city = null, 
                                                 string state = null,
                                                 string zipFrom = null,
                                                 string zipTo = null,
                                                 string alternateId = null,
                                                 Guid? constituentTypeId = null,
                                                 string fields = null,
                                                 int? offset = null, 
                                                 int? limit = 25, 
                                                 string orderBy = OrderByProperties.DisplayName)
        {
            var search = new ConstituentSearch()
            {
                QuickSearch = quickSearch,
                Name = name,
                ConstituentNumber = constituentNumber,
                Address = address,
                City = city,
                State = state,
                Offset = offset,
                Limit = limit,
                OrderBy = orderBy,
                AlternateId = alternateId,
                ZipFrom =  zipFrom,
                ZipTo = zipTo,
                Fields = fields,
                ConstituentTypeId = constituentTypeId
            };

            try
            {
                var constituents = Service.GetAll(search);

                if (constituents == null)
                {
                    return NotFound();
                }
                if (!constituents.IsSuccessful)
                {
                    return InternalServerError();
                }

                var totalCount = constituents.TotalResults;
                var urlHelper = GetUrlHelper();

                Pagination.AddPaginationHeaderToResponse(urlHelper, search, totalCount, RouteNames.Constituent);
                var dynamicConstituents = DynamicTransmogrifier.ToDynamicResponse(constituents, urlHelper, fields);

                return Ok(dynamicConstituents);

            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}", Name = RouteNames.Constituent + RouteVerbs.Get)]
        public IHttpActionResult GetConstituentById(Guid id, string fields = null)
        {
            try
            {
                var constituent = Service.GetById(id);

                if (constituent == null)
                {
                    return NotFound();
                }
                if (!constituent.IsSuccessful)
                {
                    return InternalServerError();
                }

                var dynamicConstituent = DynamicTransmogrifier.ToDynamicResponse(constituent, GetUrlHelper(), fields);
                return Ok(dynamicConstituent);

            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/constituents/number/{num}")]
        public IHttpActionResult GetConstituentByConstituentNum(int num, string fields = null)
        {
            try
            {
                var constituent = Service.GetConstituentByConstituentNum(num);

                if (constituent == null)
                {
                    return NotFound();
                }
                if (!constituent.IsSuccessful)
                {
                    return InternalServerError();
                }

                var dynamicConstituent = DynamicTransmogrifier.ToDynamicResponse(constituent, GetUrlHelper(), fields);

                return Ok(dynamicConstituent);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/v1/constituents", Name = RouteNames.Constituent + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Constituent constituent)
        {
            return base.Post(GetUrlHelper(), constituent);
        }

        [HttpPost]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Post(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = Service.NewConstituent(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }

        [HttpPatch]
        [Route("api/v1/constituents/{id}", Name = RouteNames.Constituent + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject constituentChanges)
        {
            return base.Patch(GetUrlHelper(), id, constituentChanges);
        }

        [HttpDelete]
        [Route("api/v1/constituents/{id}", Name = RouteNames.Constituent + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}/constituentaddresses", Name = RouteNames.Constituent + RouteNames.ConstituentAddress)]
        public IHttpActionResult GetConstituentAddresses(Guid constituentId, string fields = null)
        {
            try
            {
                var result = Service.GetConstituentAddresses(constituentId);

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
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}/dbas", Name = RouteNames.Constituent + RouteNames.ConstituentDBA)]
        public IHttpActionResult GetConstituentDBAs(Guid id, string fields = null)
        {
            try
            {
                var result = Service.GetConstituentDBAs(id);

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
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}/educationlevel", Name = RouteNames.Constituent + RouteNames.EducationLevel)]
        public IHttpActionResult GetEducationLevel(Guid id, string fields = null)
        {
            try
            {
                var result = Service.GetEducationLevel(id);

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
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }
    }
}
