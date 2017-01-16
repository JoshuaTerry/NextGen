using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Common;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.CRM
{
    /// <summary>
    /// Constituent name formatting business logic
    /// </summary>
    public class NameFormatter
    {
        private const string DEFAULT_NAME_FORMAT = "{P}{F}{MI}{L}{S}";

        private enum Macro
        {
            None = 0, Prefix, First, Middle, Last, FI, MI, LI, Suffix, Nickname, Name, Full, Mr, Madam, Brother, His, And, Unknown
        }

        private enum MacroNamePart
        {
            None = 0, First, Middle, Last, Suffix, All
        }

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

        private const int TOKEN_CACHE_LIMIT = 5000;

        // Dictionary for caching format strings and their tokenization
        private static Dictionary<Tuple<string,bool>, IList<Token>> _tokenCache = new Dictionary<Tuple<string, bool>, IList<Token>>();

        #region Constructors 

        public NameFormatter() : this(new UnitOfWorkEF()) { }

        public NameFormatter(IUnitOfWork uow)
        {
            UnitOfWork = uow;
            uow.AddBusinessLogic(this);

            var indivType = uow.FirstOrDefault<ConstituentType>(p => p.Code == "I");

            _defaultIndividualNameFormat = StringHelper.FirstNonBlank(indivType?.NameFormat, DEFAULT_NAME_FORMAT);
        }

        #endregion

        #region Public Properties

        public IUnitOfWork UnitOfWork { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Format a sortable name for an individual:  Last First Middle
        /// </summary>
        public string FormatIndividualSortName(Constituent name)
        {
            return FormatIndividualSortName(ConvertToSimpleName(name));
        }

        /// <summary>
        /// Formats an individual name into a single string.
        /// </summary>
        public string FormatIndividualName(Constituent name, string formatPattern)
        {
            return FormatIndividualName(ConvertToSimpleName(name), formatPattern);
        }

        /// <summary>
        /// Build name lines for one or two individual constituents.
        /// </summary>
        public void BuildIndividualNameLines(Constituent name1, Constituent name2, LabelRecipient recipient, bool separate, bool omitPrefix, bool addFirstNames, int maxChars, out string line1, out string line2)
        {
            BuildIndividualNameLines(ConvertToSimpleName(name1), ConvertToSimpleName(name2), recipient, separate, omitPrefix, addFirstNames, maxChars, out line1, out line2);
        }


        /// <summary>
        /// Build name lines for one or two constituents.
        /// </summary>
        public void BuildNameLines(Constituent name1, Constituent name2, LabelFormattingOptions options, out string line1, out string line2)
        {
            if (options == null)
            {
                options = new LabelFormattingOptions();
            }

            Constituent spouse = null;
            LabelRecipient recipient = options.Recipient;

            var constituentLogic = UnitOfWork.GetBusinessLogic<ConstituentLogic>();

            line1 = line2 = string.Empty;

            // For organizations, just return the constituent name.
            ConstituentType ctype = name1.ConstituentType ?? UnitOfWork.GetReference(name1, p => p.ConstituentType);
            if (ctype == null || ctype.Category == ConstituentCategory.Organization)
            {
                line1 = name1.Name;
                line2 = name1.Name2;
                return;
            }

            bool omitInactiveSpouse = !options.IncludeInactive; // TODO:  Need to include base CRM setting for this.

            // Keep spouse separate based on salutation format.
            bool keepSeparate = (name1.SalutationType == SalutationType.FormalSeparate || name1.SalutationType == SalutationType.InformalSeparate || options.KeepSeparate);

            if (options.IsSpouse)
            {
                spouse = name2;
            }
            else if (name2 != null || recipient != LabelRecipient.Primary)
            {
                spouse = constituentLogic.GetSpouse(name1);
            }

            if (spouse != null)
            {
                if (omitInactiveSpouse && constituentLogic.IsConstituentActive(spouse) == false)
                {
                    // Spouse is inactive and should be omitted.
                    if (name2 != null & spouse.Id == name2.Id)
                    {
                        // Since name2 is the spouse, set it to null.
                        name2 = null;
                    }
                    spouse = null;
                }
                // If primary & spouse have different last names...
                else if (options.IsSpouse && string.Compare(name1.LastName, spouse.LastName, true) != 0)
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
            if (spouse != null && name2 != null && spouse.Id == name2.Id)
            {
                name2 = null;
                if (recipient == LabelRecipient.Primary)
                {
                    recipient = LabelRecipient.Both;
                }
            }

            bool addFirstNames = options.AddFirstNames;  // TODO:  Need to include base CRM setting for this.

            // Use BuildIndividualNames to get the name lines for teh individual and spouse (which may be null)
            BuildIndividualNameLines(name1, spouse, recipient, keepSeparate, options.OmitPrefix, addFirstNames, options.MaxChars, out line1, out line2);

            // Try to add the second name line if we can
            if (string.IsNullOrWhiteSpace(line2))
            {
                if (name2 != null)
                {
                    ConstituentType ctype2 = name2.ConstituentType ?? UnitOfWork.GetReference(name2, p => p.ConstituentType);
                    if (ctype2 == null || ctype.Category == ConstituentCategory.Organization)
                    {
                        line2 = name2.Name;
                    }
                    else
                    {
                        string temp;
                        BuildIndividualNameLines(name2, null, LabelRecipient.Primary, false, options.OmitPrefix, addFirstNames, options.MaxChars, out line2, out temp);
                    }
                }
                else
                {
                    line2 = name1.Name2;
                }
            }
        }

        /// <summary>
        /// Build an addres label for one or two constituents.
        /// </summary>
        public List<string> BuildAddressLabel(Constituent name1, Constituent name2, Address address, LabelFormattingOptions opts)
        {
            List<string> label = new List<string>();
            string line1, line2;

            if (opts == null)
                opts = new LabelFormattingOptions();

            // Get options
            AddressCategory addrMode = opts.AddressCategory;
            string contactName = opts.ContactName;


            string nameLine2 = string.Empty;

            if (!string.IsNullOrWhiteSpace(opts.AddressType))
            {
                addrMode = AddressCategory.None;
            }

            if (name1 != null)
            {
                BuildNameLines(name1, name2, opts, out line1, out line2);

                // Remove name2 from the result if it's in Line2.
                if (!string.IsNullOrWhiteSpace(name1.Name2) && string.Compare(line2, name1.Name2, true) == 0)
                {
                    line2 = string.Empty;
                }

                nameLine2 = name1.Name2 ?? string.Empty;

                if (opts.Caps)
                {
                    line1 = line1.ToUpper();
                    line2 = line2.ToUpper();
                    nameLine2 = nameLine2.ToUpper();
                }
                if (opts.ExpandName)
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

                if (!string.IsNullOrWhiteSpace(contactName) && opts.Caps)
                {
                    contactName = contactName.ToUpper();
                }

                // TODO: More logic needed, pending other BL to be added.
            }

            return label;

        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Format a sortable name for an individual:  Last First Middle
        /// </summary>
        private string FormatIndividualSortName(SimpleName name)
        {
            StringBuilder sb = new StringBuilder();
            string format = StringHelper.FirstNonBlank(name.NameFormat, name.DefaultNameFormat);

            var tokens = TokenizeFormatString(format);

            if (!string.IsNullOrWhiteSpace(name.LastName))
            {
                if (tokens.Any(p => p.Macro == Macro.LI))
                {
                    sb.Append(name.LastName.Substring(0, 1)).Append(". ");
                }
                if (tokens.Any(p => p.Macro == Macro.Last))
                {
                    sb.Append(name.LastName).Append(' ');
                }
            }

            if (!string.IsNullOrWhiteSpace(name.FirstName))
            {
                if (tokens.Any(p => p.Macro == Macro.FI))
                {
                    sb.Append(name.FirstName.Substring(0, 1)).Append(". ");
                }
                else if (tokens.Any(p => p.Macro == Macro.First))
                {
                    sb.Append(name.FirstName).Append(' ');
                }
                else if (tokens.Any(p => p.Macro == Macro.Nickname))
                {
                    sb.Append(StringHelper.FirstNonBlank(name.Nickname, name.FirstName)).Append(' ');
                }
            }

            if (!string.IsNullOrWhiteSpace(name.MiddleName))
            {
                if (tokens.Any(p => p.Macro == Macro.MI))
                {
                    sb.Append(name.MiddleName.Substring(0, 1)).Append(". ");
                }
                else if (tokens.Any(p => p.Macro == Macro.Middle))
                {
                    sb.Append(name.MiddleName).Append(' ');
                }
            }

            return sb.ToString();
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
                    case Macro.Full:
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
        /// Formats an individual name into a single string.
        /// </summary>
        private string FormatIndividualName(SimpleName name, IList<Token> formatTokens, IList<Token> patternTokens)
        {
            StringBuilder rslt = new StringBuilder();

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
                    case Macro.First:
                        namePart = name.FirstName;
                        break;
                    case Macro.FI:
                        namePart = Initialize(name.FirstName);
                        break;
                    case Macro.Middle:
                        namePart = name.MiddleName;
                        break;
                    case Macro.MI:
                        namePart = Initialize(name.MiddleName);
                        break;
                    case Macro.Last:
                        namePart = name.LastName;
                        break;
                    case Macro.LI:
                        namePart = Initialize(name.LastName);
                        break;
                    case Macro.Suffix:
                        namePart = name.Suffix;
                        break;
                    case Macro.Nickname:
                        if (!string.IsNullOrWhiteSpace(name.Nickname))
                        {
                            namePart = name.Nickname;
                        }
                        else if (!string.IsNullOrWhiteSpace(name.FirstName))
                        {
                            if (!formatTokens.Any(p => p.Macro == Macro.First || p.Macro == Macro.FI))
                            {
                                namePart = name.FirstName;
                            }
                        }
                        else if (!string.IsNullOrWhiteSpace(name.MiddleName))
                        {
                            if (!formatTokens.Any(p => p.NamePart == MacroNamePart.Middle))
                            {
                                namePart = name.MiddleName;
                            }
                        }
                        break;
                    case Macro.Brother:
                        namePart = (name.Gender?.IsMasculine == false) ? "Sister" : "Brother";
                        break;
                    case Macro.His:
                        namePart = (name.Gender?.IsMasculine == false) ? "Her" : "His";
                        break;
                    case Macro.Madam:
                        namePart = (name.Gender?.IsMasculine == false) ? "Madam" : "Mr.";
                        break;
                    case Macro.Mr:
                        namePart = (name.Gender?.IsMasculine == false) ? "Ms." : "Mr.";
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
                    if (rslt.Length > 0 && !char.IsPunctuation(namePart[0]))
                    {
                        // Append a space betweeen tokens unless this token starts with punctuation.
                        rslt.Append(' ');
                    }
                    rslt.Append(namePart);
                }

            } // Each token

            return rslt.ToString();
        }

        /// <summary>
        /// Formats an individual name into a single string.
        /// </summary>
        private string FormatIndividualName(SimpleName name, IList<Token> formatTokens, IList<Token> patternTokens, IList<Token> shortPatternTokens, int maxChars)
        {
            string rslt = FormatIndividualName(name, formatTokens, patternTokens);

            if (maxChars > 0 && rslt.Length > maxChars && shortPatternTokens != null)
            {
                rslt = FormatIndividualName(name, formatTokens, shortPatternTokens);
            }

            return rslt;
        }

        /// <summary>
        /// Formats an individual name into a single string.
        /// </summary>
        private string FormatIndividualName(SimpleName name, string formatPattern)
        {
            string nameFormat = StringHelper.FirstNonBlank(name.NameFormat, name.DefaultNameFormat);

            return FormatIndividualName(name, TokenizeFormatString(nameFormat), TokenizeFormatString(formatPattern));
        }

        /// <summary>
        /// Build name lines for one or two individual constituents.
        /// </summary>
        private void BuildIndividualNameLines(SimpleName name1, SimpleName name2, LabelRecipient recipient, bool separate, bool omitPrefix, bool addFirstNames, int maxChars, out string line1, out string line2)
        {
            bool omitSpousePrefix = omitPrefix;
            bool keepPosition = omitPrefix;
            line1 = line2 = string.Empty;

            if (name2 == null)
            {
                name2 = new SimpleName();
            }

            // Get name formats
            string format = StringHelper.FirstNonBlank(name1.NameFormat, name1.DefaultNameFormat);
            string spouseFormat = StringHelper.FirstNonBlank(name2.NameFormat, name2.DefaultNameFormat);

            IList<Token> formatTokens = TokenizeFormatString(format);
            IList<Token> spouseFormatTokens = TokenizeFormatString(spouseFormat);

            // Determine if prefixes are omitted from name formats
            if (!formatTokens.Any(p => p.Macro == Macro.Prefix))
            {
                omitPrefix = true;
                name1.Prefix = null;
            }

            if (!spouseFormatTokens.Any(p => p.Macro == Macro.Prefix))
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
            bool useDefaultPrefix = false;
            bool spouseUseDefaultPrefix = false;

            if (name1.Prefix == null && (name1.Gender?.IsMasculine).HasValue)
            {
                string code = name1.Gender.IsMasculine == true ? "Mr" : "Ms";
                name1.Prefix = UnitOfWork.FirstOrDefault<Prefix>(p => p.Code == code);
                useDefaultPrefix = true;
            }

            if (name2.Prefix == null && (name2.Gender?.IsMasculine).HasValue)
            {
                string code = name2.Gender.IsMasculine == true ? "Mr" : "Ms";
                name2.Prefix = UnitOfWork.FirstOrDefault<Prefix>(p => p.Code == code);
                spouseUseDefaultPrefix = true;
            }


            IList<Token> patternTokens = null;
            IList<Token> spousePatternTokens = null;
            IList<Token> shortPatternTokens = null;
            IList<Token> spouseShortPatternTokens = null;

            int prior1 = 3;
            int prior2 = 3;

            if (!omitPrefix && name1.Prefix != null)
            {

                patternTokens = TokenizeFormatString(name1.Prefix.LabelPrefix, true);
                shortPatternTokens = TokenizeFormatString(name1.Prefix.LabelAbbreviation, true);
                prior1 = GetPrefixPriority(name1.Prefix);

                if (name1.Gender == null)
                    name1.Gender = UnitOfWork.GetReference(name1.Prefix, p => p.Gender);
            }

            if (hasSpouse)
            {
                if (!omitSpousePrefix && name2.Prefix != null)
                {
                    spousePatternTokens = TokenizeFormatString(name2.Prefix.LabelPrefix, true);
                    spouseShortPatternTokens = TokenizeFormatString(name2.Prefix.LabelAbbreviation, true);
                    prior2 = GetPrefixPriority(name2.Prefix);

                    if (name2.Gender == null)
                        name2.Gender = UnitOfWork.GetReference(name2.Prefix, p => p.Gender);
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

            // Handle recipients

            switch (recipient)
            {
                case LabelRecipient.Husband:
                    if (gender1 == "M")
                    {
                        spouseFormatTokens = null;
                        spousePatternTokens = null;
                    }
                    else if (hasSpouse && gender2 == "M")
                    {
                        formatTokens = null;
                        patternTokens = null;
                    }
                    break;

                case LabelRecipient.Wife:
                    if (gender1 == "F")
                    {
                        spouseFormatTokens = null;
                        spousePatternTokens = null;
                    }
                    else if (hasSpouse && gender2 == "F")
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
                prior1 = prior2;
                spousePatternTokens = null;
                spouseFormatTokens = null;
                name1 = new SimpleName(name2);
            }


            // Bail if there's no longer anything to format.
            if (IsTokenListEmpty(formatTokens) && IsTokenListEmpty(patternTokens))
                return;

            // Single label's name
            if (IsTokenListEmpty(spouseFormatTokens) && IsTokenListEmpty(spousePatternTokens))
            {
                line1 = FormatIndividualName(name1, formatTokens, patternTokens, shortPatternTokens, maxChars);
                //line1 = FormatIndividualName(pwork1, pabbr1, maxChars, false, format, name1);
                return;
            }

            bool swapped = false;
            bool combined = false;

            // Couple's label name.

            if (!keepPosition)
            {
                if (prior2 < prior1 ||
                    (prior2 == prior1 && gender2 == "M" && gender1 != "M"))
                {
                    // Swap primary and secondary.

                    SimpleName tempName = name1;
                    name1 = name2;
                    name2 = tempName;

                    int priort = prior1;
                    prior1 = prior2;
                    prior2 = priort;

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
                    var newPatternTokens = TokenizeFormatString(prefix.LabelPrefix, true);
                    var newShortPatternTokens = TokenizeFormatString(prefix.LabelAbbreviation, true);

                    if (newPatternTokens.Any(p => p.NamePart == MacroNamePart.All))
                    {
                        patternTokens = FormatDoublePrefix(newPatternTokens, formatTokens, spouseFormatTokens, out spousePatternTokens);
                        shortPatternTokens = FormatDoublePrefix(newShortPatternTokens, formatTokens, spouseFormatTokens, out spouseShortPatternTokens);
                        combined = true;
                    }
                }
            }

            if (!combined)
            {
                if (hasSpouse && gender1 == "M" && gender2 == "F" && spouseUseDefaultPrefix)
                {
                    // Fix for case where spouse prefix is blank, gender is female, and primary is male:  Convert Ms to Mrs
                    ChangeMsToMrs(spousePatternTokens);
                    ChangeMsToMrs(spouseShortPatternTokens);
                }
            }

            // Combine logic for all other cases.
            if (!combined && !separate &&
                prior1 > 2 && prior2 > 2 &&
                (keepPosition || ((!swapped && gender1 == "M") || (swapped && gender2 == "M"))) &&
                string.Compare(name1.LastName, name2.LastName, true) == 0 &&
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
                    spouseFormatTokens = null;

                combined = true;
            }

            if (!(IsTokenListEmpty(patternTokens) && IsTokenListEmpty(formatTokens)))
            {
                line1 = FormatIndividualName(name1, formatTokens, patternTokens, shortPatternTokens, maxChars);
            }

            if (!(IsTokenListEmpty(spousePatternTokens) && IsTokenListEmpty(spouseFormatTokens)))
            {
                line2 = FormatIndividualName(name2, spouseFormatTokens, spousePatternTokens, spouseShortPatternTokens, maxChars);
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

                    separator = " ";
                else
                    separator = " and ";

                if (maxChars == 0 || line1.Length + 1 + line2.Length <= maxChars)
                {
                    line1 = line1.TrimEnd(' ') + separator + line2.TrimStart(' ');
                    line2 = string.Empty;
                }
            }

        } // BuildIndividualNameLines

        /// <summary>
        /// Determine if a token list is null or empty.
        /// </summary>
        private bool IsTokenListEmpty(IList<Token> tokens)
        {
            return tokens == null || tokens.Count == 0;
        }

        /// <summary>
        /// Get the priority for a prefix.  Higher priority prefixes are placed to the left of lower priority prefixes.
        /// </summary>
        private int GetPrefixPriority(Prefix prefix)
        {
            if (prefix == null)
                return 9;
            string code = prefix.Code.ToUpper();

            if (code == "MRS" || code == "MS" || code == "MISS" || code == "MMLE" || code == "SISTER" || code == "SRA" || code == "SRTA" || code == "MISSES")
                return 4;

            if (prefix.LabelPrefix.StartsWith("The"))
                return 2;
            if (prefix.LabelPrefix.Contains('{') || prefix.Salutation.Contains('{'))
                return 1;

            return 3;
        }

        /// <summary>
        /// Logic for combining two identical prefixes for a constituent and their spouse.
        /// </summary>
        private IList<Token> FormatDoublePrefix(IList<Token> patternTokens, IList<Token> formatTokens, IList<Token> spouseTokens, out IList<Token> spousePatternResult)
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
                return tokenList;
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
                _tokenCache.Add(cacheKey, tokenList);
            }

            return tokenList;
        }

        private void AddTextToken(IList<Token> list, string text)
        {
            Token token = new Token(text);
            list.Add(token);
        }

        private void AddMacroToken(IList<Token> list, Macro macro)
        {
            // Enum values equate to text strings.
            string text = macro.ToString();

            AddMacroToken(list, text);
        }

        private void AddMacroToken(IList<Token> list, IList<Token> formatTokens, MacroNamePart namePart)
        {
            Token token = formatTokens.FirstOrDefault(p => p.NamePart == namePart);

            if (token != null)
            {
                AddMacroToken(list, token.Macro);
            }
        }

        private void AddMacroToken(IList<Token> list, string text)
        {
            Token token = new Token(text);

            switch (text.ToUpper().Trim())
            {
                case "PREFIX":
                case "P":
                    token.Macro = Macro.Prefix;
                    break;
                case "FIRST":
                case "F":
                    token.Macro = Macro.First;
                    token.NamePart = MacroNamePart.First;
                    break;
                case "FI":
                    token.Macro = Macro.FI;
                    token.NamePart = MacroNamePart.First;
                    break;
                case "MIDDLE":
                case "M":
                    token.Macro = Macro.Middle;
                    token.NamePart = MacroNamePart.Middle;
                    break;
                case "MI":
                    token.Macro = Macro.MI;
                    token.NamePart = MacroNamePart.Middle;
                    break;
                case "LAST":
                case "L":
                    token.Macro = Macro.Last;
                    token.NamePart = MacroNamePart.Last;
                    break;
                case "LI":
                    token.Macro = Macro.LI;
                    token.NamePart = MacroNamePart.Last;
                    break;
                case "SUFFIX":
                case "S":
                    token.Macro = Macro.Suffix;
                    token.NamePart = MacroNamePart.Suffix;
                    break;
                case "NICKNAME":
                    token.Macro = Macro.Nickname;
                    token.NamePart = MacroNamePart.First;
                    break;
                case "MR":
                    token.Macro = Macro.Mr;
                    break;
                case "MADAM":
                    token.Macro = Macro.Madam;
                    break;
                case "BROTHER":
                    token.Macro = Macro.Brother;
                    break;
                case "HIS":
                    token.Macro = Macro.His;
                    break;
                case "NAME":
                    token.Macro = Macro.Name;
                    token.NamePart = MacroNamePart.All;
                    break;
                case "FULL":
                    token.Macro = Macro.Full;
                    token.NamePart = MacroNamePart.All;
                    break;
                case "AND":
                    token.Macro = Macro.And;
                    break;
                default:
                    token.Macro = Macro.Unknown;
                    break;
            }

            list.Add(token);
        }

        /// <summary>
        /// Change "Ms" to "Mrs" where it appears in a list of tokens.
        /// </summary>
        /// <param name="tokens"></param>
        private void ChangeMsToMrs(IList<Token> tokens)
        {
            if (tokens != null && tokens.Count > 0 && tokens[0].Macro == Macro.None && tokens[0].Text.StartsWith("Ms"))
            {
                tokens[0].Text = "Mrs" + tokens[0].Text.Substring(2);
            }
        }

        /// <summary>
        /// Convert a constituent to a SimpleName object.
        /// </summary>
        private SimpleName ConvertToSimpleName (Constituent name)
        {
            SimpleName simpleName = new SimpleName();
            if (name != null)
            {
                simpleName.Prefix = name.Prefix ?? UnitOfWork.GetReference(name, p => p.Prefix);
                simpleName.FirstName = name.FirstName ?? string.Empty;
                simpleName.MiddleName = name.MiddleName ?? string.Empty;
                simpleName.LastName = name.LastName ?? string.Empty;
                simpleName.Suffix = name.Suffix ?? string.Empty;
                simpleName.Nickname = name.Nickname ?? string.Empty;
                simpleName.NameFormat = name.NameFormat ?? string.Empty;
                simpleName.Gender = name.Gender ?? UnitOfWork.GetReference(name, p => p.Gender);
                ConstituentType ctype = name.ConstituentType ?? UnitOfWork.GetReference(name, p => p.ConstituentType);

                simpleName.DefaultNameFormat = StringHelper.FirstNonBlank(ctype?.NameFormat, _defaultIndividualNameFormat);
            }
            return simpleName;
        }


        #endregion

        #region Internal Classes

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
                DefaultNameFormat = string.Empty;                
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

        #endregion

    }
}



