using System;
using System.Text.RegularExpressions;
using Fly01.Core.ValueObjects.Interfaces;

namespace Fly01.Core.ValueObjects
{
    public class CNPJ : IDocumento
    {
        public CNPJ(string numero)
        {
            if (!ValidaNumero(numero))
                throw new Exception("CNPJ informado inválido.");

            Numero = numero;
        }

        public string Numero { get; }

        public static bool ValidaNumero(string cnpj)
        {
            cnpj = Regex
                    .Replace(cnpj ?? "", @"[^\d]", "")
                    .PadLeft(14, '0');

            if (Convert.ToInt64(cnpj) == 0 || cnpj.Length != 14) return false;

            var multiplicador1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCnpj = cnpj.Substring(0, 12);
            var soma = 0;
            for (var i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            var resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            var digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (var i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto;

            return cnpj.EndsWith(digito);
        }
    }
}
