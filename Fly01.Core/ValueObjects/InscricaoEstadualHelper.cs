namespace Fly01.Core.ValueObjects
{
    public static class InscricaoEstadualHelper
    {
        private static string ERRO_MsgDigitoVerificadorInvalido = "Digito verificador inválido (para este estado).";
        private static string ERRO_QtdDigitosInvalida = "Quantidade de dígitos inválido (para este estado).";
        private static string ERRO_InscricaoInvalida = "Inscrição Estadual inválida (para este estado).";
        private static string ERRO_SiglaUFInvalida = "Sigla UF Inválida.";

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

        public static void TestMethod()
        {
            var msgError = string.Empty;
            var response = false;

            /* Acre */
            response = IsValid("AC", "0111584246767", out msgError); //true
            response = IsValid("AC", "1011584246767", out msgError); //false

            /* Alagoas */
            response = IsValid("AL", "248979752", out msgError); //true
            response = IsValid("AL", "428979752", out msgError); //false

            /* Amapá */
            response = IsValid("AP", "036037010", out msgError); //true
            response = IsValid("AP", "056037010", out msgError); //false

            /* Amazonas */
            response = IsValid("AM", "922641293", out msgError); //true
            response = IsValid("AM", "9226412931", out msgError); //false

            /* Bahia */
            response = IsValid("BA", "05974997", out msgError); //true
            response = IsValid("BA", "059749971", out msgError); //false

            /* Ceará */
            response = IsValid("CE", "295915900", out msgError); //true
            response = IsValid("CE", "2959159001", out msgError); //false

            /* Distrito Federal */
            response = IsValid("DF", "0720182800104", out msgError); //true
            response = IsValid("DF", "1720182800104", out msgError); //false

            /* Espírito Santo */
            response = IsValid("ES", "395438322", out msgError); //true
            response = IsValid("ES", "3954383221", out msgError); //false

            /* Goiás */
            response = IsValid("GO", "111914000", out msgError); //true
            response = IsValid("GO", "121914000", out msgError); //false

            /* Maranhão */
            response = IsValid("MA", "124066330", out msgError); //true
            response = IsValid("MA", "024066330", out msgError); //false

            /* Mato Grosso */
            response = IsValid("MT", "09003887859", out msgError); //true
            response = IsValid("MT", "0900388785", out msgError); //false

            /* Mato Grosso do Sul */
            response = IsValid("MS", "283060972", out msgError); //true
            response = IsValid("MS", "183060972", out msgError); //false

            /* Minas Gerais */
            response = IsValid("MG", "4667676336935", out msgError); //true
            response = IsValid("MG", "466767633693", out msgError); //false

            /* Pará */
            response = IsValid("PA", "159296250", out msgError); //true
            response = IsValid("PA", "149296250", out msgError); //false

            /* Paraíba */
            response = IsValid("PB", "881292400", out msgError); //true
            response = IsValid("PB", "8812924001", out msgError); //false

            /* Paraná */
            response = IsValid("PR", "2607283420", out msgError); //true
            response = IsValid("PR", "26072834201", out msgError); //false

            /* Pernambuco */
            response = IsValid("PE", "029997062", out msgError); //true
            response = IsValid("PE", "0299970621", out msgError); //false

            /* Piauí */
            response = IsValid("PI", "390532541", out msgError); //true
            response = IsValid("PI", "39053254", out msgError); //false

            /* Rio de Janeiro */
            response = IsValid("RJ", "65942844", out msgError); //true
            response = IsValid("RJ", "659428445", out msgError); //false

            /* Rio Grande do Norte */
            response = IsValid("RN", "203960530", out msgError); //true
            response = IsValid("RN", "20396053011", out msgError); //false

            /* Rio Grande do Sul */
            response = IsValid("RS", "3813943960", out msgError); //true
            response = IsValid("RS", "38139439601", out msgError); //false

            /* Rondônia */
            response = IsValid("RO", "93891570266485", out msgError); //true
            response = IsValid("RO", "9389157026648", out msgError); //false

            /* Rondônia */
            response = IsValid("RR", "241886343", out msgError); //true
            response = IsValid("RR", "141886343", out msgError); //false

            /* São Paulo */
            response = IsValid("SP", "294667497314", out msgError); //true
            response = IsValid("SP", "P604161982909", out msgError); //true
            response = IsValid("SP", "29466749731", out msgError); //false

            /* Santa Catarina */
            response = IsValid("SC", "882894366", out msgError); //true
            response = IsValid("SC", "8828943661", out msgError); //false

            /* Sergipe */
            response = IsValid("SE", "933380836", out msgError); //true
            response = IsValid("SE", "9333808361", out msgError); //false

            /* Tocantins */
            response = IsValid("TO", "64033515836", out msgError); //true
            response = IsValid("TO", "640335158361", out msgError); //false
        }

        public static bool IsValid(string siglaUf, string inscricaoEstadual, out string msgError)
        {
            string strIE = RemoveMascara(inscricaoEstadual);
            siglaUf = siglaUf?.ToUpper();

            msgError = string.Empty;
            if (inscricaoEstadual?.ToUpper() == "ISENTO" || siglaUf == "EX")
                return true;

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
            //FONTE: http://www.sintegra.gov.br/Cad_Estados/cad_PE.html

            int tam = ie.Length;
            if (tam == 9)
            {
                #region Cálculo do DV 1
                int soma = 0;
                int peso = 2;

                for (int i = tam - 3; i >= 0; i--)
                {
                    soma += int.Parse(ie[i].ToString()) * peso;

                    peso = peso == 9 ? 2 : ++peso;
                }

                var d1 = soma % 11;

                d1 = d1 <= 1 ? 0 : (11 - d1);

                #endregion

                #region Cálculo do DV 2
                soma = 0;
                peso = 2;

                for (int i = tam - 2; i >= 0; i--)
                {
                    soma += int.Parse(ie[i].ToString()) * peso;

                    peso = peso == 9 ? 2 : ++peso;
                }

                var d2 = soma % 11;

                d2 = d2 <= 1 ? 0 : (11 - d2);

                #endregion

                #region Validação dos DVs
                string dv1 = d1.ToString();
                string dv2 = d2.ToString();
                if (!ie.Substring(tam - 2).Equals(dv1) && !ie.Substring(tam - 1).Equals(dv2))
                {
                    msgError = ERRO_MsgDigitoVerificadorInvalido;
                    return false;
                }
                #endregion
            }
            else if (tam == 14)
            {
                #region Cálculo do DV
                int soma = 0;
                int peso = 2;

                for (int i = tam - 2; i >= 0; i--)
                {
                    soma += int.Parse(ie[i].ToString()) * peso;

                    peso = peso == 9 ? 1 : ++peso;
                }

                var d1 = soma % 11;

                d1 = d1 <= 1 ? 0 : (11 - d1);

                #endregion

                #region Validação dos DVs
                string dv1 = d1.ToString();

                if (!ie.Substring(tam - 1).Equals(dv1))
                {
                    msgError = ERRO_MsgDigitoVerificadorInvalido;
                    return false;
                }
                #endregion
            }
            else
            {
                msgError = ERRO_QtdDigitosInvalida;
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