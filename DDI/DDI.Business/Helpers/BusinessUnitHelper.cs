using DDI.Shared;
using DDI.Shared.Extensions;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using System;
using System.Collections.Generic;
using DDI.Shared.Caching;
using DDI.Business.GL;

namespace DDI.Business.Helpers
{
    /// <summary>
    /// Static helper class for overall business unit properties
    /// </summary>
    public static class BusinessUnitHelper
    {
        /// <summary>
        /// Returns TRUE if multiple business units have been defined.
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        public static bool GetIsMultiple(IUnitOfWork unitOfWork)
        {
            object result = CacheHelper.GetEntry<object>(BusinessUnitLogic.IsMultipleCacheKey, () =>
            {
                if (unitOfWork == null)
                {
                    return new BusinessUnitLogic().IsMultiple;
                }
                return unitOfWork.GetBusinessLogic<BusinessUnitLogic>().IsMultiple;
            });

            if (result is bool)
            {
                return (bool)result;
            }
            return false;
        }

        /// <summary>
        /// Returns the client-specific business unit label.
        /// </summary>
        public static string GetBusinessUnitLabel(IUnitOfWork unitOfWork)
        {
            return CacheHelper.GetEntry(BusinessUnitLogic.BusinessUnitLabelCacheKey, () =>
            {
                if (unitOfWork == null)
                {
                    return new BusinessUnitLogic().BusinessUnitLabel;
                }
                return unitOfWork.GetBusinessLogic<BusinessUnitLogic>().BusinessUnitLabel;
            });
        }
    }
}
