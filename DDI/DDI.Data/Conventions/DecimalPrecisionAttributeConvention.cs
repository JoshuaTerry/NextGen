﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Attributes.Models;

namespace DDI.Data.Conventions
{
    public class DecimalPrecisionAttributeConvention
    : PrimitivePropertyAttributeConfigurationConvention<DecimalPrecisionAttribute>
    {
        private const int MIN_PRECISION = 1;
        private const int MAX_PRECISION = 38;

        public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DecimalPrecisionAttribute attribute)
        {
            if (attribute.Precision < MIN_PRECISION || attribute.Precision > MAX_PRECISION)
            {
                throw new InvalidOperationException($"Precision must be between {MIN_PRECISION} and {MAX_PRECISION}.");
            }

            if (attribute.Scale > attribute.Precision)
            {
                throw new InvalidOperationException("Scale must be between 0 and the Precision value.");
            }

            configuration.HasPrecision(attribute.Precision, attribute.Scale);
        }
    }
}
