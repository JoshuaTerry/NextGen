using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Data.Helpers;
using DDI.Search;
using DDI.Search.Models;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class JournalLogic : EntityLogicBase<Journal>
    {        

        #region Constructors 

        public JournalLogic() : this(new UnitOfWorkEF()) { }

        public JournalLogic(IUnitOfWork uow) : base(uow)
        {
           
        }

        #endregion


        public override void UpdateSearchDocument(Journal journal)
        {
            var elasticRepository = new ElasticRepository<JournalDocument>();
            elasticRepository.Update((JournalDocument)BuildSearchDocument(journal));
        }

        public override ISearchDocument BuildSearchDocument(Journal entity)
        {
            string journalTypeName = LinkedEntityHelper.GetEntityTypeName<Journal>();

            var document = new JournalDocument();

            document.Id = entity.Id;
            document.JournalNumber = entity.JournalNumber;
            document.Amount = entity.Amount;
            document.FiscalYearId = entity.FiscalYearId;
            document.BusinessUnitId = entity.BusinessUnitId;
            document.Comment = entity.Comment;
            document.CreatedBy = entity.CreatedBy;
            document.CreatedOn = entity.CreatedOn;
            document.TransactionDate = entity.TransactionDate;
            document.LineItemComments = string.Join(" ", entity.JournalLines.Select(p => p.Comment).Where(p => !string.IsNullOrWhiteSpace(p)));
            document.JournalType = (int)entity.JournalType;

            // Status
            string status = string.Empty;
            if (entity.DeletionDate.HasValue)
            {
                status += " Deleted";
            }

            if (entity.JournalType == JournalType.Recurring)
            {
                if (entity.IsExpired)
                {
                    status += " Expired";
                }
                else
                {
                    status += " Active";
                }
            }

            if (entity.JournalType == JournalType.Normal)
            {
                var approvals = UnitOfWork.Where<EntityApproval>(p => p.EntityType == journalTypeName && p.ParentEntityId == entity.Id);
                if (approvals.Count() > 0)
                {
                    if (approvals.All(p => p.AppprovedById != null))
                    {
                        status += " Approved";
                    }
                    else
                    {
                        status += " Unapproved";
                    }
                }

                var transactions = UnitOfWork.GetEntities<EntityTransaction>(p => p.Transaction)
                                             .Where(p => p.EntityType == journalTypeName && p.ParentEntityId == entity.Id)
                                             .Select(p => p.Transaction);

                if (transactions.All(p => p.PostDate != null))
                {
                    status += " Posted";
                }
                else
                {
                    status += " Unposted";
                }
            }

            document.Status = status.Trim();

            return document;
        }

    }
}
