﻿using DDI.EFAudit.Contexts;
using DDI.EFAudit.Exceptions;
using DDI.EFAudit.Helpers;
using DDI.EFAudit.Translation;
using DDI.EFAudit.Translation.Binders;
using DDI.Shared.Models.Client.Audit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DDI.EFAudit.History
{
    /// <summary>
    /// This class reconstitutes logs into sequences of values and objects, accompanied
    /// by timestamp and author data.
    /// </summary>
    public class HistoryExplorer<TChangeSet, TPrincipal>
        where TChangeSet : IChangeSet<TPrincipal>
    {
        private IHistoryContext<TChangeSet, TPrincipal> db;
        private IBindManager binder;
        private HistoryExplorerCloneStrategies cloneStrategy;

        public HistoryExplorer(IHistoryContext<TChangeSet, TPrincipal> db, IBindManager binder = null, HistoryExplorerCloneStrategies cloneStrategy = HistoryExplorerCloneStrategies.Default)
        {
            this.db = db;
            this.binder = (binder ?? new ValueTranslationManager(db));
            this.cloneStrategy = cloneStrategy;
        }

        /// <summary>
        /// Retrieve the values that a single property has gone through, most recent first (descending date order).
        /// </summary>
        public virtual IEnumerable<IChange<TValue, TPrincipal>> ChangesTo<TModel, TValue>(TModel model, Expression<Func<TModel, TValue>> property)
        {
            string typeName = typeof(TModel).Name;
            string propertyName = property.GetPropertyName();
            string propertyPrefix = propertyName + ".";
            string reference = db.GetReferenceForObject(model);
            var propertyFunc = property.Compile();
            
            var objectChanges = changesTo(model)
                .SelectMany(o => o.PropertyChanges)
                .Where(p => p.PropertyName == propertyName || p.PropertyName.StartsWith(propertyPrefix))
                .AsEnumerable()
                .GroupBy(p => p.ObjectChange)
                .Select(g => new FilteredObjectChange<TPrincipal>(g.Key, g));
             
            var sample = objectChanges.SelectMany(o => o.PropertyChanges).FirstOrDefault();

            if (sample != null && sample.PropertyName.StartsWith(propertyPrefix))
            {               
                return applyChangesTo(HistoryHelpers.Instantiate<TValue>(), objectChanges, propertyName).OrderByDescending(c => c.Timestamp);
            }
            else
            {
                // Just directly bind the simple property values
                return objectChanges
                    .OrderByDescending(o => o.ChangeSet.Timestamp)
                    .SelectMany(o => o.PropertyChanges)
                    .Select(p => Change.FromObjectChange(binder.Bind<TValue>(p.NewValue), p.ObjectChange));
            }
        }

        /// <summary>
        /// Rehydrates versions of the object, one for each logged change to the object, most recent first (descending date order).
        /// </summary>
        public virtual IEnumerable<IChange<TModel, TPrincipal>> ChangesTo<TModel>(TModel model)
            where TModel : new()
        {
            var changes = changesTo(model);

            return applyChangesTo(new TModel(), changes)
                .OrderByDescending(c => c.Timestamp);
        }
        /// <summary>
        /// Rehydrates versions of the object, one for each logged change to the object,most recent first (descending date order).
        /// </summary>
        public virtual IEnumerable<IChange<TModel, TPrincipal>> ChangesTo<TModel>(string reference)
            where TModel : ICloneable, new()
        {
            var changes = changesTo<TModel>(reference);

            return applyChangesTo(new TModel(), changes)
                .OrderByDescending(c => c.Timestamp);
        }
         
        public virtual IChange<TModel, TPrincipal> GetCreation<TModel>(TModel model)
        {
            var firstChange = changesTo(model).FirstOrDefault();

            if (firstChange == null || TypeOfChange<TModel>(firstChange) != ChangeType.Add)
                throw new CreationDoesNotExistInLogException(model);
            else
                return Change.FromObjectChange(model, firstChange);
        }

        /// <summary>
        /// Returns all IObjectChanges that are relevant to this object, earliest first
        /// </summary>
        protected virtual IOrderedQueryable<IObjectChange<TPrincipal>> changesTo<TModel>(TModel model)
        {
            string reference = db.GetReferenceForObject(model);
            return changesTo<TModel>(reference);
        }

        /// <summary>
        /// Returns all IObjectChanges that are relevant to the object identified by this reference, earliest first
        /// </summary>
        protected virtual IOrderedQueryable<IObjectChange<TPrincipal>> changesTo<TModel>(string reference)
        {
            string typeName = typeof(TModel).Name;
            var changes = db.ObjectChanges
                .Where(o => o.TypeName == typeName)
                .Where(o => o.EntityId == reference)
                .OrderBy(o => o.ChangeSet.Timestamp);

            return changes;
        }
         
        protected virtual IEnumerable<IChange<TModel, TPrincipal>> applyChangesTo<TModel>(TModel seed, IEnumerable<IObjectChange<TPrincipal>> changes, string prefix = "")
        {
            TModel current = seed;
            foreach (var change in changes)
            {
                // If this was the change that deleted the object, just return null, don't tryapplying this change
                if (TypeOfChange<TModel>(change) == ChangeType.Delete)
                {
                    yield return Change.FromObjectChange<TModel, TPrincipal>(default(TModel), change);
                    break;
                }
                else
                {
                    var c = apply(change, current, prefix);

                    yield return c;

                    current = c.Value;
                }
            }
        }

        protected virtual IChange<TModel, TPrincipal> apply<TModel>(IObjectChange<TPrincipal> change, TModel model, string prefix)
        {
            var type = typeof(TModel);
            var newVersion = clone(model);
            var errors = new List<Exception>();

            foreach (var propertyChange in change.PropertyChanges.Select(p => new PropertyChangeProcessor<TPrincipal>(p)))
            {
                try
                {
                    propertyChange.ApplyTo(newVersion, binder, prefix);
                }
                catch (UnknownPropertyInLogException<TPrincipal> e)
                {
                    errors.Add(e);
                }
            }

            return Change.FromObjectChange(newVersion, change, errors: errors);
        }

        private TModel clone<TModel>(TModel model)
        {
            try
            {
                if (cloneStrategy.HasFlag(HistoryExplorerCloneStrategies.UseCloneable))
                {
                    var cloneable = (model as ICloneable);

                    if (cloneable != null)
                        return (TModel) cloneable.Clone();
                }

                if (cloneStrategy.HasFlag(HistoryExplorerCloneStrategies.DeepCopy))
                {
                    return model.DeepCopy();
                }

                throw new UnableToCloneObjectException(typeof(TModel));
            }
            catch (Exception ex)
            {
                throw new UnableToCloneObjectException(typeof(TModel), ex);
            }
        }

        /// <summary>
        /// Returns whether the object change consists of adding/deleting/modifying an entity
        /// </summary>
        public virtual ChangeType TypeOfChange<TModel>(IObjectChange<TPrincipal> change)
        {
            var keyChange = primaryKeyChange<TModel>(change);

            if (keyChange == null)
                return ChangeType.Modify;
            else
            {
                if (keyChange.NewValue == null)
                {
                    return ChangeType.Delete;
                }
                else
                {
                    return ChangeType.Add;
                }
            }
        }

        protected virtual IPropertyChange<TPrincipal> primaryKeyChange<T>(IObjectChange<TPrincipal> change)
        {
            var model = Activator.CreateInstance(typeof(T), true);
            string primaryKeyField = db.GetReferencePropertyForObject(model);

            return change.PropertyChanges.SingleOrDefault(p => p.PropertyName == primaryKeyField);
        } 

        public HistoryExplorerCloneStrategies CloneStrategy
        {
            get { return cloneStrategy; }
            set { cloneStrategy = value; }
        }
    }
}
