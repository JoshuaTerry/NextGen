using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.GL
{
    public class AccountLogic : EntityLogicBase<Account>
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(AccountLogic));
        public AccountLogic() : this(new UnitOfWorkEF()) { }

        public AccountLogic(IUnitOfWork uow) : base(uow)
        {
        }

        public override void Validate(Account entity)
        {
            base.Validate(entity);

        }

        
    }
}
