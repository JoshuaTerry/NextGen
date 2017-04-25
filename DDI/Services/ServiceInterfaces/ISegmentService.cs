using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;

namespace DDI.Services.ServiceInterfaces
{
    public interface ISegmentService : IService<Segment>
    {
        IDataResponse<List<ICanTransmogrify>> GetSegmentSearch(Guid fiscalYearId, Guid? parentId, string levelString, IPageable search);
    }
}
