using System;
using System.Text.RegularExpressions;
using Fly01.Core.Interfaces.ValueObjects;

namespace Fly01.Core.ValueObjects
{
    public class CPF : IDocumento
    {
        public CPF(string numero)
        {
            if(!ValidaNumero(numero))
                throw new Exception("CPF informado inválido.");

            Numero = numero;
        }

        public string Numero { get; }

        public static bool ValidaNumero(string cpf)
        {
            cpf = Regex
                    .Replace(cpf ?? "", @"[^\d]", "")
                    .PadLeft(11, '0');

            if (Convert.ToInt64(cpf) == 0 || cpf.Length != 11) return false;

            var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            var resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            var digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto;

            return cpf.EndsWith(digito);
        }
    }
}
