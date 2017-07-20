using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Conversion.Statics
{
    /// <summary>
    /// Filenames for SSIS output files
    /// </summary>
    internal static class OutputFile
    {
        #region Core SSIS Files

        public static string Core_NoteFile => "Notes.csv";
        public static string Core_NoteTopicFile => "NoteTopicNotes.csv";
        public static string Core_TransactionFile => "Transactions.csv";
        public static string Core_EntityTransactionFile => "EntityTransactions.csv";
        public static string Core_TransactionXrefFile => "TransactionXref.csv";
        public static string Core_EntityApprovalFile => "EntityApprovals.csv";
        public static string Core_AttachmentFile => "Attachments.csv";

        #endregion

        #region CRM SSIS Files

        public static string CRM_AddressFile => "Address.csv";
        public static string CRM_ConstituentFile => "Constituent.csv";
        public static string CRM_EthnicityFile => "EthnicityConstituents.csv";
        public static string CRM_DenominationFile => "DenominationConstituents.csv";
        public static string CRM_ConstituentAddresFile => "ConstituentAddress.csv";
        public static string CRM_DoingBusinessAsFile => "DoingBusinessAs.csv";
        public static string CRM_EducationFile => "Education.csv";
        public static string CRM_AlternateIdFile => "AlternateId.csv";
        public static string CRM_ContactInfoFile => "ContactInfo.csv";
        public static string CRM_RelationshipFile => "Relationship.csv";
        public static string CRM_TagFile => "TagConstituents.csv";
        public static string CRM_CustomDataFile => "CustomFieldData.csv";

        #endregion

        #region CP SSIS Files

        public static string CP_PaymentMethodFile => "PaymentMethod.csv";
        public static string CP_PaymentMethodConstituentFile => "PaymentMethodConstituents.csv";
        public static string CP_ReceiptBatchFile => "ReceiptBatch.csv";
        public static string CP_ReceiptFile => "Receipt.csv";
        public static string CP_MiscReceiptFile => "MiscReceipt.csv";
        public static string CP_MiscReceiptLineFile => "MiscReceiptLine.csv";

        #endregion

        #region GL SSIS Files

        public static string GL_SegmentFile => "Segment.csv";
        public static string GL_AccountGroupFile => "AccountGroup.csv";
        public static string GL_AccountFile => "Account.csv";
        public static string GL_AccountPriorYearFile => "AccountPriorYear.csv";
        public static string GL_AccountSegmentFile => "AccountSegment.csv";
        public static string GL_LedgerAccountFile => "LedgerAccount.csv";
        public static string GL_LedgerAccountMergeFile => "LedgerAccountMerge.csv";
        public static string GL_LedgerAccountYearFile => "LedgerAccountYear.csv";
        public static string GL_AccountBudgetFile => "AccountBudget.csv";
        public static string GL_FundFile => "Fund.csv";
        public static string GL_FundFromToFile => "FundFromTo.csv";
        public static string GL_BusinessUnitFromToFile => "BusinessUnitFromTo.csv";
        public static string GL_PostedTransactionFile => "PostedTransaction.csv";
        public static string GL_JournalFile => "Journal.csv";
        public static string GL_JournalLineFile => "JournalLine.csv";

        #endregion

        #region ID Mapping Files

        #region CRM Mapping Files
        public static string CRM_ConstituentIdMappingFile => "ConstituentId.csv";
        public static string CRM_AddressIdMappingFile => "AddressId.csv";
        #endregion

        #region CP Mapping Files
        public static string CP_PaymentMethodIdMappingFile => "PaymentMethodId.csv";
        public static string CP_BankAccountIdMappingFile => "BankAccountId.csv";
        public static string CP_ReceiptBatchIdMappingFile => "ReceiptBatchId.csv";
        public static string CP_ReceiptMappingFile => "ReceiptId.csv";
        public static string CP_MiscReceiptMappingFile => "MiscReceiptId.csv";
        public static string CP_MiscReceiptLineMappingFile => "MiscReceiptLineId.csv";
        #endregion

        #region Core Mapping Files
        public static string Core_NoteIdMappingFile => "NoteId.csv";
        public static string Core_FileStorageMappingFile => "FileStorageId.csv";
        public static string Core_AttachmentMappingFile => "AttachmentId.csv";
        #endregion

        #region GL Mapping Files
        public static string GL_BusinessUnitIdMappingFile => "BusinessUnitId.csv";
        public static string GL_LedgerIdMappingFile => "LedgerId.csv";
        public static string GL_FiscalYearIdMappingFile => "FiscalYearId.csv";
        public static string GL_SegmentIdMappingFile => "SegmentId.csv";
        public static string GL_AccountGroupIdMappingFile => "AccountGroupId.csv";
        public static string GL_AccountIdMappingFile => "AccountId.csv";
        public static string GL_LedgerAccountIdMappingFile => "LedgerAccountId.csv";
        public static string GL_LedgerAccountYearIdMappingFile => "LedgerAccountYearId.csv";
        public static string GL_FundIdMappingFile => "FundId.csv";
        public static string GL_PostedTransactionMappingFile => "PostedTransactionId.csv";
        public static string GL_JournalMappingFile => "JournalId.csv";
        public static string GL_JournalLineMappingFile => "JournalLineId.csv";
        #endregion

        #endregion

    }
}
