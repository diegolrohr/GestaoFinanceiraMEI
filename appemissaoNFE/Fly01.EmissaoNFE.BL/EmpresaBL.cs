using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Domain;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System;
using System.Text.RegularExpressions;

namespace Fly01.EmissaoNFE.BL
{
    public class EmpresaBL : PlataformaBaseBL<EmpresaVM>
    {
        private static string ERRO_MsgDigitoVerificadorInvalido = "1";
        private static string ERRO_QtdDigitosInvalida = "2";
        private static string ERRO_InscricaoInvalida = "3";
        private static string ERRO_SiglaUFInvalida = "4";
        private static string ERRO_SiglaRequerida = "5";
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
            entity.Fail(entity.CodigoIBGE == null, CodigoIBGERequerido);
            entity.Fail(entity.Endereco == null, EnderecoRequerido);
            entity.Fail(entity.NIRE == null, NIRERequerido);
            entity.Fail(entity.Nome == null, NomeRequerido);
            entity.Fail(entity.Cpf == null && entity.Cnpj == null, CpfOuCnpjRequeridos);
            entity.Fail(entity.Cpf != null && (!ValidaCPF(entity.Cpf) | entity.Cpf.Length != 11), CpfInvalido);
            entity.Fail(entity.Cnpj != null && (!ValidaCNPJ(entity.Cnpj) | entity.Cnpj.Length != 14), CnpjInvalido);
            entity.Fail(entity.Cep == null, CEPRequerido);
            entity.Fail(entity.Cep != null && !ValidaCEP(entity.Cep), CEPInvalido);
            entity.Fail(entity.Email != null && !ValidaEmail(entity.Email), EmailInvalido);
            if(entity.InscricaoEstadual != null)
                if(!ValidaIE(entity.UF, entity.InscricaoEstadual, out msgError))
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

        public static Error CodigoIBGERequerido = new Error("Código IBGE é um campo obrigatório.", "CodigoIBGE");
        public static Error EnderecoRequerido = new Error("Endereço é um campo obrigatório.", "Endereco");
        public static Error NIRERequerido = new Error("NIRE é um campo obrigatório.", "NIRE");
        public static Error NomeRequerido = new Error("Nome é um campo obrigatório.", "Nome");
        public static Error CpfOuCnpjRequeridos = new Error("CPF ou CNPJ precisa ser informado.", "Cnpj");
        public static Error CpfInvalido = new Error("CPF inválido.", "Cpf");
        public static Error CnpjInvalido = new Error("CNPJ inválido.", "Cnpj");
        public static Error CEPRequerido = new Error("CEP é um campo obrigatório.", "Cep");
        public static Error CEPInvalido = new Error("CEP inválido", "Cep");
        public static Error EmailInvalido = new Error("E-mail inválido.", "Email");
        public static Error IEDigitoVerificador = new Error("Digito verificador inválido (para este estado).", "InscricaoEstadual");
        public static Error IEQuantidadeDeDigitos = new Error("Quantidade de dígitos inválido (para este estado).", "InscricaoEstadual");
        public static Error IEInvalida = new Error("Inscrição Estadual inválida (para este estado).", "InscricaoEstadual");
        public static Error UFInvalida = new Error("Sigla da UF inválida.", "UF");
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
            Regex mail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");

            if (mail.IsMatch(email))
                return true;

            return false;
        }

        private static string RemoveMascara(string ie)
        {
            string strIE = "";
            for (int i = 0; i < ie.Length; i++)
            {
                if (char.IsDigit(ie[i]))
                {
                    strIE += ie[i];
                }
            }
            return strIE;
        }

