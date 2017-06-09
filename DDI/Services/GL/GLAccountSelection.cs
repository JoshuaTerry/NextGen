using DDI.Shared.Models.Client.GL;
using DDI.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace DDI.Services.GL
{
    public class GLAccountSelectionService : ServiceBase<GLAccountSelection>
    {
        public GLAccountSelectionService(IUnitOfWork uow) : base(uow)
        {
          
        }
        public IEnumerable<GLAccountSelection> GetGLAccountsForFiscalYearId(Guid fiscalYearId)
        {
            return UnitOfWork.GetEntities<GLAccountSelection>().Where(gs=> gs.FiscalYearId == fiscalYearId) ; 
        }
    }
}
