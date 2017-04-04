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
        public static bool IsMultiple
        {
            get
            {
                object result = CacheHelper.GetEntry<object>(BusinessUnitLogic.IsMultipleCacheKey, () => GetIsMultiple());
                if (result is bool)
                {
                    return (bool)result;
                }
                return false;
            }
        }

        private static object GetIsMultiple()
        {
            return new BusinessUnitLogic().IsMultiple;
        }
    }
}