        public static bool ValidaIE(string siglaUf, string inscricaoEstadual, out string msgError)
        {
            if (string.IsNullOrEmpty(siglaUf))
            {
                msgError = ERRO_SiglaRequerida;
                return false;
            }

            string strIE = RemoveMascara(inscricaoEstadual);
            siglaUf = siglaUf.ToUpper();

            msgError = string.Empty;
            if (inscricaoEstadual.ToUpper() == "ISENTO" || siglaUf == "EX")
                return true;

            if (string.IsNullOrEmpty(inscricaoEstadual))
            {
                msgError = ERRO_InscricaoInvalida;
                return false;
            }

            switch (siglaUf)
            {
                case "AC":
                    return ValidaIEAcre(strIE, out msgError);
                case "AL":
                    return ValidaIEAlagoas(strIE, out msgError);
                case "AP":
                    return ValidaIEAmapa(strIE, out msgError);
                case "AM":
                    return ValidaIEAmazonas(strIE, out msgError);
                case "BA":
                    return ValidaIEBahia(strIE, out msgError);
                case "CE":
                    return ValidaIECeara(strIE, out msgError);
                case "ES":
                    return ValidaIEEspiritoSanto(strIE, out msgError);
                case "GO":
                    return ValidaIEGoias(strIE, out msgError);
                case "MA":
                    return ValidaIEMaranhao(strIE, out msgError);
                case "MT":
                    return ValidaIEMatoGrosso(strIE, out msgError);
                case "MS":
                    return ValidaIEMatoGrossoSul(strIE, out msgError);
                case "MG":
                    return ValidaIEMinasGerais(strIE, out msgError);
                case "PA":
                    return ValidaIEPara(strIE, out msgError);
                case "PB":
                    return ValidaIEParaiba(strIE, out msgError);
                case "PR":
                    return ValidaIEParana(strIE, out msgError);
                case "PE":
                    return ValidaIEPernambuco(strIE, out msgError);
                case "PI":
                    return ValidaIEPiaui(strIE, out msgError);
                case "RJ":
                    return ValidaIERioJaneiro(strIE, out msgError);
                case "RN":
                    return ValidaIERioGrandeNorte(strIE, out msgError);
                case "RS":
                    return ValidaIERioGrandeSul(strIE, out msgError);
                case "RO":
                    return ValidaIERondonia(strIE, out msgError);
                case "RR":
                    return ValidaIERoraima(strIE, out msgError);
                case "SC":
                    return ValidaIESantaCatarina(strIE, out msgError);
                case "SP":
                    return ValidaIESaoPaulo(strIE, out msgError);
                case "SE":
                    return ValidaIESergipe(strIE, out msgError);
                case "TO":
                    return ValidaIETocantins(strIE, out msgError);
                case "DF":
                    return ValidaIEDistritoFederal(strIE, out msgError);
                default:
                    {
                        msgError = ERRO_SiglaUFInvalida;
                        return false;
                    }
            };
        }

