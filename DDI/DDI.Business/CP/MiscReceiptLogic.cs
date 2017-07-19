using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDI.Business.Helpers;
using DDI.Logger;
using DDI.Search;
using DDI.Search.Models;
using DDI.Shared;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.CP;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Statics;

namespace DDI.Business.CP
{
    public class MiscReceiptLogic : EntityLogicBase<MiscReceipt>
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(PaymentMethodLogic));
        private string _miscReceiptTypeName;

        public MiscReceiptLogic(IUnitOfWork uow) : base(uow)
        {
            _miscReceiptTypeName = LinkedEntityHelper.GetEntityTypeName<MiscReceipt>();
        }

        public override void Validate(MiscReceipt entity)
        {
            ScheduleUpdateSearchDocument(entity);
        }

        public override void UpdateSearchDocument(MiscReceipt miscReceipt)
        {
            var elasticRepository = new ElasticRepository<MiscReceiptDocument>();
            elasticRepository.Update((MiscReceiptDocument)BuildSearchDocument(miscReceipt));
        }

        public override ISearchDocument BuildSearchDocument(MiscReceipt entity)
        {
            var document = new MiscReceiptDocument()
            {
                Id = entity.Id,
                MiscReceiptNumber = entity.MiscReceiptNumber,
                Amount = entity.Amount,
                FiscalYearId = entity.FiscalYearId,
                BusinessUnitId = entity.BusinessUnitId,
                Comment = entity.Comment,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                TransactionDate = entity.TransactionDate,
                MiscReceiptType = (int)entity.MiscReceiptType,
                ConstituentId = entity.ConstituentId
            };

            if (entity.MiscReceiptLines != null)
            {
                document.LineItemComments = string.Join(" ", entity.MiscReceiptLines.Select(p => p.Comment).Where(p => !string.IsNullOrWhiteSpace(p)));
            }

            // Status
            List<string> statusTerms = new List<string>();
            if (entity.DeletionDate.HasValue)
            {
                statusTerms.Add(EntityStatus.Deleted);
            }
           

            if (entity.MiscReceiptType == MiscReceiptType.OneTime)
            {
                var approvals = UnitOfWork.Where<EntityApproval>(p => p.EntityType == _miscReceiptTypeName && p.ParentEntityId == entity.Id);
                if (approvals.Count() > 0)
                {
                    if (approvals.All(p => p.ApprovedById != null))
                    {
                        statusTerms.Add(EntityStatus.Approved);
                    }
                    else
                    {
                        statusTerms.Add(EntityStatus.Unapproved);
                    }
                }

                var transactions = UnitOfWork.GetEntities<EntityTransaction>(p => p.Transaction)
                                             .Where(p => p.EntityType == _miscReceiptTypeName && p.ParentEntityId == entity.Id && p.Relationship == EntityTransactionRelationship.Owner)
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

            if (entity.IsReversed)
            {
                statusTerms.Add(EntityStatus.Reversed);
            }

            if (entity.DeletionDate != null)
            {
                statusTerms.Add(EntityStatus.Deleted);
            }

            document.Status = string.Join(" ", statusTerms);
            return document;
        }

        /// <summary>
        /// Format the misc. receipt status as a string.
        /// </summary>
        /// <param name="miscReceipt">Misc. Receipt object</param>
        /// <returns></returns>
        public string GetMiscReceiptStatusDescription(MiscReceipt miscReceipt)
        {
            if (miscReceipt.DeletionDate.HasValue)
            {
                return $"Deleted on {miscReceipt.DeletionDate:d}";
            }

            if (miscReceipt.MiscReceiptType == MiscReceiptType.OneTime)
            {
                string result = string.Empty;

                var approvals = UnitOfWork.Where<EntityApproval>(p => p.EntityType == _miscReceiptTypeName && p.ParentEntityId == miscReceipt.Id);
                if (approvals.Count() > 0)
                {
                    if (approvals.All(p => p.ApprovedById != null))
                    {
                        result = "Approved";
                        var approval = approvals.First();
                        var approver = UnitOfWork.GetReference(approval, p => p.ApprovedBy);
                        if (approver != null)
                        {
                            result += $" by {approver.FullName}";
                        }
                        if (approval.ApprovedOn.HasValue)
                        {
                            result += $" on {approval.ApprovedOn.ToRoundTripString()}";
                        }
                    }
                    else
                    {
                        return "Unapproved";
                    }
                }

                var transactions = UnitOfWork.GetEntities<EntityTransaction>(p => p.Transaction)
                                             .Where(p => p.EntityType == _miscReceiptTypeName && p.ParentEntityId == miscReceipt.Id && p.Relationship == EntityTransactionRelationship.Owner)
                                             .Select(p => p.Transaction);

                if (result.Length > 0)
                {
                    result += ", ";
                }
                if (transactions.All(p => p.PostDate != null))
                {
                    result += $"Posted on {transactions.Max(p => p.PostDate.Value).ToRoundTripString()}";
                }
                else
                {
                    result += "Unposted";
                }
                return result;
            }

            return "Active";
        }

        /// <summary>
        /// Get a description for a misc. receipt.
        /// </summary>
        public string GetMiscReceiptDescription(MiscReceipt miscReceipt)
        {
            if (miscReceipt == null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            switch (miscReceipt.MiscReceiptType)
            {
                case MiscReceiptType.OneTime:
                    sb.Append("One-Time Misc. Receipt");
                    break;
                case MiscReceiptType.Template:
                    sb.Append("Misc. Receipt Template");
                    break;
            }

            sb.Append(" #").Append(miscReceipt.MiscReceiptNumber);

            if (miscReceipt.MiscReceiptType == MiscReceiptType.OneTime)
            {
                var fiscalyear = UnitOfWork.GetReference(miscReceipt, p => p.FiscalYear);
                sb.Append(" for ").Append(fiscalyear.Name);
            }

            return sb.ToString();
        }

    }
}
