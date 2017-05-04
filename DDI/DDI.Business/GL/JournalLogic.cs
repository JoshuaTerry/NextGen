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


        public override void Validate(Journal journal)
        {
            journal.AssignPrimaryKey();
            ScheduleUpdateSearchDocument(journal);
        }

        public override void UpdateSearchDocument(Journal journal)
        {
            var elasticRepository = new ElasticRepository<JournalDocument>();
            elasticRepository.Update((JournalDocument)BuildSearchDocument(journal));
        }

        public override ISearchDocument BuildSearchDocument(Journal entity)
        {
            string journalTypeName = LinkedEntityHelper.GetEntityTypeName<Journal>();

            var document = new JournalDocument()
            {
                Id = entity.Id,
                JournalNumber = entity.JournalNumber,
                Amount = entity.Amount,
                FiscalYearId = entity.FiscalYearId,
                BusinessUnitId = entity.BusinessUnitId,
                Comment = entity.Comment,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                TransactionDate = entity.TransactionDate,
                LineItemComments = string.Join(" ", entity.JournalLines.Select(p => p.Comment).Where(p => !string.IsNullOrWhiteSpace(p))),
                JournalType = (int)entity.JournalType
            };

            // Status
            List<string> statusTerms = new List<string>();
            if (entity.DeletionDate.HasValue)
            {
                statusTerms.Add(EntityStatus.Deleted);
            }

            if (entity.JournalType == JournalType.Recurring)
            {
                if (entity.IsExpired)
                {
                    statusTerms.Add(EntityStatus.Expired);
                }
                else
                {
                    statusTerms.Add(EntityStatus.Active);
                }
            }

            if (entity.JournalType == JournalType.Normal)
            {
                var approvals = UnitOfWork.Where<EntityApproval>(p => p.EntityType == journalTypeName && p.ParentEntityId == entity.Id);
                if (approvals.Count() > 0)
                {
                    if (approvals.All(p => p.AppprovedById != null))
                    {
                        statusTerms.Add(EntityStatus.Approved);
                    }
                    else
                    {
                        statusTerms.Add(EntityStatus.Unapproved);
                    }
                }

                var transactions = UnitOfWork.GetEntities<EntityTransaction>(p => p.Transaction)
                                             .Where(p => p.EntityType == journalTypeName && p.ParentEntityId == entity.Id)
                                             .Select(p => p.Transaction);

                if (transactions.All(p => p.PostDate != null))
                {
                    statusTerms.Add(EntityStatus.Posted);
                }
                else
                {
                    statusTerms.Add(EntityStatus.Unposted);
                }
            }

            document.Status = string.Join(" ", statusTerms);
            return document;
        }

    }
}
