using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using System.Data.Entity;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using System;
using System.Collections.Generic;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Compras.BL
{
    public class PedidoBL : PlataformaBaseBL<Pedido>
    {
        protected PedidoItemBL PedidoItemBL;
        protected NFeEntradaBL NFeEntradaBL { get; set; }
        protected NFeProdutoEntradaBL NFeProdutoEntradaBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected NotaFiscalItemTributacaoEntradaBL NotaFiscalItemTributacaoEntradaBL { get; set; }
        public const int MaxLengthObservacao = 200;
        protected KitItemBL KitItemBL { get; set; }
        protected EstadoBL EstadoBL { get; set; }
        protected OrdemCompraBL OrdemCompraBL { get; set; }
        protected ConfiguracaoPersonalizacao ConfiguracaoPersonalizacao { get; set; }
        protected bool emiteNotaFiscal;
        protected bool exibirTransportadora;

        private readonly string descricaoCompra = @"Compra nº: {0}";
        private readonly string observacaoCompra = @"Observação gerada pela compra nº {0} applicativo Bemacash Compras: {1}";
        private readonly string routePrefixNameMovimentoPedidoCompra = @"MovimentoPedidoCompra";
        private readonly string routePrefixNameContaPagar = @"ContaPagar";
        private readonly string routePrefixNameContaReceber = @"ContaReceber";


        public PedidoBL(AppDataContextBase context, PedidoItemBL pedidoItemBL, OrdemCompraBL ordemCompraBL, NFeEntradaBL nFeEntradaBL, NFeProdutoEntradaBL nFeProdutoEntradaBL, NotaFiscalItemTributacaoEntradaBL notaFiscalItemTributacaoEntradaBL, TotalTributacaoBL totalTributacaoBL, KitItemBL kitItemBL, EstadoBL estadoBL, ConfiguracaoPersonalizacaoBL configuracaoPersonalizacaoBL) : base(context)
        {
            PedidoItemBL = pedidoItemBL;
            OrdemCompraBL = ordemCompraBL;
            NFeEntradaBL = nFeEntradaBL;
            NFeProdutoEntradaBL = nFeProdutoEntradaBL;
            NotaFiscalItemTributacaoEntradaBL = notaFiscalItemTributacaoEntradaBL;
            TotalTributacaoBL = totalTributacaoBL;
            KitItemBL = kitItemBL;
            EstadoBL = estadoBL;
            ConfiguracaoPersonalizacao = configuracaoPersonalizacaoBL.All.AsNoTracking().FirstOrDefault();
            emiteNotaFiscal = ConfiguracaoPersonalizacao != null ? ConfiguracaoPersonalizacao.EmiteNotaFiscal : true;
            exibirTransportadora = ConfiguracaoPersonalizacao != null ? ConfiguracaoPersonalizacao.ExibirStepTransportadoraCompras : true;
        }


        public List<Pedido> GetPedidos()
        {
            var pedidos = new List<Pedido>();

            pedidos = All.Where(x => x.Status == StatusOrdemCompra.Aberto && x.Ativo == true).ToList();

            return pedidos;
        }

        public override void ValidaModel(Pedido entity)
        {
            if (!string.IsNullOrEmpty(entity.MensagemPadraoNota))
                entity.Fail(entity.MensagemPadraoNota.Length > 4000, new Error("O SEFAZ permite até 4000 caracteres."));
            entity.Fail(entity.ValorFrete.HasValue && entity.ValorFrete.Value < 0, new Error("Valor frete não pode ser negativo", "valorFrete"));
            entity.Fail(entity.PesoBruto.HasValue && entity.PesoBruto.Value < 0, new Error("Peso bruto não pode ser negativo", "pesoBruto"));
            entity.Fail(entity.PesoLiquido.HasValue && entity.PesoLiquido.Value < 0, new Error("Peso liquido não pode ser negativo", "pesoLiquido"));
            entity.Fail(entity.QuantidadeVolumes.HasValue && entity.QuantidadeVolumes.Value < 0, new Error("Quantidade de volumes não pode ser negativo", "quantidadeVolumes"));
            entity.Fail(entity.Observacao != null && entity.Observacao.Length > 200, new Error("A observacao não poder ter mais de 200 caracteres", "observacao"));
            entity.Fail(entity.Numero < 1, new Error("O número do pedido é inválido"));
            entity.Fail(All.Any(x => x.Numero == entity.Numero && x.Id != entity.Id && x.Ativo), new Error("O número do pedido já foi utilizado"));
            entity.Fail(entity.TipoCompra == TipoCompraVenda.Devolucao && !string.IsNullOrEmpty(entity.ChaveNFeReferenciada) && entity.ChaveNFeReferenciada.Length != 44, new Error("A chave da nota fiscal referenciada deve conter 44 caracteres"));

            if (entity.Status == StatusOrdemCompra.Finalizado)
            {
                var produtos = PedidoItemBL.All.AsNoTracking().Where(x => x.PedidoId == entity.Id).ToList();
                var hasEstoqueNegativo = VerificaEstoqueNegativo(entity.Id, entity.TipoCompra.ToString()).Any();

                bool isNfeImportacao = (entity.NFeImportacaoId != null && entity.NFeImportacaoId != default(Guid));
                bool freteEmpresa = (
                    (entity.TipoFrete == TipoFrete.FOB || entity.TipoFrete == TipoFrete.Destinatario) && exibirTransportadora
                );
                entity.Fail(freteEmpresa && !isNfeImportacao && (entity.TransportadoraId == null || entity.TransportadoraId == default(Guid)), new Error("Se configurou o frete por conta da sua empresa, informe a transportadora"));

                if (entity.GeraNotaFiscal && emiteNotaFiscal)
                {
                    TotalTributacaoBL.DadosValidosCalculoTributario(entity, entity.FornecedorId);
                    entity.Fail(entity.FormaPagamentoId == null, new Error("Para finalizar o pedido que gera nota fiscal, informe a forma de pagamento"));
                    entity.Fail(entity.TipoCompra == TipoCompraVenda.Devolucao && string.IsNullOrEmpty(entity.ChaveNFeReferenciada), new Error("Para finalizar o pedido de devolução que gera nota fiscal, informe a chave da nota fiscal referenciada"));
                }

                entity.Fail(entity.MovimentaEstoque && hasEstoqueNegativo & !entity.AjusteEstoqueAutomatico, new Error("Para finalizar o pedido o estoque não poderá ficar negativo, realize os ajustes de entrada ou marque para gerar as movimentações de entrada automáticas"));
                entity.Fail((entity.GeraNotaFiscal && emiteNotaFiscal) && string.IsNullOrEmpty(entity.NaturezaOperacao), new Error("Para finalizar o pedido que gera nota fiscal, informe a natureza de operação"));
                entity.Fail(entity.TipoOrdemCompra == TipoOrdemCompra.Orcamento, new Error("Orçamento não pode ser finalizado. Converta em pedido para finalizar"));
                //entity.Fail(!produtos.Any(), new Error("Para finalizar a compra é necessário ao menos ter adicionado um produto."));
                entity.Fail(produtos.Any(x => x.GrupoTributarioId == default(Guid)) && (entity.GeraNotaFiscal && emiteNotaFiscal), new Error("Para finalizar o pedido que gera nota fiscal, informe o grupo tributário nos produtos."));

                entity.Fail(
                    (entity.GeraFinanceiro && (entity.FormaPagamentoId == null || entity.CondicaoParcelamentoId == null || entity.CategoriaId == null || entity.DataVencimento == null)),
                    new Error("Compra que gera financeiro é necessário informar forma de pagamento, condição de parcelamento, categoria e data vencimento")
                    );
            }

            base.ValidaModel(entity);
        }

        public IQueryable<OrdemCompra> Everything => repository.All.Where(x => x.PlataformaId == PlataformaUrl);

        protected dynamic GetNotaFiscal(Pedido entity, TipoNotaFiscal tipo)
        {
            dynamic notaFiscal;

            notaFiscal = new NFeEntrada();

            notaFiscal.Id = Guid.NewGuid();
            notaFiscal.ChaveNFeReferenciada = tipo == TipoNotaFiscal.NFe ? entity.ChaveNFeReferenciada : null;
            notaFiscal.OrdemCompraOrigemId = entity.Id;
            notaFiscal.TipoCompra = entity.TipoCompra;
            notaFiscal.Status = StatusNotaFiscal.NaoTransmitida;
            notaFiscal.Data = entity.Data;
            notaFiscal.FornecedorId = entity.FornecedorId;
            notaFiscal.TransportadoraId = entity.TransportadoraId;
            notaFiscal.TipoFrete = entity.TipoFrete;
            notaFiscal.PlacaVeiculo = entity.PlacaVeiculo;
            notaFiscal.EstadoPlacaVeiculoId = entity.EstadoPlacaVeiculoId;
            notaFiscal.ValorFrete = tipo == TipoNotaFiscal.NFe ? entity.ValorFrete : 0;
            notaFiscal.PesoBruto = entity.PesoBruto;
            notaFiscal.PesoLiquido = entity.PesoLiquido;
            notaFiscal.QuantidadeVolumes = entity.QuantidadeVolumes;
            notaFiscal.FormaPagamentoId = entity.FormaPagamentoId;
            notaFiscal.CentroCustoId = entity.CentroCustoId;
            notaFiscal.CondicaoParcelamentoId = entity.CondicaoParcelamentoId;
            notaFiscal.CategoriaId = entity.CategoriaId;
            notaFiscal.DataVencimento = entity.DataVencimento;
            notaFiscal.Observacao = entity.Observacao;
            notaFiscal.NaturezaOperacao = entity.NaturezaOperacao;
            notaFiscal.GeraFinanceiro = entity.GeraFinanceiro;
            notaFiscal.ContaFinanceiraParcelaPaiId = entity.ContaFinanceiraParcelaPaiId;
            notaFiscal.MensagemPadraoNota = entity.MensagemPadraoNota;
            return notaFiscal;
        }

        protected void GeraNotasFiscais(Pedido entity)
        {
            var produtos = PedidoItemBL.AllIncluding(x => x.GrupoTributario).Where(x => x.PedidoId == entity.Id).ToList();

            if (produtos != null & produtos.Any())
            {
                var NFe = (NFeEntrada)GetNotaFiscal(entity, TipoNotaFiscal.NFe);
                var tributacoesProdutos = TotalTributacaoBL.TributacoesOrdemCompraItem(produtos, entity.FornecedorId, entity.TipoCompra, entity.TipoFrete, entity.ValorFrete);
                var totalImpostosProdutos = TotalTributacaoBL.TributacaoItemAgregaNota(tributacoesProdutos.ToList<TributacaoItemRetorno>());
                var totalImpostosProdutosNaoAgrega = TotalTributacaoBL.TributacaoItemNaoAgregaNota(tributacoesProdutos.ToList<TributacaoItemRetorno>());

                NFe.TotalImpostosProdutos = totalImpostosProdutos;
                NFe.TotalImpostosProdutosNaoAgrega = totalImpostosProdutosNaoAgrega;
                entity.TotalImpostosProdutos = totalImpostosProdutos;
                entity.TotalImpostosProdutosNaoAgrega = totalImpostosProdutosNaoAgrega;

                var nfeProdutos = produtos.Select(
                        x => new NFeProdutoEntrada
                        {
                            Id = Guid.NewGuid(),
                            NotaFiscalEntradaId = NFe.Id,
                            ProdutoId = x.ProdutoId,
                            GrupoTributarioId = x.GrupoTributarioId.Value,
                            Quantidade = x.Quantidade,
                            Valor = x.Valor,
                            Desconto = x.Desconto,
                            Observacao = x.Observacao,
                            ValorBCSTRetido = x.ValorBCSTRetido,
                            ValorICMSSTRetido = x.ValorICMSSTRetido,
                            ValorCreditoICMS = x.ValorCreditoICMS,
                            ValorFCPSTRetidoAnterior = x.ValorFCPSTRetidoAnterior,
                            ValorBCFCPSTRetidoAnterior = x.ValorBCFCPSTRetidoAnterior,
                            PercentualReducaoBC = x.PercentualReducaoBC,
                            PercentualReducaoBCST = x.PercentualReducaoBCST,
                            PedidoItemId = x.Id
                        }).ToList();

                var nfeProdutosTributacao = new List<NotaFiscalItemTributacao>();
                foreach (var x in tributacoesProdutos)
                {
                    var nfeProduto = nfeProdutos.Where(y => y.PedidoItemId == x.PedidoItemId).FirstOrDefault();
                    var grupoTributario = TotalTributacaoBL.GetGrupoTributario(nfeProduto.GrupoTributarioId);
                    NotaFiscalItemTributacaoEntradaBL.Insert(
                        new NotaFiscalItemTributacaoEntrada
                        {
                            NotaFiscalItemEntradaId = nfeProduto.Id,
                            CalculaICMS = grupoTributario.CalculaIcms,
                            ICMSBase = x.ICMSBase,
                            ICMSAliquota = x.ICMSAliquota,
                            ICMSValor = x.ICMSValor,
                            CalculaIPI = grupoTributario.CalculaIpi,
                            IPIBase = x.IPIBase,
                            IPIAliquota = x.IPIAliquota,
                            IPIValor = x.IPIValor,
                            CalculaST = grupoTributario.CalculaSubstituicaoTributaria,
                            STBase = x.STBase,
                            STAliquota = x.STAliquota,
                            STValor = x.STValor,
                            CalculaCOFINS = grupoTributario.CalculaCofins,
                            COFINSBase = x.COFINSBase,
                            COFINSAliquota = x.COFINSAliquota,
                            COFINSValor = x.COFINSValor,
                            CalculaPIS = grupoTributario.CalculaPis,
                            PISBase = x.PISBase,
                            PISAliquota = x.PISAliquota,
                            PISValor = x.PISValor,
                            FreteValorFracionado = x.FreteValorFracionado,
                            FCPSTBase = x.FCPSTBase,
                            FCPSTAliquota = x.FCPSTAliquota,
                            FCPSTValor = x.FCPSTValor,
                            FCPBase = x.FCPBase,
                            FCPAliquota = x.FCPAliquota,
                            FCPValor = x.FCPValor,
                        });
                }

                NFeEntradaBL.Insert(NFe);

                foreach (var nfeProduto in nfeProdutos)
                {
                    NFeProdutoEntradaBL.Insert(nfeProduto);
                }
            }
        }

        protected void CopiaDadosNFeReferenciadaDevolucao(Pedido entity)
        {
            Guid? idPedidoReferenciado = default(Guid);
            var NFe = NFeEntradaBL.All.AsNoTracking().Where(x => x.SefazId.ToUpper() == entity.ChaveNFeReferenciada.ToUpper()).FirstOrDefault();

            if (NFe != null)
            {
                idPedidoReferenciado = NFe.OrdemCompraOrigemId;

                var pedidoReferenciado = All.AsNoTracking().Where(x => x.Id == idPedidoReferenciado).FirstOrDefault();
                if (pedidoReferenciado != null)
                {
                    var previousData = entity.Data;
                    var previousNumero = entity.Numero;
                    var previousId = entity.Id;
                    var previousFornecedorId = entity.FornecedorId;
                    var previousGrupoTributarioPadraoId = entity.GrupoTributarioPadraoId;
                    var previousChaveNFeReferenciada = entity.ChaveNFeReferenciada;
                    var previousTipoCompra = entity.TipoCompra;

                    #region Copia os dados do pedido de origem da nota fiscal referenciada
                    var fornecedorReferenciado = TotalTributacaoBL.GetPessoa(pedidoReferenciado.FornecedorId);
                    entity.Fail(entity.TipoCompra == TipoCompraVenda.Devolucao && pedidoReferenciado.TipoCompra == TipoCompraVenda.Devolucao, new Error("Não é possível realizar devolução de uma nota fiscal de devolução. Referencie outra nota fiscal.", "tipoCompra"));
                    entity.Fail((fornecedorReferenciado == null || fornecedorReferenciado.Ativo == false), new Error("Informe um fornecedor ativo. Fornecedor da nota fiscal referenciada inexistente ou excluído.", "fornecedorId"));

                    if (entity.IsValid())
                    {
                        pedidoReferenciado.CopyProperties<Pedido>(entity);
                        entity.Id = previousId;
                        entity.Numero = previousNumero;
                        entity.Data = previousData;
                        entity.TipoCompra = previousTipoCompra;

                        if (entity.TipoCompra == TipoCompraVenda.Devolucao)
                        {
                            entity.NaturezaOperacao = null;
                            entity.GrupoTributarioPadraoId = previousGrupoTributarioPadraoId;
                            entity.CategoriaId = null;//inverte receita/despesa, terá que informar no front
                        }

                        entity.FornecedorId = (fornecedorReferenciado != null && fornecedorReferenciado.Ativo == true) ? pedidoReferenciado.FornecedorId : previousFornecedorId;
                        entity.Status = StatusOrdemCompra.Aberto;
                        entity.ChaveNFeReferenciada = previousChaveNFeReferenciada;
                        entity.TipoCompra = previousTipoCompra;

                        var produtos = PedidoItemBL.All.AsNoTracking().Where(x => x.PedidoId == idPedidoReferenciado).ToList();

                        if (produtos != null & produtos.Any())
                        {
                            foreach (var produto in produtos)
                            {
                                var produtoClonado = new PedidoItem();
                                produto.CopyProperties<PedidoItem>(produtoClonado);
                                produtoClonado.Id = default(Guid);
                                produtoClonado.PedidoId = entity.Id;
                                if (entity.TipoCompra == TipoCompraVenda.Devolucao)
                                {
                                    //na devolucao o grupo tributarioPadrão informado é setado, pois os de origem são CFOP venda e teria que entrar um por um para alterar
                                    produtoClonado.GrupoTributarioId = entity.GrupoTributarioPadraoId.HasValue ? entity.GrupoTributarioPadraoId.Value : produtoClonado.GrupoTributarioId;
                                }
                                else if (entity.TipoCompra == TipoCompraVenda.Complementar)
                                {
                                    //na nfe de complemento de preço zeramos os valores para que o usuário possa informar apenas os valores a serem complementados
                                    produtoClonado.GrupoTributarioId = entity.GrupoTributarioPadraoId.HasValue ? entity.GrupoTributarioPadraoId.Value : produtoClonado.GrupoTributarioId;
                                    produtoClonado.Quantidade = 0.00;
                                    produtoClonado.Valor = 0.00;
                                    produtoClonado.Desconto = 0.00;
                                }
                                PedidoItemBL.Insert(produtoClonado);
                            }
                        }
                    }
                    #endregion
                }
            }
            else
            {
                var fornecedorReferenciado = TotalTributacaoBL.GetPessoa(entity.FornecedorId);
                entity.Fail((fornecedorReferenciado == null || fornecedorReferenciado.Ativo == false), new Error("Informe um fornecedor ativo. Fornecedor da nota fiscal referenciada inexistente ou excluído.", "fornecedorId"));
            }
        }

        public override void Insert(Pedido entity)
        {
            var numero = default(int);

            if (entity.Id == default(Guid))
            {
                entity.Id = Guid.NewGuid();
            }

            //rpc = new RpcClient();
            //numero = int.Parse(rpc.Call($"plataformaid={PlataformaUrl},tipoordemcompra={(int)TipoOrdemCompra.Pedido}"));
            numero = All.Max(x => x.Numero) + 1;
            entity.Numero = numero;

            ValidaModel(entity);

            if (entity.Status == StatusOrdemCompra.Aberto && entity.TipoOrdemCompra == TipoOrdemCompra.Pedido && entity.TipoCompra == TipoCompraVenda.Devolucao && !string.IsNullOrEmpty(entity.ChaveNFeReferenciada) && entity.IsValid() && emiteNotaFiscal)
            {
                CopiaDadosNFeReferenciadaDevolucao(entity);
            }

            if (entity.Status == StatusOrdemCompra.Finalizado && entity.TipoOrdemCompra == TipoOrdemCompra.Pedido && (entity.GeraNotaFiscal && emiteNotaFiscal) && entity.IsValid())
            {
                GeraNotasFiscais(entity);
            }

            GetIdPlacaEstado(entity);
            base.Insert(entity);
        }

        public void GetIdPlacaEstado(Pedido entity)
        {
            if (!entity.EstadoPlacaVeiculoId.HasValue && !string.IsNullOrEmpty(entity.EstadoCodigoIbge))
            {
                var dadosEstado = EstadoBL.All.AsNoTracking().FirstOrDefault(x => x.CodigoIbge == entity.EstadoCodigoIbge);
                if (dadosEstado != null)
                {
                    entity.EstadoPlacaVeiculoId = dadosEstado.Id;
                }
            }
        }

        public override void Update(Pedido entity)
        {
            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            if (previous != null)
                entity.Fail(previous.Status != StatusOrdemCompra.Aberto && entity.Status != StatusOrdemCompra.Aberto, new Error("Somente compra em aberto pode ser alterada", "status"));
            ValidaModel(entity);

            GeraIdContaFinanceiraRecuperarDadosParcela(entity);

            //se estava marcado, mas desabilitou depois via personalização, desmarcar
            if (entity.Status == StatusOrdemCompra.Finalizado)
            {
                entity.GeraNotaFiscal = entity.GeraNotaFiscal ? (entity.GeraNotaFiscal && emiteNotaFiscal) : false;
                entity.ValorFrete = exibirTransportadora ? entity.ValorFrete : 0.0;
            }

            if (entity.Status == StatusOrdemCompra.Finalizado && entity.TipoOrdemCompra == TipoOrdemCompra.Pedido && (entity.GeraNotaFiscal && emiteNotaFiscal) && entity.IsValid())
            {
                GeraNotasFiscais(entity);
            }

            base.Update(entity);
        }

        private void GeraIdContaFinanceiraRecuperarDadosParcela(Pedido entity)
        {
            if (entity.GeraFinanceiro && (entity.ContaFinanceiraParcelaPaiId == default(Guid) || entity.ContaFinanceiraParcelaPaiId == null))
            {
                entity.ContaFinanceiraParcelaPaiId = Guid.NewGuid();
            }
        }

        public override void Delete(Pedido entityToDelete)
        {
            entityToDelete.Fail(entityToDelete.Status != StatusOrdemCompra.Aberto, new Error("Somente compra em aberto pode ser deletada", "status"));

            if (!entityToDelete.IsValid())
                throw new BusinessException(entityToDelete.Notification.Get());

            base.Delete(entityToDelete);
        }

        public override void AfterSave(Pedido entity)
        {
            if (entity.Status != StatusOrdemCompra.Finalizado || entity.TipoOrdemCompra != TipoOrdemCompra.Pedido)
                return;

            var produtos = PedidoItemBL.All.Where(e => e.PedidoId == entity.Id && e.Ativo).ToList();

            var observacaoTitulo = string.Format(observacaoCompra, entity.Numero, entity.Observacao);
            if (observacaoTitulo.Length > MaxLengthObservacao)
                observacaoTitulo = observacaoTitulo.Substring(0, MaxLengthObservacao);

            var descricaoTitulo = string.Format(descricaoCompra, entity.Numero);
            if (descricaoTitulo.Length > MaxLengthObservacao)
                descricaoTitulo = descricaoTitulo.Substring(0, MaxLengthObservacao);

            if (entity.GeraFinanceiro)
            {
                bool freteEmpresa = (
                    (entity.TipoFrete == TipoFrete.FOB || entity.TipoFrete == TipoFrete.Destinatario) && exibirTransportadora
                );

                double totalProdutos = produtos != null ? produtos.Select(e => (e.Quantidade * e.Valor) - e.Desconto).Sum() : 0;
                double totalImpostosProdutos = produtos != null && entity.TotalImpostosProdutos.HasValue ? entity.TotalImpostosProdutos.Value : 0;
                double valorPrevisto = totalProdutos + ((entity.GeraNotaFiscal && emiteNotaFiscal) ? totalImpostosProdutos : 0);

                if (freteEmpresa)
                {
                    var contaPagarTransp = new ContaPagar()
                    {
                        ValorPrevisto = entity.ValorFrete.HasValue ? entity.ValorFrete.Value : 0,
                        CategoriaId = entity.CategoriaId.Value,
                        CondicaoParcelamentoId = entity.CondicaoParcelamentoId.Value,
                        PessoaId = entity.TransportadoraId.Value,
                        DataEmissao = entity.Data,
                        DataVencimento = entity.DataVencimento.Value,
                        Descricao = descricaoTitulo,
                        Observacao = observacaoTitulo,
                        CentroCustoId = entity.CentroCustoId,
                        FormaPagamentoId = entity.FormaPagamentoId.Value,
                        PlataformaId = PlataformaUrl,
                        UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao
                    };
                    Producer<ContaPagar>.Send(routePrefixNameContaPagar, AppUser, PlataformaUrl, contaPagarTransp, RabbitConfig.EnHttpVerb.POST);
                }

                if (entity.TipoCompra == TipoCompraVenda.Normal)
                {
                    var contaPagar = new ContaPagar()
                    {
                        Id = entity.ContaFinanceiraParcelaPaiId ?? default(Guid),
                        ValorPrevisto = valorPrevisto,
                        CategoriaId = entity.CategoriaId.Value,
                        CondicaoParcelamentoId = entity.CondicaoParcelamentoId.Value,
                        PessoaId = entity.FornecedorId,
                        DataEmissao = entity.Data,
                        DataVencimento = entity.DataVencimento.Value,
                        Descricao = descricaoTitulo,
                        CentroCustoId = entity.CentroCustoId,
                        Observacao = observacaoTitulo,
                        FormaPagamentoId = entity.FormaPagamentoId.Value,
                        PlataformaId = PlataformaUrl,
                        UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao
                    };
                    Producer<ContaPagar>.Send(routePrefixNameContaPagar, AppUser, PlataformaUrl, contaPagar, RabbitConfig.EnHttpVerb.POST);
                }
                else if (entity.TipoCompra == TipoCompraVenda.Devolucao)
                {
                    var contaReceber = new ContaReceber()
                    {
                        Id = entity.ContaFinanceiraParcelaPaiId ?? default(Guid),
                        ValorPrevisto = valorPrevisto,
                        CategoriaId = entity.CategoriaId.Value,
                        CondicaoParcelamentoId = entity.CondicaoParcelamentoId.Value,
                        PessoaId = entity.FornecedorId,
                        DataEmissao = entity.Data,
                        DataVencimento = entity.DataVencimento.Value,
                        CentroCustoId = entity.CentroCustoId,
                        Descricao = descricaoTitulo,
                        Observacao = observacaoTitulo,
                        FormaPagamentoId = entity.FormaPagamentoId.Value,
                        PlataformaId = PlataformaUrl,
                        UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao
                    };
                    Producer<ContaReceber>.Send(routePrefixNameContaReceber, AppUser, PlataformaUrl, contaReceber, RabbitConfig.EnHttpVerb.POST);
                }
            }

            if (entity.MovimentaEstoque)
            {
                var movimentos = (from ordemCompraProduto in produtos
                                  group ordemCompraProduto by ordemCompraProduto.ProdutoId into groupResult
                                  select new
                                  {
                                      ProdutoId = groupResult.Key,
                                      Total = groupResult.Sum(f => f.Quantidade)
                                  })
                    .Select(x => new MovimentoPedidoCompra
                    {
                        Quantidade = (x.Total),
                        PedidoNumero = entity.Numero,
                        ProdutoId = x.ProdutoId,
                        UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao,
                        TipoCompra = entity.TipoCompra,
                        PlataformaId = PlataformaUrl
                    }).ToList();

                foreach (var movimento in movimentos)
                    Producer<MovimentoPedidoCompra>.Send(routePrefixNameMovimentoPedidoCompra, AppUser, PlataformaUrl, movimento, RabbitConfig.EnHttpVerb.POST);
            }
        }

        public List<PedidoProdutoEstoqueNegativo> VerificaEstoqueNegativo(Guid pedidoId, string tipoCompra)
        {
            var produtos = PedidoItemBL.AllIncluding(p => p.Produto).Where(x => x.PedidoId == pedidoId)
                .GroupBy(x => x.ProdutoId).Select(y => new PedidoProdutoEstoqueNegativo()
                {
                    ProdutoId = y.Key,
                    QuantPedido = y.Sum(f => f.Quantidade),
                    QuantEstoque = y.Select(f => f.Produto.SaldoProduto.HasValue ? f.Produto.SaldoProduto.Value : 0.0).FirstOrDefault(),
                    SaldoEstoque = tipoCompra == "Devolucao" ? y.Select(f => f.Produto.SaldoProduto.HasValue ? f.Produto.SaldoProduto.Value : 0.0).FirstOrDefault() - y.Sum(f => f.Quantidade)
                        : y.Select(f => f.Produto.SaldoProduto.HasValue ? f.Produto.SaldoProduto.Value : 0.0).FirstOrDefault() + y.Sum(f => f.Quantidade),
                    ProdutoDescricao = y.Select(f => f.Produto.Descricao).FirstOrDefault(),
                });

            return produtos.Where(x => x.SaldoEstoque < 0).ToList();
        }

        public TotalPedidoNotaFiscal CalculaTotalOrdemCompra(Guid ordemCompraId, Guid fornecedorId, bool geraNotaFiscal, string tipoCompra, string tipoFrete, double? valorFrete = 0, bool onList = false)
        {
            var tipoVendaEnum = (TipoCompraVenda)Enum.Parse(typeof(TipoCompraVenda), tipoCompra, true);
            var tipoFreteEnum = (TipoFrete)Enum.Parse(typeof(TipoFrete), tipoFrete, true);
            if (tipoFreteEnum != TipoFrete.FOB) { valorFrete = 0; }

            var ordemCompra = All.Where(x => x.Id == ordemCompraId).FirstOrDefault();
            if ((geraNotaFiscal && emiteNotaFiscal) && ordemCompra.Status != StatusOrdemCompra.Finalizado)
            {
                TotalTributacaoBL.DadosValidosCalculoTributario(ordemCompra, fornecedorId, onList);
            }

            var produtos = PedidoItemBL.All.Where(x => x.PedidoId == ordemCompraId).ToList();
            var totalProdutos = produtos != null ? produtos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;

            var totalImpostosProdutos = (ordemCompra.Status == StatusOrdemCompra.Finalizado && ordemCompra.TotalImpostosProdutos.HasValue) ? ordemCompra.TotalImpostosProdutos.Value
                : (produtos != null && (geraNotaFiscal && emiteNotaFiscal) ? TotalTributacaoBL.TotalSomaOrdemCompraProdutos(produtos, fornecedorId, tipoVendaEnum, tipoFreteEnum, valorFrete) : 0.0);

            var totalImpostosProdutosNaoAgrega = ordemCompra.Status == StatusOrdemCompra.Finalizado ? ordemCompra.TotalImpostosProdutosNaoAgrega
                : (produtos != null && (geraNotaFiscal && emiteNotaFiscal)? TotalTributacaoBL.TotalSomaOrdemCompraProdutosNaoAgrega(produtos, fornecedorId, tipoVendaEnum, tipoFreteEnum, valorFrete) : 0.0);

            var result = new TotalPedidoNotaFiscal()
            {
                TotalProdutos = Math.Round(totalProdutos, 2, MidpointRounding.AwayFromZero),
                ValorFrete = Math.Round(valorFrete.Value, 2, MidpointRounding.AwayFromZero),
                TotalImpostosProdutos = Math.Round(totalImpostosProdutos, 2, MidpointRounding.AwayFromZero),
                TotalImpostosProdutosNaoAgrega = Math.Round(totalImpostosProdutosNaoAgrega, 2, MidpointRounding.AwayFromZero),
            };

            return result;
        }

        public void UtilizarKitPedido(UtilizarKitVM entity)
        {
            try
            {
                if (All.Any(x => x.Id == entity.OrcamentoPedidoId))
                {
                    if (KitItemBL.All.Any(x => x.KitId == entity.KitId))
                    {
                        #region Produtos
                        if (entity.AdicionarProdutos)
                        {
                            var kitProdutos = KitItemBL.All.Where(x => x.KitId == entity.KitId && x.TipoItem == TipoItem.Produto);

                            var existentesPedido =
                                from pi in PedidoItemBL.AllIncluding(x => x.Produto).Where(x => x.PedidoId == entity.OrcamentoPedidoId)
                                join ki in kitProdutos on pi.ProdutoId equals ki.ProdutoId
                                select new { ProdutoId = ki.ProdutoId, PedidoItemId = pi.Id, Quantidade = ki.Quantidade };

                            var novasPedidoItens =
                                from kit in kitProdutos
                                where !existentesPedido.Select(x => x.ProdutoId).Contains(kit.ProdutoId)
                                select new
                                {
                                    PedidoId = entity.OrcamentoPedidoId,
                                    ProdutoId = kit.ProdutoId.Value,
                                    Valor = kit.Produto.ValorVenda,
                                    Quantidade = kit.Quantidade,
                                    GrupoTributarioId = entity.GrupoTributarioProdutoId
                                };

                            foreach (var item in novasPedidoItens)
                            {
                                PedidoItemBL.Insert(new PedidoItem()
                                {
                                    GrupoTributarioId = item.GrupoTributarioId != default(Guid) ? item.GrupoTributarioId : (Guid?)null,
                                    ProdutoId = item.ProdutoId,
                                    PedidoId = item.PedidoId,
                                    Valor = item.Valor,
                                    Quantidade = item.Quantidade
                                });
                            }

                            if (entity.SomarExistentes)
                            {
                                foreach (var item in existentesPedido)
                                {
                                    var pedidoItem = PedidoItemBL.Find(item.PedidoItemId);
                                    pedidoItem.Quantidade += item.Quantidade;
                                    PedidoItemBL.Update(pedidoItem);
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public static string ValorBCSTRetidoRequerido = "Valor da base de cálculo do ICMS substituído é obrigatório para CSOSN 500. Item {itemcount} da lista de produtos";
        public static string ValorICMSSTRetidoRequerido = "Valor do ICMS substituído é obrigatório para CSOSN 500. Item {itemcount} da lista de produtos";
        public static string ValorCreditoSNRequerido = "Valor crédito do ICMS é obrigatório para CSOSN 101 e 201. Item {itemcount} da lista de produtos";
    }
}