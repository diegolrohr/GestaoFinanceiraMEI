namespace Fly01.OrdemServico.BL
{
    public class OrdemServicoItemProdutoBL
    {
        //public OrdemServicoItemProdutoBL(AppDataContextBase context) : base(context)
        //{
        //}

        //public object All { get; private set; }

        //public override void ValidaModel(OrdemServicoItemProduto entity)
        //{
        //    entity.Fail(entity.Valor < 0, new Error("Valor não pode ser negativo", "valor"));
        //    entity.Fail(entity.Quantidade < 0, new Error("Quantidade não pode ser negativo", "quantidade"));
        //    entity.Fail(entity.Desconto < 0, new Error("Desconto não pode ser negativo", "desconto"));
        //    entity.Fail(entity.Desconto > (entity.Quantidade * entity.Valor), new Error("O Desconto não pode ser maior ao total", "desconto"));
        //    entity.Fail(entity.Total < 0, new Error("O Total não pode ser negativo", "total"));

        //    var jaExiste = All.Any(x => x.OrdemVendaId == entity.OrdemVendaId && x.ProdutoId == entity.ProdutoId && x.GrupoTributarioId == entity.GrupoTributarioId && x.Id != entity.Id);
        //    entity.Fail(jaExiste, new Error("Este produto com este grupo tributário já está adicionado"));

        //    base.ValidaModel(entity);
        //}
    }
}
