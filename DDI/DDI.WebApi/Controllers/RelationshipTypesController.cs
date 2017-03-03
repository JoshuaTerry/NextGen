using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{

    public class RelationshipTypesController : ControllerBase<RelationshipType>
    {
        private string _defaultFields = null;
        private string _allFields = null;

        [HttpGet]
        [Route("api/v1/relationshiptypes", Name = RouteNames.RelationshipTypes)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.RelationshipTypes, limit, offset, orderBy, ConvertFieldList(fields));
        }

        [HttpGet]
        [Route("api/v1/relationshiptypes/{id}", Name = RouteNames.RelationshipTypes + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = "all")
        {
            return base.GetById(id, ConvertFieldList(fields));
        }

        protected override Expression<Func<RelationshipType, object>>[] GetDataIncludesForSingle() =>
            new Expression<Func<RelationshipType, object>>[]
            {
                p => p.RelationshipCategory
            };


        protected override Expression<Func<RelationshipType, object>>[] GetDataIncludesForList() => GetDataIncludesForSingle();

        private string ConvertFieldList(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                if (_defaultFields == null)
                {
                    _defaultFields = new PathHelper.FieldListBuilder<RelationshipType>()
                        .Include(p => p.Id)
                        .Include(p => p.DisplayName)
                        .Include(p => p.IsActive);
                }
                return _defaultFields;
            }
            else if (string.Compare(fields, "all", true) == 0)
            {
                if (_allFields == null)
                {
                    // For most entity types, set fields = string.empty to send all properties.  
                    // For entity types with recursive properties or large collections, we need to exclude these.
                    _allFields = new PathHelper.FieldListBuilder<RelationshipType>()
                        .Exclude(p => p.ReciprocalTypeFemale)
                        .Exclude(p => p.ReciprocalTypeMale)
                        .Exclude(p => p.FemaleTypes)
                        .Exclude(p => p.MaleTypes)
                        .Exclude(p => p.Relationships)
                        .Exclude(p => p.RelationshipCategory.RelationshipTypes);
                }
                return _allFields;
            }

            return fields;
        }


    }
}