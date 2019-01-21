using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class UtilizarKitVM
    {
        [JsonProperty("orcamentoPedidoId")]
        public Guid OrcamentoPedidoId { get; set; }

        [JsonProperty("kitId")]
        public Guid KitId  { get; set; }

        [JsonProperty("grupoTributarioProdutoId")]
        public Guid? GrupoTributarioProdutoId { get; set; }

        [JsonProperty("grupoTributarioServicoId")]
        public Guid? GrupoTributarioServicoId { get; set; }

        [JsonProperty("fornecedorPadraoId")]
        public Guid? FornecedorPadraoId { get; set; }

        [JsonProperty("adicionarProdutos")]
        public bool AdicionarProdutos { get; set; }

        [JsonProperty("adicionarServicos")]
        public bool AdicionarServicos { get; set; }

        [JsonProperty("somarExistentes")]
        public bool SomarExistentes { get; set; }

        [JsonProperty("kit")]
        public virtual KitVM Kit { get; set; }

        [JsonProperty("grupoTributarioProduto")]
        public virtual GrupoTributarioVM GrupoTributarioProduto { get; set; }

        [JsonProperty("grupoTributarioServico")]
        public virtual GrupoTributarioVM GrupoTributarioServico { get; set; }

        [JsonProperty("fornecedorPadrao")]
        public virtual PessoaVM FornecedorPadrao { get; set; }

    }
}