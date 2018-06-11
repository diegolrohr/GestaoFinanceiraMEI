using Boleto2Net;
using Fly01.Core.Entities.Domains.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.Core.ViewModels.Presentation
{
    public class ValidaDadosBancoVM
    {
        public string CarteiraPadrao { get; set; }
        public string VariacaoCarteira { get; set; }

        public static ValidaDadosBancoVM GetTipoCarteira(int codigoBanco)
        {
            var carteira = new ValidaDadosBancoVM();
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
                    carteira.CarteiraPadrao = "112";
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
            }
            return TipoArquivo.CNAB240;
        }
    }
}
