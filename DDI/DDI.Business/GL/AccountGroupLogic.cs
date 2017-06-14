using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DDI.Business.Helpers;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class AccountGroupLogic : EntityLogicBase<AccountGroup>
    {
        #region Fields

        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(AccountGroupLogic));

        #endregion

        public AccountGroupLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #region Public Methods

        public override void Validate(AccountGroup entity)
        {


            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new ValidationException(UserMessagesGL.AccountGroupNameBlank);
            }


        }

    }

    #endregion public methods

}
