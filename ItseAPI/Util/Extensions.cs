using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculatorAPI.Util
{
    public static class Extensions
    {
        public static string ToBase64(this byte[] buffer) => Convert.ToBase64String(buffer);

        public static byte[] FromBase64(this string base64) => Convert.FromBase64String(base64);

        public static string RemoveAcentuation(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToUpper();
        }

        public static bool NotEquals(this object obj1, object obj2) => !obj1.Equals(obj2);

    }
}
