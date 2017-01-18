using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Common;
using DDI.Shared;
using DDI.Shared.Caching;

namespace DDI.Business.Helpers
{
    /// <summary>
    /// Class for abbreviating and expanding words in strings and providing a cached list of abbreviations.
    /// </summary>
    public static class AbbreviationHelper
    {
        #region Private Fields

        private const string ABBREVIATIONS_KEY = "ABBR";
        private const int ABBREVIATIONS_TIMEOUT = 3600;

        private static IEnumerable<string> _suffixesWithPeriods, _prefixesWithPeriods, _numericSuffixes, _directionals;

        #endregion

        #region Constructors

        static AbbreviationHelper()
        {
            _suffixesWithPeriods = "JR,SR,INC".Split(',');
            _numericSuffixes = "II,III,IV".Split(',');
            _prefixesWithPeriods = "MR,MRS,MS,DR,REV,ST,JR,SR,INC".Split(',');
            _directionals = "E,S,W,N,SE,NE,SW,NW".Split(',');
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get a cached set of abbreviations.
        /// </summary>
        public static IList<Abbreviation> GetAbbreviations()
        {
            return GetAbbreviations(null);
        }

        /// <summary>
        /// Get a cached set of abbreviations.
        /// </summary>
        public static IList<Abbreviation> GetAbbreviations(IUnitOfWork uow)
        {
            // Cannot use CachedRepository because we don't want to create static reference to a cached repository.
            IList<Abbreviation> list = CacheHelper.GetEntry(ABBREVIATIONS_KEY, ABBREVIATIONS_TIMEOUT, false,
                () => GetAbbreviationsFromDatabase(uow), null);
            return list;
        }

        /// <summary>
        /// Provide a set of abbreviations.  This method is only intended to be called by the unit testing infrastructure.
        /// </summary>
        /// <param name="abbreviations"></param>
        internal static void SetAbbreviations(IEnumerable<Abbreviation> abbreviations)
        {
            CacheHelper.SetEntry(ABBREVIATIONS_KEY, abbreviations.ToList(), ABBREVIATIONS_TIMEOUT, false, null);
        }

        private static IList<Abbreviation> GetAbbreviationsFromDatabase(IUnitOfWork uow)
        {
            if (uow != null)
            {
                return uow.GetEntities<Abbreviation>().ToList();
            }
            else
            {
                using (var context = new UnitOfWorkEF())
                {
                    return context.GetEntities<Abbreviation>().ToList();
                }
            }
        }

        /// <summary>
        /// Abbreviate words in a name.
        /// </summary>
        /// <param name="text">The name to abbreviate.</param>
        /// <param name="allCaps">True to return result in all caps.</param>
        /// <param name="abbreviateAll">True to abbreviate every possible word.</param>
        public static string AbbreviateNameLine(string text, bool allCaps, bool abbreviateAll)
        {
            Token[] tokens = TokenizeNameLine(text, abbreviateAll);
            StringBuilder sb = new StringBuilder();

            foreach (var token in tokens)
            {
                if (string.IsNullOrEmpty(token.AbbrevWord))
                {
                    if (allCaps)
                    {
                        sb.Append(token.Word.ToUpper());
                    }
                    else
                    {
                        sb.Append(token.Word);
                    }
                }
                else
                {
                    if (allCaps || token.Capitalize)
                    {
                        sb.Append(token.AbbrevWord);
                    }
                    else
                    {
                        sb.Append(StringHelper.FormalCase(token.AbbrevWord));
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Abbreviate words in a name.
        /// </summary>
        /// <param name="text">The name to abbreviate.</param>
        /// <param name="targetLength">Maximum allowed length</param>
        /// <param name="allCaps">True to return result in all caps.</param>
        public static string AbbreviateNameLine(string text, int targetLength, bool allCaps)
        {
            Token[] tokens = TokenizeNameLine(text, false);
            StringBuilder sb = new StringBuilder();
            int len = 0;

            foreach (var token in tokens)
            {
                len += token.Word.Length;
            }

            if (len > targetLength)
            {
                foreach (var token in tokens.Where(p => p.Priority > 0 && !string.IsNullOrEmpty(p.AbbrevWord)).OrderByDescending(p => p.Priority))
                {
                    len -= (token.Word.Length - token.AbbrevWord.Length);
                    if (!(allCaps || token.Capitalize))
                    {
                        token.Word = StringHelper.FormalCase(token.AbbrevWord);
                    }

                    if (len <= targetLength)
                    {
                        break;
                    }
                }
            }

            // Final steps if length is still too long
            if (len > targetLength)
            {
                foreach (var token in tokens)
                {
                    if (token.Word.ToUpper() == "AND")
                    {
                        // Convert "AND" to "&"
                        token.Word = "&";
                    }
                    else if (token.Word.EndsWith("."))
                    {
                        // Remove trailing periods from words
                        token.Word = token.Word.TrimEnd('.');
                    }
                }
            }


            foreach (var token in tokens)
            {
                if (allCaps)
                {
                    sb.Append(token.Word.ToUpper());
                }
                else
                {
                    sb.Append(token.Word);
                }
            }

            return sb.ToString();

        }

        /// <summary>
        /// Expand all words in a name.
        /// </summary>
        /// <param name="text">The name to expand.</param>
        /// <param name="addPeriods">True to add periods after initials or prefixes.</param>
        /// <returns></returns>
        public static string ExpandNameLine(string text, bool addPeriods)
        {
            Token[] tokens = TokenizeNameLine(text, false);
            StringBuilder sb = new StringBuilder();

            foreach (var token in tokens)
            {
                if (!string.IsNullOrWhiteSpace(token.AbbrevWord) && _suffixesWithPeriods.Contains(token.AbbrevWord.ToUpper()))
                {
                    // Precede specific suffixes by a comma
                    string temp = sb.ToString().TrimEnd();
                    if (!temp.EndsWith(",")) // Make sure there's not already a comma 
                    {
                        sb.Clear();
                        sb.Append(temp);
                        sb.Append(", ");
                    }
                }

                if (string.IsNullOrEmpty(token.ExpandedWord))
                {
                    if (string.IsNullOrWhiteSpace(token.Word) || !char.IsLetter(token.Word[0]))
                    {
                        sb.Append(token.Word);
                    }
                    else
                    {
                        if (_numericSuffixes.Contains(token.Word.ToUpper()))
                        {
                            // Precede specific suffixes by a comma
                            string temp = sb.ToString().TrimEnd();
                            if (!temp.EndsWith(",")) // Make sure there's not already a comma 
                            {
                                sb.Clear();
                                sb.Append(temp);
                                sb.Append(", ");
                            }
                        }


                        sb.Append(token.Word);

                        // Single-character words and some prefixes get a trailing period
                        if (addPeriods &&
                            (token.Word.Length == 1 ||
                             _prefixesWithPeriods.Contains(token.Word.ToUpper())
                            )
                           )
                        {
                            sb.Append('.');
                        }
                    }
                }
                else if (token.Capitalize && token.ExpandedWord == token.Word)
                {
                    sb.Append(token.ExpandedWord);
                }
                else
                {
                    if (token.ExpandedWord == "AND")
                    {
                        sb.Append("and");
                    }
                    else
                    {
                        sb.Append(StringHelper.FormalCase(token.ExpandedWord));
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Abbreviate words in an address.
        /// </summary>
        /// <param name="text">Address to abbreviate.</param>
        /// <param name="allCaps">True to return result in all caps.</param>
        /// <param name="abbreviateAll">True to abbreviate every possible word.</param>
        public static string AbbreviateAddressLine(string text, bool allCaps, bool abbreviateAll)
        {
            Token[] tokens = TokenizeAddressLine(text, abbreviateAll);
            StringBuilder sb = new StringBuilder();

            foreach (var token in tokens)
            {
                if (string.IsNullOrEmpty(token.AbbrevWord))
                {
                    if (allCaps)
                    {
                        sb.Append(token.Word.ToUpper());
                    }
                    else
                    {
                        sb.Append(token.Word);
                    }
                }
                else
                {
                    if (allCaps || token.Capitalize)
                    {
                        sb.Append(token.AbbrevWord);
                    }
                    else
                    {
                        sb.Append(StringHelper.FormalCase(token.AbbrevWord));
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Abbreviate words in an address.
        /// </summary>
        /// <param name="text">The address to abbreviate.</param>
        /// <param name="targetLength">Maximum allowed length</param>
        /// <param name="allCaps">True to return result in all caps.</param>
        public static string AbbreviateAddressLine(string text, int targetLength, bool allCaps)
        {
            Token[] tokens = TokenizeAddressLine(text, false);
            StringBuilder sb = new StringBuilder();
            int len = 0;

            foreach (var token in tokens)
            {
                len += token.Word.Length;
            }

            if (len > targetLength)
            {
                foreach (var token in tokens.Where(p => p.Priority > 0 && !string.IsNullOrEmpty(p.AbbrevWord)).OrderByDescending(p => p.Priority))
                {
                    len -= (token.Word.Length - token.AbbrevWord.Length);
                    if (!(allCaps || token.Capitalize))
                    {
                        token.Word = StringHelper.FormalCase(token.AbbrevWord);
                    }

                    if (len <= targetLength)
                    {
                        break;
                    }
                }
            }

            // Final steps if length is still too long
            if (len > targetLength)
            {
                foreach (var token in tokens)
                {
                    if (token.Word.ToUpper() == "AND")
                    {
                        // Convert "AND" to "&"
                        token.Word = "&";
                    }
                    else if (token.Word.EndsWith("."))
                    {
                        // Remove trailing periods from words
                        token.Word = token.Word.TrimEnd('.');
                    }
                }
            }

            foreach (var token in tokens)
            {
                if (allCaps)
                {
                    sb.Append(token.Word.ToUpper());
                }
                else
                {
                    sb.Append(token.Word);
                }
            }

            return sb.ToString();

        }

        /// <summary>
        /// Expand words in an address.
        /// </summary>
        /// <param name="text">Address to expand.</param>
        /// <returns></returns>
        public static string ExpandAddressLine(string text)
        {
            Token[] tokens = TokenizeAddressLine(text, false);
            StringBuilder sb = new StringBuilder();

            foreach (var token in tokens)
            {
                if (string.IsNullOrEmpty(token.ExpandedWord))
                {
                    sb.Append(token.Word);
                }
                else if (token.Capitalize)
                {
                    sb.Append(token.ExpandedWord);
                }
                else
                {
                    sb.Append(StringHelper.FormalCase(token.ExpandedWord));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Return TRUE if a word is a directional abbreviation such as "N".
        /// </summary>
        public static bool IsDirectional(string word)
        {
            return !string.IsNullOrEmpty(word) && _directionals.Contains(word);
        }

        #endregion

        #region Private Methods

        private static Token[] TokenizeNameLine(string text, bool abbreviateAll)
        {
            Token[] tokens = Tokenize(text);
            int idx;
            bool skipDirectional = false;

            // Process the words in reverse order.
            for (idx = tokens.Length - 1; idx >= 0; idx--)
            {
                string thisword = tokens[idx].Word;

                if (!string.IsNullOrWhiteSpace(thisword))
                {

                    // Remove trailing periods from words
                    if (thisword.EndsWith("."))
                    {
                        thisword = thisword.TrimEnd('.');
                    }


                    // Is this word a name abbreviation or word?
                    var abbr = GetAbbreviations().FirstOrDefault(p => string.Compare(thisword, p.Word, true) == 0 && !string.IsNullOrEmpty(p.NameWord));
                    if (abbr != null)
                    {

                        tokens[idx].Capitalize = abbr.IsCaps;
                        tokens[idx].Priority = abbr.Priority + 1;

                        // Expansion:
                        if (!string.IsNullOrEmpty(abbr.NameWord) && abbr.NameWord.Length > thisword.Length)
                        {
                            // Don't expand ST to STREET unless it's at the end.  But if it's at the start, expand it to "SAINT"
                            if (idx == tokens.Length - 1 || thisword.ToUpper() != "ST")
                            {
                                tokens[idx].ExpandedWord = abbr.NameWord;
                            }
                            else if (idx == 0 && thisword.ToUpper() == "ST")
                            {
                                tokens[idx].ExpandedWord = "SAINT";
                            }
                        }

                        // If this is a directional (east, west, etc.) skip it if we just abbreviated a suffix word.
                        if (!abbreviateAll && skipDirectional && IsDirectional(abbr.USPSAbbreviation))
                        {
                            skipDirectional = false;
                        }
                        else
                        {
                            // If this is a suffix word, don't abbreviate the directional that might precede it.  (e.g. East St.)
                            skipDirectional = abbr.IsSuffix;

                            // Replace this word with the abbreviation.
                            tokens[idx].AbbrevWord = abbr.USPSAbbreviation;
                        }

                    }
                    else
                    {
                        skipDirectional = false;
                    }
                }
            }

            return tokens;
        } // TokenizeNameLine

        private static Token[] TokenizeAddressLine(string text, bool abbreviateAll)
        {
            Token[] tokens = Tokenize(text);
            int idx;
            bool abbrevFound = false;
            bool skipDirectional = false;

            // Process the words in reverse order.
            for (idx = tokens.Length - 1; idx >= 0; idx--)
            {
                string thisword = tokens[idx].Word;

                if (!string.IsNullOrWhiteSpace(thisword))
                {

                    // Remove trailing periods from words
                    if (thisword.EndsWith("."))
                    {
                        thisword = thisword.TrimEnd('.');
                    }

                    // Is this word an address abbreviation or word?
                    var abbr = GetAbbreviations().FirstOrDefault(p => string.Compare(thisword, p.Word, true) == 0 && p.AddressWord.Length > 0);
                    if (abbr != null)
                    {

                        tokens[idx].Capitalize = abbr.IsCaps;
                        tokens[idx].Priority = abbr.Priority + 1;

                        // Expansion:
                        if (abbr.AddressWord.Length > thisword.Length)
                        {
                            // Don't expand ST to STREET unless it's at the end.  But if it's at the start, expand it to "SAINT"
                            if (idx == tokens.Length - 1 || thisword.ToUpper() != "ST")
                            {
                                tokens[idx].ExpandedWord = abbr.AddressWord;
                            }
                            else if (idx == 0 && thisword.ToUpper() == "ST")
                            {
                                tokens[idx].ExpandedWord = "SAINT";
                            }
                        }

                        // Abbreviate suffixes only if they are at the end.  Suffixes are usually found at the end of an address line and there should be only one of these.
                        if (abbreviateAll || !(abbrevFound && abbr.IsSuffix))
                        {
                            // If this is a directional (east, west, etc.) skip it if we just abbreviated a suffix word.
                            if (!abbreviateAll && skipDirectional && IsDirectional(abbr.USPSAbbreviation))
                            {
                                skipDirectional = false;
                            }
                            else
                            {
                                // Mark that an abbreviation has been found.  Trailing directional doesn't count.
                                abbrevFound = abbrevFound || !IsDirectional(abbr.USPSAbbreviation);

                                // If this is a suffix word, don't abbreviate the directional that might precede it.  (e.g. East St.)
                                skipDirectional = abbr.IsSuffix;

                                // Replace this word with the abbreviation.
                                tokens[idx].AbbrevWord = abbr.USPSAbbreviation;
                            }

                        }
                        else
                        {
                            skipDirectional = false;
                        }
                    }
                    else
                    {
                        skipDirectional = false;
                    }
                }
            }

            return tokens;
        } // TokenizeAddressLine

        /// <summary>
        /// FSM state values used for Tokenize
        /// </summary>
        private enum TokenizeState
        {
            None, ExpectTable, Whitespace, Word, QuotedString
        }

        /// <summary>
        /// Convert a string into word tokens
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static Token[] Tokenize(string text)
        {
            List<Token> tlist = new List<Token>();

            if (string.IsNullOrWhiteSpace(text))
            {
                return tlist.ToArray();
            }

            int len = text.Length;
            TokenizeState state = TokenizeState.None;
            StringBuilder sb = new StringBuilder();
            char quotechar = ' ';

            int idx = 0;
            while (idx < len)
            {
                char c = text[idx++];
                char cnext = (idx < len ? text[idx] : ' ');

                switch (state)
                {
                    case TokenizeState.None:  // Start state
                        if (char.IsWhiteSpace(c))
                        {
                            tlist.Add(new Token(c));
                            state = TokenizeState.Whitespace;
                        }
                        else if (IsWordChar(c) && c != '\'')
                        {
                            sb.Clear();
                            sb.Append(c);
                            state = TokenizeState.Word;
                        }
                        else if (c == '"' || c == '\'')
                        {
                            quotechar = c;
                            sb.Clear();
                            sb.Append(c);
                            state = TokenizeState.QuotedString;
                        }
                        else
                        {
                            tlist.Add(new Token(c));
                        }
                        break;

                    case TokenizeState.Whitespace:
                        if (!char.IsWhiteSpace(c))
                        {
                            state = TokenizeState.None;
                            idx--;
                        }
                        else
                        {
                            tlist.Add(new Token(c));
                        }
                        break;

                    case TokenizeState.Word:
                        // Keep appending word characters
                        if (IsWordChar(c))
                        {
                            sb.Append(c);
                        }
                        else
                        {
                            // Otherwise, restart the SM on the same character
                            tlist.Add(new Token(sb.ToString()));
                            state = TokenizeState.None;
                            idx--;
                        }
                        break;

                    case TokenizeState.QuotedString: // A quoted string
                        // Otherwise, keep appending chars to the quoted string, restarting the SM on the ending quote
                        sb.Append(c);
                        if (c == quotechar)
                        {
                            tlist.Add(new Token(sb.ToString()));
                            state = TokenizeState.None;
                        }
                        break;
                } // Switch

            } // for each character...

            // Add any remaining text in the string builder
            if (state == TokenizeState.QuotedString || state == TokenizeState.Word)
            {
                tlist.Add(new Token(sb.ToString()));
            }

            return tlist.ToArray();
        } // Tokenize

        private static bool IsWordChar(char c)
        {
            return char.IsLetterOrDigit(c) || c == '-' || c == '.' || c == '\'';
        }


        #endregion

        #region Internal Classes

        /// <summary>
        /// Class for representing a single token (word) in a string.
        /// </summary>
        private class Token
        {
            public string Word { get; set; }
            public string AbbrevWord { get; set; }
            public string ExpandedWord { get; set; }
            public bool Capitalize { get; set; }
            public int Priority { get; set; }

            public Token(string word)
            {
                this.Word = word;
            }

            public Token(char c)
            {
                this.Word = c.ToString();
            }
        }

        #endregion

    }




}