        private static bool ValidaIEAcre(string ie, out string msgError)
        {
            if (ie.Length != 13)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //valida os dois primeiros digitos - devem ser iguais a 01
            for (int i = 0; i < 2; i++)
            {
                if (int.Parse(ie[i].ToString()) != i)
                {
                    msgError = ERRO_InscricaoInvalida;
                    return false;
                }
            }

            int soma = 0;
            int pesoInicial = 4;
            int pesoFinal = 9;
            int d1 = 0; //primeiro digito verificador
            int d2 = 0; //segundo digito verificador

            //calcula o primeiro digito
            for (int i = 0; i < ie.Length - 2; i++)
            {
                if (i < 3)
                {
                    soma += int.Parse(ie[i].ToString()) * pesoInicial;
                    pesoInicial--;
                }
                else
                {
                    soma += int.Parse(ie[i].ToString()) * pesoFinal;
                    pesoFinal--;
                }
            }
            d1 = 11 - (soma % 11);
            if (d1 == 10 || d1 == 11)
            {
                d1 = 0;
            }

            //calcula o segundo digito
            soma = d1 * 2;
            pesoInicial = 5;
            pesoFinal = 9;
            for (int i = 0; i < ie.Length - 2; i++)
            {
                if (i < 4)
                {
                    soma += int.Parse(ie[i].ToString()) * pesoInicial;
                    pesoInicial--;
                }
                else
                {
                    soma += int.Parse(ie[i].ToString()) * pesoFinal;
                    pesoFinal--;
                }
            }

            d2 = 11 - (soma % 11);
            if (d2 == 10 || d2 == 11)
            {
                d2 = 0;
            }

            //valida os digitos verificadores
            string dv = d1 + "" + d2;
            if (!dv.Equals(ie.Substring(ie.Length - 2)))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEAlagoas(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //valida os dois primeiros dígitos - deve ser iguais a 24
            if (!ie.Substring(0, 2).Equals("24"))
            {
                msgError = ERRO_InscricaoInvalida;
                return false;
            }

            //valida o terceiro dígito - deve ser 0,3,5,7,8
            int[]
            digits = { 0, 3, 5, 7, 8 };
            bool check = false;
            for (int i = 0; i < digits.Length; i++)
            {
                if (int.Parse(ie[2].ToString()) == digits[i])
                {
                    check = true;
                    break;
                }
            }
            if (!check)
            {
                msgError = ERRO_InscricaoInvalida;
                return false;
            }

            //calcula o dígito verificador
            int soma = 0;
            int peso = 9;
            int d = 0; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }
            d = ((soma * 10) % 11);
            if (d == 10)
            {
                d = 0;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEAmapa(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //verifica os dois primeiros dígitos - deve ser igual 03
            if (!ie.Substring(0, 2).Equals("03"))
            {
                msgError = ERRO_InscricaoInvalida;
                return false;
            }

            //calcula o dígito verificador
            int d1 = -1;
            int soma = -1;
            int peso = 9;

            //configura o valor do dígito verificador e da soma de acordo com faixa das inscrições
            long x = long.Parse(ie.Substring(0, ie.Length - 1)); //x = inscrição estadual sem o dígito verificador
            if (x >= 3017001L && x <= 3019022L)
            {
                d1 = 1;
                soma = 9;
            }
            else if (x >= 3000001L && x <= 3017000L)
            {
                d1 = 0;
                soma = 5;
            }
            else if (x >= 3019023L)
            {
                d1 = 0;
                soma = 0;
            }

            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            int d = 11 - ((soma % 11)); //d = armazena o dígito verificador apenas calculo
            if (d == 10)
            {
                d = 0;
            }
            else if (d == 11)
            {
                d = d1;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEAmazonas(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            int soma = 0;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            if (soma < 11)
            {
                d = 11 - soma;
            }
            else if ((soma % 11) <= 1)
            {
                d = 0;
            }
            else
            {
                d = 11 - (soma % 11);
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEBahia(string ie, out string msgError)
        {

            if (ie.Length != 8 && ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //Calculo do modulo de acordo com o primeiro dígito da inscrição Estadual
            int modulo = 10;
            int firstDigit = int.Parse(ie.Length == 8 ? ie[0].ToString() : ie[1].ToString());
            if (firstDigit == 6 || firstDigit == 7 || firstDigit == 9)
                modulo = 11;

            //Calculo do segundo dígito
            int d2 = -1; //segundo dígito verificador
            int soma = 0;
            int peso = ie.Length == 8 ? 7 : 8;
            for (int i = 0; i < ie.Length - 2; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            int resto = soma % modulo;

            if (resto == 0 || (modulo == 11 && resto == 1))
            {
                d2 = 0;
            }
            else
            {
                d2 = modulo - resto;
            }

            //Calculo do primeiro dígito
            int d1 = -1; //primeiro dígito verificador
            soma = d2 * 2;
            peso = ie.Length == 8 ? 8 : 9;
            for (int i = 0; i < ie.Length - 2; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            resto = soma % modulo;

            if (resto == 0 || (modulo == 11 && resto == 1))
            {
                d1 = 0;
            }
            else
            {
                d1 = modulo - resto;
            }

            //valida os digitos verificadores
            string dv = d1 + "" + d2;
            if (!dv.Equals(ie.Substring(ie.Length - 2)))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIECeara(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //Calculo do dígito verificador
            int soma = 0;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            d = 11 - (soma % 11);
            if (d == 10 || d == 11)
            {
                d = 0;
            }
            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEEspiritoSanto(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //Calculo do dígito verificador
            int soma = 0;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            int resto = soma % 11;
            if (resto < 2)
            {
                d = 0;
            }
            else if (resto > 1)
            {
                d = 11 - resto;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEGoias(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //válida os dois primeiros dígito
            if (!"10".Equals(ie.Substring(0, 2)))
            {
                if (!"11".Equals(ie.Substring(0, 2)))
                {
                    if (!"15".Equals(ie.Substring(0, 2)))
                    {
                        msgError = ERRO_InscricaoInvalida;
                        return false;
                    }
                }
            }

            if (ie.Substring(0, ie.Length - 1).Equals("11094402"))
            {
                if (!ie.Substring(ie.Length - 1).Equals("0"))
                {
                    if (!ie.Substring(ie.Length - 1).Equals("1"))
                    {
                        msgError = ERRO_InscricaoInvalida;
                        return false;
                    }
                }
            }
            else
            {

                //Calculo do dígito verificador
                int soma = 0;
                int peso = 9;
                int d = -1; //dígito verificador
                for (int i = 0; i < ie.Length - 1; i++)
                {
                    soma += int.Parse(ie[i].ToString()) * peso;
                    peso--;
                }

                int resto = soma % 11;
                long faixaInicio = 10103105;
                long faixaFim = 10119997;
                long insc = long.Parse(ie.Substring(0, ie.Length - 1));
                if (resto == 0)
                {
                    d = 0;
                }
                else if (resto == 1)
                {
                    if (insc >= faixaInicio && insc <= faixaFim)
                    {
                        d = 1;
                    }
                    else
                    {
                        d = 0;
                    }
                }
                else if (resto != 0 && resto != 1)
                {
                    d = 11 - resto;
                }

                //valida o digito verificador
                string dv = d + "";
                if (!ie.Substring(ie.Length - 1).Equals(dv))
                {
                    msgError = ERRO_MsgDigitoVerificadorInvalido;
                    return false;
                }
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEMaranhao(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //valida os dois primeiros dígitos
            if (!ie.Substring(0, 2).Equals("12"))
            {
                msgError = ERRO_InscricaoInvalida;
                return false;
            }

            //Calcula o dígito verificador
            int soma = 0;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            d = 11 - (soma % 11);
            if ((soma % 11) == 0 || (soma % 11) == 1)
            {
                d = 0;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEMatoGrosso(string ie, out string msgError)
        {

            if (ie.Length != 11)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //Calcula o dígito verificador
            int soma = 0;
            int pesoInicial = 3;
            int pesoFinal = 9;
            int d = -1;

            for (int i = 0; i < ie.Length - 1; i++)
            {
                if (i < 2)
                {
                    soma += int.Parse(ie[i].ToString()) * pesoInicial;
                    pesoInicial--;
                }
                else
                {
                    soma += int.Parse(ie[i].ToString()) * pesoFinal;
                    pesoFinal--;
                }
            }

            d = 11 - (soma % 11);
            if ((soma % 11) == 0 || (soma % 11) == 1)
            {
                d = 0;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEMatoGrossoSul(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //valida os dois primeiros dígitos
            if (!ie.Substring(0, 2).Equals("28"))
            {
                msgError = ERRO_InscricaoInvalida;
                return false;
            }

            //Calcula o dígito verificador
            int soma = 0;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            int resto = soma % 11;
            int result = 11 - resto;
            if (resto == 0)
            {
                d = 0;
            }
            else if (resto > 0)
            {
                if (result > 9)
                {
                    d = 0;
                }
                else if (result < 10)
                {
                    d = result;
                }
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEMinasGerais(string ie, out string msgError)
        {
            /*
             * FORMATO GERAL: A1A2A3B1B2B3B4B5B6C1C2D1D2
             * Onde: A= Código do Município
             * B= Número da inscrição
             * C= Número de ordem do estabelecimento
             * D= dígitos de controle
             */

            // valida quantida de dígitos
            if (ie.Length != 13)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //iguala a casas para o cólculo
            //em inserir o algarismo zero "0" imediatamente após o Número de código do municópio, 
            //desprezando-se os dígitos de controle.
            string str = "";
            for (int i = 0; i < ie.Length - 2; i++)
            {
                if (char.IsDigit(ie[0]))
                {
                    if (i == 3)
                    {
                        str += "0";
                        str += ie[i];
                    }
                    else
                    {
                        str += ie[i];
                    }
                }
            }

            //calculo do primeiro dígito verificador
            int soma = 0;
            int pesoInicio = 1;
            int pesoFim = 2;
            int d1 = -1; //primeiro dígito verificador
            for (int i = 0; i < str.Length; i++)
            {
                if (i % 2 == 0)
                {
                    int x = int.Parse(str[i].ToString()) * pesoInicio;
                    string strX = x.ToString();
                    for (int j = 0; j < strX.Length; j++)
                    {
                        soma += int.Parse(strX[j].ToString());
                    }
                }
                else
                {
                    int y = int.Parse(str[i].ToString()) * pesoFim;
                    string strY = y.ToString();
                    for (int j = 0; j < strY.Length; j++)
                    {
                        soma += int.Parse(strY[j].ToString());
                    }
                }
            }

            int dezenaExata = soma;
            while (dezenaExata % 10 != 0)
            {
                dezenaExata++;
            }
            d1 = dezenaExata - soma; //resultado - primeiro dígito verificador

            //calculo do segundo dígito verificador
            soma = d1 * 2;
            pesoInicio = 3;
            pesoFim = 11;
            int d2 = -1;
            for (int i = 0; i < ie.Length - 2; i++)
            {
                if (i < 2)
                {
                    soma += int.Parse(ie[i].ToString()) * pesoInicio;
                    pesoInicio--;
                }
                else
                {
                    soma += int.Parse(ie[i].ToString()) * pesoFim;
                    pesoFim--;
                }
            }

            d2 = 11 - (soma % 11); //resultado - segundo dígito verificador
            if ((soma % 11 == 0) || (soma % 11 == 1))
            {
                d2 = 0;
            }

            //valida os digitos verificadores
            string dv = d1 + "" + d2;
            if (!dv.Equals(ie.Substring(ie.Length - 2)))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEPara(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //valida os dois primeiros dígitos
            if (!ie.Substring(0, 2).Equals("15"))
            {
                msgError = ERRO_InscricaoInvalida;
                return false;
            }

            //Calcula o dígito verificador
            int soma = 0;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            d = 11 - (soma % 11);
            if ((soma % 11) == 0 || (soma % 11) == 1)
            {
                d = 0;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEParaiba(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //Calcula o dígito verificador
            int soma = 0;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            d = 11 - (soma % 11);
            if (d == 10 || d == 11)
            {
                d = 0;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEParana(string ie, out string msgError)
        {

            if (ie.Length != 10)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //calculo do primeiro dígito
            int soma = 0;
            int pesoInicio = 3;
            int pesoFim = 7;
            int d1 = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 2; i++)
            {
                if (i < 2)
                {
                    soma += int.Parse(ie[i].ToString()) * pesoInicio;
                    pesoInicio--;
                }
                else
                {
                    soma += int.Parse(ie[i].ToString()) * pesoFim;
                    pesoFim--;
                }
            }

            d1 = 11 - (soma % 11);
            if ((soma % 11) == 0 || (soma % 11) == 1)
            {
                d1 = 0;
            }

            //calculo do segundo dígito
            soma = d1 * 2;
            pesoInicio = 4;
            pesoFim = 7;
            int d2 = -1; //segundo dígito
            for (int i = 0; i < ie.Length - 2; i++)
            {
                if (i < 3)
                {
                    soma += int.Parse(ie[i].ToString()) * pesoInicio;
                    pesoInicio--;
                }
                else
                {
                    soma += int.Parse(ie[i].ToString()) * pesoFim;
                    pesoFim--;
                }
            }

            d2 = 11 - (soma % 11);
            if ((soma % 11) == 0 || (soma % 11) == 1)
            {
                d2 = 0;
            }

            //valida os digitos verificadores
            string dv = d1 + "" + d2;
            if (!dv.Equals(ie.Substring(ie.Length - 2)))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEPernambuco(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //calculo do dígito verificador
            int soma = 0;
            int pesoInicio = 5;
            int pesoFim = 9;
            int d = -1; //dígito verificador

            for (int i = 0; i < ie.Length - 1; i++)
            {
                if (i < 5)
                {
                    soma += int.Parse(ie[i].ToString()) * pesoInicio;
                    pesoInicio--;
                }
                else
                {
                    soma += int.Parse(ie[i].ToString()) * pesoFim;
                    pesoFim--;
                }
            }

            d = 11 - (soma % 11);
            if (d > 9)
            {
                d -= 10;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEPiaui(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //calculo do dígito verficador
            int soma = 0;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            d = 11 - (soma % 11);
            if (d == 11 || d == 10)
            {
                d = 0;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIERioJaneiro(string ie, out string msgError)
        {

            if (ie.Length != 8)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //calculo do dígito verficador
            int soma = 0;
            int peso = 7;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                if (i == 0)
                {
                    soma += int.Parse(ie[i].ToString()) * 2;
                }
                else
                {
                    soma += int.Parse(ie[i].ToString()) * peso;
                    peso--;
                }
            }

            d = 11 - (soma % 11);
            if ((soma % 11) <= 1)
            {
                d = 0;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIERioGrandeNorte(string ie, out string msgError)
        {

            if (ie.Length != 10 && ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //valida os dois primeiros dígitos
            if (!ie.Substring(0, 2).Equals("20"))
            {
                msgError = ERRO_InscricaoInvalida;
                return false;
            }

            //calcula o dígito para inscrição de 9 dígitos
            if (ie.Length == 9)
            {
                int soma = 0;
                int peso = 9;
                int d = -1; //dígito verificador
                for (int i = 0; i < ie.Length - 1; i++)
                {
                    soma += int.Parse(ie[i].ToString()) * peso;
                    peso--;
                }

                d = ((soma * 10) % 11);
                if (d == 10)
                {
                    d = 0;
                }

                //valida o digito verificador
                string dv = d + "";
                if (!ie.Substring(ie.Length - 1).Equals(dv))
                {
                    msgError = ERRO_MsgDigitoVerificadorInvalido;
                    return false;
                }
            }
            else
            {
                int soma = 0;
                int peso = 10;
                int d = -1; //dígito verificador
                for (int i = 0; i < ie.Length - 1; i++)
                {
                    soma += int.Parse(ie[i].ToString()) * peso;
                    peso--;
                }
                d = ((soma * 10) % 11);
                if (d == 10)
                {
                    d = 0;
                }

                //valida o digito verificador
                string dv = d + "";
                if (!ie.Substring(ie.Length - 1).Equals(dv))
                {
                    msgError = ERRO_MsgDigitoVerificadorInvalido;
                    return false;
                }
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIERioGrandeSul(string ie, out string msgError)
        {

            if (ie.Length != 10)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //calculo do d&#65533;fito verificador
            int soma = int.Parse(ie[0].ToString()) * 2;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 1; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            d = 11 - (soma % 11);
            if (d == 10 || d == 11)
            {
                d = 0;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIERondonia(string ie, out string msgError)
        {

            if (ie.Length != 14)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //calculo do dígito verificador
            int soma = 0;
            int pesoInicio = 6;
            int pesoFim = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                if (i < 5)
                {
                    soma += int.Parse(ie[i].ToString()) * pesoInicio;
                    pesoInicio--;
                }
                else
                {
                    soma += int.Parse(ie[i].ToString()) * pesoFim;
                    pesoFim--;
                }
            }

            d = 11 - (soma % 11);
            if (d == 11 || d == 10)
            {
                d -= 10;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIERoraima(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //valida os dois primeiros dígitos
            if (!ie.Substring(0, 2).Equals("24"))
            {
                msgError = ERRO_InscricaoInvalida;
                return false;
            }

            int soma = 0;
            int peso = 1;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso++;
            }

            d = soma % 9;

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIESantaCatarina(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //calculo do d&#65533;fito verificador
            int soma = 0;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            d = 11 - (soma % 11);
            if ((soma % 11) == 0 || (soma % 11) == 1)
            {
                d = 0;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIESaoPaulo(string ie, out string msgError)
        {
            if (ie[0] == 'P')
                ie = "P" + ie;

            if (ie.Length != 12 && ie.Length != 13)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            if (ie.Length == 12)
            {
                int soma = 0;
                int peso = 1;
                int d1 = -1; //primeiro dígito verificador
                             //calculo do primeiro dígito verificador (nona posição)
                for (int i = 0; i < ie.Length - 4; i++)
                {
                    if (i == 1 || i == 7)
                    {
                        soma += int.Parse(ie[i].ToString()) * ++peso;
                        peso++;
                    }
                    else
                    {
                        soma += int.Parse(ie[i].ToString()) * peso;
                        peso++;
                    }
                }

                d1 = soma % 11;
                string strD1 = d1.ToString(); //O dígito &#65533; igual ao algarismo mais a direita do resultado de (soma % 11)
                d1 = int.Parse(strD1[strD1.Length - 1].ToString());

                //calculo do segunfo dígito
                soma = 0;
                int pesoInicio = 3;
                int pesoFim = 10;
                int d2 = -1; //segundo dígito verificador
                for (int i = 0; i < ie.Length - 1; i++)
                {
                    if (i < 2)
                    {
                        soma += int.Parse(ie[i].ToString()) * pesoInicio;
                        pesoInicio--;
                    }
                    else
                    {
                        soma += int.Parse(ie[i].ToString()) * pesoFim;
                        pesoFim--;
                    }
                }

                d2 = soma % 11;
                string strD2 = d2.ToString(); //O dígito &#65533; igual ao algarismo mais a direita do resultado de (soma % 11)
                d2 = int.Parse(strD2[strD2.Length - 1].ToString());

                //valida os dígitos verificadores
                if (!ie.Substring(8, 1).Equals(d1 + ""))
                {
                    msgError = ERRO_InscricaoInvalida;
                    return false;
                }
                if (!ie.Substring(11, 1).Equals(d2 + ""))
                {
                    msgError = ERRO_InscricaoInvalida;
                    return false;
                }

            }
            else
            {
                //valida o primeiro caracter
                if (ie[0] != 'P')
                {
                    msgError = ERRO_InscricaoInvalida;
                    return false;
                }

                string strIE = ie.Substring(1, 10); //Obt&#65533;m somente os dígitos utilizados no cólculo do dígito verificador
                int soma = 0;
                int peso = 1;
                int d1 = -1; //primeiro dígito verificador
                             //calculo do primeiro dígito verificador (nona posição)
                for (int i = 0; i < strIE.Length - 1; i++)
                {
                    if (i == 1 || i == 7)
                    {
                        soma += int.Parse(strIE[i].ToString()) * ++peso;
                        peso++;
                    }
                    else
                    {
                        soma += int.Parse(strIE[i].ToString()) * peso;
                        peso++;
                    }
                }

                d1 = soma % 11;
                string strD1 = d1.ToString(); //O dígito &#65533; igual ao algarismo mais a direita do resultado de (soma % 11)
                d1 = int.Parse(strD1[strD1.Length - 1].ToString());

                //valida o dígito verificador
                if (!ie.Substring(9, 1).Equals(d1 + ""))
                {
                    msgError = ERRO_InscricaoInvalida;
                    return false;
                }
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIESergipe(string ie, out string msgError)
        {

            if (ie.Length != 9)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //calculo do dígito verificador
            int soma = 0;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                soma += int.Parse(ie[i].ToString()) * peso;
                peso--;
            }

            d = 11 - (soma % 11);
            if (d == 11 || d == 11 || d == 10)
            {
                d = 0;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIETocantins(string ie, out string msgError)
        {

            if (ie.Length != 9 && ie.Length != 11)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }
            else if (ie.Length == 9)
            {
                ie = ie.Substring(0, 2) + "02" + ie.Substring(2);
            }

            int soma = 0;
            int peso = 9;
            int d = -1; //dígito verificador
            for (int i = 0; i < ie.Length - 1; i++)
            {
                if (i != 2 && i != 3)
                {
                    soma += int.Parse(ie[i].ToString()) * peso;
                    peso--;
                }
            }
            d = 11 - (soma % 11);
            if ((soma % 11) < 2)
            {
                d = 0;
            }

            //valida o digito verificador
            string dv = d + "";
            if (!ie.Substring(ie.Length - 1).Equals(dv))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }

        private static bool ValidaIEDistritoFederal(string ie, out string msgError)
        {

            if (ie.Length != 13)
            {
                msgError = ERRO_QtdDigitosInvalida;
                return false;
            }

            //calculo do primeiro dígito verificador
            int soma = 0;
            int pesoInicio = 4;
            int pesoFim = 9;
            int d1 = -1; //primeiro dígito verificador
            for (int i = 0; i < ie.Length - 2; i++)
            {
                if (i < 3)
                {
                    soma += int.Parse(ie[i].ToString()) * pesoInicio;
                    pesoInicio--;
                }
                else
                {
                    soma += int.Parse(ie[i].ToString()) * pesoFim;
                    pesoFim--;
                }
            }

            d1 = 11 - (soma % 11);
            if (d1 == 11 || d1 == 10)
            {
                d1 = 0;
            }

            //calculo do segundo dígito verificador
            soma = d1 * 2;
            pesoInicio = 5;
            pesoFim = 9;
            int d2 = -1; //segundo dígito verificador
            for (int i = 0; i < ie.Length - 2; i++)
            {
                if (i < 4)
                {
                    soma += int.Parse(ie[i].ToString()) * pesoInicio;
                    pesoInicio--;
                }
                else
                {
                    soma += int.Parse(ie[i].ToString()) * pesoFim;
                    pesoFim--;
                }
            }

            d2 = 11 - (soma % 11);
            if (d2 == 11 || d2 == 10)
            {
                d2 = 0;
            }

            //valida os digitos verificadores
            string dv = d1 + "" + d2;
            if (!dv.Equals(ie.Substring(ie.Length - 2)))
            {
                msgError = ERRO_MsgDigitoVerificadorInvalido;
                return false;
            }

            msgError = string.Empty;
            return true;
        }
    }
}
