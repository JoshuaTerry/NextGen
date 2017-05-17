using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;

namespace DDI.Services.ServiceInterfaces
{
    public interface IJournalService : IService<Journal>
    {
        IDataResponse<Journal> NewJournal(JournalType journalType, Guid? businessUnitId, Guid? fiscalYearId);
    }
}
