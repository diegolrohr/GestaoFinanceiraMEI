using Fly01.Core.Entities.Domains.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fly01.Core.ViewModels.Presentation
{
    public sealed class CarteiraVM
    {
        public CarteiraVM(int codigoBanco)
        {
            GetTipoCarteira(codigoBanco);
        }

        public string CarteiraPadrao { get; set; }
        public string VariacaoCarteira { get; set; }

        public void GetTipoCarteira(int codigoBanco)
        {
            TipoCodigoBanco tipo = (TipoCodigoBanco)Enum.ToObject(typeof(TipoCodigoBanco), codigoBanco);

            switch (tipo)
            {
                case TipoCodigoBanco.BancoBrasil:
                    this.CarteiraPadrao = "11"; this.VariacaoCarteira = "019";
                    break;
                case TipoCodigoBanco.Santander:
                    this.CarteiraPadrao = "101"; this.VariacaoCarteira = "";
                    break;
                case TipoCodigoBanco.Bradesco:
                    this.CarteiraPadrao = "09"; this.VariacaoCarteira = "";
                    break;
                case TipoCodigoBanco.Caixa:
                    this.CarteiraPadrao = "SIG14"; this.VariacaoCarteira = "";
                    break;
                case TipoCodigoBanco.Itau:
                    this.CarteiraPadrao = "112"; this.VariacaoCarteira = "";
                    break;
                case TipoCodigoBanco.Banrisul:
                    this.CarteiraPadrao = "1"; this.VariacaoCarteira = "";
                    break;
            }
        }
    }
}
