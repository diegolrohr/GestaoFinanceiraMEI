using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace Fly01.Compras.Helpers
{
    public static class FormatTextHelper
    {
        public static string FormatCpfCnpj(string strCpfCnpj)
        {
            if (strCpfCnpj.Length <= 11)
            {

                MaskedTextProvider mtpCpf = new MaskedTextProvider(@"000\.000\.000-00");
                mtpCpf.Set(ZeroLeft(strCpfCnpj, 11));
                return mtpCpf.ToString();

            }
            else
            {
                MaskedTextProvider mtpCnpj = new MaskedTextProvider(@"00\.000\.000/0000-00");
                mtpCnpj.Set(ZeroLeft(strCpfCnpj, 11));

                return mtpCnpj.ToString();
            }

        }

        public static string ZeroLeft(string strString, int intTamanho)

        {
            string strResult = "";

            for (int intCont = 1; intCont <= (intTamanho - strString.Length); intCont++)
            {
                strResult += "0";
            }

            return strResult + strString;
        }
    }
}