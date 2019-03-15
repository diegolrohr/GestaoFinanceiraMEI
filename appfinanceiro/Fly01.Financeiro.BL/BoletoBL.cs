using Boleto2Net;
using Fly01.Core.Entities.Domains.Enum;
using System;

namespace Fly01.Financeiro.BL
{
    public class BoletoBL
    {
        public string CarteiraPadrao { get; set; }
        public string VariacaoCarteira { get; set; }

        public static BoletoBL GetTipoCarteira(int codigoBanco)
        {
            var carteira = new BoletoBL();
            var tipo = (TipoCodigoBanco)Enum.ToObject(typeof(TipoCodigoBanco), codigoBanco);

            switch (tipo)
            {
                case TipoCodigoBanco.BancoBrasil:
                    carteira.CarteiraPadrao = "11";
                    carteira.VariacaoCarteira = "019";
                    break;
                case TipoCodigoBanco.Santander:
                    carteira.CarteiraPadrao = "101";
                    carteira.VariacaoCarteira = "";
                    break;
                case TipoCodigoBanco.Bradesco:
                    carteira.CarteiraPadrao = "09";
                    carteira.VariacaoCarteira = "";
                    break;
                case TipoCodigoBanco.Caixa:
                    carteira.CarteiraPadrao = "SIG14";
                    carteira.VariacaoCarteira = "";
                    break;
                case TipoCodigoBanco.Itau:
                    carteira.CarteiraPadrao = "109";
                    carteira.VariacaoCarteira = "";
                    break;
                case TipoCodigoBanco.Banrisul:
                    carteira.CarteiraPadrao = "1";
                    carteira.VariacaoCarteira = "";
                    break;
            }

            return carteira;
        }

        public static TipoArquivo GetTipoCnab(int codigo)
        {
            TipoCodigoBanco tipo = (TipoCodigoBanco)Enum.ToObject(typeof(TipoCodigoBanco), codigo);
            switch (tipo)
            {
                case TipoCodigoBanco.Itau:
                    return TipoArquivo.CNAB400;

                case TipoCodigoBanco.Banrisul:
                    return TipoArquivo.CNAB400;

                case TipoCodigoBanco.Bradesco:
                    return TipoArquivo.CNAB400;

                case TipoCodigoBanco.BancoBrasil:
                    return TipoArquivo.CNAB400;
            }
            return TipoArquivo.CNAB240;
        }
    }
}
