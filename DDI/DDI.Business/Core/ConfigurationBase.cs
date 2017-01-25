using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Enums;

namespace DDI.Business.Core
{
    public abstract class ConfigurationBase
    {

        public abstract ModuleType ModuleType { get; }
      

        /// <summary>
        /// Load a property by converting it from a string.
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="value">Value to convert</param>
        /// <param name="uow">UnitOfWork</param>
        public virtual void LoadProperty(string name, string value, IUnitOfWork uow)
        {
        }

        /// <summary>
        /// Save a property by converting it to a string.
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>String representation of property</returns>
        public virtual string SaveProperty(string name)
        {
            return null;
        }

        /// <summary>
        /// Attach entities in the configuration to a UnitOfWork.
        /// </summary>
        public virtual void Attach(IUnitOfWork uow) { }

        /// <summary>
        /// Convert a set of entities into a comma delimited list of Guid strings.
        /// </summary>
        public string GetGuidStrings(IEnumerable<EntityBase> entityList)
        {
            return string.Join(",", entityList.Where(p => p != null).Select(p => p.Id.ToString()));
        }

        /// <summary>
        /// Retrieve an entity from the database using a Guid string value.
        /// </summary>
        public T GetEntity<T>(string guidString, IUnitOfWork uow) where T : EntityBase
        {
            if (string.IsNullOrWhiteSpace(guidString))
            {
                return null;
            }

            Guid id;
            if (Guid.TryParse(guidString, out id))
            {
                return uow.GetById<T>(id);
            }
            
            return null;
        }

        /// <summary>
        /// Retrieve a list of entities from the database using a comma delimited list of Guid string values.
        /// </summary>
        public IList<T> GetEntityList<T>(string guids, IUnitOfWork uow) where T : EntityBase
        {
            List<T> list = new List<T>();

            if (string.IsNullOrWhiteSpace(guids))
            {
                return list;
            }

            // Convert comma delimited list to Guids.
            IEnumerable<Guid> ids = guids.Split(',').Select(p =>
            {
                Guid id;
                Guid.TryParse(p, out id);
                return id;
            }).Where(p => p != default(Guid));

            if (ids.Count() > 0)
            {
                // Retrieve entities.  (Linq to SQL does this in one step.)
                list.AddRange(uow.GetEntities<T>().Where(p => ids.Contains(p.Id)));
            }

            return list;
        }
    }
}
