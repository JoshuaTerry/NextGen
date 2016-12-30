using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data.Models;

namespace DDI.Data
{
    /// <summary>
    /// A managed collection of linked entities.
    /// </summary>
    /// <typeparam name="T">A subclass of BaseLinkedEntity</typeparam>
    public class LinkedEntityCollection<T> : ObservableCollection<T> where T : BaseLinkedEntity
    {
        private BaseEntity _entity = null;
        private bool _ignoreEvents = false;

        public LinkedEntityCollection(BaseEntity entity) : base()
        {
            _entity = entity;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_ignoreEvents)
                return;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (BaseLinkedEntity item in e.NewItems)
                {
                    item.ParentEntityId = _entity.Id;
                    item.EntityType = _entity.GetEntityType();
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (BaseLinkedEntity item in e.OldItems)
                {
                    item.ParentEntityId = null;
                    item.EntityType = null;
                }
            }
        }

        /// <summary>
        /// Load the entity's collection.
        /// </summary>
        public void LoadCollection(DbContext context)
        {
            DbSet<T> dbSet = context.Set<T>();
            _ignoreEvents = true;

            foreach (var item in dbSet.Where(p => p.EntityType == _entity.GetEntityType() && p.ParentEntityId == _entity.Id))
            {
                this.Add(item);
                item.ParentEntity = _entity;
            }

            _ignoreEvents = false;

        }

    }
}