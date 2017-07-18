using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Extensions;
using DDI.Shared.Models;

namespace DDI.Business.Helpers
{
    /// <summary>
    /// A dictionary that maps one Entity Id to another Entity Id.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityMapper<T> where T : IEntity
    {
        private Dictionary<Guid, Guid> _mappings;

        public EntityMapper()
        {
            _mappings = new Dictionary<Guid, Guid>();
        }

        /// <summary>
        /// Add a mapping between two entities.
        /// </summary>
        public void Add(T fromEntity, T toEntity)
        {
            _mappings.Add(fromEntity.Id, toEntity.Id);
        }

        /// <summary>
        /// Add a mapping between two entity Id's.
        /// </summary>
        public void Add(Guid fromId, Guid ToId)
        {
            _mappings.Add(fromId, ToId);
        }


        /// <summary>
        /// Get the mapped Id for an entity.
        /// </summary>
        public Guid? Get(T fromEntity)
        {
            if (fromEntity == null)
            {
                return null;
            }

            Guid result = _mappings.GetValueOrDefault(fromEntity.Id);
            return (result == default(Guid) ? (Guid?)null : result);
        }

        /// <summary>
        /// Get the mapped Id for an entity Id.
        /// </summary>
        public Guid? Get(Guid? fromId)
        {
            if (fromId == null)
            {
                return null;
            }
            Guid result = _mappings.GetValueOrDefault(fromId.Value);
            return (result == default(Guid) ? (Guid?)null : result);
        }
    }
}
