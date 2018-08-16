namespace Fly01.Core.Entities.Domains.Commons
{
    public class ProdutoOrdemServico : Produto
    {
        public bool ObjetoDeManutencao { get; set; }

        #region Navigation
        public virtual Produto Produto { get; set; }
        #endregion
    }
}
