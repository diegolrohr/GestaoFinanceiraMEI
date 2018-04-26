using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
{
    public class CnabBL : PlataformaBaseBL<Cnab>
    {
        public CnabBL(AppDataContextBase context) : base(context) { }

        public override void ValidaModel(Cnab entity)
        {
            var contaBancaria = new Boleto2Net.ContaBancaria
            {
                Agencia = entity.ContaBancariaSacado.Agencia,
                DigitoAgencia = entity.ContaBancariaSacado.DigitoAgencia,
                Conta = entity.ContaBancariaSacado.Conta,
                DigitoConta = entity.ContaBancariaSacado.DigitoConta,
                CarteiraPadrao = "109",
                TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                TipoFormaCadastramento = TipoFormaCadastramento.ComRegistro,
                TipoImpressaoBoleto = TipoImpressaoBoleto.Empresa
            };


            var boleto = new Boleto2Net.Boleto()
            Boleto2Net.Banco.Instancia(1).FormataNossoNumero();
            //var nossoNumero = 

            base.ValidaModel(entity);
        }
    }
}