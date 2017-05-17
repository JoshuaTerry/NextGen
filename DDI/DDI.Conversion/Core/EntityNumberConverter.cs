using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Conversion.Statics;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;
using System.Text.RegularExpressions;
using DDI.Data;
using System.Data.Entity;
using DDI.Shared.Enums.Core;

namespace DDI.Conversion.Core
{
    internal class EntityNumberConverter
    {
        Dictionary<int, Guid> _businessUnitIds;
        Dictionary<string, Guid> _fiscalYearIds;

        public EntityNumberConverter() : this(null, null) { }

        public EntityNumberConverter(Dictionary<int, Guid> businessUnitIds, Dictionary<string, Guid> fiscalYearIds)
        {
            _businessUnitIds = businessUnitIds ?? new Dictionary<int, Guid>();
            _fiscalYearIds = fiscalYearIds ?? new Dictionary<string, Guid>();
        }

        public void ConvertEntityNumbers(Func<FileImport> entityNumberImporter)
        {
            using (var context = new DomainContext())
            {
                Guid id;
                Guid? rangeId;

                using (var importer = entityNumberImporter())
                {           
                    while (importer.GetNextRow())
                    {
                        int unitKey = importer.GetInt(0);
                        string yearName = importer.GetString(1);
                        rangeId = null;

                        if (unitKey > 0)
                        {
                            if (!_businessUnitIds.TryGetValue(unitKey, out id))
                            {
                                importer.LogError($"Invalid business unit legacy key {unitKey}.");
                                continue;
                            }
                            rangeId = id;
                        }

                        if (!string.IsNullOrWhiteSpace(yearName))
                        {
                            string fiscalYearKey = $"{unitKey},{yearName}";
                            if (!_fiscalYearIds.TryGetValue(fiscalYearKey, out id))
                            {
                                importer.LogError($"Invalid fiscal year legacy key \"{fiscalYearKey}\".");
                                continue;
                            }
                            rangeId = id;
                        }

                        EntityNumberType type = importer.GetEnum<EntityNumberType>(2);
                        int nextNumber = importer.GetInt(3);

                        EntityNumber number = context.EntityNumbers.FirstOrDefault(p => p.EntityNumberType == type && p.RangeId == rangeId);
                        if (number == null)
                        {
                            number = new EntityNumber();
                            number.RangeId = rangeId;
                            number.EntityNumberType = type;
                            context.EntityNumbers.Add(number);
                        }
                        number.NextNumber = nextNumber;
                    }

                }
                context.SaveChanges();
            }
        }
    }
}
