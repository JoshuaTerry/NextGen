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
        private bool _isLoading = false;
        private bool _isLoaded = false;

        public LinkedEntityCollection(BaseEntity entity) : base()
        {
            _entity = entity;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (_isLoading)
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

        public void LoadCollection(DbContext context)
        {
            LoadCollection(context, false);
        }

        /// <summary>
        /// Load the entity's collection.
        /// </summary>
        public void LoadCollection(DbContext context, bool reload)
        {
            if (_isLoaded && !reload)
            {
                return;
            }

            DbSet<T> dbSet = context.Set<T>();
            _isLoading = true;

            string entityType = _entity.GetEntityType();

            foreach (var item in dbSet.Where(p => p.EntityType == entityType && p.ParentEntityId == _entity.Id))
            {
                this.Add(item);
                item.SetParentEntityValue(_entity, false);
            }

            _isLoading = false;
            _isLoaded = true;

        }

    }
}