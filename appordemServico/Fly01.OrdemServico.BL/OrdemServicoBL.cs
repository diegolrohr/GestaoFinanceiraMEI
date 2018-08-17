namespace Fly01.OrdemServico.BL
{
    public class OrdemServicoBL
    {
        protected OrdemServicoItemServicoBL OrdemServicoItemServicoBL { get; set; }
        protected OrdemServicoItemProdutoBL OrdemServicoItemProdutoBL { get; set; }
        protected OrdemServicoManutencaoBL OrdemServicoManutencaoBL { get; set; }

        //public OrdemServicoBL(AppDataContextBase context,
        //                      OrdemServicoItemServicoBL ordemServicoItemServicoBL,
        //                      OrdemServicoItemProdutoBL ordemServicoItemProdutoBL,
        //                      OrdemServicoManutencaoBL ordemServicoManutencaoBL) : base(context)
        //{
        //    OrdemServicoItemServicoBL = ordemServicoItemServicoBL;
        //    OrdemServicoItemProdutoBL = ordemServicoItemProdutoBL;
        //    OrdemServicoManutencaoBL = ordemServicoManutencaoBL;
        //}

        //public override void ValidaModel(OrdemServico entity)
        //{

        //}

        //public override void Insert(OrdemServico entity)
        //{
        //    if (entity.Id == default(Guid))
        //    {
        //        entity.Id = Guid.NewGuid();
        //    }
        //}

        //public override void Update(OrdemServico entity)
        //{
        //    var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
        //    entity.Fail(previous.Status != StatusOrdemVenda.Aberto && entity.Status != StatusOrdemVenda.Aberto, new Error("Somente venda em aberto pode ser alterada", "status"));

        //    ValidaModel(entity);

        //    if (entity.Status == StatusOrdemVenda.Finalizado && entity.TipoOrdemVenda == TipoOrdemVenda.Pedido && entity.GeraNotaFiscal && entity.IsValid())
        //    {
        //        GeraNotasFiscais(entity);
        //    }

        //    base.Update(entity);
        //}

        //public override void Delete(OrdemServico entityToDelete)
        //{
        //    entityToDelete.Fail(entityToDelete.Status != StatusOrdemVenda.Aberto, new Error("Somente venda em aberto pode ser deletada", "status"));

        //    if (!entityToDelete.IsValid())
        //        throw new BusinessException(entityToDelete.Notification.Get());

        //    base.Delete(entityToDelete);
        //}

        //public static string ValorBCSTRetidoRequerido = "Valor da base de cálculo do ICMS substituído é obrigatório para CSOSN 500. Item {itemcount} da lista de produtos";
        //public static string ValorICMSSTRetidoRequerido = "Valor do ICMS substituído é obrigatório para CSOSN 500. Item {itemcount} da lista de produtos";
        //public static string ValorCreditoSNRequerido = "Valor crédito do ICMS é obrigatório para CSOSN 101 e 201. Item {itemcount} da lista de produtos";
    }
}
