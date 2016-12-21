using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DDI.Business.Extensions
{
    public static class StringExtensions
    {
        public static string ConvertProperCaseToCamelCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            StringBuilder sb = new StringBuilder(text.Length);
            bool uc = true;
            bool priorCharacterWasUpperCase = true;
            foreach (char c in text)
            {
                if (priorCharacterWasUpperCase && Char.IsUpper(c))
                {
                    sb.Append(Char.ToLower(c));
                }
                else if (priorCharacterWasUpperCase && !Char.IsUpper(c))
                {
                    sb.Append(c);
                    priorCharacterWasUpperCase = false;
                }
                else if (!priorCharacterWasUpperCase)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }

}