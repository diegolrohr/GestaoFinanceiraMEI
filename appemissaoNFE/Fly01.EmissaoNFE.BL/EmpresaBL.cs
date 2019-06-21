using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.ValueObjects;
using Fly01.Core.Notifications;
using System;
using System.Text.RegularExpressions;
using Fly01.Core.ValueObjects;

namespace Fly01.EmissaoNFE.BL
{
    public class EmpresaBL : PlataformaBaseBL<EmpresaVM>
    {
        private static string msgError;

        public EmpresaBL(AppDataContextBase context) : base(context)
        {
        }

        public bool TSSException(Exception ex)
        {
            if (ex.Message.Contains("TOTVS Service Soa") | ex.Message.Contains("TOTVS SPED Services"))
                return true;

            return false;
        }

        public override void ValidaModel(EmpresaVM entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.CodigoIBGECidade), CodigoIBGERequerido);
            entity.Fail(string.IsNullOrEmpty(entity.Endereco), EnderecoRequerido);

            if (!string.IsNullOrEmpty(entity.Cnpj))
            {
                entity.Fail(string.IsNullOrEmpty(entity.NIRE), NIRERequerido);
                entity.Fail((!ValidaCNPJ(entity.Cnpj) | entity.Cnpj.Length != 14), CnpjInvalido);
                entity.Fail(string.IsNullOrEmpty(entity.UF), UFRequerida);
            }

            entity.Fail(string.IsNullOrEmpty(entity.Nome), NomeRequerido);
            entity.Fail(string.IsNullOrEmpty(entity.Cpf) && string.IsNullOrEmpty(entity.Cnpj), CpfOuCnpjRequeridos);
            entity.Fail(!string.IsNullOrEmpty(entity.Cpf) && (!ValidaCPF(entity.Cpf) | entity.Cpf.Length != 11), CpfInvalido);
            entity.Fail(string.IsNullOrEmpty(entity.Cep), CEPRequerido);
            entity.Fail(entity.Cep != null && !ValidaCEP(entity.Cep), CEPInvalido);
            entity.Fail(entity.Email != null && !ValidaEmail(entity.Email), EmailInvalido);

            if (entity.InscricaoEstadual != null)
                if (!InscricaoEstadualHelper.IsValid(entity.UF, entity.InscricaoEstadual, out msgError))
                {
                    switch (msgError)
                    {
                        case "1":
                            entity.Fail(true, IEDigitoVerificador);
                            break;
                        case "2":
                            entity.Fail(true, IEQuantidadeDeDigitos);
                            break;
                        case "3":
                            entity.Fail(true, IEInvalida);
                            break;
                        case "4":
                            entity.Fail(true, UFInvalida);
                            break;
                        case "5":
                            entity.Fail(true, UFRequerida);
                            break;
                        default:
                            break;
                    }
                }

            base.ValidaModel(entity);
        }

        public static Error CodigoIBGERequerido = new Error("Código IBGE da cidade da empresa é um campo obrigatório.", "CodigoIBGECidade");
        public static Error EnderecoRequerido = new Error("Endereço da empresa é um campo obrigatório.", "Endereco");
        public static Error NIRERequerido = new Error("NIRE da empresa é um campo obrigatório.", "NIRE");
        public static Error NomeRequerido = new Error("Nome da empresa é um campo obrigatório.", "Nome");
        public static Error CpfOuCnpjRequeridos = new Error("CPF ou CNPJ da empresa precisa ser informado.", "Cnpj");
        public static Error CpfInvalido = new Error("CPF da empresa inválido.", "Cpf");
        public static Error CnpjInvalido = new Error("CNPJ da empresa inválido.", "Cnpj");
        public static Error CEPRequerido = new Error("CEP da empresa é um campo obrigatório.", "Cep");
        public static Error CEPInvalido = new Error("CEP da empresa inválido", "Cep");
        public static Error EmailInvalido = new Error("E-mail da empresa inválido.", "Email");
        public static Error IEDigitoVerificador = new Error("Digito verificador inválido (para este estado).", "InscricaoEstadual");
        public static Error IEQuantidadeDeDigitos = new Error("Quantidade de dígitos inválido (para este estado).", "InscricaoEstadual");
        public static Error IEInvalida = new Error("Inscrição Estadual inválida (para este estado).", "InscricaoEstadual");
        public static Error UFInvalida = new Error("Sigla da UF da empresa inválida.", "UF");
        public static Error UFRequerida = new Error("UF é um campo obrigatório.", "UF");

        public bool ValidaCPF(string cpf)

        {

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf;

            string digito;

            int soma;

            int resto;

            cpf = cpf.Trim();

            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)

                return false;

            tempCpf = cpf.Substring(0, 9);

            soma = 0;

            for (int i = 0; i < 9; i++)

                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;

            for (int i = 0; i < 10; i++)

                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);

        }

        public bool ValidaCNPJ(string cnpj)

        {

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma;

            int resto;

            string digito;

            string tempCnpj;

            cnpj = cnpj.Trim();

            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)

                return false;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;

            for (int i = 0; i < 12; i++)

                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;

            soma = 0;

            for (int i = 0; i < 13; i++)

                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)

                resto = 0;

            else

                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);

        }

        public bool ValidaCEP(string cep)
        {
            cep = cep.Replace(".", "");
            cep = cep.Replace("-", "");
            cep = cep.Replace(" ", "");

            Regex Rgx = new Regex(@"^\d{8}$");

            if (!Rgx.IsMatch(cep))
                return false;
            else
                return true;
        }

        public bool ValidaEmail(string email)
        {
            //Regex mail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            Regex mail = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            if (mail.IsMatch(email))
                return true;

            return false;
        }
    }
}
