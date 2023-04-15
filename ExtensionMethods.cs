using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSplit
{
    public static class StringExtensions
    {
        public static string SubstringWithMaxLength(this string value, int startIndex, int maxLength)
        {
            if (value == null)
            {
                return "";
            }

            return value.Substring(startIndex, Math.Min(value.Length-startIndex, maxLength));
        }
    }
}
