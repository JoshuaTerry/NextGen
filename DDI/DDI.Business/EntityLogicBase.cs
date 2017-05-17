using DDI.Business.Helpers;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models;
using System;
using System.Linq;
using DDI.Shared.Statics;
using System.Collections.Generic;

namespace DDI.Business
{
    /// <summary>
    /// Strongly typed base class for entity business logic.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class EntityLogicBase<T> : EntityLogicBase where T : class, IEntity
    {

        private const int UPDATE_SEARCH_DOCUMENT_DELAY = 2; // Wait 2 seconds after saving before updating search documents.
        private List<Guid> _scheduledUpdates = null;
        private static object _lockObject = new object();

        #region Constructors 

        public EntityLogicBase(IUnitOfWork uow) : base(uow)
        {
            _scheduledUpdates = new List<Guid>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validate an entity.
        /// </summary>
        public virtual void Validate(T entity) { }

        /// <summary>
        /// Build search document for Elasticsearch
        /// </summary>
        public virtual ISearchDocument BuildSearchDocument(T entity)
        {
            return null;
        }

        /// <summary>
        /// Update an entity in Elasticsearch by building the search document and indexing it.
        /// </summary>
        public virtual void UpdateSearchDocument(T entity) { }

        /// <summary>
        /// Schedule an update of an entity in Elasticsearch.
        /// </summary>
        public void ScheduleUpdateSearchDocument(T entity)
        {
            if (entity != null)
            {
                ScheduleUpdateSearchDocument(entity.Id);
            }
        }

        /// <summary>
        /// Schedule an update of an entity in Elasticsearch.
        /// </summary>
        public void ScheduleUpdateSearchDocument(Guid? id)
        {

            UnitOfWork.AddPostSaveAction(() =>
            {
                lock (_lockObject)
                {
                    if (!id.HasValue || _scheduledUpdates.Contains(id.Value))  // Shouldn't schedule an update that's just been scheduled.
                    {
                        return;
                    }

                    _scheduledUpdates.Add(id.Value);
                }

                Shared.TaskScheduler.ScheduleTask(() =>
                {
                    using (var unitOfWork = new UnitOfWorkEF())
                    {
                        T entityToUpdate = unitOfWork.GetById<T>(id.Value);
                        if (entityToUpdate != null)
                        {
                            BusinessLogicHelper.GetBusinessLogic<T>(unitOfWork).UpdateSearchDocument(entityToUpdate);
                        }
                        _scheduledUpdates.Remove(id.Value);  // Remove this id from the list of scheduled updates.
                    }
                }, UPDATE_SEARCH_DOCUMENT_DELAY);
            });
        }


        /// <summary>
        /// Validate an entity.
        /// </summary>
        public override void Validate(IEntity entity)
        {
            T typedEntity = entity as T;
            if (typedEntity != null)
            {
                Validate(typedEntity);
            }
        }

        /// <summary>
        /// Update an entity in Elasticsearch by building the search document and indexing it.
        /// </summary>
        public override void UpdateSearchDocument(IEntity entity)
        {
            T typedEntity = entity as T;
            if (typedEntity != null)
            {
                UpdateSearchDocument(typedEntity);
            }
        }

        /// <summary>
        /// Validate a set of strings, ensuring they are non-blank and unique.
        /// </summary>
        /// <param name="numberOfStrings">If non-zero, validate only the first n strings.</param>
        /// <param name="errorMessageParameter">Paramter to include in the error message (i.e. what the strings represent.)</param>
        /// <param name="strings">List of strings to be validated.</param>
        protected void ValidateNonBlankAndUnique(int numberOfStrings, string errorMessageParameter, params string[] strings)
        {
            if (strings.Length == 0)
            {
                return;
            }

            if (numberOfStrings == 0)
            {
                numberOfStrings = strings.Length;
            }

            var stringsToUpper = strings.Take(numberOfStrings).Select(p => p.ToUpper());

            if (stringsToUpper.Any(p => string.IsNullOrWhiteSpace(p)))
            {
                throw new ValidationException(UserMessages.MustBeNonBlank, errorMessageParameter);
            }
            if (stringsToUpper.Distinct().Count() < numberOfStrings)
            {
                throw new ValidationException(UserMessages.MustBeUnique, errorMessageParameter);
            }
        }
        #endregion

    }

    /// <summary>
    /// Non-generic, non-strongly-typed base class for entity business logic.
    /// </summary>
    public class EntityLogicBase : IEntityLogic, IDisposable
    {
        /// <summary>
        /// Unit of work used by the business logic.
        /// </summary>
        public IUnitOfWork UnitOfWork { get; private set; }

        public EntityLogicBase(IUnitOfWork uow)
        {
            this.UnitOfWork = uow;
            uow.AddBusinessLogic(this);            
        }

        /// <summary>
        /// Validate an entity.
        /// </summary>
        public virtual void Validate(IEntity entity) { }

        /// <summary>
        /// Update an entity in Elasticsearch by building the search document and indexing it.
        /// </summary>
        public virtual void UpdateSearchDocument(IEntity entity) { }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    UnitOfWork?.Dispose();
                }

                UnitOfWork = null;

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion

    }
}
