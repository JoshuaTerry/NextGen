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
        #endregion

        #region DDI Files
        public static string DDI_User => "DDIUser.csv";
        #endregion

        #region GL Files
        public static string GL_FWCodes => "FWCodes.csv";
        public static string GL_BusinessUnits => "Entity.csv";
        public static string GL_BusinessUnitUsers => "EntityUser.csv";
        public static string GL_Ledgers => "Ledger.csv";
        public static string GL_FiscalYears => "FiscalYear.csv";
        public static string GL_FiscalPeriods => "FiscalPeriod.csv";
        #endregion

        #region CP Files
        public static string CP_EFTInfo => "EFTInfo.csv";
        #endregion




    }
}
