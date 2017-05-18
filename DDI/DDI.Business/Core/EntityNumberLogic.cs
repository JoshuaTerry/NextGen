using DDI.Data;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DDI.Shared.Enums.Core;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Core
{
    public class EntityNumberLogic : EntityLogicBase<EntityNumber>
    {
        private enum EntityNumberMode { Get, Set, Return }

        #region Private Fields

        #endregion

        #region Constructors 

        public EntityNumberLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the next entity number.
        /// </summary>
        public int GetNextEntityNumber(EntityNumberType type, Guid? rangeId = null)
        {
            return GetSetNextEntityNumber(type, EntityNumberMode.Get, 0, rangeId);
        }

        /// <summary>
        /// Get the next entity number.
        /// </summary>
        public int GetNextEntityNumber(EntityNumberType type, IEntity entity)
        {
            return GetSetNextEntityNumber(type, EntityNumberMode.Get, 0, entity?.Id);
        }

        /// <summary>
        /// Set the next entity number.
        /// </summary>
        public void SetNextEntityNumber(EntityNumberType type, int nextNumber, Guid? rangeId = null)
        {
            GetSetNextEntityNumber(type, EntityNumberMode.Set, nextNumber, rangeId);
        }

        /// <summary>
        /// Set the next entity number.
        /// </summary>
        public void SetNextEntityNumber(EntityNumberType type, int nextNumber, IEntity entity)
        {
            GetSetNextEntityNumber(type, EntityNumberMode.Set, nextNumber, entity?.Id);
        }

        /// <summary>
        /// Allow a discarded entity number to be reused.
        /// </summary>
        public void ReturnEntityNumber(EntityNumberType type, int number,  Guid? rangeId = null)
        {
            GetSetNextEntityNumber(type, EntityNumberMode.Return, number, rangeId);
        }

        /// <summary>
        /// Allow a discarded entity number to be reused.
        /// </summary>
        public void ReturnEntityNumber(EntityNumberType type, int number, IEntity entity)
        {
            GetSetNextEntityNumber(type, EntityNumberMode.Return, number, entity?.Id);
        }

        #endregion

        #region Private Methods

        private int GetSetNextEntityNumber(EntityNumberType type, EntityNumberMode mode, int numberArgument, Guid? rangeId)
        {
            bool retry;
            int resultNumber = 0;

            do
            {
                retry = false;

                try
                {
                    UnitOfWork.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                    EntityNumber entityNumber;
                    
                    if (rangeId.HasValue)
                    {
                        entityNumber = UnitOfWork.FirstOrDefault<EntityNumber>(p => p.EntityNumberType == type && p.RangeId == rangeId);
                    }
                    else
                    {
                        entityNumber = UnitOfWork.FirstOrDefault<EntityNumber>(p => p.EntityNumberType == type && p.RangeId == null);
                    }

                    if (entityNumber == null)
                    {
                        entityNumber = new EntityNumber()
                        {
                            EntityNumberType = type,
                            RangeId = rangeId,
                            NextNumber = 1,
                            PreviousNumber = 0
                        };
                        UnitOfWork.Insert(entityNumber);
                    }
                    if (numberArgument > 0 && mode == EntityNumberMode.Set)
                    {
                        entityNumber.NextNumber = resultNumber = numberArgument;
                        entityNumber.PreviousNumber = 0;
                    }
                    else if (numberArgument > 0 && mode == EntityNumberMode.Return)
                    {
                        entityNumber.PreviousNumber = numberArgument;
                        resultNumber = numberArgument;
                    }
                    else if (entityNumber.PreviousNumber > 0 && mode == EntityNumberMode.Get)
                    {
                        resultNumber = entityNumber.PreviousNumber;
                        entityNumber.PreviousNumber = 0;
                    }
                    else if (mode == EntityNumberMode.Get)
                    {
                        resultNumber = entityNumber.NextNumber++;
                    }

                    if (!UnitOfWork.CommitTransaction())
                    {
                        UnitOfWork.RollbackTransaction();
                        retry = true;
                    }
                }
                catch (Exception ex)
                {
                    UnitOfWork.RollbackTransaction();
                    throw ex;
                }
            }
            while (retry);

            return resultNumber;
        }


        #endregion

    }
}
