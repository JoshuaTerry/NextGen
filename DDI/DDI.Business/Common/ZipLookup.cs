using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Data.Enums.Common;
using DDI.Data.Models.Common;

namespace DDI.Business.Common
{
    /// <summary>
    /// Business logic class for zip code lookups
    /// </summary>
    public class ZipLookup : IDisposable
    {
        #region Fields

        private static List<Abbreviation> _abbreviations = null;
        private IUnitOfWork _uow = null;
        #endregion

        #region Constructors 
        public ZipLookup() : this(new UnitOfWorkEF()) { }

        public ZipLookup(IUnitOfWork uow)
        {
            _uow = uow;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Perform Zip+4 lookup on an address.
        /// </summary>
        /// <returns></returns>
        public string Zip4Lookup(ZipLookupInfo addr, out string resultAddress)
        {

            resultAddress = string.Empty;

            string fullAddr = string.Join(" ", addr.AddressLine1, addr.AddressLine2);
            int rating = 99999999;
            ZipPlus4 bestZip4 = null;
            List<string> zipList = new List<string>();
            string preferredBranch = string.Empty;

            Initialize();

            Address workAddr = SplitAddress(fullAddr);
            string zipCode = addr.PostalCode ?? string.Empty;
            if (zipCode.Length > 5)
                zipCode = zipCode.Substring(0, 5);

            if (addr.State != null)
            {
                _uow.Attach(addr.State);
            }

            if (zipCode.Length == 5)
            {
                Zip z = _uow.GetEntities<Zip>().IncludePath(p => p.City.State).IncludePath(p => p.ZipBranches).FirstOrDefault(p => p.ZipCode == zipCode);

                if (z != null)
                {
                    preferredBranch = GetPreferredBranch(z);

                    // If no city was provided, or if the city doesn't match the preferred or any other branch, set the city to the preferred branch name.
                    if (string.IsNullOrWhiteSpace(addr.City) || (addr.City != preferredBranch && !z.ZipBranches.Any(p => p.Description == addr.City)))
                    {
                        addr.City = preferredBranch;
                    }
                    // Set the state code.
                    addr.State = z.City.State;
                }

                zipList.Add(zipCode);

                if (z != null && z.City != null)
                {
                    _uow.LoadReference(z.City, p => p.Zips);
                    // Add in all the other zips for this city.  They will be checked if we can't find a match for the provided zip.
                    foreach (Zip other in z.City.Zips)
                    {
                        if (!zipList.Contains(other.ZipCode))
                        {
                            zipList.Add(other.ZipCode);
                        }
                    }
                }

            }
            else if (string.IsNullOrWhiteSpace(zipCode) && !string.IsNullOrWhiteSpace(fullAddr))
            {
                // No ZIP provided, use city & state to find branches & build a list of ZIPs.
                foreach (var branch in _uow.GetEntities<ZipBranch>().IncludePath(p => p.Zip).Where(p => p.Description == addr.City && p.Zip.City.StateId == addr.State.Id))
                //new XPCollection<ZipBranch>(uow, CriteriaOperator.Parse("Description == ? && Zip.City.State.StateCode == ?", addr.City, addr.StateCode)))
                {
                    if (!zipList.Contains(branch.Zip.ZipCode))
                    {
                        zipList.Add(branch.Zip.ZipCode);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(fullAddr))
            {
                return string.Empty;
            }

            Address tempAddr = new Address(workAddr);
            foreach (string zipItem in zipList)
            {
                zipCode = zipItem;
                workAddr.CopyFrom(tempAddr);

                Zip zip = _uow.GetEntities<Zip>().IncludePath(p => p.ZipStreets).FirstOrDefault(p => p.ZipCode == zipItem);

                List<ZipStreet> streetList = GetStreetList(zip.ZipStreets, workAddr);

                foreach (ZipStreet item in streetList)
                {
                    _uow.LoadReference(item, p => p.ZipPlus4s);

                    foreach (ZipPlus4 z4item in item.ZipPlus4s)
                    {
                        int rtemp = 0;

                        if (!string.IsNullOrWhiteSpace(workAddr.StreetNum) || z4item.AddressLow != z4item.SecondaryLow)
                        {
                            if (!CompareAddressNumber(workAddr.StreetNum, z4item.AddressLow, z4item.AddressHigh, z4item.AddressType))
                            {
                                continue;
                            }
                            rtemp = CalcRating(z4item.AddressLow, z4item.AddressHigh);
                            if (rtemp == -1)
                            {
                                rtemp = (string.IsNullOrWhiteSpace(z4item.AddressHigh) && string.IsNullOrWhiteSpace(z4item.AddressLow) ? 0 : rating);
                            }

                        }
                        else
                            rtemp = (!string.IsNullOrWhiteSpace(workAddr.SecondaryAbbr) ? 0 : rating);

                        if (!string.IsNullOrWhiteSpace(workAddr.SecondaryAbbr))
                        {
                            if (string.IsNullOrWhiteSpace(z4item.SecondaryAbbreviation))
                            {
                                rtemp += 100000;
                                rtemp += CalcRating(z4item.SecondaryLow, z4item.SecondaryHigh);
                                if (rtemp < rating)
                                {
                                    rating = rtemp;
                                    bestZip4 = z4item;
                                }
                                continue;
                            }

                            if (workAddr.SecondaryAbbr != z4item.SecondaryAbbreviation)
                            {
                                rtemp += 2000000;
                            }

                            if (!CompareAddressNumber(workAddr.SecondaryNum, z4item.SecondaryLow, z4item.SecondaryHigh, z4item.SecondaryType))
                            {
                                continue;
                            }
                        }
                        else if (!string.IsNullOrWhiteSpace(z4item.SecondaryAbbreviation))
                        {
                            rtemp += 3000000;
                        }

                        if (rtemp < rating)
                        {
                            rating = rtemp;
                            bestZip4 = z4item;
                        }

                    } // each zip4
                } // each street

                if (bestZip4 != null)
                {
                    break;
                }
            }

            if (bestZip4 == null)
            {
                return string.Empty;
            }

            addr.PostalCode = zipCode;

            if (addr.County == null)
            {
                _uow.LoadReference(bestZip4.ZipStreet.Zip, p => p.City);
                _uow.LoadReference(bestZip4.ZipStreet.Zip.City, p => p.County);

                addr.County = bestZip4.ZipStreet.Zip.City.County;
            }

            if (addr.Country == null && addr.State != null)
            {
                addr.Country = _uow.GetReference(addr.State, p => p.Country);
            }

            // Build result address
            List<string> rslt = new List<string>();
            rslt.Add(workAddr.StreetNum);

            if (!string.IsNullOrWhiteSpace(bestZip4.ZipStreet.Prefix))
                rslt.Add(bestZip4.ZipStreet.Prefix);
            if (!string.IsNullOrWhiteSpace(bestZip4.ZipStreet.Street))
                rslt.Add(bestZip4.ZipStreet.Street);
            if (!string.IsNullOrWhiteSpace(bestZip4.ZipStreet.Suffix))
                rslt.Add(bestZip4.ZipStreet.Suffix);
            if (!string.IsNullOrWhiteSpace(bestZip4.ZipStreet.Suffix2))
                rslt.Add(bestZip4.ZipStreet.Suffix2);

            if (!string.IsNullOrWhiteSpace(bestZip4.SecondaryAbbreviation))
            {
                rslt.Add(bestZip4.SecondaryAbbreviation);
                if (!string.IsNullOrWhiteSpace(workAddr.SecondaryNum))
                    rslt.Add(workAddr.SecondaryNum);
            }

            resultAddress = string.Join(" ", rslt);

            // Non-deliverable address...
            if (bestZip4.Plus4.IndexOf("ND") >= 0)
            {
                return string.Empty;
            }

            string plus4;

            if (!bestZip4.IsRange)
            {
                plus4 = bestZip4.Plus4;
            }
            else
            {
                try
                {
                    int diff = int.Parse(workAddr.StreetNum) - int.Parse(bestZip4.AddressLow);
                    plus4 = (int.Parse(bestZip4.Plus4) + diff).ToString("D4");
                }
                catch
                {
                    plus4 = string.Empty;
                }
            }
            if (plus4.Length > 0)
            {
                addr.PostalCode = addr.PostalCode + "-" + plus4;
            }
            return plus4;

        }
        #endregion

        #region Private/Internal Methods

        internal void Initialize()
        {
            if (_abbreviations == null)
            {
                _abbreviations = _uow.GetEntities<Abbreviation>().ToList();                
            }
        }

        private string GetPreferredBranch(Zip zip)
        {
            ZipBranch zb = zip.ZipBranches.FirstOrDefault(p => p.IsPreferred)
                                ??
                           zip.ZipBranches.FirstOrDefault();
            return zb?.Description ?? string.Empty;
        }


        internal int CalcRating(string low, string high)
        {
            if (low == null)
            {
                low = string.Empty;
            }
            if (high == null)
            {
                high = string.Empty;
            }

            int len = low.Length;

            if (len != high.Length)
            {
                len = MaxInt(len, high.Length);
                low = low.PadRight(len, '0');
                high = high.PadRight(len, '0');
            }

            int rslt = 0;
            for (int idx = 0; idx < len; idx++)
            {
                rslt = rslt * 10 + high[idx] - low[idx];
            }

            return rslt;
        }

        private bool StringSameAs(string s1, string s2)
        {
            return (string.IsNullOrWhiteSpace(s1) && string.IsNullOrWhiteSpace(s2)) ||
                s1 == s2;
        }

        internal List<ZipStreet> GetStreetList(IEnumerable<ZipStreet> zipStreets, Address workAddr)
        {

            int passNum = 0;
            Address tempAddr = new Address(workAddr);

            List<ZipStreet> streetList1 = new List<ZipStreet>();
            List<ZipStreet> streetList2 = new List<ZipStreet>();
            List<ZipStreet> streetList3 = new List<ZipStreet>();

            while (true)
            {
                // Get the set of matching ZipStreets
                string abbrStreet = AbbreviateWords(workAddr.Street);
                foreach (var item in zipStreets.Where(p => p.Street == abbrStreet))
                    //new XPCollection<ZipStreet>(ses, CriteriaOperator.Parse("Zip.ZipCode == ? && Street == ?", zip, abbrStreet)))
                {
                    // If prefix & suffix are specified, ignore explicit mismatches.
                    if (!string.IsNullOrWhiteSpace(item.Prefix) && !string.IsNullOrWhiteSpace(workAddr.Prefix) && item.Prefix != workAddr.Prefix)
                    {
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(item.Suffix) && !string.IsNullOrWhiteSpace(workAddr.Suffix) && item.Suffix != workAddr.Suffix)
                    {
                        continue;
                    }

                    if (!StringSameAs(item.Suffix, workAddr.Suffix) && !StringSameAs(item.Prefix, workAddr.Prefix))
                    {
                        streetList3.Add(item);
                    }
                    else if (!StringSameAs(item.Suffix, workAddr.Suffix) || !StringSameAs(item.Prefix, workAddr.Prefix))
                    {
                        streetList2.Add(item);
                    }
                    else
                    {
                        streetList1.Add(item);
                    }
                }

                if (streetList2.Count == 0)
                {
                    streetList2 = streetList3;
                }

                if (streetList1.Count == 0)
                {
                    streetList1 = streetList2;
                }

                if (streetList1.Count > 0)
                {
                    break;
                }

                // At this point, no matches were found.
                if (passNum == 0 && !string.IsNullOrWhiteSpace(workAddr.Suffix2))
                {
                    workAddr.Street = workAddr.Street + " " + workAddr.Suffix2;
                    workAddr.Suffix2 = string.Empty;
                    passNum = 1;
                }
                else if (passNum <= 1 && !string.IsNullOrWhiteSpace(tempAddr.Suffix))
                {
                    workAddr.Suffix2 = tempAddr.Suffix2;
                    workAddr.Street = tempAddr.Street + " " + workAddr.Suffix;
                    workAddr.Suffix = string.Empty;
                    passNum = 2;
                }
                else if (passNum <= 2 && !string.IsNullOrWhiteSpace(tempAddr.Suffix) && !string.IsNullOrWhiteSpace(tempAddr.Suffix2))
                {
                    workAddr.Street = tempAddr.Street + " " + tempAddr.Suffix + " " + tempAddr.Suffix2;
                    workAddr.Suffix = workAddr.Suffix2 = string.Empty;
                    passNum = 3;
                }
                else if (passNum <= 3 && !string.IsNullOrWhiteSpace(tempAddr.Suffix) && !string.IsNullOrWhiteSpace(tempAddr.Suffix2))
                {
                    workAddr.Street = tempAddr.Street + " " + tempAddr.Suffix2 + " " + tempAddr.Suffix;
                    passNum = 4;
                }
                else break;
            } // while true...

            return streetList1;
        }

        /// <summary>
        /// Split a street address into an Address object.
        /// </summary>
        internal Address SplitAddress(string text)
        {
            Address rslt = new Address();

            int idx;

            List<string> words = WordSplit(text.ToUpper());
            List<string> abbrs = new List<string>();
            WordCombine(words);

            foreach (var word in words)
            {
                abbrs.Add(AbbreviateWords(word));
            }

            // Empty address?
            if (abbrs.Count == 0)
            {
                return rslt;
            }

            // Logic from "wordclass" procedure in z4spladr.p

            // First, scan for a secondary address:  APT n, BLDG n, FLR n, etc.
            for (idx = abbrs.Count - 1; idx >= 0; idx--)
            {
                var sec = _abbreviations.FirstOrDefault(p => p.Word == abbrs[idx] && p.IsSecondary == true);
                if (sec != null)
                {
                    // It might be "APARTMENT ROAD" or "FLOOR STREET" - these aren't secondary addresses.
                    if (idx + 1 < abbrs.Count &&
                        _abbreviations.Any(p => p.Word == abbrs[idx + 1] && p.IsSuffix))
                    {
                        continue;
                    }

                    rslt.SecondaryAbbr = sec.USPSAbbreviation;

                    // If the secondary is at the end of the address, the number is the previous word in some cases.
                    if (idx == abbrs.Count - 1 && idx > 0 && "BLDG,DEPT,FLR,OFC,RM,STE,UNIT".Contains(abbrs[idx]))
                    {
                        rslt.SecondaryNum = words[idx - 1];
                        words[idx - 1] = string.Empty;
                        abbrs[idx - 1] = string.Empty;
                    }

                    else
                    {
                        // Build the secondary address number:  APT 5 A => APT 5A
                        for (int pos = idx + 1; pos < abbrs.Count; pos++)
                        {
                            string term = words[pos];
                            // Stop if we find something that looks like an abbreviation
                            if (_abbreviations.Any(p => p.Word == term && (p.IsSuffix || p.IsSecondary)))
                            {
                                break;
                            }

                            if (term.IndexOf('/') >= 0)
                            {
                                // Something like BLDG 5 1/2
                                rslt.SecondaryNum = (rslt.SecondaryNum + " " + term).Trim();
                            }
                            else
                            {
                                rslt.SecondaryNum = rslt.SecondaryNum + term;
                            }
                            words[pos] = string.Empty;
                            abbrs[pos] = string.Empty;
                        }
                    }

                    words[idx] = string.Empty;
                    abbrs[idx] = string.Empty;

                    // Convert ordinal numbers to regular numbers in secondary address number.
                    if (rslt.SecondaryNum == "1ST")
                    {
                        rslt.SecondaryNum = "1";
                    }
                    else if (rslt.SecondaryNum == "2ND")
                    {
                        rslt.SecondaryNum = "2";
                    }
                    else if (rslt.SecondaryNum == "3RD")
                    {
                        rslt.SecondaryNum = "3";
                    }
                    else if (rslt.SecondaryNum.Length >= 3 && char.IsDigit(rslt.SecondaryNum[0]) && rslt.SecondaryNum.EndsWith("TH"))
                    {
                        rslt.SecondaryNum = rslt.SecondaryNum.Substring(0, rslt.SecondaryNum.Length - 2);
                    }

                    break;
                }
            } // secondary address logic

            // Handle RR, HC

            if (abbrs[0] == "RR" || abbrs[0] == "HC" || abbrs[0] == "R")
            {
                rslt.Street = (abbrs[0] == "HC" ? "HC" : "RR");

                bool foundBox = false;
                for (int pos = 1; pos < words.Count; pos++)
                {
                    if (!foundBox && abbrs[pos] == "BOX")
                    {
                        foundBox = true;
                    }
                    else if (!foundBox)
                    {
                        rslt.Street = (rslt.Street + " " + words[pos]).Trim();
                    }
                    else
                    {
                        rslt.StreetNum = (rslt.StreetNum + " " + words[pos]).Trim();
                    }
                }
                return rslt;
            }

            // Handle PO Box

            if ((abbrs[0] == "PO" && abbrs.Count >= 2 && abbrs[1] == "BOX") || (abbrs[0] == "BOX"))
            {
                rslt.Street = "PO BOX";
                bool foundBox = false;

                for (int pos = 0; pos < words.Count; pos++)
                {
                    if (!foundBox && abbrs[pos] == "BOX")
                    {
                        foundBox = true;
                    }
                    else if (foundBox)
                    {
                        rslt.StreetNum = rslt.StreetNum + words[pos];
                    }
                }
                return rslt;
            }

            // Remove empty entries at the end
            for (idx = abbrs.Count - 1; idx >= 0; idx--)
            {
                if (string.IsNullOrWhiteSpace(abbrs[idx]))
                {
                    abbrs.RemoveAt(idx);
                }
            }

            for (idx = words.Count - 1; idx >= 0; idx--)
            {
                if (string.IsNullOrWhiteSpace(words[idx]))
                {
                    words.RemoveAt(idx);
                }
            }

            int suffixLen = int.MaxValue;
            int suffix2Len = 0;
            string tempSuffix = string.Empty;
            string tempSuffix2 = string.Empty;


            for (idx = 0; idx < abbrs.Count; idx++)
            {
                string abbr = abbrs[idx];
                string word = words[idx];
                if (string.IsNullOrWhiteSpace(abbr))
                {
                    continue;
                }

                if (IsDirectional(abbr))
                {
                    if (!string.IsNullOrWhiteSpace(rslt.Street) && (idx == abbrs.Count - 1 || !string.IsNullOrWhiteSpace(rslt.Suffix2)))
                    {
                        rslt.Suffix2 = abbr;
                        tempSuffix2 = word;
                        suffix2Len = rslt.Street.Length;
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(rslt.Street) && string.IsNullOrWhiteSpace(rslt.Prefix))
                    {
                        rslt.Prefix = abbr;
                        continue;
                    }
                }
                else
                {
                    var suffix = _abbreviations.FirstOrDefault(p => p.Word == abbr && p.IsSuffix == true);
                    if (suffix != null)
                    {
                        if (!string.IsNullOrWhiteSpace(rslt.Suffix2))
                        {
                            rslt.Street = (rslt.Street.Substring(0, suffix2Len) + " " + tempSuffix2 +
                                           rslt.Street.Substring(suffix2Len)).Trim();
                            rslt.Suffix2 = string.Empty;
                        }

                        if (!string.IsNullOrWhiteSpace(rslt.Suffix))
                        {
                            rslt.Street = (rslt.Street.Substring(0, suffixLen) + " " + tempSuffix +
                                           rslt.Street.Substring(suffix2Len)).Trim();

                        }

                        suffixLen = rslt.Street.Length;
                        rslt.Suffix = abbr;
                        tempSuffix = word;
                        continue;
                    }
                }

                if (!string.IsNullOrWhiteSpace(rslt.Suffix))
                {
                    rslt.Street = (rslt.Street + " " + tempSuffix).Trim();
                    rslt.Suffix = string.Empty;
                }

                //  Street number may not begin with a digit.  Examples include "ONE", A9A.  Anything with a digit could be a street number.
                if (word.Length > 0 && ("ONE,TWO,THREE,FOUR,FIVE,SIX,SEVEN,EIGHT,NINE,TEN".IndexOf(word) >= 0 || Regex.IsMatch(word, @"\d"))) // char.IsDigit(word[0]))
                {
                    if (string.IsNullOrWhiteSpace(rslt.Street) && string.IsNullOrWhiteSpace(rslt.Prefix) && string.IsNullOrWhiteSpace(rslt.Suffix) &&
                        !word.Contains("ST") && !word.Contains("ND") && !word.Contains("RD") && !word.Contains("TH"))
                    {
                        if (word.Contains('/'))
                        {
                            rslt.StreetNum = (rslt.StreetNum + " " + word).Trim();
                        }
                        else
                        {
                            rslt.StreetNum = rslt.StreetNum + word;
                        }
                    }
                    else
                    {
                        rslt.Street = (rslt.Street + " " + word).Trim();
                    }
                }
                else
                {
                    rslt.Street = (rslt.Street + " " + word).Trim();
                }
            }

            if (string.IsNullOrWhiteSpace(rslt.Street))
            {
                rslt.Street = rslt.Prefix;
                rslt.Prefix = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(rslt.Street))
            {
                rslt.Street = rslt.Suffix;
                rslt.Suffix = string.Empty;
            }

            if (rslt.Street.StartsWith("MC "))
            {
                rslt.Street = "MC" + rslt.Street.Substring(3);
            }

            return rslt;
        }

        /// <summary>
        /// Address number comparison for Zip+4 lookup
        /// </summary>
        /// <param name="addrNum">Actual address number</param>
        /// <param name="addrLow">Low address number from ZipPlus4</param>
        /// <param name="addrHigh">High address number from ZipPlus4</param>
        /// <param name="evenOdd">EvenOddType property from ZipPlus4</param>
        /// <returns>True if address number is within specified range and matches EvenOddType.</returns>
        internal bool CompareAddressNumber(string addrNum, string addrLow, string addrHigh, EvenOddType evenOdd)
        {
            List<string> addrNumParts, addrLowParts, addrHighParts;

            bool rslt = false;

            addrNum = addrNum.ToUpper();

            // Split the address number params into two parts, one numeric and the other alpha.            

            addrNumParts = SplitNumber(addrNum);
            addrLowParts = SplitNumber(addrLow);
            addrHighParts = SplitNumber(addrHigh);

            // # of parts in addrLow and addrHigh must be the same.  
            if (addrLowParts.Count != addrHighParts.Count)
            {
                return false;
            }

            // USPS should ensure that only one of the parts is variable.  Determine which part this is.
            int variablePart = -1;
            for (int idx = 0; idx < addrLowParts.Count; idx++)
            {
                if (addrLowParts[idx] != addrHighParts[idx])
                {
                    variablePart = idx;
                    break;
                }
            }

            // Determine how many parts need to be checked.
            int numParts = MinInt(addrNumParts.Count, addrLowParts.Count);
            for (int idx = 0; idx < numParts; idx++)
            {
                if (idx != variablePart)
                {
                    // Non-variable part:  address num must match exactly.
                    if (addrNumParts[idx] != addrLowParts[idx])
                        return false;
                }
                else
                {
                    string addrPart = addrNumParts[idx];
                    string lowPart = addrLowParts[idx];
                    string highPart = addrHighParts[idx];

                    // Variable part: pad with spaces on left (for numeric) or right (for alpha) so these are all the same length.
                    int len = MaxInt(addrPart.Length, lowPart.Length, highPart.Length);
                    if (char.IsDigit(addrPart[0]))
                    {
                        addrPart = addrPart.PadLeft(len);
                        lowPart = lowPart.PadLeft(len);
                        highPart = highPart.PadLeft(len);
                    }
                    else
                    {
                        addrPart = addrPart.PadRight(len);
                        lowPart = lowPart.PadRight(len);
                        highPart = highPart.PadRight(len);
                    }

                    // Compare
                    if (string.Compare(addrPart, lowPart) < 0 ||
                        string.Compare(addrPart, highPart) > 0)
                    {
                        return false;
                    }

                    rslt = true;

                    // Even / odd check
                    if (evenOdd != EvenOddType.Any)
                    {
                        int n;
                        if (int.TryParse(addrNum, out n))
                        {
                            if (n > 0)
                            {
                                //        Odd?     ==               Odd?
                                rslt = ((n % 2 == 1) == (evenOdd == EvenOddType.Odd));
                            }
                        }
                    }
                    if (!rslt)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Split text into numeric and non-numeric parts
        /// </summary>
        internal List<String> SplitNumber(string text)
        {
            List<string> rslt = new List<string>();
            bool numeric = false;
            StringBuilder sb = new StringBuilder();

            // for each character...
            foreach (char c in text)
            {
                if (char.IsDigit(c) || (c == '/' && numeric))
                {
                    if (!numeric)  // Start of a numeric part
                    {
                        if (sb.Length > 0)
                        {
                            rslt.Add(sb.ToString()); // Add previous non-numeric part to the list.
                        }
                        sb.Clear();
                        numeric = true;
                    }
                    sb.Append(c);
                }
                else
                {
                    if (numeric) // Start of a non-numeric part
                    {
                        if (sb.Length > 0)
                        {
                            rslt.Add(sb.ToString()); // Add previous numeric part to the list.
                        }
                        sb.Clear();
                        numeric = false;
                    }
                    sb.Append(c);
                }
            }

            // Add any final part to the list.
            if (sb.Length > 0)
            {
                rslt.Add(sb.ToString());
            }

            return rslt;

        }

        /// <summary>
        /// Get maximum integer
        /// </summary>
        private int MaxInt(params int[] args)
        {
            return args.Max();
        }

        /// <summary>
        /// Get minimum integer
        /// </summary>
        private int MinInt(params int[] args)
        {
            return args.Min();
        }


        /// <summary>
        /// Split an address into words delmited by space or .,'"-
        /// </summary>
        internal List<string> WordSplit(string text)
        {
            List<string> rslt = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in text)
            {
                if (" .,'\"-".IndexOf(c) >= 0)
                {
                    if (sb.Length > 0)
                    {
                        rslt.Add(sb.ToString());
                    }
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }

            // Get final entry
            if (sb.Length > 0)
            {
                rslt.Add(sb.ToString());
            }

            return rslt;
        }

        /// <summary>
        /// Combine certain words in a list of words.
        /// </summary>
        internal void WordCombine(List<string> wordList)
        {
            if (wordList.Count < 1)
            {
                return;
            }

            List<string> rslt = new List<string>();
            int startPos = 0;

            // Assumes all words have been capitalized

            if (wordList.Count > 1)
            {
                // P O => PO
                if (wordList[0] == "P" && wordList[1] == "O")
                {
                    wordList[0] = "PO";
                    wordList.RemoveAt(1);
                }

                // R R => RR, RURAL RT => RR, etc.
                else if ((wordList[0] == "RURAL" || wordList[0] == "R") &&
                         (wordList[1] == "RT" || wordList[1] == "R"))
                {
                    wordList[0] = "RR";
                    wordList.RemoveAt(1);
                }

                // H C => HC, etc.
                else if ((wordList[0] == "HWY" || wordList[0] == "H" || wordList[0] == "HIGHWAY") &&
                         (wordList[1] == "CONTRACT" || wordList[1] == "C"))
                {
                    wordList[0] = "HC";
                    wordList.RemoveAt(1);
                }
            }

            // RT => RR
            if (wordList[startPos] == "RT")
                wordList[0] = "RR";

            // Append all remaining words, inserting APT if necessary
            bool aptFound = false;
            for (int pos = startPos; pos < wordList.Count; pos++)
            {
                if (wordList[pos] == "APT" || wordList[pos] == "APARTMENT")
                {
                    aptFound = true;
                    wordList[pos] = "APT";
                }
                else if (wordList[pos].StartsWith("#"))
                {
                    if (!aptFound)
                    {
                        wordList.Insert(pos, "APT");
                        aptFound = true;
                        continue;
                    }

                    if (wordList[pos] == "#")
                    {
                        wordList.RemoveAt(pos);
                        continue;
                    }

                    wordList[pos] = wordList[pos].TrimStart('#');
                }
            }
        }

        /// <summary>
        /// Abbreviate all words in a string.  (String must be all caps.)
        /// </summary>
        internal string AbbreviateWords(string text)
        {
            StringBuilder sb = new StringBuilder(); // word (seq. of letters)
            StringBuilder rslt = new StringBuilder(); // result string
            int idx;

            for (idx = 0; idx < text.Length; idx++)
            {
                char c = text[idx];

                if (char.IsLetter(c))
                {
                    // build sequence of letters
                    sb.Append(c);
                }
                if (idx == text.Length - 1 || !char.IsLetter(c)) // if end of a word or end of the text
                {
                    if (sb.Length > 0)
                    {
                        // Abbreviate the word
                        string word = sb.ToString();
                        var abbr = _abbreviations.FirstOrDefault(p => p.Word == word && p.AddressWord != null && p.AddressWord.Length > 0);
                        if (abbr != null)
                        {
                            rslt.Append(abbr.USPSAbbreviation);
                        }
                        else
                        {
                            rslt.Append(word); // no abbrevation found for this word.
                        }
                        sb.Clear();
                    }

                    // Omit periods, otherwise append non-letters to the result
                    if (c != '.' && !char.IsLetter(c))
                    {
                        rslt.Append(c);
                    }
                }
            }

            return rslt.ToString();
        }

        private bool IsDirectional(string word)
        {
            return !string.IsNullOrEmpty(word) && "E,S,W,N,SE,NE,SW,NW".IndexOf(word) >= 0;
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _uow?.Dispose();
                }

                _uow = null;
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        #region Nested Classes
        /// <summary>
        /// Class for representing address information.
        /// </summary>
        internal class Address
        {
            public string Prefix { get; set; }
            public string Street { get; set; }
            public string Suffix { get; set; }
            public string Suffix2 { get; set; }
            public string StreetNum { get; set; }
            public string SecondaryAbbr { get; set; }
            public string SecondaryNum { get; set; }
            public Address()
            {
                Prefix = Street = Suffix = Suffix2 = StreetNum = SecondaryAbbr = SecondaryNum = string.Empty;
            }

            public Address(Address c) : base()
            {
                CopyFrom(c);
            }

            public void CopyFrom(Address c)
            {
                Prefix = c.Prefix;
                Street = c.Street;
                Suffix = c.Suffix;
                Suffix2 = c.Suffix2;
                StreetNum = c.StreetNum;
                SecondaryAbbr = c.SecondaryAbbr;
                SecondaryNum = c.SecondaryNum;
            }
        }
        #endregion


    }
}
