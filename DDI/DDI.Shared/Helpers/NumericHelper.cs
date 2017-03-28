using System;
using System.Text;

namespace DDI.Shared.Helpers
{
    public static class NumericHelper
    {

        #region Public Methods

        /// <summary>
        /// Convert a decimal value to words (e.g. "one hundred twenty-five")
        /// </summary>
        public static string ToWords(decimal number)
        {
            /* Notes: Code taken from genntxt.p (OpenEdge).  
             * Can handle numbers up to 999 billion, positive or negative.  Also handles decimals to thousandths, including some common fractions like sixteenths.
             */
            StringBuilder sb = new StringBuilder();

            string[] ones = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve",
            "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };

            string[] tens = { "", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            string[] fracs = { "half", "quarter", "fifth", "eighth", "tenth", "sixteenth", "hundredth", "thousandth" };
            decimal[] nfracs = { 2m, 4m, 5m, 8m, 10m, 16m, 100m, 1000m };

            string[] mults = { "billion", "million", "thousand" };

            decimal cm;

            bool needComma = false;
            bool needDash = false;

            if (number < 0m)
            {
                number = -number;
                sb.Append("negative");
            }

            /* Either (a) accept decimal values that are a fraction of 1/16 
                or (b) round to the 3rd decimal place since this is all the logic can handle */

            if (!Is16th(number))
                number = Math.Round(number, 3, MidpointRounding.AwayFromZero);

            cm = 1000000000m; // one billion
            if (number >= cm * 1000m)
                throw new ArgumentException("Number too large to be converted to words.");

            for (int i = 0; i <= 3; i++)
            {
                if (cm <= number)
                {
                    decimal x = Math.Truncate(number / cm);
                    if (needComma)
                    {
                        sb.Append(',');
                    }

                    if (x >= 100m)
                    {
                        sb.Append(' ')
                          .Append(ones[(int)(x / 100m)])
                          .Append(" hundred");
                        x = GetFraction(x / 100m) * 100m;
                    }

                    needDash = false;

                    if (x >= 20m)
                    {
                        sb.Append(' ')
                          .Append(tens[(int)(x / 10m)]);
                        x = GetFraction(x / 10m) * 10m;
                        needDash = true;
                    }

                    if (x < 20m && x > 0m)
                    {
                        sb.Append(needDash ? '-' : ' ')
                          .Append(ones[(int)(x)]);
                    }

                    if (i != 3)
                    {
                        sb.Append(' ')
                          .Append(mults[i]);

                        number = GetFraction(number / cm) * cm;
                        needComma = true;
                    }
                }
                cm /= 1000m;
            } // for 0 to 3...

            if (sb.Length == 0)
            {
                sb.Append(ones[0]); // i.e. "zero"
            }

            number = GetFraction(number);
            needDash = false;

            if (number >= 0.0005m)
            {
                string denominator = string.Empty;
                decimal x = 0m;

                for (int i = 0; i < fracs.Length; i++)
                {
                    x = nfracs[i] * number;
                    if (GetFraction(x) == 0m)
                    {
                        denominator = fracs[i];
                        break;
                    }
                }

                sb.Append(" and");

                if (x >= 100m)
                {
                    sb.Append(' ')
                      .Append(ones[(int)(x / 100m)])
                      .Append(" hundred");
                    x = GetFraction(x / 100m) * 100m;
                }
                if (x >= 20m)
                {
                    sb.Append(' ')
                      .Append(tens[(int)(x / 10m)]);
                    x = GetFraction(x / 10m) * 10m;
                    needDash = true;
                }
                if (x <= 20m && x > 0m)
                {
                    sb.Append(needDash ? '-' : ' ')
                      .Append(ones[(int)(x)]);
                }

                sb.Append(' ').Append(denominator);
                if (x > 1m)
                {
                    sb.Append('s');
                }
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Return the non-integer part of a decimal.
        /// </summary>
        public static decimal GetFraction(decimal d)
        {
            return d - Math.Truncate(d);
        }

        /// <summary>
        /// Format an amount for a check safety amount.  Format must contain two placeholders: 1st for dollars, 2nd for cents.
        /// {N} is replaced by numerals, and {A} is replaced by written text.
        /// </summary>
        public static string FormatSafetyAmount(decimal amount, string format)
        {
            const string NUMERIC_MACRO = "{N}";
            const string ALPHA_MACRO = "{A}";
            const int MACRO_LENGTH = 3;
            const string ERROR_MESSAGE = "Invalid safety amount format.";

            decimal dollars = Math.Truncate(amount);
            decimal cents = Math.Truncate((amount - dollars) * 100m);
            StringBuilder sb = new StringBuilder();

            // Dollars
            int idx1 = format.IndexOf(NUMERIC_MACRO);
            int idx2 = format.IndexOf(ALPHA_MACRO);

            if (idx1 < 0)
            {
                idx1 = format.Length;
            }
            if (idx2 < 0)
            {
                idx2 = format.Length;
            }

            if (idx1 < idx2)
            {
                sb.Append(format.Substring(0, idx1));
                sb.Append(dollars.ToString("N0")); // Need commas in dollar amount

                format = format.Substring(idx1 + MACRO_LENGTH);
            }
            else if (idx2 >= 0 && idx2 < idx1)
            {
                sb.Append(format.Substring(0, idx2));
                string dollarText = ToWords(dollars);
                sb.Append(char.ToUpper(dollarText[0]));
                sb.Append(dollarText.Substring(1));

                format = format.Substring(idx2 + MACRO_LENGTH);
            }
            else
                throw new ArgumentException(ERROR_MESSAGE);

            // Cents
            idx1 = format.IndexOf(NUMERIC_MACRO);
            idx2 = format.IndexOf(ALPHA_MACRO);

            if (idx1 < 0)
            {
                idx1 = format.Length;
            }

            if (idx2 < 0)
            {
                idx2 = format.Length;
            }

            if (idx1 >= 0 && idx1 < idx2)
            {
                sb.Append(format.Substring(0, idx1));
                sb.Append(cents.ToString());

                format = format.Substring(idx1 + MACRO_LENGTH);
            }
            else if (idx2 >= 0 && idx2 < idx1)
            {
                sb.Append(format.Substring(0, idx2));
                sb.Append(ToWords(cents));

                format = format.Substring(idx2 + MACRO_LENGTH);
            }
            else
                throw new ArgumentException(ERROR_MESSAGE);

            // Append any remaining text in the format
            sb.Append(format);

            return sb.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determine if a number is a multiple of 1/16.
        /// </summary>
        private static bool Is16th(decimal number)
        {
            decimal dTemp = GetFraction(number) * 16m;

            return dTemp == Math.Truncate(dTemp);
        }

        #endregion

    }
}