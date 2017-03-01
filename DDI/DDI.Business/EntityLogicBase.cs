﻿using System.Collections;
using System.Collections.Generic;
using System;
using DDI.Shared;
using DDI.Shared.Models;
using System.Threading.Tasks;
using DDI.Business.Helpers;
using DDI.Data;

namespace DDI.Business
{
    /// <summary>
    /// Strongly typed base class for entity business logic.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class EntityLogicBase<T> : EntityLogicBase where T : class, IEntity
    {
        private const int UPDATE_SEARCH_DOCUMENT_DELAY = 2; // Wait 2 seconds after saving before updating search documents.

        #region Constructors 

        public EntityLogicBase(IUnitOfWork uow) : base(uow) { }

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
            ScheduleUpdateSearchDocument(entity.Id);
        }

        /// <summary>
        /// Schedule an update of an entity in Elasticsearch.
        /// </summary>
        public void ScheduleUpdateSearchDocument(Guid? id)
        {
            if (!id.HasValue)
            {
                return;
            }
            Shared.TaskScheduler.ScheduleTask(() =>
            {
                using (var unitOfWork = new UnitOfWorkEF())
                {
                    T entityToUpdate = unitOfWork.GetById<T>(id.Value);
                    BusinessLogicHelper.GetBusinessLogic<T>(unitOfWork).UpdateSearchDocument(entityToUpdate);
                }
            }, UPDATE_SEARCH_DOCUMENT_DELAY);
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
