using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.Tests.Core.DataSources
{
    public static class EntityNumberDataSource
    {
        private static Guid _rangeIdForTest = new Guid("{6BEADF84-DE99-4E3D-A4FD-55A55FA2BC96}");
        public static Guid RangeIdForTest => _rangeIdForTest;
        public const int STARTING_NEXT_NUMBER = 12345;

        public static IList<EntityNumber> GetDataSource(UnitOfWorkNoDb uow)
        {

            IList<EntityNumber> existing = uow.GetRepositoryOrNull<EntityNumber>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }
            
            var entityNumbers = new List<EntityNumber>();
            entityNumbers.Add(new EntityNumber() { EntityNumberType = EntityNumberType.Constituent, NextNumber = STARTING_NEXT_NUMBER, Id = GuidHelper.NewSequentialGuid() });
            entityNumbers.Add(new EntityNumber() { EntityNumberType = EntityNumberType.Journal, NextNumber = STARTING_NEXT_NUMBER, RangeId = _rangeIdForTest, Id = GuidHelper.NewSequentialGuid() });

            uow.CreateRepositoryForDataSource(entityNumbers);
            return entityNumbers;
        }    

    }

    
}
