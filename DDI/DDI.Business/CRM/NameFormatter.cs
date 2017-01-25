using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Core;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.CRM
{
    /// <summary>
    /// Constituent name formatting business logic
    /// </summary>
    public class NameFormatter
    {
        private const string MALE = "M";
        private const string FEMALE = "F";

        private const string OPEN_PAREN_CHARS = "([{";

        // These macro abbreviations are for backwards compatibility.
        private const string PREFIX_ABBREVIATION = "P";
        private const string FIRST_NAME_ABBREVIATION = "F";
        private const string MIDDLE_NAME_ABBREVIATION = "M";
        private const string LAST_NAME_ABBREVIATION = "L";
        private const string SUFFIX_ABBREVIATION = "S";
        private const string NICKNAME_ABBREVIATION = "N";

        private enum Macro
        {
            None = 0, Prefix, First, Middle, Last, FI, MI, LI, Suffix, Nickname, Name, Mr, Madam, Brother, His, And, Unknown
        }

        private enum MacroNamePart
        {
            None = 0, First, Middle, Last, Suffix, All
        }

        // Prefix priorities
        private enum PrefixPriority
        {
            High = 1, Medium = 2, Default = 3, FemaleDefault = 4, Undefined = 9
        }

        private string _defaultIndividualNameFormat;
  
        // Dictionary for caching format strings and their tokenization
        private static Dictionary<Tuple<string,bool>, IList<Token>> _tokenCache = new Dictionary<Tuple<string, bool>, IList<Token>>();

        // Max number of entries in _tokenCache
        private const int TOKEN_CACHE_LIMIT = 5000;

        #region Constructors 

        public NameFormatter() : this(new UnitOfWorkEF()) { }

        public NameFormatter(IUnitOfWork uow)
        {
            UnitOfWork = uow;
            uow.AddBusinessLogic(this);

            var individualType = uow.FirstOrDefault<ConstituentType>(p => p.Code == ConstituentTypeCodes.Individual);

            string defaultFormat = $"{{{NameFormatMacros.Prefix}}}{{{NameFormatMacros.FirstName}}}{{{NameFormatMacros.MiddleInitial}}}{{{NameFormatMacros.LastName}}}{{{NameFormatMacros.Suffix}}}";

            // Default individual name format comes from Individual constituent type - or if blank, the hard-coded defaultFormat.
            _defaultIndividualNameFormat = StringHelper.FirstNonBlank(individualType?.NameFormat, defaultFormat);
        }

        #endregion

        #region Public Properties

        public IUnitOfWork UnitOfWork { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Format a sortable name for an individual:  Last First Middle
        /// </summary>
        public string FormatIndividualSortName(Constituent constituent)
        {
            return FormatIndividualSortName(ConvertToPerson(constituent));
        }

        /// <summary>
        /// Formats an individual name into a single string.
        /// </summary>
        public string FormatIndividualName(Constituent constituent, string formatPattern)
        {
            return FormatIndividualName(ConvertToPerson(constituent), formatPattern);
        }

        /// <summary>
        /// Get name lines for one or two individual constituents.
        /// </summary>
        public void GetIndividualNameLines(Constituent constituent1, Constituent constituent2, LabelRecipient recipient, bool separate, bool omitPrefix, bool addFirstNames, int maxChars, out string line1, out string line2)
        {
            FormatIndividualNameLines(ConvertToPerson(constituent1), ConvertToPerson(constituent2), recipient, separate, omitPrefix, addFirstNames, maxChars, out line1, out line2);
        }


        /// <summary>
        /// Get name lines for one or two constituents.
        /// </summary>
        public void GetNameLines(Constituent constituent1, Constituent constituent2, NameFormattingOptions options, out string line1, out string line2)
        {
            if (options == null)
            {
                options = new LabelFormattingOptions();
            }

            Constituent spouse = null;
            LabelRecipient recipient = options.Recipient;
            CRMConfiguration configuration = GetCRMConfiguration();

            ConstituentLogic constituentLogic = UnitOfWork.GetBusinessLogic<ConstituentLogic>();
            line1 = line2 = string.Empty;

            // For organizations, just return the constituent name.
            ConstituentType type = constituent1.ConstituentType ?? UnitOfWork.GetReference(constituent1, p => p.ConstituentType);
            if (type == null || type.Category == ConstituentCategory.Organization)
            {
                line1 = constituent1.Name;
                line2 = constituent1.Name2 ?? string.Empty;
                return;
            }

            bool omitInactiveSpouse = configuration.OmitInactiveSpouse && !options.IncludeInactive;

            // Keep spouse separate based on salutation format.
            bool keepSeparate = (constituent1.SalutationType == SalutationType.FormalSeparate || constituent1.SalutationType == SalutationType.InformalSeparate || options.KeepSeparate);

            if (options.IsSpouse)
            {
                spouse = constituent2;
            }
            else if (constituent2 != null || recipient != LabelRecipient.Primary)
            {
                spouse = constituentLogic.GetSpouse(constituent1);
            }

            if (spouse != null)
            {
                if (omitInactiveSpouse && constituentLogic.IsConstituentActive(spouse) == false)
                {
                    // Spouse is inactive and should be omitted.
                    if (constituent2 != null && spouse.Id == constituent2.Id)
                    {
                        // Since name2 is the spouse, set it to null.
                        constituent2 = null;
                    }
                    spouse = null;
                }
                // If primary & spouse have different last names...
                else if (options.IsSpouse && string.Compare(constituent1.LastName, spouse.LastName, true) != 0)
                {
                    // ... treat const2 as a separate constituent and not a spouse.
                    recipient = LabelRecipient.Primary;
                    spouse = null;
                }
                else
                {
                    // Keep spouse separate based on salultation format.
                    keepSeparate = keepSeparate ||
                        (spouse.SalutationType == SalutationType.FormalSeparate || spouse.SalutationType == SalutationType.InformalSeparate);
                }
            }
            
            // If spouse is name2, ignore name2 and change recipient to "Both".
            if (spouse != null && constituent2 != null && spouse.Id == constituent2.Id)
            {
                constituent2 = null;
                if (recipient == LabelRecipient.Primary)
                {
                    recipient = LabelRecipient.Both;
                }
            }

            bool addFirstNames = options.AddFirstNames || configuration.AddFirstNamesToSpouses;

            // Use GetIndividualNames to get the name lines for the individual and spouse (which may be null)
            GetIndividualNameLines(constituent1, spouse, recipient, keepSeparate, options.OmitPrefix, addFirstNames, options.MaxChars, out line1, out line2);

            // Try to add the second name line if we can
            if (string.IsNullOrWhiteSpace(line2))
            {
                if (constituent2 != null)
                {
                    ConstituentType type2 = constituent2.ConstituentType ?? UnitOfWork.GetReference(constituent2, p => p.ConstituentType);
                    if (type2 == null || type.Category == ConstituentCategory.Organization)
                    {
                        line2 = constituent2.Name;
                    }
                    else
                    {
                        string temp;
                        GetIndividualNameLines(constituent2, null, LabelRecipient.Primary, false, options.OmitPrefix, addFirstNames, options.MaxChars, out line2, out temp);
                    }
                }
                else
                {
                    line2 = constituent1.Name2;
                }
            }

            // Ensure returned strings aren't null.
            line1 = line1 ?? string.Empty;
            line2 = line2 ?? string.Empty;
        }

        /// <summary>
        /// Get an address label for one or two constituents.
        /// </summary>
        public List<string> GetAddressLabel(Constituent constituent1, Constituent constituent2, Address address, LabelFormattingOptions options, bool shouldDisplayContactInfo = false)
        {
            List<string> label = new List<string>();
            string line1, line2;

            if (options == null)
            {
                options = new LabelFormattingOptions();
            }

            AddressCategory addressCategory = options.AddressCategory;
            string contactName = options.ContactName;
            string nameLine2 = string.Empty;

            // If options specify a specific address type code, force address category to None.
            if (!string.IsNullOrWhiteSpace(options.AddressTypeCode))
            {
                addressCategory = AddressCategory.None;
            }

            // Name formatting

            if (constituent1 != null) // Constituent's are optional - the label can consist of only an address.
            {
                GetNameLines(constituent1, constituent2, options, out line1, out line2);

                // Remove name2 from the result if it's in Line2.
                if (!string.IsNullOrWhiteSpace(constituent1.Name2) && string.Compare(line2, constituent1.Name2, true) == 0)
                {
                    line2 = string.Empty;
                }

                nameLine2 = constituent1.Name2 ?? string.Empty;

                if (options.Caps)
                {
                    line1 = line1.ToUpper();
                    line2 = line2.ToUpper();
                    nameLine2 = nameLine2.ToUpper();
                }
                if (options.ExpandName)
                {
                    if (!string.IsNullOrWhiteSpace(line1))
                    {
                        line1 = AbbreviationHelper.ExpandNameLine(line1, true);
                    }
                    if (!string.IsNullOrWhiteSpace(line2))
                    {
                        line2 = AbbreviationHelper.ExpandNameLine(line2, true);
                    }
                    if (!string.IsNullOrWhiteSpace(nameLine2))
                    {
                        nameLine2 = AbbreviationHelper.ExpandNameLine(nameLine2, true);
                    }
                }

                if (!string.IsNullOrWhiteSpace(line1))
                {
                    label.Add(line1);
                }

                if (!string.IsNullOrWhiteSpace(line2))
                {
                    label.Add(line2);
                }

                if (!string.IsNullOrWhiteSpace(contactName) && options.Caps)
                {
                    contactName = contactName.ToUpper();
                }

                // Get the address if it wasn't passed in
                if (address == null)
                {
                    ConstituentAddress constituentAddress =
                        UnitOfWork.GetBusinessLogic<ConstituentAddressLogic>().GetAddress(constituent1, addressCategory, options.AddressTypeCode, options.AllowVacationAddress, true, null, null);
                    if (constituentAddress != null)
                    {
                        address = UnitOfWork.GetReference(constituentAddress, p => p.Address);
                    }
                }
            }

            // Address formatting

            string[] addressLines = null;
            if (address != null)
            {
                string addrText = UnitOfWork.GetBusinessLogic<AddressLogic>().FormatAddress(address, options.Caps, options.ExpandAddress, options.MaxChars);
                addressLines = addrText.Split('\n');
            }

            int addressLength = (addressLines == null ? 0 : addressLines.Length);

            // Reserve a line for the contact name
            if (!string.IsNullOrWhiteSpace(contactName))
            {
                addressLength++;
            }

            // Add the constituent's Name2 line (if there's room)
            if (!string.IsNullOrWhiteSpace(nameLine2) && (options.MaxLines == 0 || (options.MaxLines > 0 && label.Count + addressLength + 1 <= options.MaxLines)))
            {
                label.Add(nameLine2);
            }

            // Add the contact name
            if (!string.IsNullOrWhiteSpace(contactName))
            {
                label.Add(contactName);
            }

            // Add the address lines
            if (addressLines != null)
            {
                label.AddRange(addressLines);
            }

            // Contact info
            if (shouldDisplayContactInfo && constituent1 != null)
            {                
                foreach (var entry in UnitOfWork.GetReference(constituent1, p => p.ContactInfo))
                {
                    label.Add(entry.Info);
                }
            }
            
            // Remove any extra lines if there are too many
            if (options.MaxLines > 0 && label.Count > options.MaxLines)
            {
                int toRemove = label.Count - options.MaxLines;
                label.RemoveRange(options.MaxLines - 1, toRemove);
            }

            return label;

        }

        /// <summary>
        /// Get a salutation for a constituent.
        /// </summary>
        public string GetSalutation(Constituent constituent, SalutationFormattingOptions options)
        {
            string result = string.Empty;

            if (constituent == null)
            {
                return string.Empty;
            }

            if (options == null)
            {
                options = new SalutationFormattingOptions();
            }

            // Load the constituent type and CRM configuration.

            ConstituentType constituentType = UnitOfWork.GetReference(constituent, p => p.ConstituentType);
            CRMConfiguration configuration = GetCRMConfiguration();
            SalutationType systemDefaultType = configuration.DefaultSalutationType;

            SalutationType salutationType = options.PreferredType;
            
            // Determine if salutation type should be formal or informal
            if (!options.ForcePreferredtype)
            {
                // Not forcing a particular type, so defer to the constituent's salutation type.
                salutationType = constituent.SalutationType;

                if (salutationType == SalutationType.Default)
                {
                    // Constituent has no specific type, so defer to the preferred type
                    salutationType = options.PreferredType;

                    if (salutationType == SalutationType.Default)
                    {
                        // No preferred type, so use the default type
                        salutationType = systemDefaultType; 
                    }
                }
            }

            // Determine formal/informal and final punctuation character.
            char finalChar = ':';
            bool isFormal = true;

            // If informal salutation is requested, use informal formatting.
            if (salutationType == SalutationType.Informal || salutationType == SalutationType.InformalSeparate)
            {
                isFormal = false;
                finalChar = ',';
            }
            // Else if doing a custom salutation, check to see if the system default type is informal.  If so, use informal formatting.
            else if ((salutationType == SalutationType.Custom || salutationType == SalutationType.Default) && 
                     (systemDefaultType == SalutationType.Informal || systemDefaultType == SalutationType.InformalSeparate))
            {
                isFormal = false;
                finalChar = ',';
            }

            // Get salutation format.  
            string salutationFormat;
            string fixedFormat = string.Empty;
            if (!string.IsNullOrWhiteSpace(options.CustomSalutation))
            {
                // If a custom format was passed in, force the salutation type to CustomSalutation.
                salutationFormat = options.CustomSalutation;
                salutationType = SalutationType.Custom;
                fixedFormat = salutationFormat;
            }
            else
            {
                if (constituent.SalutationType == SalutationType.Custom && !string.IsNullOrWhiteSpace(constituent.Salutation))
                {
                    // Constituent has a custom salutation format.
                    salutationType = SalutationType.Custom;
                    fixedFormat = constituent.Salutation;
                }
             
                // Get the salutation format based on constituent type and salutation type.
                salutationFormat = isFormal ? constituentType.SalutationFormal : constituentType.SalutationInformal;
             
            }
            // For non-individuals, the salutation format is the final salutation.
            if (constituentType.Category == ConstituentCategory.Organization)
            {
                // Any fixed format takes precedence.
                if (!string.IsNullOrWhiteSpace(fixedFormat))
                {
                    salutationFormat = fixedFormat;
                }

                // There might be macros?
                if (salutationFormat.Contains("{"))
                {
                    // Tokenize the format, and replace name macros with the organization name.
                    IList<Token> tokens = TokenizeFormatString(salutationFormat);
                    result = FormatOrganizationName(constituent.Name, tokens);
                }
                else
                {
                    // No macros, so it's a fixed salutation.
                    result = salutationFormat;
                }

                result = result.Trim();

                if (result.Length > 0 && !char.IsPunctuation(result.Last()))
                { 
                    // If last char isn't a punctuation char, add the final colon or comma.
                    result += finalChar;
                }

                return result;
            }

            // Individual salutations are more complex.

            // If there's a custom salutation and it contains no macros, that's the salutation.
            if (!string.IsNullOrWhiteSpace(options.CustomSalutation) && !options.CustomSalutation.Contains("{"))
            {
                result = options.CustomSalutation.Trim();
                if (result.Length > 0 && !char.IsPunctuation(result.Last()))
                {
                    // If last char isn't a punctuation char, add the final colon or comma.
                    result += finalChar;
                }

                return result;
            }

            Constituent spouse = null;
            ConstituentLogic constituentLogic = UnitOfWork.GetBusinessLogic<ConstituentLogic>();
            string spouseFixedFormat = string.Empty;

            if (options.Recipient != LabelRecipient.Primary)
            {
                // If recipient is not Primary, we need to grab the spouse.
                spouse = constituentLogic.GetSpouse(constituent);

                // If spouse is inactive and options don't specify including inactive spouse and settings specify omit inactive spouse, then omit the spouse.
                if (spouse != null && !constituentLogic.IsConstituentActive(spouse) && !options.IncludeInactive && configuration.OmitInactiveSpouse)
                {
                    spouse = null;
                }
                else if (spouse != null && spouse.SalutationType == SalutationType.Custom && !string.IsNullOrWhiteSpace(spouse.Salutation))
                {
                    // Spouse has a custom salutation.
                    spouseFixedFormat = spouse.Salutation;
                }
            }
            
            // Call BuildIndividualSalutation to do remaining work.
            result = FormatIndividualSalutation(constituent, spouse, salutationType, fixedFormat, salutationFormat, spouseFixedFormat, isFormal, options, configuration);

            result = result.Trim();

            if (result.Length > 0 && !char.IsPunctuation(result.Last()))
            {
                // If last char isn't a punctuation char, add the final colon or comma.
                result += finalChar;
            }

            return result;
        }

        #endregion

        #region Private Methods

        private CRMConfiguration GetCRMConfiguration()
        {
            var configurationLogic = UnitOfWork.GetBusinessLogic<ConfigurationLogic>();
            return configurationLogic.GetConfiguration<CRMConfiguration>();
        }

        /// <summary>
        /// Format a sortable name for an individual:  Last First Middle
        /// </summary>
        private string FormatIndividualSortName(Person person)
        {
            StringBuilder sb = new StringBuilder();
            string format = StringHelper.FirstNonBlank(person.NameFormat, person.DefaultNameFormat);

            var tokens = TokenizeFormatString(format);

            if (!string.IsNullOrWhiteSpace(person.LastName))
            {
                if (tokens.Any(p => p.Macro == Macro.LI))
                {
                    sb.Append(person.LastName.Substring(0, 1)).Append(". ");
                }
                if (tokens.Any(p => p.Macro == Macro.Last))
                {
                    sb.Append(person.LastName).Append(' ');
                }
            }

            if (!string.IsNullOrWhiteSpace(person.FirstName))
            {
                if (tokens.Any(p => p.Macro == Macro.FI))
                {
                    sb.Append(person.FirstName.Substring(0, 1)).Append(". ");
                }
                else if (tokens.Any(p => p.Macro == Macro.First))
                {
                    sb.Append(person.FirstName).Append(' ');
                }
                else if (tokens.Any(p => p.Macro == Macro.Nickname))
                {
                    sb.Append(StringHelper.FirstNonBlank(person.Nickname, person.FirstName)).Append(' ');
                }
            }

            if (!string.IsNullOrWhiteSpace(person.MiddleName))
            {
                if (tokens.Any(p => p.Macro == Macro.MI))
                {
                    sb.Append(person.MiddleName.Substring(0, 1)).Append(".");
                }
                else if (tokens.Any(p => p.Macro == Macro.Middle))
                {
                    sb.Append(person.MiddleName);
                }
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Merge a set of format tokens into a set of pattern tokens.
        /// </summary>
        private IList<Token> MergeMacros(IList<Token> patternTokens, IList<Token> formatTokens)
        {
            if (IsTokenListEmpty(formatTokens))
            {
                return new List<Token>(patternTokens);
            }

            IList<Token> mergeResult = new List<Token>();

            // Merge the macros for the name format into the pattern format.
            foreach (var token in patternTokens)
            {
                switch (token.Macro)
                {
                    case Macro.Name:
                        {
                            bool first = true;
                            foreach (var other in formatTokens.Where(p => p.NamePart != MacroNamePart.None))
                            {
                                if (first)
                                {
                                    mergeResult.Add(new Token(other));
                                    first = false;
                                }
                                else
                                {
                                    mergeResult.Add(new Token(other));
                                }
                            }
                        }
                        break;
                    case Macro.First:
                    case Macro.FI:
                        {
                            Token other = formatTokens.FirstOrDefault(p => p.NamePart == MacroNamePart.First);
                            if (other != null)
                            {
                                mergeResult.Add(new Token(other));
                            }
                        }
                        break;
                    case Macro.Middle:
                    case Macro.MI:
                        {
                            Token other = formatTokens.FirstOrDefault(p => p.NamePart == MacroNamePart.Middle);
                            if (other != null)
                            {
                                mergeResult.Add(new Token(other));
                            }
                        }
                        break;
                    case Macro.Last:
                    case Macro.LI:
                        {
                            Token other = formatTokens.FirstOrDefault(p => p.NamePart == MacroNamePart.Last);
                            if (other != null)
                            {
                                mergeResult.Add(new Token(other));
                            }
                        }
                        break;
                    case Macro.Suffix:
                        {
                            Token other = formatTokens.FirstOrDefault(p => p.NamePart == MacroNamePart.Suffix);
                            if (other != null)
                            {
                                mergeResult.Add(new Token(other));
                            }
                        }
                        break;
                    case Macro.Nickname:
                        {
                            if (formatTokens.Any(p => p.NamePart == MacroNamePart.First))
                            {
                                mergeResult.Add(new Token(token));
                            }
                        }
                        break;
                    default:
                        mergeResult.Add(new Token(token));
                        break;
                }
            }

            return mergeResult;
        }

        /// <summary>
        /// Format an organization name line, performing macro substitution.
        /// </summary>
        /// <returns></returns>
        private string FormatOrganizationName(string nameText, IList<Token> formatTokens)
        {
            StringBuilder result = new StringBuilder();

            if (IsTokenListEmpty(formatTokens))
            {
                return nameText;
            }

            foreach (Token token in formatTokens)
            {
                string namePart = string.Empty;

                // Only a few macros are supported for organization names.
                switch (token.Macro)
                {
                    case Macro.Name:
                        namePart = nameText;
                        break;
                    case Macro.And:
                        namePart = "and";
                        break;
                    case Macro.None:
                        namePart = token.Text;
                        break;
                }

                namePart = namePart.Trim();

                if (!string.IsNullOrEmpty(namePart))
                {
                    if (result.Length > 0 && !char.IsPunctuation(namePart[0]))
                    {
                        // Append a space betweeen tokens unless this token starts with punctuation.
                        result.Append(' ');
                    }
                    result.Append(namePart);
                }

            } // Each token

            return result.ToString();

        }

        /// <summary>
        /// Formats an individual name into a single string.
        /// </summary>
        private string FormatIndividualName(Person person, IList<Token> formatTokens, IList<Token> patternTokens, params string[] prefixText)
        {
            StringBuilder result = new StringBuilder();
            int parameterCount = prefixText.Length;
            int parameterNumber = 0;

            if (!IsTokenListEmpty(patternTokens))
            {
                formatTokens = MergeMacros(patternTokens, formatTokens);
            }

            if (IsTokenListEmpty(formatTokens))
            {
                return string.Empty;
            }

            foreach (Token token in formatTokens)
            {
                string namePart = string.Empty;

                switch (token.Macro)
                {
                    case Macro.Prefix:
                        if (parameterNumber < parameterCount)
                        {
                            namePart = prefixText[parameterNumber++];
                        }
                        break;
                    case Macro.First:
                        namePart = person.FirstName;
                        break;
                    case Macro.FI:
                        namePart = ConvertNameToInitial(person.FirstName);
                        break;
                    case Macro.Middle:
                        namePart = person.MiddleName;
                        break;
                    case Macro.MI:
                        namePart = ConvertNameToInitial(person.MiddleName);
                        break;
                    case Macro.Last:
                        namePart = person.LastName;
                        break;
                    case Macro.LI:
                        namePart = ConvertNameToInitial(person.LastName);
                        break;
                    case Macro.Suffix:
                        namePart = person.Suffix;
                        break;
                    case Macro.Nickname:
                        if (!string.IsNullOrWhiteSpace(person.Nickname))
                        {
                            namePart = person.Nickname;
                        }
                        else if (!string.IsNullOrWhiteSpace(person.FirstName))
                        {
                            if (!formatTokens.Any(p => p.Macro == Macro.First || p.Macro == Macro.FI))
                            {
                                namePart = person.FirstName;
                            }
                        }
                        else if (!string.IsNullOrWhiteSpace(person.MiddleName))
                        {
                            if (!formatTokens.Any(p => p.NamePart == MacroNamePart.Middle))
                            {
                                namePart = person.MiddleName;
                            }
                        }
                        break;
                    case Macro.Brother:
                        namePart = (person.Gender?.IsMasculine == false) ? "Sister" : "Brother";
                        break;
                    case Macro.His:
                        namePart = (person.Gender?.IsMasculine == false) ? "Her" : "His";
                        break;
                    case Macro.Madam:
                        namePart = (person.Gender?.IsMasculine == false) ? "Madam" : "Mr.";
                        break;
                    case Macro.Mr:
                        namePart = (person.Gender?.IsMasculine == false) ? "Ms." : "Mr.";
                        break;
                    case Macro.And:
                        namePart = "and";
                        break;
                    case Macro.None:
                        namePart = token.Text;
                        break;
                }

                namePart = namePart?.Trim();

                if (!string.IsNullOrEmpty(namePart))
                {
                    if (result.Length > 0 && (OPEN_PAREN_CHARS.IndexOf(namePart[0]) >= 0 || !char.IsPunctuation(namePart[0])))
                    {
                        char lastChar = result[result.Length - 1];
                        if (OPEN_PAREN_CHARS.IndexOf(lastChar) < 0)
                        {
                            // Append a space betweeen tokens unless this token starts with punctuation.
                            result.Append(' ');
                        }
                    }
                    result.Append(namePart);
                }

            } // Each token

            return result.ToString();
        }

        /// <summary>
        /// Formats an individual name into a single string.
        /// </summary>
        private string FormatIndividualName(Person person, IList<Token> formatTokens, IList<Token> patternTokens, IList<Token> shortPatternTokens, int maxChars)
        {
            string result = FormatIndividualName(person, formatTokens, patternTokens);

            if (maxChars > 0 && result.Length > maxChars && shortPatternTokens != null)
            {
                result = FormatIndividualName(person, formatTokens, shortPatternTokens);
            }

            return result;
        }

        /// <summary>
        /// Formats an individual name into a single string.
        /// </summary>
        private string FormatIndividualName(Person person, string formatPattern)
        {
            string nameFormat = StringHelper.FirstNonBlank(person.NameFormat, person.DefaultNameFormat);

            return FormatIndividualName(person, TokenizeFormatString(nameFormat), TokenizeFormatString(formatPattern));
        }

        /// <summary>
        /// Get name lines for one or two individual constituents.
        /// </summary>
        private void FormatIndividualNameLines(Person person1, Person person2, LabelRecipient recipient, bool separate, bool omitPrefix, bool addFirstNames, int maxChars, out string line1, out string line2)
        {
            bool omitSpousePrefix = omitPrefix;
            bool keepPosition = omitPrefix;
            line1 = line2 = string.Empty;

            // NOTE:  The logic in FormatIndividualNameLines is similar to logic in FormatIndividualSalutation, but there are enough differences
            // to keep these methods separate.  Nevertheless, changes to one of these methods might require changes to the the other.

            if (person2 == null)
            {
                person2 = new Person();
            }

            // Get name formats
            string format = StringHelper.FirstNonBlank(person1.NameFormat, person1.DefaultNameFormat);
            string spouseFormat = StringHelper.FirstNonBlank(person2.NameFormat, person2.DefaultNameFormat);

            IList<Token> formatTokens = TokenizeFormatString(format);
            IList<Token> spouseFormatTokens = TokenizeFormatString(spouseFormat);

            // Determine if prefixes are omitted from name formats
            if (!formatTokens.Any(p => p.Macro == Macro.Prefix))
            {
                omitPrefix = true;
                person1.Prefix = null;
            }

            if (!spouseFormatTokens.Any(p => p.Macro == Macro.Prefix))
            {
                omitSpousePrefix = true;
                person2.Prefix = null;
            }

            // If no prefixes, keep the position (i.e. don't force the husband first)
            if (person1.Prefix == null && person2.Prefix == null)
            {
                keepPosition = true;
            }

            // If separate names, keep the position
            else if (separate)
            {
                keepPosition = true;
            }

            // Establish default values
            bool hasSpouse = !string.IsNullOrWhiteSpace(person2.LastName);
            bool useDefaultPrefix = false;
            bool spouseUseDefaultPrefix = false;

            if (person1.Prefix == null && (person1.Gender?.IsMasculine).HasValue)
            {
                string code = person1.Gender.IsMasculine == true ? "Mr" : "Ms";
                person1.Prefix = UnitOfWork.FirstOrDefault<Prefix>(p => p.Code == code);
                useDefaultPrefix = true;
            }

            if (person2.Prefix == null && (person2.Gender?.IsMasculine).HasValue)
            {
                string code = person2.Gender.IsMasculine == true ? "Mr" : "Ms";
                person2.Prefix = UnitOfWork.FirstOrDefault<Prefix>(p => p.Code == code);
                spouseUseDefaultPrefix = true;
            }

            IList<Token> patternTokens = null;
            IList<Token> spousePatternTokens = null;
            IList<Token> shortPatternTokens = null;
            IList<Token> spouseShortPatternTokens = null;

            PrefixPriority priority1 = PrefixPriority.Default;
            PrefixPriority priority2 = PrefixPriority.Default;

            if (!omitPrefix && person1.Prefix != null)
            {

                patternTokens = TokenizeFormatString(person1.Prefix.LabelPrefix, true);
                shortPatternTokens = TokenizeFormatString(person1.Prefix.LabelAbbreviation, true);
                priority1 = GetPrefixPriority(person1.Prefix, false);

                if (person1.Gender == null)
                {
                    person1.Gender = UnitOfWork.GetReference(person1.Prefix, p => p.Gender);
                }
            }

            if (hasSpouse)
            {
                if (!omitSpousePrefix && person2.Prefix != null)
                {
                    spousePatternTokens = TokenizeFormatString(person2.Prefix.LabelPrefix, true);
                    spouseShortPatternTokens = TokenizeFormatString(person2.Prefix.LabelAbbreviation, true);
                    priority2 = GetPrefixPriority(person2.Prefix, false);

                    if (person2.Gender == null)
                    {
                        person2.Gender = UnitOfWork.GetReference(person2.Prefix, p => p.Gender);
                    }
                }
            }

            // Get basic genders;
            string gender1 = string.Empty;
            string gender2 = string.Empty;

            if (person1.Gender != null)
            {
                gender1 = person1.Gender.IsMasculine == true ? MALE : FEMALE;
            }

            if (person2.Gender != null)
            {
                gender2 = person2.Gender.IsMasculine == true ? MALE : FEMALE;
            }

            // If genders are the same, or if either is blank, force separation.
            if (gender1 == gender2 || string.IsNullOrWhiteSpace(gender1) || string.IsNullOrWhiteSpace(gender2))
            {
                keepPosition = separate = true;
            }

            // Handle recipients

            switch (recipient)
            {
                case LabelRecipient.Husband:
                    if (gender1 == MALE)
                    {
                        spouseFormatTokens = null;
                        spousePatternTokens = null;
                    }
                    else if (hasSpouse && gender2 == MALE)
                    {
                        formatTokens = null;
                        patternTokens = null;
                    }
                    break;

                case LabelRecipient.Wife:
                    if (gender1 == FEMALE)
                    {
                        spouseFormatTokens = null;
                        spousePatternTokens = null;
                    }
                    else if (hasSpouse && gender2 == FEMALE)
                    {
                        formatTokens = null;
                        patternTokens = null;
                    }
                    break;
                case LabelRecipient.Primary:
                    spouseFormatTokens = null;
                    spousePatternTokens = null;
                    break;
                case LabelRecipient.Secondary:
                    formatTokens = null;
                    patternTokens = null;
                    break;
            }


            // If there is no primary, the spouse becomes primary
            if (IsTokenListEmpty(patternTokens) && IsTokenListEmpty(formatTokens))
            {
                formatTokens = spouseFormatTokens;
                patternTokens = spousePatternTokens;
                shortPatternTokens = spouseShortPatternTokens;
                priority1 = priority2;
                spousePatternTokens = null;
                spouseFormatTokens = null;
                omitPrefix = omitSpousePrefix;
                person1 = new Person(person2);
            }


            // Bail if there's no longer anything to format.
            if (IsTokenListEmpty(formatTokens) && IsTokenListEmpty(patternTokens))
                return;

            // Single label's name
            if (IsTokenListEmpty(spouseFormatTokens) && IsTokenListEmpty(spousePatternTokens))
            {
                line1 = FormatIndividualName(person1, formatTokens, patternTokens, shortPatternTokens, maxChars);
                return;
            }

            bool swapped = false;
            bool combined = false;

            // Couple's label name.

            if (!keepPosition)
            {
                if (priority2 < priority1 ||
                    (priority2 == priority1 && gender2 == MALE && gender1 != MALE))
                {
                    // Swap primary and secondary.

                    Person tempPerson = person1;
                    person1 = person2;
                    person2 = tempPerson;

                    PrefixPriority priort = priority1;
                    priority1 = priority2;
                    priority2 = priort;

                    var tempList = formatTokens;
                    formatTokens = spouseFormatTokens;
                    spouseFormatTokens = tempList;

                    tempList = patternTokens;
                    patternTokens = spousePatternTokens;
                    spousePatternTokens = tempList;

                    tempList = shortPatternTokens;
                    shortPatternTokens = spouseShortPatternTokens;
                    spouseShortPatternTokens = tempList;

                    bool tempBool = omitPrefix;
                    omitPrefix = omitSpousePrefix;
                    omitSpousePrefix = tempBool;

                    tempBool = useDefaultPrefix;
                    useDefaultPrefix = spouseUseDefaultPrefix;
                    spouseUseDefaultPrefix = tempBool;

                    swapped = true;
                }
            }

            // Combine logic where prefixes are identical
            if (!IsTokenListEmpty(patternTokens) && !IsTokenListEmpty(spousePatternTokens) && !separate &&
                person1.Prefix != null && person2.Prefix != null &&
                person1.Prefix.Id == person2.Prefix.Id &&  // Prefixes identical
                !string.IsNullOrWhiteSpace(person1.LastName) && // Primary has a last name
                string.Compare(person1.LastName, person2.LastName, true) == 0 && // Last names identical
                formatTokens.Any(p => p.NamePart == MacroNamePart.Last) &&   // Primary has last name in format
                spouseFormatTokens.Any(p => p.NamePart == MacroNamePart.Last)) // Secondary has last name in format

            {
                // Try to find a plural version of this prefix, e.g. "Drs".
                Prefix prefix = UnitOfWork.FirstOrDefault<Prefix>(p => p.Code == person1.Prefix.Code + "s");
                if (prefix != null)
                {
                    // Load the new pattern
                    var newPatternTokens = TokenizeFormatString(prefix.LabelPrefix, true);
                    var newShortPatternTokens = TokenizeFormatString(prefix.LabelAbbreviation, true);

                    if (newPatternTokens.Any(p => p.NamePart == MacroNamePart.All))
                    {
                        patternTokens = FormatPluralPrefix(newPatternTokens, formatTokens, spouseFormatTokens, out spousePatternTokens);
                        shortPatternTokens = FormatPluralPrefix(newShortPatternTokens, formatTokens, spouseFormatTokens, out spouseShortPatternTokens);
                        combined = true;
                    }
                }
            }

            if (!combined)
            {
                if (hasSpouse && gender1 == MALE && gender2 == FEMALE && spouseUseDefaultPrefix)
                {
                    // Fix for case where spouse prefix is blank, gender is female, and primary is male:  Convert Ms to Mrs
                    ChangeMsToMrs(spousePatternTokens);
                    ChangeMsToMrs(spouseShortPatternTokens);
                }
            }

            // Combine logic for all other cases.
            if (!combined && !separate &&
                priority1 >= PrefixPriority.Default && priority2 >= PrefixPriority.FemaleDefault &&
                (keepPosition || ((!swapped && gender1 == MALE) || (swapped && gender2 == MALE))) &&
                string.Compare(person1.LastName, person2.LastName, true) == 0 && // Last names must be equal
                !IsTokenListEmpty(patternTokens) && // Primary prefix pattern is non-empty
                patternTokens.Last().NamePart == MacroNamePart.All  // Primary prefix must end with {NAME} or {FULL}
                )
            {
                // Call logic to combine prefix patterns.
                CombinePrefixes(patternTokens, spousePatternTokens, formatTokens, spouseFormatTokens, omitPrefix, omitSpousePrefix, addFirstNames,
                    out patternTokens, out spousePatternTokens);

                CombinePrefixes(shortPatternTokens, spouseShortPatternTokens, formatTokens, spouseFormatTokens, omitPrefix, omitSpousePrefix, addFirstNames,
                    out shortPatternTokens, out spouseShortPatternTokens);

                if (IsTokenListEmpty(spousePatternTokens))
                {
                    spouseFormatTokens = null;
                }

                combined = true;
            }

            if (!(IsTokenListEmpty(patternTokens) && IsTokenListEmpty(formatTokens)))
            {
                line1 = FormatIndividualName(person1, formatTokens, patternTokens, shortPatternTokens, maxChars);
            }

            if (!(IsTokenListEmpty(spousePatternTokens) && IsTokenListEmpty(spouseFormatTokens)))
            {
                line2 = FormatIndividualName(person2, spouseFormatTokens, spousePatternTokens, spouseShortPatternTokens, maxChars);
            }

            if (string.IsNullOrWhiteSpace(line1))
            {
                line1 = line2;
                line2 = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(line2))
            {
                string separator = " ";
                if (!IsTokenListEmpty(patternTokens) && patternTokens.Any(p => p.Macro == Macro.And) ||
                    !IsTokenListEmpty(spousePatternTokens) && spousePatternTokens.Any(p => p.Macro == Macro.And))
                {
                    separator = " ";
                }
                else
                {
                    separator = " and ";
                }

                if (maxChars == 0 || line1.Length + 1 + line2.Length <= maxChars)
                {
                    line1 = line1.TrimEnd(' ') + separator + line2.TrimStart(' ');
                    line2 = string.Empty;
                }
            }

        } 

        private string FormatIndividualSalutation(Constituent person1, Constituent person2, SalutationType salutationType, string fixedFormat, string individualFormat, string spouseFormat, bool isFormal, SalutationFormattingOptions options, CRMConfiguration configuration)
        {
            bool addFirstNames = options.AddFirstNames || configuration.AddFirstNamesToSpouses;
            bool keepSeparate = (salutationType == SalutationType.FormalSeparate || salutationType == SalutationType.InformalSeparate || options.KeepSeparate);
            bool keepPosition = false;
            bool omitPrefix = options.OmitPrefix;
            bool omitSpousePrefix = omitPrefix;

            // NOTE:  The logic in FormatIndividualSalutation is similar to logic in FormatIndividualNameLines, but there are enough differences
            // to keep these methods separate.  Nevertheless, changes to one of these methods might require changes to the the other.

            Person name1 = ConvertToPerson(person1);
            Person name2 = (person2 != null ? ConvertToPerson(person2) : null);

            string nameFormat = StringHelper.FirstNonBlank(name1.NameFormat, name1.DefaultNameFormat);
            string spouseNameFormat = string.Empty;

            if (name2 != null)
            {
                spouseNameFormat = StringHelper.FirstNonBlank(name2.NameFormat, name2.DefaultNameFormat);
            }

            // Force formal salutation if first name missing or invalid
            if (!isFormal)
            {
                if ((name1.FirstName == null || name1.FirstName.Length < 2) &&
                    (name1.Nickname == null || name1.Nickname.Length < 2))
                {
                    isFormal = true;
                }
                else if (name2 != null)
                {
                    if ((name2.FirstName == null || name2.FirstName.Length < 2) &&
                        (name2.Nickname == null || name2.Nickname.Length < 2))
                    {
                        isFormal = true;
                    }
                }
            }

            IList<Token> formatTokens = TokenizeFormatString(nameFormat);
            IList<Token> spouseFormatTokens = TokenizeFormatString(spouseNameFormat);

            // Determine if prefixes are omitted from name formats
            if (!formatTokens.Any(p => p.Macro == Macro.Prefix))
            {
                omitPrefix = true;
                name1.Prefix = null;
            }

            if (name2 != null && !spouseFormatTokens.Any(p => p.Macro == Macro.Prefix))
            {
                omitSpousePrefix = true;
                name2.Prefix = null;
            }

            // If no prefixes specified, keep the position
            if (name1.Prefix == null && (name2 == null || name2.Prefix == null))
            {
                keepPosition = true;
            }
            // Else if separate names or for informal salutation, keep the position
            else if (keepSeparate || !isFormal)
            {
                keepPosition = true;
            }

            // Establish default values
            bool hasSpouse = name2 != null && !string.IsNullOrWhiteSpace(name2.LastName);
            bool useDefaultPrefix = false;
            bool spouseUseDefaultPrefix = false;

            // If there's no prefix, select the default prefix based on gender.
            if (name1.Prefix == null && (name1.Gender?.IsMasculine).HasValue)
            {
                string code = name1.Gender.IsMasculine == true ? "Mr" : "Ms";
                name1.Prefix = UnitOfWork.FirstOrDefault<Prefix>(p => p.Code == code);
                useDefaultPrefix = true;
            }
            else if (name1.Prefix != null && name1.Gender == null)
            {
                name1.Gender = UnitOfWork.GetReference(name1.Prefix, p => p.Gender);
            }
            else if (name1.Prefix == null)
            {
                omitPrefix = true;
            }

            if (name2 != null)
            {
                // If there's no spouse prefix, select the default prefix based on gender.
                if (name2.Prefix == null && (name2.Gender?.IsMasculine).HasValue)
                {
                    string code = name2.Gender.IsMasculine == true ? "Mr" : "Ms";
                    name2.Prefix = UnitOfWork.FirstOrDefault<Prefix>(p => p.Code == code);
                    spouseUseDefaultPrefix = true;
                }
                else if (name2.Prefix != null && name2.Gender == null)
                {
                    name2.Gender = UnitOfWork.GetReference(name2.Prefix, p => p.Gender);
                }
                else if (name2.Prefix == null)
                {
                    omitSpousePrefix = true;
                }
            }

            // Get basic genders;
            string gender1 = string.Empty;
            string gender2 = string.Empty;

            if (name1.Gender != null)
            {
                gender1 = name1.Gender.IsMasculine == true ? MALE : FEMALE;
            }

            if (name2 != null && name2.Gender != null)
            {
                gender2 = name2.Gender.IsMasculine == true ? MALE : FEMALE;
            }

            IList<Token> patternTokens = null;
            IList<Token> spousePatternTokens = null;

            PrefixPriority priority1 = (gender1 == MALE) ? PrefixPriority.Default : PrefixPriority.FemaleDefault;
            PrefixPriority priority2 = PrefixPriority.Undefined;

            string format = individualFormat;

            if (!string.IsNullOrWhiteSpace(fixedFormat))
            {
                format = fixedFormat;
            }
            else if (isFormal)
            {
                // Formal salutation is based on the prefix
                // set the default.                
                if (!omitPrefix && name1.Prefix != null)
                {
                    format = StringHelper.FirstNonBlank(name1.Prefix.Salutation, individualFormat);
                    priority1 = GetPrefixPriority(name1.Prefix, true);
                }
            }

            patternTokens = TokenizeFormatString(format, false);

            format = individualFormat;
            if (name2 != null)
            {
                priority2 = (gender1 == MALE) ? PrefixPriority.Default : PrefixPriority.FemaleDefault;
                if (!string.IsNullOrWhiteSpace(spouseFormat))
                {
                    format = spouseFormat;
                }
                else if (isFormal)
                {
                    // Formal salutation is based on the prefix
                    // set the default.                
                    if (!omitPrefix && name2.Prefix != null)
                    {
                        format = StringHelper.FirstNonBlank(name2.Prefix.Salutation, individualFormat);
                        priority2 = GetPrefixPriority(name2.Prefix, true);
                    }
                }

                spousePatternTokens = TokenizeFormatString(format, false);

                if (isFormal && hasSpouse && gender1 == MALE && gender2 == FEMALE && spouseUseDefaultPrefix)
                {
                    // Fix for case where spouse prefix is blank, gender is female, and primary is male:  Convert Ms to Mrs
                    ChangeMsToMrs(spousePatternTokens);
                }
            } // spouse salutation

            // If genders are the same or if either is blank, force separation
            if (gender1 == gender2 || string.IsNullOrWhiteSpace(gender1) || string.IsNullOrWhiteSpace(gender2))
            {
                keepSeparate = keepPosition = true;
            }

            // If there's no prefix and only the last name appears in the format, replace the last name with {NAME}.                            
            if (omitPrefix && !IsTokenListEmpty(patternTokens) && HasLastNameAndNoOtherNames(patternTokens))
            {
                ChangeLastNameToFullname(patternTokens);
            }

            if (omitSpousePrefix && !IsTokenListEmpty(spousePatternTokens) && HasLastNameAndNoOtherNames(spousePatternTokens))
            {
                ChangeLastNameToFullname(spousePatternTokens);
            }

            List<Token> savedPatternTokens = CopyTokenList(patternTokens);

            if (!IsTokenListEmpty(patternTokens))
            {
                ReplacePrefixMacro(patternTokens, name1.Prefix, ref omitPrefix); 
            }

            if (!IsTokenListEmpty(spousePatternTokens))
            {
                ReplacePrefixMacro(spousePatternTokens, name2.Prefix, ref omitSpousePrefix);
            }
            
            // Handle recipients

            switch (options.Recipient)
            {
                case LabelRecipient.Husband:
                    if (gender1 == MALE)
                    {
                        spouseFormatTokens = null;
                        spousePatternTokens = null;
                    }
                    else if (hasSpouse && gender2 == MALE)
                    {
                        formatTokens = null;
                        patternTokens = null;
                    }
                    break;

                case LabelRecipient.Wife:
                    if (gender1 == FEMALE)
                    {
                        spouseFormatTokens = null;
                        spousePatternTokens = null;
                    }
                    else if (hasSpouse && gender2 == FEMALE)
                    {
                        formatTokens = null;
                        patternTokens = null;
                    }
                    break;
                case LabelRecipient.Primary:
                    spouseFormatTokens = null;
                    spousePatternTokens = null;
                    break;
                case LabelRecipient.Secondary:
                    formatTokens = null;
                    patternTokens = null;
                    break;
            }

            // If there is no primary, the spouse becomes primary
            if (name2 != null && IsTokenListEmpty(patternTokens) && IsTokenListEmpty(formatTokens))
            {
                formatTokens = spouseFormatTokens;
                patternTokens = spousePatternTokens;
                priority1 = priority2;
                spousePatternTokens = null;
                spouseFormatTokens = null;
                omitPrefix = omitSpousePrefix;
                name1 = new Person(name2);
                name2 = null;
            }

            // Bail if there's no longer anything to format.
            if (IsTokenListEmpty(patternTokens))
                return string.Empty;

            // Single salutation - no spouse
            if (name2 == null)
            {
                return FormatIndividualName(name1, formatTokens, patternTokens);
            }

            // Couple's salutation - sort according to priority and gender

            bool swapped = false;
            bool combined = false;

            if (!keepPosition)
            {
                if (priority2 < priority1 ||
                    (priority2 == priority1 && gender2 == MALE && gender1 != MALE))
                {
                    // Swap primary and secondary.

                    Person tempName = name1;
                    name1 = name2;
                    name2 = tempName;

                    PrefixPriority priort = priority1;
                    priority1 = priority2;
                    priority2 = priort;

                    var tempList = formatTokens;
                    formatTokens = spouseFormatTokens;
                    spouseFormatTokens = tempList;

                    tempList = patternTokens;
                    patternTokens = spousePatternTokens;
                    spousePatternTokens = tempList;

                    bool tempBool = omitPrefix;
                    omitPrefix = omitSpousePrefix;
                    omitSpousePrefix = tempBool;

                    tempBool = useDefaultPrefix;
                    useDefaultPrefix = spouseUseDefaultPrefix;
                    spouseUseDefaultPrefix = tempBool;

                    swapped = true;
                }
            }

            // Strip off "Dear" from the second prefix.
            if (!IsTokenListEmpty(spousePatternTokens) &&
                spousePatternTokens[0].Macro == Macro.None &&
                IsFirstTokenDear(spousePatternTokens))
            {
                RemoveDearFromFirstToken(spousePatternTokens);
            }

            // Combine logic where prefixes are identical
            if (!IsTokenListEmpty(patternTokens) && !IsTokenListEmpty(spousePatternTokens) && !keepSeparate &&
                name1.Prefix != null && name2.Prefix != null &&
                name1.Prefix.Id == name2.Prefix.Id &&  // Prefixes identical
                !string.IsNullOrWhiteSpace(name1.LastName) && // Primary has a last name
                string.Compare(name1.LastName, name2.LastName, true) == 0 && // Last names identical
                formatTokens.Any(p => p.NamePart == MacroNamePart.Last) &&   // Primary has last name in format
                spouseFormatTokens.Any(p => p.NamePart == MacroNamePart.Last)) // Secondary has last name in format
            {
                // Try to find a plural version of this prefix, e.g. "Drs".
                Prefix prefix = UnitOfWork.FirstOrDefault<Prefix>(p => p.Code == name1.Prefix.Code + "s");
                if (prefix != null)
                {
                    // Load the new pattern
                    format = StringHelper.FirstNonBlank(prefix.Salutation, individualFormat);
                    var newPatternTokens = TokenizeFormatString(format, false);

                    // If there's no full name in the salutation pattern, try to convert {LAST} to {NAME}.
                    if (HasLastNameAndNoOtherNames(newPatternTokens))
                    {
                        ChangeLastNameToFullname(newPatternTokens);
                    }

                    // Replace the prefix macro with the prefix
                    ReplacePrefixMacro(newPatternTokens, prefix, ref omitPrefix);

                    if (newPatternTokens.Any(p => p.NamePart == MacroNamePart.All))
                    {
                        patternTokens = FormatPluralPrefix(newPatternTokens, formatTokens, spouseFormatTokens, out spousePatternTokens);
                        combined = true;
                    }
                }

            }

            if (!combined && !keepSeparate &&
                priority1 >= PrefixPriority.Default && priority2 >= PrefixPriority.Default &&
                (keepPosition || ((!swapped && gender1 == MALE) || (swapped && gender2 == MALE))) &&
                string.Compare(name1.LastName, name2.LastName, true) == 0 && // Last names must be equal
                !IsTokenListEmpty(patternTokens)) // Primary prefix pattern is non-empty
            {
                bool canCombine = false;
                if (patternTokens.Last().NamePart == MacroNamePart.All)
                {
                    // Can combine if primary pattern ends with {NAME}
                    canCombine = true;
                }
                else if (HasLastNameAndNoOtherNames(patternTokens))
                {
                    ChangeLastNameToFullname(patternTokens);
                    canCombine = true;
                }

                if (canCombine)
                {
                    CombinePrefixes(patternTokens, spousePatternTokens, formatTokens, spouseFormatTokens, omitPrefix, omitSpousePrefix, addFirstNames,
                        out patternTokens, out spousePatternTokens);

                    if (IsTokenListEmpty(spousePatternTokens))
                        spouseFormatTokens = null;

                    combined = true;
                }
            }

            string line1 = string.Empty;
            string line2 = string.Empty;

            if (!(IsTokenListEmpty(patternTokens) && IsTokenListEmpty(formatTokens)))
            {
                line1 = FormatIndividualName(name1, formatTokens, patternTokens);
            }

            if (!(IsTokenListEmpty(spousePatternTokens) && IsTokenListEmpty(spouseFormatTokens)))
            {
                line2 = FormatIndividualName(name2, spouseFormatTokens, spousePatternTokens);
            }

            if (string.IsNullOrWhiteSpace(line1))
            {
                line1 = line2;
                line2 = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(line2))
            {
                string separator = " ";
                if (!IsTokenListEmpty(patternTokens) && patternTokens.Any(p => p.Macro == Macro.And) ||
                    !IsTokenListEmpty(spousePatternTokens) && spousePatternTokens.Any(p => p.Macro == Macro.And))
                {
                    separator = " ";
                }
                else
                {
                    separator = " and ";
                }

                line1 = line1.TrimEnd(' ') + separator + line2.TrimStart(' ');
            }

            return line1;
        } 


        /// <summary>
        /// Convert a name (John) to initials (J.)
        /// </summary>
        private string ConvertNameToInitial(string s)
        {
            return (
                string.IsNullOrWhiteSpace(s)
                ?
                string.Empty
                :
                char.ToUpper(s[0]) + "."
                );
        }

        /// <summary>
        /// Determine if a token list is null or empty.
        /// </summary>
        private bool IsTokenListEmpty(IList<Token> tokens)
        {
            return tokens == null || tokens.Count == 0;
        }

        /// <summary>
        /// Determine if the first token in a list is "Dear".
        /// </summary>
        private bool IsFirstTokenDear(IList<Token> tokens)
        {
            if (!IsTokenListEmpty(tokens) && !string.IsNullOrWhiteSpace(tokens[0].Text) && tokens[0].Text.Length >= 4)
            {
                string text = tokens[0].Text.ToUpper();
                return (text == "DEAR" || text.StartsWith("DEAR "));
            }
            return false;
        }
        
        /// <summary>
        /// Remove "Dear" from the first token.  Prerequisite is to verify the token contains "Dear" by calling IsFirstTokenDear.
        /// </summary>
        /// <param name="tokens"></param>
        private void RemoveDearFromFirstToken(IList<Token> tokens)
        {
            string text = tokens[0].Text.Substring(4).Trim();
            tokens[0] = new Token(text);
        }

        /// <summary>
        /// Get the priority for a prefix.  Higher priority prefixes are placed to the left of lower priority prefixes.
        /// Priorities are: 1:High, 2:Medium, 3:Default, 4:Female default
        /// </summary>
        private PrefixPriority GetPrefixPriority(Prefix prefix, bool isForSalutation)
        {
            if (prefix == null)
            {
                return PrefixPriority.Undefined;
            }
            string code = prefix.Code.ToUpper();

            if (code == "MRS" || code == "MS" || code == "MISS" || code == "MMLE" || code == "SISTER" || code == "SRA" || code == "SRTA" || code == "MISSES")
            {
                return PrefixPriority.FemaleDefault;
            }

            if (isForSalutation)
            {
                // For salutations, anything that doesn't have a "special" salutation returns priority 3.
                if (string.IsNullOrWhiteSpace(prefix.Salutation) || prefix.Salutation.ToUpper().StartsWith("DEAR"))
                {
                    return PrefixPriority.Default;
                }

                return PrefixPriority.High; 
            }

            if (prefix.LabelPrefix.StartsWith("The"))
            {
                return PrefixPriority.Medium;
            }

            if (prefix.LabelPrefix.Contains('{'))
            {
                return PrefixPriority.High;
            }

            return PrefixPriority.Default;
        }

        /// <summary>
        /// Determine if a list of tokens has {LAST} but no other name tokens.
        /// </summary>
        private bool HasLastNameAndNoOtherNames(IList<Token> tokens)
        {
            bool hasLast = false;
            bool hasOther = false;
            foreach (var token in tokens)
            {
                hasLast |= (token.NamePart == MacroNamePart.Last);
                hasOther |= (token.NamePart != MacroNamePart.None && token.NamePart != MacroNamePart.Last);
            }

            return hasLast && !hasOther;
        }

        /// <summary>
        /// Convert {LAST} with {NAME} in a set of tokens if no other name parts other than {LAST} appear.
        /// </summary>
        /// <param name="tokens"></param>
        private void ChangeLastNameToFullname(IList<Token> tokens)
        {
            var token = tokens.FirstOrDefault(p => p.NamePart == MacroNamePart.Last);
            if (token != null)
            {
                tokens[tokens.IndexOf(token)] = new Token(string.Empty) { Macro = Macro.Name, NamePart = MacroNamePart.All };
            }
        }


        /// <summary>
        /// Create a plural prefix by combining two identical prefixes for a constituent and their spouse.
        /// </summary>
        private IList<Token> FormatPluralPrefix(IList<Token> patternTokens, IList<Token> formatTokens, IList<Token> spouseTokens, out IList<Token> spousePatternResult)
        {
            IList<Token> result = new List<Token>();
            spousePatternResult = new List<Token>();

            bool addToSpouse = false;
            foreach (var token in patternTokens)
            {
                if (token.NamePart != MacroNamePart.All)
                {
                    if (addToSpouse)
                    {
                        spousePatternResult.Add(token);
                    }
                    else
                    {
                        result.Add(token);
                    }
                }
                else if (!addToSpouse)
                {
                    AddMacroToken(result, formatTokens, MacroNamePart.First);
                    AddMacroToken(result, formatTokens, MacroNamePart.Middle);

                    AddMacroToken(spousePatternResult, Macro.And);

                    AddMacroToken(spousePatternResult, spouseTokens, MacroNamePart.First);
                    AddMacroToken(spousePatternResult, spouseTokens, MacroNamePart.Middle);
                    AddMacroToken(spousePatternResult, formatTokens, MacroNamePart.Last); // Yes, this one's a bit different.
                    AddMacroToken(spousePatternResult, spouseTokens, MacroNamePart.Suffix);

                    addToSpouse = true;

                }
            }

            return result;
        }

        /// <summary>
        /// Logic for combining two different prefixes for a constituent and their spouse.
        /// </summary>
        private void CombinePrefixes(IList<Token> patternTokens, IList<Token> spousePatternTokens, IList<Token> formatTokens, IList<Token> spouseTokens, bool omitPrefix, bool omitSpousePrefix, bool addFirstNames, out IList<Token> patternResult, out IList<Token> spousePatternResult)
        {
            patternResult = new List<Token>();
            spousePatternResult = new List<Token>();

            if (omitPrefix && omitSpousePrefix)
            {
                // If both prefixes omitted, format as "John Q. and Mary Doe"
                if (IsFirstTokenDear(patternTokens))
                {
                    AddTextToken(patternResult, patternTokens[0].Text);                    
                }
                AddMacroToken(patternResult, formatTokens, MacroNamePart.First);
                AddMacroToken(patternResult, formatTokens, MacroNamePart.Middle);
                AddMacroToken(patternResult, Macro.And);
                AddMacroToken(spousePatternResult, spouseTokens, MacroNamePart.First);
                AddMacroToken(spousePatternResult, spouseTokens, MacroNamePart.Middle);
                AddMacroToken(spousePatternResult, spouseTokens, MacroNamePart.Last);
                AddMacroToken(spousePatternResult, spouseTokens, MacroNamePart.Suffix);
            }
            else if (omitPrefix)
            {
                // If primary prefix omitted, format as "John Q. and Mrs. Mary Doe"
                if (IsFirstTokenDear(patternTokens))
                {
                    AddTextToken(patternResult, patternTokens[0].Text);
                }
                AddMacroToken(patternResult, formatTokens, MacroNamePart.First);
                AddMacroToken(patternResult, formatTokens, MacroNamePart.Middle);
                AddMacroToken(patternResult, Macro.And);
                spousePatternResult = new List<Token>(spousePatternTokens);
            }
            else if (omitSpousePrefix)
            {
                // If spouse prefix omitted, format as "Mr. John Q. and Mary Doe"
                // Replace {NAME} in Pattern1 with "{F}{M} and"
                foreach (var token in patternTokens.Where(p => p.NamePart != MacroNamePart.All))
                {
                    patternResult.Add(token);
                }
                AddMacroToken(patternResult, formatTokens, MacroNamePart.First);
                AddMacroToken(patternResult, formatTokens, MacroNamePart.Middle);
                AddMacroToken(patternResult, Macro.And);
                // Pattern2 is {F}{M}{L}{S}
                AddMacroToken(spousePatternResult, spouseTokens, MacroNamePart.First);
                AddMacroToken(spousePatternResult, spouseTokens, MacroNamePart.Middle);
                AddMacroToken(spousePatternResult, spouseTokens, MacroNamePart.Last);
                AddMacroToken(spousePatternResult, spouseTokens, MacroNamePart.Suffix);
            }
            else if (addFirstNames)
            {
                // For "Add First Names" mode, format as "Mr. John Q. and Mrs. Mary Doe"
                // Replace {NAME} in Pattern1 with "{F}{M} and"
                foreach (var token in patternTokens.Where(p => p.NamePart != MacroNamePart.All))
                {
                    patternResult.Add(token);
                }
                AddMacroToken(patternResult, formatTokens, MacroNamePart.First);
                AddMacroToken(patternResult, formatTokens, MacroNamePart.Middle);
                AddMacroToken(patternResult, Macro.And);
                spousePatternResult = new List<Token>(spousePatternTokens);
                // Ensure spouse uses a full name, and not just the last name.
                if (HasLastNameAndNoOtherNames(spousePatternResult))
                {
                    ChangeLastNameToFullname(spousePatternResult);
                }
            }
            else
            {
                // Format as "Mr. and Mrs. John Q. Doe"
                // Replace {NAME} in Pattern 1 with "and"
                foreach (var token in patternTokens.Where(p => p.NamePart != MacroNamePart.All))
                {
                    patternResult.Add(token);
                }
                AddMacroToken(patternResult, Macro.And);
                // append Pattern 2 to Pattern 1
                foreach (var token in spousePatternTokens)
                {
                    if (token.Macro == Macro.None)
                    {
                        AddTextToken(patternResult, token.Text);
                    }
                    else
                    {
                        AddMacroToken(patternResult, token.Macro);
                    }
                }
            }
        }

        /// <summary>
        /// Convert a format string into a list of tokens.  Each token will be plain text or a 
        /// macro in form of {...}
        /// </summary>
        private IList<Token> TokenizeFormatString(string format, bool addNameMacro = false)
        {
            bool escape = false;
            bool macro = false;
            bool hasMacro = false;
            
            if (string.IsNullOrWhiteSpace(format))
            {
                return new List<Token>();
            }

            IList<Token> tokenList;
            var cacheKey = new Tuple<string, bool>(format, addNameMacro);

            // There aren't that many format strings, so they are cached in a dictionary for performance. 
            if (_tokenCache.TryGetValue(cacheKey, out tokenList))
            {
                return CopyTokenList(tokenList);
            }

            // Parse the format string into tokens.

            tokenList = new List<Token>();
            StringBuilder tokenText = new StringBuilder();

            foreach (char c in format)
            {
                if (c == '\\')
                {
                    escape = true;
                }
                else if (c == '{' && !escape && !macro)
                {
                    if (tokenText.Length > 0)
                    {
                        AddTextToken(tokenList, tokenText.ToString());
                    }
                    macro = true;
                    tokenText.Clear();
                }
                else if (c == '}' && !escape && macro)
                {
                    AddMacroToken(tokenList, tokenText.ToString());
                    tokenText.Clear();
                    macro = false;
                    hasMacro = true;
                }
                else
                {
                    tokenText.Append(c);
                    escape = false;
                }
            }

            if (tokenText.Length > 0)
            {
                // Add any trailing text as the final token.
                AddTextToken(tokenList, tokenText.ToString());
            }

            if (addNameMacro && !hasMacro)
            {
                AddMacroToken(tokenList, Macro.Name);
            }

            if (_tokenCache.Count < TOKEN_CACHE_LIMIT)
            {
                _tokenCache.Add(cacheKey, CopyTokenList(tokenList));
            }

            return tokenList;
        }

        private void AddTextToken(IList<Token> list, string text)
        {
            Token token = new Token(text.Trim());
            list.Add(token);
        }

        private void AddMacroToken(IList<Token> list, Macro macro)
        {
            // Macro text is simply the enum value.
            string text = macro.ToString();

            AddMacroToken(list, text);
        }

        /// <summary>
        /// Find a specific token in a list of format tokens, then add it to a different token list.
        /// </summary>
        private void AddMacroToken(IList<Token> list, IList<Token> formatTokens, MacroNamePart namePart)
        {
            Token token = formatTokens.FirstOrDefault(p => p.NamePart == namePart);

            if (token != null)
            {
                AddMacroToken(list, token.Macro);
            }
        }

        /// <summary>
        /// Add a macro token by converting the macro name (as text) to its enum value.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="text"></param>
        private void AddMacroToken(IList<Token> list, string text)
        {
            Token token = new Token(text.Trim());

            switch (token.Text.ToUpper())
            {
                case NameFormatMacros.Prefix:
                case PREFIX_ABBREVIATION:
                    token.Macro = Macro.Prefix;
                    break;
                case NameFormatMacros.FirstName:
                case FIRST_NAME_ABBREVIATION: 
                    token.Macro = Macro.First;
                    token.NamePart = MacroNamePart.First;
                    break;
                case NameFormatMacros.FirstInitial:
                    token.Macro = Macro.FI;
                    token.NamePart = MacroNamePart.First;
                    break;
                case NameFormatMacros.MiddleName:
                case MIDDLE_NAME_ABBREVIATION:
                    token.Macro = Macro.Middle;
                    token.NamePart = MacroNamePart.Middle;
                    break;
                case NameFormatMacros.MiddleInitial:
                    token.Macro = Macro.MI;
                    token.NamePart = MacroNamePart.Middle;
                    break;
                case NameFormatMacros.LastName:
                case LAST_NAME_ABBREVIATION:
                    token.Macro = Macro.Last;
                    token.NamePart = MacroNamePart.Last;
                    break;
                case NameFormatMacros.LastInitial:
                    token.Macro = Macro.LI;
                    token.NamePart = MacroNamePart.Last;
                    break;
                case NameFormatMacros.Suffix:
                case SUFFIX_ABBREVIATION: 
                    token.Macro = Macro.Suffix;
                    token.NamePart = MacroNamePart.Suffix;
                    break;
                case NameFormatMacros.Nickname:
                case NICKNAME_ABBREVIATION:
                    token.Macro = Macro.Nickname;
                    token.NamePart = MacroNamePart.First;
                    break;
                case NameFormatMacros.Mr:
                    token.Macro = Macro.Mr;
                    break;
                case NameFormatMacros.Madam:
                    token.Macro = Macro.Madam;
                    break;
                case NameFormatMacros.Brother:
                    token.Macro = Macro.Brother;
                    break;
                case NameFormatMacros.His:
                    token.Macro = Macro.His;
                    break;
                case NameFormatMacros.Name:
                    token.Macro = Macro.Name;
                    token.NamePart = MacroNamePart.All;
                    break;
                case NameFormatMacros.And:
                    token.Macro = Macro.And;
                    break;
                default:
                    token.Macro = Macro.Unknown;
                    break;
            }

            list.Add(token);
        }

        /// <summary>
        /// Return a copy of a token list, copying the Token objects as well.
        /// </summary>
        private List<Token> CopyTokenList(IList<Token> list)
        {
            List<Token> result = new List<Token>();
            if (list != null)
            {
                foreach (var entry in list)
                {
                    result.Add(new Token(entry));
                }
            }

            return result;
        }


        /// <summary>
        /// Change "Ms" to "Mrs" where it appears in a list of tokens.
        /// </summary>
        /// <param name="tokens"></param>
        private void ChangeMsToMrs(IList<Token> tokens)
        {
            if (tokens != null && tokens.Count > 0 && tokens[0].Macro == Macro.None && tokens[0].Text.StartsWith("Ms"))
            {
                tokens[0] = new Token("Mrs" + tokens[0].Text.Substring(2));
            }
        }

        /// <summary>
        /// Replace the {PREFIX} macro with Prefix.Name.
        /// </summary>
        /// <param name="tokens">List of tokens</param>
        /// <param name="prefix">A Prefix entity</param>
        /// <param name="omitPrefix">Set to true if the token list doesn't have a {PREFIX} token or the prefix argument is null.</param>
        private void ReplacePrefixMacro(IList<Token> tokens, Prefix prefix, ref bool omitPrefix)
        {
            Token token = tokens.FirstOrDefault(p => p.Macro == Macro.Prefix);
            if (token == null)
            {
                omitPrefix = true;
                return;
            }

            int index = tokens.IndexOf(token);
            if (omitPrefix || prefix == null)
            {
                tokens[index] = new Token(string.Empty);
                omitPrefix = true;
            }

            else
            {
                tokens[index] = new Token(prefix.Name);
            }
        }

        /// <summary>
        /// Convert a constituent to a Person object.
        /// </summary>
        private Person ConvertToPerson (Constituent constituent)
        {
            Person person = new Person();
            if (constituent != null)
            {
                person.Prefix = constituent.Prefix ?? UnitOfWork.GetReference(constituent, p => p.Prefix);
                person.FirstName = constituent.FirstName ?? string.Empty;
                person.MiddleName = constituent.MiddleName ?? string.Empty;
                person.LastName = constituent.LastName ?? string.Empty;
                person.Suffix = constituent.Suffix ?? string.Empty;
                person.Nickname = constituent.Nickname ?? string.Empty;
                person.NameFormat = constituent.NameFormat ?? string.Empty;
                person.Gender = constituent.Gender ?? UnitOfWork.GetReference(constituent, p => p.Gender);
                ConstituentType type = constituent.ConstituentType ?? UnitOfWork.GetReference(constituent, p => p.ConstituentType);

                person.DefaultNameFormat = StringHelper.FirstNonBlank(type?.NameFormat, _defaultIndividualNameFormat);
            }
            return person;
        }


        #endregion

        #region Internal Classes
        /// <summary>
        /// Name formatting token - represents a macro like {NAME} or a text string.
        /// </summary>
        private class Token
        {
            public Macro Macro { get; set; }
            public MacroNamePart NamePart { get; set; }
            public string Text { get; set; }

            public Token (Token other)
            {
                Macro = other.Macro;
                NamePart = other.NamePart;
                Text = other.Text;
            }

            public Token (string text)
            {
                Text = text;
            }

            public override string ToString()
            {
                if (this.Macro != Macro.None)
                {
                    return "{" + this.Macro.ToString().ToUpper() + "}";
                }
                return this.Text;
            }
        }

        /// <summary>
        /// A simplified version of a Constituent, used for formatting individuals.
        /// </summary>
        private class Person
        {
            public Prefix Prefix { get; set; }
            public string FirstName { get; set; } = string.Empty;
            public string MiddleName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Suffix { get; set; } = string.Empty;
            public string Nickname { get; set; } = string.Empty;
            public string NameFormat { get; set; } = string.Empty;
            public Gender Gender { get; set; } 
            public string DefaultNameFormat { get; set; } = string.Empty;
            public string DefaultSalutationFormat { get; set; } = string.Empty;

            public Person() { }

            public Person(Person other)
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

        #endregion

    }
}



