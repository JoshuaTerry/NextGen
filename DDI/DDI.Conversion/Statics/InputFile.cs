﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Conversion.Statics
{
    /// <summary>
    /// Filenames for data conversion input files generated by OpenEdge
    /// </summary>
    internal static class InputFile
    {
        // Note, for now these are named after the input files.  Later they could be renamed to be more general.
        #region CRM Files
        public static string CRM_NACodes => "NACodes.csv";
        public static string CRM_ContactType => "ContactType.csv";
        public static string CRM_NamePrefix => "NamePrefix.csv";
        public static string CRM_RegionLevel => "RegionLevel.csv";
        public static string CRM_Region => "Region.csv";
        public static string CRM_RegionAreas => "RegionAreas.csv";
        public static string CRM_RelationshipType => "RelationshipType.csv";
        public static string CRM_TagGroup => "TagGroup.csv";
        public static string CRM_TagCode => "TagCode.csv";
        public static string CRM_NASetup => "NASetup.csv";

        public static string CRM_Individual => "Individual.csv";
        public static string CRM_IndividualFW => "IndividualFW.csv";
        public static string CRM_Organization => "Organization.csv";
        public static string CRM_OrganizationFW => "OrganizationFW.csv";
        public static string CRM_Address => "Address.csv";
        public static string CRM_AddressFW => "AddressFW.csv";
        public static string CRM_ConstituentAddress => "ConstituentAddress.csv";
        public static string CRM_ConstituentAddressFW => "ConstituentAddressFW.csv";
        public static string CRM_ConstituentDBA => "ConstituentDBA.csv";
        public static string CRM_EducationInfo => "EducationInfo.csv";
        public static string CRM_AlternateID => "AlternateID.csv";
        public static string CRM_ContactInfo => "ContactInfo.csv";
        public static string CRM_ContactInfoFW => "ContactInfoFW.csv";
        public static string CRM_Relationship => "Relationship.csv";
        public static string CRM_ConstituentTag => "ConstituentTag.csv";
        public static string CRM_CustomData => "CustomData.csv";

        public static string CRM_NoteCategory => "MemoCategory.csv";

        public static string CRM_MemoConstituent => "Memo_Constituent.csv";
        public static string CRM_EntityNumber => "EntityNumber_Constituent.csv";
        public static string CRM_FileStorageConstituent => "FileStorage_Constituent.csv";
        public static string CRM_Attachment => "Attachment_Constituent.csv";

        #endregion

        #region DDI Files
        public static string DDI_User => "DDIUser.csv";
        public static string DDI_Settings => "DDISettings.csv";
        #endregion

        #region GL Files

        public static string GL_FWCodes => "FWCodes.csv";
        public static string GL_BusinessUnits => "Entity.csv";
        public static string GL_BusinessUnitUsers => "EntityUser.csv";
        public static string GL_Ledgers => "Ledger.csv";
        public static string GL_FiscalYears => "FiscalYear.csv";
        public static string GL_FiscalPeriods => "FiscalPeriod.csv";
        public static string GL_SegmentLevels => "SegmentLevel.csv";
        public static string GL_Segments => "Segment.csv";
        public static string GL_AccountGroups => "AccountGroup.csv";
        public static string GL_Accounts => "Account.csv";
        public static string GL_AccountPriorYears => "AccountPriorYear.csv";
        public static string GL_LedgerAccounts => "LedgerAccount.csv";
        public static string GL_LedgerAccountYears => "LedgerAccountYear.csv";
        public static string GL_LedgerAccountMerges => "LedgerAccountMerge.csv";
        public static string GL_AccountBudgets => "Budget.csv";
        public static string GL_Funds => "Fund.csv";
        public static string GL_FundFromTos => "FundXref.csv";
        public static string GL_BusinessUnitFromTos => "EntityXref.csv";
        public static string GL_PostedTransactions => "PostedTran.csv";
        public static string GL_Journals => "Journal.csv";
        public static string GL_JournalLines => "JournalLine.csv";
        public static string GL_JournalTransactions => "Transaction_Journal.csv";
        public static string GL_JournalEntityTransactions => "EntityTransaction_Journal.csv";
        public static string GL_JournalEntityNumbers => "EntityNumber_Journal.csv";
        public static string GL_JournalApprovals => "Approval_Journal.csv";
        public static string GL_MemoJournals => "Memo_Journal.csv";
        public static string GL_FileStorage => "FileStorage_Journal.csv";
        public static string GL_Attachment => "Attachment_Journal.csv";

        #endregion

        #region CP Files

        public static string CP_EFTInfo => "EFTInfo.csv";
        public static string CP_EFTFormat => "EFTFormat.csv";

        #endregion




    }
}
