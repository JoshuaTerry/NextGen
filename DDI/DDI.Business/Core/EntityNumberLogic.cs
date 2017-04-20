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

        public EntityNumberLogic() : this(new UnitOfWorkEF()) { }

        public EntityNumberLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the next entity number.
        /// </summary>
        public int GetNextEntityNumber(EntityNumberType type)
        {
            return GetSetNextEntityNumber(type, EntityNumberMode.Get, 0, null, null);
        }

        /// <summary>
        /// Get the next entity number, specific to a business unit.
        /// </summary>
        public int GetNextEntityNumber(EntityNumberType type, BusinessUnit unit)
        {
            return GetSetNextEntityNumber(type, EntityNumberMode.Get, 0, unit.Id, null);
        }

        /// <summary>
        /// Get the next entity number, specific to a fiscal year.
        /// </summary>
        public int GetNextEntityNumber(EntityNumberType type, FiscalYear year)
        {
            return GetSetNextEntityNumber(type, EntityNumberMode.Get, 0, null, year.Id);
        }
        
        public void SetNextEntityNumber(EntityNumberType type, int nextNumber)
        {
            GetSetNextEntityNumber(type, EntityNumberMode.Set, nextNumber, null, null);
        }

        public void SetNextEntityNumber(EntityNumberType type, int nextNumber, BusinessUnit unit)
        {
            GetSetNextEntityNumber(type, EntityNumberMode.Set, nextNumber, unit.Id, null);
        }

        public void SetNextEntityNumber(EntityNumberType type, int nextNumber, FiscalYear year)
        {
            GetSetNextEntityNumber(type, EntityNumberMode.Set, nextNumber, null, year.Id);
        }

        public void ReturnEntityNumber(EntityNumberType type, int number)
        {
            GetSetNextEntityNumber(type, EntityNumberMode.Return, number, null, null);
        }

        public void ReturnEntityNumber(EntityNumberType type, int number, BusinessUnit unit)
        {
            GetSetNextEntityNumber(type, EntityNumberMode.Return, number, unit.Id, null);
        }

        public void ReturnEntityNumber(EntityNumberType type, int number, FiscalYear year)
        {
            GetSetNextEntityNumber(type, EntityNumberMode.Return, number, null, year.Id);
        }

        private int GetSetNextEntityNumber(EntityNumberType type, EntityNumberMode mode, int numberArgument, Guid? businessUnitId, Guid? fiscalYearId)
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
                    
                    if (fiscalYearId.HasValue)
                    {
                        businessUnitId = null;
                        entityNumber = UnitOfWork.FirstOrDefault<EntityNumber>(p => p.EntityNumberType == type && p.FiscalYearId == fiscalYearId);
                    }
                    else if (businessUnitId.HasValue)
                    {
                        entityNumber = UnitOfWork.FirstOrDefault<EntityNumber>(p => p.EntityNumberType == type && p.BusinessUnitId == businessUnitId);
                    }
                    else
                    {
                        entityNumber = UnitOfWork.FirstOrDefault<EntityNumber>(p => p.EntityNumberType == type && p.BusinessUnitId == null && p.FiscalYearId == null);
                    }

                    if (entityNumber == null)
                    {
                        entityNumber = new EntityNumber()
                        {
                            EntityNumberType = type,
                            BusinessUnitId = businessUnitId,
                            FiscalYearId = fiscalYearId,
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

        #region Private Methods



        #endregion

    }
}
