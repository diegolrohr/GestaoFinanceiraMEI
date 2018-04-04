using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Fly01.Core.Helpers
{
    public static class FormatterUtils
    {
        public static string RemoveNotNumbers(string str)
        {
            StringBuilder sb = new StringBuilder();
            if (str != null)
            {
                char[] chars = str.ToCharArray();
                foreach (char c in chars)
                {
                    if (c >= '0' && c <= '9')
                        sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string FormatDocument(string document)
        {
            string newDocument = RemoveNotNumbers(document);
            if (newDocument.Length == 11)
            {
                //CPF
                return Convert.ToUInt64(newDocument).ToString(@"000\.000\.000\-00");
            }
            else if (newDocument.Length == 14)
            {
                //CNPJ
                return Convert.ToUInt64(newDocument).ToString(@"00\.000\.000\/0000\-00");
            }
            else
            {
                return newDocument;
            }
        }

        public static string CurrencyFormat(double currDouble)
        {
            string formatted = string.Format("{0:N2}", currDouble).Replace(".", "#").Replace(",", ".").Replace("#", ",");
            return string.Format("R$ {0}", formatted);
        }

        public static double CurrencyUnformat(string currency)
        {
            currency = !string.IsNullOrWhiteSpace(currency) ? currency.Replace("R$ ", "").Replace(".", "").Replace(",", ".") : "0.0";
            return double.Parse(currency);
        }

        public static string DateFormat(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static string RemoveDiacritics(this string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
        }
    }
}