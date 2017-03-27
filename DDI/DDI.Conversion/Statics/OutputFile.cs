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
        #region ID Mapping Files

        public static string ConstituentIdMappingFile = "ConstituentId.csv";
        public static string AddressIdMappingFile = "AddressId.csv";
        public static string PaymentMethodIdMappingFile = "PaymentMethodId.csv";
        public static string NoteIdMappingFile = "NoteId.csv";
        public static string BusinessUnitIdMappingFile = "BusinessUnitId.csv";
        public static string LedgerIdMappingFile = "LedgerId.csv";
        public static string FiscalYearIdMappingFile = "FiscalYearId.csv";
        public static string SegmentIdMappingFile = "SegmentId.csv";
        public static string AccountGroupIdMappingFile = "AccountGroupId.csv";
        public static string AccountIdMappingFile = "AccountId.csv";
        public static string LedgerAccountIdMappingFile = "LedgerAccountId.csv";
        public static string LedgerAccountYearIdMappingFile = "LedgerAccountYearId.csv";

        #endregion

        #region Core SSIS Files

        public static string Core_NoteFile => "Notes.csv";
        public static string Core_NoteTopicFile => "NoteTopicNotes.csv";
        
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
        
        #endregion


    }
}
