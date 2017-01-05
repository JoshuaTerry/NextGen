using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Data.Models.Client.CRM;
using DDI.Shared.Helpers;

namespace DDI.Business.CRM
{
    public class NameFormatter
    {
        private const string DEFAULT_NAME_FORMAT = "{P}{F}{MI}{L}{S}";

        private string _defaultIndividualNameFormat;

        private string Initialize(string s)
        {
            return (
                string.IsNullOrWhiteSpace(s) 
                ?
                string.Empty
                :
                char.ToUpper(s[0]) + "."
                );
        }

        public enum LabelRecipient { Both, Primary, Spouse, Husband, Wife }

        #region Constructors 

        public NameFormatter() : this(new UnitOfWorkEF()) { }

        public NameFormatter(IUnitOfWork uow)
        {
            UnitOfWork = uow;
            var indivType = uow.FirstOrDefault<ConstituentType>(p => p.Code == "I");

            _defaultIndividualNameFormat = StringHelper.FirstNonBlank(indivType?.NameFormat, DEFAULT_NAME_FORMAT);            
        }


        public IUnitOfWork UnitOfWork { get; private set; }

        public string FormatIndividualSortName(Constituent name)
        {
            return FormatIndividualSortName(ConvertToSimpleName(name));
        }

        /// <summary>
        /// Format a sortable name for an individual:  Last First Middle
        /// </summary>
        private string FormatIndividualSortName(SimpleName name)
        {
            StringBuilder sb = new StringBuilder();
            string format = StringHelper.FirstNonBlank(name.NameFormat, name.DefaultNameFormat).ToUpper();

            if (!string.IsNullOrWhiteSpace(name.LastName))
            {
                if (format.Contains("{LI}"))
                {
                    sb.Append(name.LastName.Substring(0, 1)).Append(". ");
                }
                else if (format.Contains("{L}") || format.Contains("{LAST}"))
                {
                    sb.Append(name.LastName).Append(' ');
                }
            }

            if (!string.IsNullOrWhiteSpace(name.FirstName))
            {
                if (format.Contains("{FI}"))
                {
                    sb.Append(name.FirstName.Substring(0, 1)).Append(". ");
                }
                else if (format.Contains("{F}") || format.Contains("{FIRST}"))
                {
                    sb.Append(name.FirstName).Append(' ');
                }
            }

            if (!string.IsNullOrWhiteSpace(name.MiddleName))
            {
                if (format.Contains("{MI}"))
                {
                    sb.Append(name.MiddleName.Substring(0, 1)).Append(". ");
                }
                else if (format.Contains("{M}") || format.Contains("{MIDDLE}"))
                {
                    sb.Append(name.MiddleName).Append(' ');
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Formats a list of individual names into a single string. 
        /// The format string should reference only one of the names in the list.  By default only the
        /// first name in the list is used.
        /// (Legacy note: Similar to nagenl3.p minus the logic for abbrevation format.)
        /// </summary>
        private string FormatIndividualName(string formatString, bool ignoreNameFormat, string defaultFormat, SimpleName name)
        {
            StringBuilder rslt = new StringBuilder();

            if (string.IsNullOrWhiteSpace(formatString))
            {
                formatString = StringHelper.FirstNonBlank(name.NameFormat, name.DefaultNameFormat);
            }

            /*
            if (formatString.Contains("{D}"))
            {
                if (defaultFormat.IsBlank())
                    defaultFormat = nameList[0].DefaultSalutationFormat;
                formatString = formatString.Replace("{D}", defaultFormat);
            }
            */

            if (!formatString.Contains('{'))
                formatString = formatString + " {NAME}";

            List<string> tokenList = StringHelper.TokenizeFormatString(formatString);
            bool addSpace = false;

            foreach (string token in tokenList)
            {
                if (!token.StartsWith("{") || !token.EndsWith("}"))
                {
                    // Not a macro, so just append this text to the result.
                    rslt.Append(token);
                    addSpace = false;
                    continue;
                }

                string format = token.Trim('{', '}');

                SimpleName thisName = name;

                // Get the individual's name format.
                string nameFormat;
                if (ignoreNameFormat)
                    nameFormat = formatString; // Ignore the individual's name format and always use the passed-in format string.
                else
                    nameFormat = StringHelper.FirstNonBlank(thisName.NameFormat, thisName.DefaultNameFormat);

                // Nickname:  If there's no nickname, use the first name or middle name.
                if (format == "NICKNAME" && string.IsNullOrWhiteSpace(thisName.Nickname))
                {
                    if (nameFormat.Contains("{FI}") && (nameFormat.Contains("{M}") || nameFormat.Contains("{MIDDLE}")) ||
                        string.IsNullOrWhiteSpace(thisName.FirstName))
                        format = "M";
                    else
                        format = "F";
                }

                string namePart = string.Empty;

                // Prefix - minimal support for now.
                if (format == "P" || format == "PREFIX")
                {
                    namePart = thisName.Prefix?.LabelAbbreviation ?? string.Empty;
                }

                // First name
                else if (format == "FI" || format == "F" || format == "FIRST")
                {
                    if (!string.IsNullOrWhiteSpace(thisName.FirstName) && (nameFormat.Contains("{F}") || nameFormat.Contains("{FI}") || nameFormat.Contains("{FIRST}")))
                    {
                        if (format == "FI" || nameFormat.Contains("{FI}"))
                            namePart = Initialize(thisName.FirstName);
                        else
                            namePart = thisName.FirstName;
                    }
                }

                // Middle name
                else if (format == "MI" || format == "M" || format == "MIDDLE")
                {
                    if (!string.IsNullOrWhiteSpace(thisName.MiddleName) && (nameFormat.Contains("{M}") || nameFormat.Contains("{MI}") || nameFormat.Contains("{MIDDLE}")))
                    {
                        if (format == "MI" || nameFormat.Contains("{MI}"))
                            namePart = Initialize(thisName.MiddleName);
                        else
                            namePart = thisName.MiddleName;
                    }
                }

                // Last name
                else if (format == "LI" || format == "L" || format == "LAST")
                {
                    if (!string.IsNullOrWhiteSpace(thisName.LastName) && (nameFormat.Contains("{L}") || nameFormat.Contains("{LI}") || nameFormat.Contains("{LAST}")))
                    {
                        if (format == "LI" || nameFormat.Contains("{LI}"))
                            namePart = Initialize(thisName.LastName);
                        else
                            namePart = thisName.LastName;
                    }
                }
                else if (format == "NICKNAME")
                    namePart = thisName.Nickname;
                else if (format == "SUFFIX" || format == "S")
                {
                    if (!string.IsNullOrWhiteSpace(thisName.Suffix) && (nameFormat.Contains("{S}") || nameFormat.Contains("SUFFIX")))
                        namePart = thisName.Suffix;
                }

                if (namePart.Length > 0)
                {
                    if (addSpace)
                        rslt.Append(' ');
                    rslt.Append(namePart);
                    addSpace = true;
                }

            } // each token

            return rslt.ToString();
        }

        private string FormatIndividualName (string formatString, string shortFormatString, int maxChars, bool ignoreNameFormat, string defaultFormat,  SimpleName name)
        {
            string rslt = FormatIndividualName(formatString, ignoreNameFormat, defaultFormat, name);

            if (maxChars > 0 && rslt.Length > maxChars && !string.IsNullOrWhiteSpace(shortFormatString) && shortFormatString != formatString)
            {
                rslt = FormatIndividualName(shortFormatString, ignoreNameFormat, defaultFormat, name);
            }

            return rslt;
        }

        public void BuildIndividualNameLines(Constituent name1, Constituent name2, LabelRecipient recipient, bool separate, bool omitPrefix, bool addFirstNames, int maxChars, out string line1, out string line2)
        {
            BuildIndividualNameLines(ConvertToSimpleName(name1), ConvertToSimpleName(name2), recipient, separate, omitPrefix, addFirstNames, maxChars, out line1, out line2);
        }

        private void BuildIndividualNameLines(SimpleName name1, SimpleName name2, LabelRecipient recipient, bool separate, bool omitPrefix, bool addFirstNames, int maxChars, out string line1, out string line2)
        {
            bool omitSpousePrefix = omitPrefix;
            bool keepPosition = omitPrefix;
            line1 = line2 = string.Empty;

            if (name2 == null)
                name2 = new SimpleName();


            // Get name formats
            string format = StringHelper.FirstNonBlank(name1.NameFormat, name1.DefaultNameFormat);
            string spouseFormat = StringHelper.FirstNonBlank(name2.NameFormat, name2.DefaultNameFormat);

            // Determine if prefixes are omitted from name formats
            if (!(format.Contains("{P}") || format.Contains("{PREFIX}")))
            {
                omitPrefix = true;
                name1.Prefix = null;
            }

            if (!(spouseFormat.Contains("{P}") || spouseFormat.Contains("{PREFIX}")))
            {
                omitSpousePrefix = true;
                name2.Prefix = null;
            }

            // If no prefixes, keep the position (i.e. don't force the husband first)
            if (name1.Prefix == null && name2.Prefix == null)
            {
                keepPosition = true;
            }

            // If separate names, keep the position
            else if (separate)
            {
                keepPosition = true;
            }

            // Establish default values
            bool hasSpouse = !string.IsNullOrWhiteSpace(name2.LastName);
            string pwork1 = "{F}{M}{L}{S}";
            string pabbr1 = pwork1;
            string pwork2 = string.Empty;
            string pabbr2 = string.Empty;
            int prior1 = 3;
            int prior2 = 3;

            if (!omitPrefix)
            {
                if (name1.Prefix != null)
                {
                    pwork1 = name1.Prefix.LabelPrefix;
                    pabbr1 = StringHelper.FirstNonBlank(name1.Prefix.LabelAbbreviation, pwork1);
                    prior1 = GetPrefixPriority(name1.Prefix);

                    if (name1.Gender == null)
                        name1.Gender = UnitOfWork.GetReference(name1.Prefix, p => p.Gender);
                }
            }

            if (hasSpouse)
            {
                pwork2 = "{F}{M}{L}{S}";
                pabbr2 = pwork2;
                if (!omitSpousePrefix)
                {
                    if (name2.Prefix != null)
                    { 
                        pwork2 = name2.Prefix.LabelPrefix;
                        pabbr2 = StringHelper.FirstNonBlank(name2.Prefix.LabelAbbreviation, pwork2);
                        prior2 = GetPrefixPriority(name2.Prefix);

                        if (name2.Gender == null)
                            name2.Gender = UnitOfWork.GetReference(name2.Prefix, p => p.Gender);

                    }
                }
            }

            // Get basic genders;
            string gender1 = string.Empty;
            string gender2 = string.Empty;

            if (name1.Gender != null)
                gender1 = name1.Gender.IsMasculine == true ? "M" : "F";

            if (name2.Gender != null)
                gender2 = name2.Gender.IsMasculine == true ? "M" : "F";

            // If genders are the same, or if either is blank, force separation.
            if (gender1 == gender2 || string.IsNullOrWhiteSpace(gender1) || string.IsNullOrWhiteSpace(gender2))
                keepPosition = separate = true;

            else if (hasSpouse && gender1 == "M" && gender2 == "F" && name2.Prefix == null)
            {
                // Fix for case where spouse prefix is blank, gender is female, and primary is male:  Convert Ms to Mrs
                if (pwork2.StartsWith("Ms"))
                    pwork2 = "Mrs" + pwork2.Substring(2);
                if (pabbr2.StartsWith("Ms"))
                    pabbr2 = "Mrs" + pabbr2.Substring(2);
            }

            // Handle recipients

            switch (recipient)
            {
                case LabelRecipient.Husband:
                    if (gender1 == "M")
                        pwork2 = string.Empty;
                    else if (hasSpouse && gender2 == "M")
                        pwork1 = string.Empty;
                    break;

                case LabelRecipient.Wife:
                    if (gender1 == "F")
                        pwork2 = string.Empty;
                    else if (hasSpouse && gender2 == "F")
                        pwork1 = string.Empty;
                    break;
                case LabelRecipient.Primary:
                    pwork2 = string.Empty;
                    break;
                case LabelRecipient.Spouse:
                    pwork1 = string.Empty;
                    break;
            }


            // If there is no primary, the spouse becomes primary
            if (string.IsNullOrWhiteSpace(pwork1))
            {
                pwork1 = pwork2;
                pabbr1 = pabbr2;
                prior1 = prior2;
                pwork2 = string.Empty;
                name1 = new SimpleName(name2);
            }


            // Bail if there's no longer anything to format.
            if (string.IsNullOrWhiteSpace(pwork1))
                return;

            // Single label's name
            if (string.IsNullOrWhiteSpace(pwork2))
            {
                line1 = FormatIndividualName(pwork1, pabbr1, maxChars, false, format, name1);
                return;
            }



        }

        private int GetPrefixPriority(Prefix prefix)
        {
            if (prefix == null)
                return 9;
            string code = prefix.Code.ToUpper();

            if (code == "MRS" || code == "MS" || code == "MISS" || code == "MMLE" || code == "SISTER")
                return 4;

            if (prefix.LabelPrefix.StartsWith("The"))
                return 2;
            if (prefix.LabelPrefix.Contains('{') || prefix.Salutation.Contains('{'))
                return 1;

            return 3;
        }

        private SimpleName ConvertToSimpleName (Constituent name)
        {
            SimpleName simpleName = new SimpleName();
            if (name != null)
            {
                simpleName.Prefix = UnitOfWork.GetReference(name, p => p.Prefix);
                simpleName.FirstName = name.FirstName ?? string.Empty;
                simpleName.MiddleName = name.MiddleName ?? string.Empty;
                simpleName.LastName = name.LastName ?? string.Empty;
                simpleName.Suffix = name.Suffix ?? string.Empty;
                simpleName.Nickname = name.Nickname ?? string.Empty;
                simpleName.NameFormat = name.NameFormat ?? string.Empty;

                simpleName.Gender = UnitOfWork.GetReference(name, p => p.Gender);
                ConstituentType ctype = UnitOfWork.GetReference(name, p => p.ConstituentType);

                simpleName.DefaultNameFormat = StringHelper.FirstNonBlank(ctype?.NameFormat, _defaultIndividualNameFormat);
            }
            return simpleName;
        }

        private class SimpleName
        {
            public Prefix Prefix { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string Suffix { get; set; }
            public string Nickname { get; set; }
            public string NameFormat { get; set; }
            public Gender Gender { get; set; }
            public string DefaultNameFormat { get; set; }
            public string DefaultSalutationFormat { get; set; }

            public SimpleName()
            {
                FirstName = MiddleName = LastName = Suffix = Nickname = NameFormat = string.Empty;
                Prefix = null;
                Gender = null;
                DefaultNameFormat = DEFAULT_NAME_FORMAT;                
            }

            public SimpleName(SimpleName other)
            {
                Prefix = other.Prefix;
                FirstName = other.FirstName;
                MiddleName = other.MiddleName;
                LastName = other.LastName;
                Suffix = other.Suffix;
                Nickname = other.Nickname;
                NameFormat = other.NameFormat;
                Gender = other.Gender;
                DefaultNameFormat = other.DefaultNameFormat;
                DefaultSalutationFormat = other.DefaultSalutationFormat;
            }

        }
    }
}


    #endregion

