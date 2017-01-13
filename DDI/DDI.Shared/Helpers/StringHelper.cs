using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DDI.Shared.Helpers
{
    public static class StringHelper
    {

        #region Private Fields

        private static readonly char[] NameSplitChars = { ' ', '(', ')', '-', '_', '+', '=', '[', '{', ']', '}', '|', '/', '\\', ':', ';', '"', '\'', '?', ',', '.', '<', '>', '`', '!', '@', '#', '$', '%', '^', '&', '*' };

        private static string[] fpsExceptP = { "teeth","potatoes","heroes","roofs","criteria","children","women","men","people","geese","mice","crises","theses","hooves","hives","dives","jives","chives","strives","drives","thrives",
                                                 "alumni","foci","radii","genera","fungi","millennia","ova","matrix","vertex" };

        private static string[] fpsExceptS = { "tooth","potato","hero","roof","criterion","child","woman","man","person","goose","mouse","crisis","thesis","hoof","hive","dive","jive","chive","strive","drive","thrive",
                                                 "alumnus","focus","radius","genus","fungus","millennium","ovum","matrices","vertices" };

        private static string[] fpsSame = { "data", "barracks", "deer", "pants", "glasses", "scissors", "series", "species" };
        private static string[] fpsSuffixP = { "ays", "eys", "iys", "oys", "uys", "ies", "ches", "shes", "ives", "oaves", "xes", "zzes", "eaves", "elves", "sses", "ss" };
        private static string[] fpsSuffixS = { "ay", "ey", "iy", "oy", "uy", "y", "ch", "sh", "ife", "oaf", "x", "zz", "eaf", "elf", "ss", "ss" };

        #endregion Private Fields

        /// <summary>
        /// Replaces any occurrences of multiple consecutive spaces with a single space.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CollapseSpaces(string text)
        {
            return Regex.Replace(text, @"\s+", " ");
        }

        /// <summary>
        /// Return the first non-null non-whitespace string.
        /// </summary>
        /// <param name="first">The first string</param>
        /// <param name="rest">Additional strings</param>
        public static string FirstNonBlank(string first, params string[] rest)
        {
            if (string.IsNullOrWhiteSpace(first))
            {
                return rest.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p)) ?? string.Empty;
            }

            return first;
        }

        /// <summary>
        /// Returns TRUE if this string is the same as another string, using a case-insensitive
        /// comparison and ignoring leading and trailing whitespace.
        /// </summary>
        public static bool IsSameAs(string text, string tеxt)
        {
            if (string.IsNullOrWhiteSpace(text) && string.IsNullOrWhiteSpace(tеxt))
            {
                return true;
            }

            return string.Compare(text.Trim(), tеxt.Trim(), true) == 0;
        }

        /// <summary>
        /// Returns only the letters and digits in a string. All other characters are omitted.
        /// </summary>
        /// <returns></returns>
        public static string LettersAndDigits(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            return Regex.Replace(text, "\\W", string.Empty);
        }

        /// <summary>
        /// Capitalize the first letter of a word with all remaining letters in lowercase.
        /// </summary>
        public static string FormalCase(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;
            if (text.Length == 1)
                return text.ToUpper();
            else
                return text.Substring(0, 1).ToUpper() + text.Substring(1).ToLower();
        }

        /// <summary>
        /// Similar to SQL LIKE function: _ matches a single character, and % matches any number of
        /// characters.
        /// </summary>
        public static bool SqlLike(
            string text, string pattern)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(pattern))
            {
                return false;
            }
            return Regex.IsMatch(text, "(?i)^" + pattern.Replace("%", ".*").Replace("_", ".") + "$");
        }

        public static string[] SplitIntoWords(string text)
        {
            return text.Split(NameSplitChars, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string ToPlural(string text)
        {
            string ltext = text;
            string ptext = string.Empty;
            string startWords = string.Empty;
            bool capAll = false, capFirst = false;

            if (text == null || text.Length < 2)
            {
                return text;
            }

            // If there's more than one word, get the last word into ltext.
            int idx = text.TrimEnd(' ').LastIndexOf(' ');

            if (idx > 0)
            {
                startWords = text.Substring(0, idx + 1);
                ltext = text.Substring(idx + 1);
                if (ltext.Length < 2)
                {
                    return text;
                }
            }

            // Remember capitalization status
            if (char.IsUpper(ltext[0]))
            {
                if (char.IsUpper(ltext[1]))
                {
                    capAll = true;
                }
                else
                {
                    capFirst = true;
                }
            }

            ltext = ltext.ToLower();

            // Handle words whose singlar and plural are the same, and words that are already plural.
            if (fpsSame.Contains(ltext) || fpsExceptP.Contains(ltext))
            {
                return text;
            }

            // Lookup the word in the exception list.
            idx = Array.IndexOf(fpsExceptS, ltext);
            if (idx >= 0)
            {
                ptext = fpsExceptP[idx];
            }
            else
            {
                // Use the suffix list. Default is to add an "s".
                ptext = ltext + "s";
                for (idx = 0; idx < fpsSuffixS.Length; idx++)
                {
                    if (ltext.EndsWith(fpsSuffixS[idx]))
                    {
                        ptext = ltext.Substring(0, ltext.Length - fpsSuffixS[idx].Length) + fpsSuffixP[idx];
                        break;
                    }
                }
            }

            // Restore capitalization:
            if (capAll)
            {
                ptext = ptext.ToUpper();
            }
            else if (capFirst)
            {
                ptext = char.ToUpper(ptext[0]) + ptext.Substring(1);
            }

            return startWords + ptext;
        }

        /// <summary>
        /// Converts CamelCase strings to Separate Words (e.g. Camel Case) intelligently: "thisIDValue"
        /// =&gt; "This ID Value"
        /// </summary>
        public static string ToSeparateWords(string text)
        {
            StringBuilder sb = new StringBuilder();
            UnicodeCategory cat = UnicodeCategory.OtherNotAssigned;
            UnicodeCategory priorCat = UnicodeCategory.OtherNotAssigned;

            foreach (char c in text)
            {
                // Get the unicode category. Note that enum values 0 - 4 (OtherLetter) are alphabetic
                // (letters).
                cat = char.GetUnicodeCategory(c);

                switch (cat)
                {
                    case UnicodeCategory.DecimalDigitNumber:

                        // Digit - If prior char wasn't a digit 0-9, insert a space. (Other numeric chars get
                        // ignored.)
                        if (priorCat != UnicodeCategory.DecimalDigitNumber)
                        {
                            sb.Append(' ');
                        }
                        sb.Append(c);
                        break;

                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:

                        // Upper case - If prior char wasn't uppercase, insert a space.
                        if (priorCat != UnicodeCategory.UppercaseLetter && priorCat != UnicodeCategory.TitlecaseLetter)
                        {
                            sb.Append(' ');
                        }
                        sb.Append(c);
                        break;

                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.OtherLetter:

                        // Lower case - If prior isn't alphabetic, insert a space.
                        if (priorCat > UnicodeCategory.OtherLetter) // i.e. if it's not a letter
                        {
                            sb.Append(' ');
                        }
                        else if (priorCat == UnicodeCategory.UppercaseLetter || priorCat == UnicodeCategory.TitlecaseLetter)
                        {
                            // If prior is uppercase, make sure that uppercase char is preceded by a space.
                            if (sb[sb.Length - 2] != ' ')
                            {
                                sb.Insert(sb.Length - 1, ' ');
                            }
                        }

                        // If prior isn't alphabetic, force this char to be uppercase.
                        if (priorCat > UnicodeCategory.OtherLetter)
                        {
                            // i.e. if it's not a letter
                            sb.Append(char.ToUpper(c));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;

                    default:

                        // Not a digit or a character - Ignore it.
                        break;
                }

                priorCat = cat; // Current category becomes prior category
            }
            return sb.ToString().Trim();
        }

        public static string ToSingular(string text)
        {
            string ltext = text;
            string stext = string.Empty;
            string startWords = string.Empty;
            bool capAll = false, capFirst = false;

            if (text == null || text.Length < 2)
            {
                return text;
            }

            // If there's more than one word, get the last word into ltext.
            int idx = text.TrimEnd(' ').LastIndexOf(' ');

            if (idx > 0)
            {
                startWords = text.Substring(0, idx + 1);
                ltext = text.Substring(idx + 1);
                if (ltext.Length < 2)
                {
                    return text;
                }
            }

            // Remember capitalization status
            if (char.IsUpper(ltext[0]))
            {
                if (char.IsUpper(ltext[1]))
                {
                    capAll = true;
                }
                else
                {
                    capFirst = true;
                }
            }

            ltext = ltext.ToLower();

            // Handle words whose singlar and plural are the same, and words that are already singular.
            if (fpsSame.Contains(ltext) || fpsExceptS.Contains(ltext))
            {
                return text;
            }

            // Lookup the word in the exception list.
            idx = Array.IndexOf(fpsExceptP, ltext);
            if (idx >= 0)
            {
                stext = fpsExceptS[idx];
            }

            // At this point if the word doesn't end in "s" or if it ends in "ss", it's not plural.
            else if (!ltext.EndsWith("s") || ltext.EndsWith("ss"))
            {
                return text;
            }
            else
            {
                // Default is to remove the "s"
                stext = ltext.TrimEnd('s');

                // Use the suffix list.
                for (idx = 0; idx < fpsSuffixP.Length; idx++)
                {
                    if (ltext.EndsWith(fpsSuffixP[idx]))
                    {
                        stext = ltext.Substring(0, ltext.Length - fpsSuffixP[idx].Length) + fpsSuffixS[idx];
                        break;
                    }
                }
            }

            // Restore capitalization:
            if (capAll)
            {
                stext = stext.ToUpper();
            }
            else if (capFirst)
            {
                stext = char.ToUpper(stext[0]) + stext.Substring(1);
            }

            return startWords + stext;
        }

        /// <summary>
        /// If the value of this <see cref="string"/> is longer than the given maximum number of
        /// characters, the value will be truncated to the maximum length.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string Truncate(string text, int maxLength)
        {
            if (text != null && text.Length > maxLength)
            {
                return text.Substring(0, maxLength);
            }

            return text;
        }

        public static string Masked(string text, int lastCharactersUnmasked, char maskCharacter = '*')
        {
            var maskedLength = text.Length - lastCharactersUnmasked;
            if (maskedLength > 0 && lastCharactersUnmasked >= 0)
            {
                return $"{new String(maskCharacter, maskedLength)}{text.Substring(maskedLength)}";
            }
            return new String(maskCharacter, 5);
        }

       

        /// <summary>
        /// Remove all macros like {...} from a string.  
        /// </summary>
        public static string RemoveAllMacros(string text)
        {
            StringBuilder sb = new StringBuilder();
            bool inMacro = false;

            foreach (char c in text)
            {
                if (c == '{')
                {
                    inMacro = true;
                }
                else if (inMacro && c == '}')
                {
                    inMacro = false;
                }
                else if (!inMacro)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }      

        /// <summary>
        /// Split "abc/def" or "abc & def" into two strings.
        /// </summary>
        /// <param name="text">Text to split</param>
        /// <param name="part2">Part 2</param>
        /// <returns>Part 1</returns>
        public static string SplitDualName(string text, out string part2)
        {
            string part1 = string.Empty;
            part2 = string.Empty;

            int pos = text.IndexOf('&');
            if (pos < 0)
            {
                pos = text.IndexOf('/');
            }
            if (pos < 0)
            {
                return text;
            }

            if (pos > 0)
            {
                part1 = text.Substring(0, pos).Trim();
            }

            if (pos < text.Length - 1)
            {
                part2 = text.Substring(pos + 1).Trim();
            }

            return part1;
        }


    }
}
