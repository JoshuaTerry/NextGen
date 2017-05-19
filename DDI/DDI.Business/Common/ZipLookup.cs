using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DDI.Business.Helpers;
using DDI.Shared;
using DDI.Shared.Enums.Common;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.Common;

namespace DDI.Business.Common
{
    /// <summary>
    /// Business logic class for zip code lookups
    /// </summary>
    public class ZipLookup : IDisposable
    {
        private char[] _trimChars = new char[] { ',', ' ', '#', '.', '(', ')', '"', ':', ':', '\'', '@', '&' };
        #region Fields

        private static IList<Abbreviation> _abbreviations = null;
        private IUnitOfWork _uow = null;
        private string[] _writtenNumbers = { "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN" };

        #endregion

        #region Constructors 
        public ZipLookup() : this(Factory.CreateUnitOfWork()) { }

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
        public string GetZipPlus4(ref Address addressToFormat, out string plus4Zip)
        {

            plus4Zip = string.Empty;

            string fullAddressLines = string.Join(" ", addressToFormat.AddressLine1, addressToFormat.AddressLine2);
            int rating = 99999999;
            ZipPlus4 bestZip4 = null;
            List<string> zipList = new List<string>();
            string preferredBranch = string.Empty;

            Initialize();

            USPSAddress workAddr = CreateFormattedAbbreviatedAddressLines(fullAddressLines);
            string zipCode = addressToFormat.PostalCode ?? string.Empty;
            if (zipCode.Length > 5)
            {
                zipCode = zipCode.Substring(0, 5);
            }

            if (addressToFormat.State != null)
            {
                _uow.Attach(addressToFormat.State);
            }

            if (zipCode.Length == 5)
            {
                Zip z = _uow.GetEntities<Zip>(p => p.City.State, p => p.ZipBranches).FirstOrDefault(p => p.ZipCode == zipCode);

                if (z != null)
                {
                    preferredBranch = GetPreferredBranch(z);

                    // If no city was provided, or if the city doesn't match the preferred or any other branch, set the city to the preferred branch name.
                    var addressToFormatCity = addressToFormat.City;
                    if (string.IsNullOrWhiteSpace(addressToFormat.City) || (addressToFormat.City != preferredBranch && !z.ZipBranches.Any(p => p.Description == addressToFormatCity)))
                    {
                        addressToFormat.City = preferredBranch;
                    }
                    // Set the state code.
                    addressToFormat.State = z.City.State;
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
            else if (string.IsNullOrWhiteSpace(zipCode) && !string.IsNullOrWhiteSpace(addressToFormat.City))
            {
                // No ZIP provided, use city & state to find branches & build a list of ZIPs.
                var addressToFormatCity = addressToFormat.City;
                var addressToFormatState = addressToFormat.State;
                foreach (var branch in _uow.GetEntities<ZipBranch>(p => p.Zip).Where(p => p.Description == addressToFormatCity && p.Zip.City.StateId == addressToFormatState.Id))
                //new XPCollection<ZipBranch>(uow, CriteriaOperator.Parse("Description == ? && Zip.City.State.StateCode == ?", addr.City, addr.StateCode)))
                {
                    if (!zipList.Contains(branch.Zip.ZipCode))
                    {
                        zipList.Add(branch.Zip.ZipCode);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(fullAddressLines))
            {

                USPSAddress tempAddr = new USPSAddress(workAddr);
                foreach (string zipItem in zipList)
                {
                    zipCode = zipItem;
                    workAddr.CopyFrom(tempAddr);

                    Zip zip = _uow.GetEntities<Zip>(p => p.ZipStreets).FirstOrDefault(p => p.ZipCode == zipItem);

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
                } // each item in ZipList
            } // Address not empty

            if (bestZip4 == null)
            {
                // Zip+4 lookup failed.

                if (zipList.Count > 0)
                {
                    // Try to find the best 5-digit Zip.

                    string zipItem = null;
                    if (!string.IsNullOrWhiteSpace(addressToFormat.PostalCode))
                    {
                        var addressToFormatPostalCode = addressToFormat.PostalCode;
                        zipItem = zipList.FirstOrDefault(p => addressToFormatPostalCode.StartsWith(p));
                    } 
                    else if (zipList.Count == 1)
                    {
                        zipItem = zipList[0];
                        addressToFormat.PostalCode = zipList[0]; // Go ahead and fill in the zip code if none was provided and only one match found.
                    }
                    else 
                    {
                        zipItem = zipList[0];
                    }

                    if (zipItem != null)
                    {
                        Zip zip = _uow.GetEntities<Zip>().FirstOrDefault(p => p.ZipCode == zipItem);

                        // If necessary, populate state, country, and county.
                        if (addressToFormat.State == null)
                        {
                            addressToFormat.State = zip.City?.State;
                        }

                        if (addressToFormat.Country == null)
                        {
                            addressToFormat.Country = addressToFormat.State?.Country;
                        }

                        _uow.LoadReference(zip, p => p.City);

                        if (addressToFormat.County == null && zip.City != null)
                        {
                            addressToFormat.County = _uow.GetReference(zip.City, p => p.County);
                        }
                    }
                }

                return string.Empty;
            }


            // Zip+4 found.  Best ZIP code in zipCode.

            addressToFormat.PostalCode = zipCode;

            // Get the county and country.
            if (addressToFormat.County == null)
            {
                _uow.LoadReference(bestZip4.ZipStreet.Zip, p => p.City);
                _uow.LoadReference(bestZip4.ZipStreet.Zip.City, p => p.County);

                addressToFormat.County = bestZip4.ZipStreet.Zip.City.County;
            }

            if (addressToFormat.Country == null && addressToFormat.State != null)
            {
                addressToFormat.Country = _uow.GetReference(addressToFormat.State, p => p.Country);
            }

            // Build result address
            List<string> resultList = new List<string>();
            resultList.Add(workAddr.StreetNum);

            if (!string.IsNullOrWhiteSpace(bestZip4.ZipStreet.Prefix))
            {
                resultList.Add(bestZip4.ZipStreet.Prefix);
            }
            if (!string.IsNullOrWhiteSpace(bestZip4.ZipStreet.Street))
            {
                resultList.Add(bestZip4.ZipStreet.Street);
            }
            if (!string.IsNullOrWhiteSpace(bestZip4.ZipStreet.Suffix))
            {
                resultList.Add(bestZip4.ZipStreet.Suffix);
            }
            if (!string.IsNullOrWhiteSpace(bestZip4.ZipStreet.Suffix2))
            {
                resultList.Add(bestZip4.ZipStreet.Suffix2);
            }

            if (!string.IsNullOrWhiteSpace(bestZip4.SecondaryAbbreviation))
            {
                resultList.Add(bestZip4.SecondaryAbbreviation);
                if (!string.IsNullOrWhiteSpace(workAddr.SecondaryNum))
                {
                    resultList.Add(workAddr.SecondaryNum);
                }
            }

            plus4Zip = string.Join(" ", resultList);

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
                addressToFormat.PostalCode = addressToFormat.PostalCode + "-" + plus4;
            }
            return plus4;

        }
        #endregion

        #region Private/Internal Methods

        internal void Initialize()
        {
            if (_abbreviations == null)
            {
                _abbreviations = AbbreviationHelper.GetAbbreviations(_uow);                
            }
        }

        private string GetPreferredBranch(Zip zip)
        {
            ZipBranch zb = zip.ZipBranches?.FirstOrDefault(p => p.IsPreferred)
                                ??
                           zip.ZipBranches?.FirstOrDefault();
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

        internal List<ZipStreet> GetStreetList(IEnumerable<ZipStreet> zipStreets, USPSAddress workAddr)
        {
            if (zipStreets == null)
            {
                return new List<ZipStreet>();
            }

            int passNum = 0;
            USPSAddress tempAddr = new USPSAddress(workAddr);

            List<ZipStreet> streetList1 = new List<ZipStreet>();
            List<ZipStreet> streetList2 = new List<ZipStreet>();
            List<ZipStreet> streetList3 = new List<ZipStreet>();

            while (true)
            {
                // Get the set of matching ZipStreets
                string abbrStreet = AbbreviateWordsFromAddressLine(workAddr.Street);
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
        internal USPSAddress CreateFormattedAbbreviatedAddressLines(string text)
        {
            USPSAddress resultAddress = new USPSAddress();
            if (IsRuralRoute(text, out resultAddress))
            {
                return resultAddress;
            }
            if (IsHighwayContract(text, out resultAddress))
            {
                return resultAddress;
            }
            if (IsMilitaryBox(text, out resultAddress))
            {
                return resultAddress;
            }
            if (IsPOBox(text, out resultAddress))
            {
                return resultAddress;
            }
            text = FormatForApartment(text);
            List<string> words = SplitStringIntoListOfWords(text.ToUpper());
            List<string> abbreviatedWords = new List<string>();

            foreach (var word in words)
            {
                abbreviatedWords.Add(GetAbbreviatedWord(word));
            }

            // Empty address?
            if (abbreviatedWords.Count == 0)
            {
                return resultAddress;
            }

            // Logic from "wordclass" procedure in z4spladr.p

            // First, scan for a secondary address:  APT n, BLDG n, FLR n, etc.
            FormatSecondaryAddressLineWords(ref abbreviatedWords, ref resultAddress, ref words);

            RemoveEmptyWordsFromEnd(ref abbreviatedWords, ref words);

            FormattAddress(abbreviatedWords, words, ref resultAddress);

            if (string.IsNullOrWhiteSpace(resultAddress.Street))
            {
                resultAddress.Street = resultAddress.Prefix;
                resultAddress.Prefix = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(resultAddress.Street))
            {
                resultAddress.Street = resultAddress.Suffix;
                resultAddress.Suffix = string.Empty;
            }

            if (resultAddress.Street.StartsWith("MC "))
            {
                resultAddress.Street = "MC" + resultAddress.Street.Substring(3);
            }

            return resultAddress;
        }

        private string FormatForApartment(string text)
        {
            return Regex.Replace(text, AddressStrings.ApartmentRegex, $" {AddressStrings.ApartmentAbbreviation} ");
        }

        private bool IsMilitaryBox(string text, out USPSAddress resultAddress)
        {
            resultAddress = new USPSAddress();
            Regex regex = new Regex(AddressStrings.MilitaryBoxRegex, RegexOptions.ExplicitCapture);
            if (regex.IsMatch(text))
            {
                var matches = regex.Match(text).Groups;
                resultAddress.Street = $"{matches[AddressStrings.MilitaryRegexGroupName].Value.Trim(_trimChars)} {matches[AddressStrings.StreetRegexGroupName].Value.Trim(_trimChars)}";
                resultAddress.StreetNum = matches[AddressStrings.BoxRegexGroupName].Value.Trim(_trimChars);
                return true;
            }
            return false;
        }

        private bool IsRuralRoute(string text, out USPSAddress resultAddress)
        {
            resultAddress = new USPSAddress();
            Regex regex = new Regex(AddressStrings.RuralRouteRegex, RegexOptions.ExplicitCapture);
            if (regex.IsMatch(text))
            {
                var matches = regex.Match(text).Groups;
                resultAddress.Street = $"{AddressStrings.RuralRouteAbbreviation} {matches[AddressStrings.StreetRegexGroupName].Value.Trim(_trimChars)}";
                resultAddress.StreetNum = matches[AddressStrings.BoxRegexGroupName].Value.Trim(_trimChars);
                return true;
            }
            return false;
        }

        private bool IsHighwayContract(string text, out USPSAddress resultAddress)
        {
            resultAddress = new USPSAddress();
            Regex regex = new Regex(AddressStrings.HighwayContractRegex, RegexOptions.ExplicitCapture);
            if (regex.IsMatch(text))
            {
                var matches = regex.Match(text).Groups;
                resultAddress.Street = $"{AddressStrings.HighwayContractAbbreviation} {matches[AddressStrings.StreetRegexGroupName].Value.Trim(_trimChars)}";
                resultAddress.StreetNum = matches[AddressStrings.BoxRegexGroupName].Value.Trim(_trimChars);
                return true;
            }
            return false;
        }

        private bool IsPOBox(string text, out USPSAddress resultAddress)
        {
            resultAddress = new USPSAddress();
            Regex regex = new Regex(AddressStrings.POBoxRegex);
            if (regex.IsMatch(text))
            {
                var regexPieces = regex.Split(text);
                if (regexPieces.Length == 2)
                {
                    resultAddress.Street = AddressStrings.POBoxAbbreviation;
                    if (string.IsNullOrWhiteSpace(regexPieces[1]))
                    {
                        resultAddress.StreetNum = regexPieces[0].Trim(_trimChars);
                    }
                    else
                    {
                        resultAddress.StreetNum = regexPieces[1].Trim(_trimChars);
                        resultAddress.SecondaryNum = regexPieces[0].Trim(_trimChars);
                    }
                    return true;
                }
            }
            return false;
        }

        private void FormattAddress(List<string> abbreviatedWords, List<string> words, ref USPSAddress resultAddress)
        {
            int index;
            int suffixLen = int.MaxValue;
            int suffix2Len = 0;
            string tempSuffix = string.Empty;
            string tempSuffix2 = string.Empty;


            for (index = 0; index < abbreviatedWords.Count; index++)
            {
                string abbr = abbreviatedWords[index];
                string word = words[index];
                if (string.IsNullOrWhiteSpace(abbr))
                {
                    continue;
                }

                if (AbbreviationHelper.IsDirectional(abbr))
                {
                    if (!string.IsNullOrWhiteSpace(resultAddress.Street) && (index == abbreviatedWords.Count - 1 || !string.IsNullOrWhiteSpace(resultAddress.Suffix2)))
                    {
                        resultAddress.Suffix2 = abbr;
                        tempSuffix2 = word;
                        suffix2Len = resultAddress.Street.Length;
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(resultAddress.Street) && string.IsNullOrWhiteSpace(resultAddress.Prefix))
                    {
                        resultAddress.Prefix = abbr;
                        continue;
                    }
                }
                else
                {
                    var suffix = _abbreviations.FirstOrDefault(p => p.Word == abbr && p.IsSuffix == true);
                    if (suffix != null)
                    {
                        if (!string.IsNullOrWhiteSpace(resultAddress.Suffix2))
                        {
                            resultAddress.Street = (resultAddress.Street.Substring(0, suffix2Len) + " " + tempSuffix2 +
                                                    resultAddress.Street.Substring(suffix2Len)).Trim();
                            resultAddress.Suffix2 = string.Empty;
                        }

                        if (!string.IsNullOrWhiteSpace(resultAddress.Suffix))
                        {
                            resultAddress.Street = (resultAddress.Street.Substring(0, suffixLen) + " " + tempSuffix +
                                                    resultAddress.Street.Substring(suffixLen)).Trim();
                        }

                        suffixLen = resultAddress.Street.Length;
                        resultAddress.Suffix = abbr;
                        tempSuffix = word;
                        continue;
                    }
                }

                if (!string.IsNullOrWhiteSpace(resultAddress.Suffix))
                {
                    resultAddress.Street = (resultAddress.Street + " " + tempSuffix).Trim();
                    resultAddress.Suffix = string.Empty;
                }

                //  Street number may not begin with a digit.  Examples include "ONE", A9A.  Anything with a digit could be a street number.
                if (word.Length > 0)
                {
                    if ((resultAddress.StreetNum.Length == 0 && _writtenNumbers.Contains(word))
                        ||
                        Regex.IsMatch(word, @"\d"))
                    {
                        if (string.IsNullOrWhiteSpace(resultAddress.Street) && string.IsNullOrWhiteSpace(resultAddress.Prefix) && string.IsNullOrWhiteSpace(resultAddress.Suffix) &&
                            !word.Contains("ST") && !word.Contains("ND") && !word.Contains("RD") && !word.Contains("TH"))
                        {
                            if (word.Contains('/'))
                            {
                                resultAddress.StreetNum = (resultAddress.StreetNum + " " + word).Trim();
                            }
                            else
                            {
                                resultAddress.StreetNum = resultAddress.StreetNum + word;
                            }
                        }
                        else
                        {
                            resultAddress.Street = (resultAddress.Street + " " + word).Trim();
                        }
                    }
                    else
                    {
                        resultAddress.Street = (resultAddress.Street + " " + word).Trim();
                    }
                }
            }
        }

        private void RemoveEmptyWordsFromEnd(ref List<string> abbreviatedWords, ref List<string> words)
        {
            abbreviatedWords = abbreviatedWords.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
            words = words.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
        }

        private void FormatSecondaryAddressLineWords(ref List<string> abbreviatedWords, ref USPSAddress resultAddress, ref List<string> words)
        {
            for (int index = abbreviatedWords.Count - 1; index >= 0; index--)
            {
                var abbreviatedWord = abbreviatedWords[index];
                var sec = _abbreviations.FirstOrDefault(p => p.Word == abbreviatedWord && p.IsSecondary == true);
                if (sec != null)
                {
                    // It might be "APARTMENT ROAD" or "FLOOR STREET" - these aren't secondary addresses.
                    if (index + 1 < abbreviatedWords.Count)
                    {
                        var nextAbbreviatedWord = abbreviatedWords[index + 1];
                        if (_abbreviations.Any(p => p.Word == nextAbbreviatedWord && p.IsSuffix))
                        {
                            continue;
                        }
                    }

                    resultAddress.SecondaryAbbr = sec.USPSAbbreviation;

                    // If the secondary is at the end of the address, the number is the previous word in some cases.
                    if (index == abbreviatedWords.Count - 1 && index > 0 && "BLDG,DEPT,FLR,OFC,RM,STE,UNIT".Contains(abbreviatedWord))
                    {
                        resultAddress.SecondaryNum = words[index - 1];
                        words[index - 1] = string.Empty;
                        abbreviatedWords[index - 1] = string.Empty;
                    }

                    else
                    {
                        // Build the secondary address number:  APT 5 A => APT 5A
                        for (int pos = index + 1; pos < abbreviatedWords.Count; pos++)
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
                                resultAddress.SecondaryNum = (resultAddress.SecondaryNum + " " + term).Trim();
                            }
                            else
                            {
                                resultAddress.SecondaryNum = resultAddress.SecondaryNum + term;
                            }
                            words[pos] = string.Empty;
                            abbreviatedWords[pos] = string.Empty;
                        }
                    }

                    words[index] = string.Empty;
                    abbreviatedWords[index] = string.Empty;

                    // Convert ordinal numbers to regular numbers in secondary address number.
                    if (resultAddress.SecondaryNum == "1ST")
                    {
                        resultAddress.SecondaryNum = "1";
                    }
                    else if (resultAddress.SecondaryNum == "2ND")
                    {
                        resultAddress.SecondaryNum = "2";
                    }
                    else if (resultAddress.SecondaryNum == "3RD")
                    {
                        resultAddress.SecondaryNum = "3";
                    }
                    else if (resultAddress.SecondaryNum.Length >= 3 && char.IsDigit(resultAddress.SecondaryNum[0]) && resultAddress.SecondaryNum.EndsWith("TH"))
                    {
                        resultAddress.SecondaryNum = resultAddress.SecondaryNum.Substring(0, resultAddress.SecondaryNum.Length - 2);
                    }

                    break;
                }
            } // secondary address logic
        }

        /// <summary>
        /// Convert a written number like "FIVE" to "5".
        /// </summary>
        private string ConvertWrittenNumber(string text)
        {
            if (_writtenNumbers.Contains(text))
            {
                for (int n = 0; n < 9; n++)
                {
                    if (_writtenNumbers[n] == text)
                    {
                        text = (n + 1).ToString();
                        break;
                    }
                }
            }

            return text;

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

            addrNum = ConvertWrittenNumber(addrNum.ToUpper());
            addrLow = ConvertWrittenNumber(addrLow.ToUpper());
            addrHigh = ConvertWrittenNumber(addrHigh.ToUpper());

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
            string pattern = @"([0-9\/]+)"; //split on any non-alpha character, include all pieces in the result
            var result = Regex.Split(text, pattern).Where(a => !string.IsNullOrEmpty(a)).ToList();
            return result;
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
        internal List<string> SplitStringIntoListOfWords(string text)
        {
            return text.Split(new[] {' ', '.', ',', '\'', '"', '-'}).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
        }

        /// <summary>
        /// Abbreviate all words in a string.  (String must be all caps.)
        /// </summary>
        internal string AbbreviateWordsFromAddressLine(string originalAddressLine)
        {
            StringBuilder rslt = new StringBuilder(); // result string

            string pattern = "([^A-Za-z])"; //split on any non-alpha character, include all pieces in the result
            string[] result = Regex.Split(originalAddressLine, pattern);
            foreach (var eachWordSegment in result)
            {
                rslt.Append(GetAbbreviatedWord(eachWordSegment));
            }

            return rslt.ToString();
        }

        internal string GetAbbreviatedWord(string word)
        {
            if (word == ".")
            {
                return string.Empty;
            }
            var abbr = _abbreviations.FirstOrDefault(p => p.Word == word && !string.IsNullOrEmpty(p.AddressWord))?.USPSAbbreviation ?? word;
            return abbr;
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
        internal class USPSAddress
        {
            public string Prefix { get; set; }
            public string Street { get; set; }
            public string Suffix { get; set; }
            public string Suffix2 { get; set; }
            public string StreetNum { get; set; }
            public string SecondaryAbbr { get; set; }
            public string SecondaryNum { get; set; }
            public USPSAddress()
            {
                Prefix = Street = Suffix = Suffix2 = StreetNum = SecondaryAbbr = SecondaryNum = string.Empty;
            }

            public USPSAddress(USPSAddress c) : base()
            {
                CopyFrom(c);
            }

            public void CopyFrom(USPSAddress c)
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
