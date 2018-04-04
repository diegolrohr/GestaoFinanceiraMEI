using Fly01.Compras.Domain.Enums;

namespace Fly01.Compras.Domain.Entities
{
    public class Orcamento : OrdemCompra
    {
        public Orcamento()
        {
            TipoOrdemCompra = TipoOrdemCompra.Orcamento;
        }

        //#region Navigations Properties
        //public virtual List<OrcamentoItem> OrcamentoItens { get; set; }
        //#endregion
    }
}
