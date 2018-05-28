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
            if (codigoBanco.ToString() == "1") { this.CarteiraPadrao = "11"; this.VariacaoCarteira = "019"; } //Banco do Brasil
            else if (codigoBanco.ToString() == "33") { this.CarteiraPadrao = "101"; this.VariacaoCarteira = ""; } //Banco Santander
            else if (codigoBanco.ToString() == "237") { this.CarteiraPadrao = "09"; this.VariacaoCarteira = ""; } //Banco Bradesco SIG14
            else if (codigoBanco.ToString() == "104") { this.CarteiraPadrao = "SIG14"; this.VariacaoCarteira = ""; } //Banco Caixa
            else if (codigoBanco.ToString() == "341") { this.CarteiraPadrao = "112"; this.VariacaoCarteira = ""; } //Banco Itau
            else if (codigoBanco.ToString() == "41") { this.CarteiraPadrao = "1"; this.VariacaoCarteira = ""; } //Banrisul
        }
    }
}
