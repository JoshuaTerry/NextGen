using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;

namespace DDI.Business
{
    public interface IEntityLogic
    {
        /// <summary>
        /// Validate an entity.
        /// </summary>
        void Validate(IEntity entity);

        /// <summary>
        /// Update an entity in Elasticsearch by building the search document and indexing it.
        /// </summary>
        void UpdateSearchDocument(IEntity entity);
    }
}
