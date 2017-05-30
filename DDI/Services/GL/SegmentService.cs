using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;

namespace DDI.Services.GL
{
    public class SegmentService : ServiceBase<Segment>, ISegmentService
    {
        public SegmentService(IUnitOfWork uow) : base(uow) { }

        public IDataResponse<List<ICanTransmogrify>> GetSegmentSearch(Guid fiscalYearId, Guid? parentId, string levelString, IPageable search)
        {
            var result = UnitOfWork.GetEntities<Segment>();
            
            result = result.Where(p => p.FiscalYearId == fiscalYearId);

            if (parentId != null)
            {
                result = result.Where(p => p.ParentSegmentId == parentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(levelString))
            {
                if (levelString.Length > 1)
                {
                    Guid id;
                    if (Guid.TryParse(levelString, out id))
                    {
                        result = result.Where(p => p.SegmentLevelId == id);
                    }
                }
                else
                {
                    int level;
                    if (int.TryParse(levelString, out level) && level > 0)
                    {
                        result = result.Where(p => p.Level == level);
                    }
                }
            }

            return GetPagedResults(result, search);
           
        }
    }
}
