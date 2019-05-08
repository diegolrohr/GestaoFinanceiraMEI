using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Fly01.Core.Helpers
{
    public static class Base64Helper
    {
        public static string GenerateVerificationKey(string activationKeyPassword)
        {
            //usar formato dd/MM/yyyy_VerificationKey
            string activationKey = string.Format("{0}_{1}", DateTime.Now.Date.ToString("dd/MM/yyyy"), activationKeyPassword);
            return CalculaMD5Hash(activationKey);
        }

        public static string CodificaBase64(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string DecodificaBase64(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string CalculaMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));
            return sb.ToString();
        }

        public static string RemoverAcentos(string texto)
        {   
            //"ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            if (string.IsNullOrEmpty(texto))
                return String.Empty;

            texto = RemoveSpecialCharacters(texto);
            texto = texto.Replace(Environment.NewLine, " ");//remove enters
            texto = texto.Replace("> ", ">");
            texto = texto.Replace(" <", "<");
            byte[] bytes = Encoding.GetEncoding("iso-8859-8").GetBytes(texto);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str ?? "", "[~|'|!|¨|&|*|;|'|`|´|{|}|ª|º]", "");
        }
    }
}