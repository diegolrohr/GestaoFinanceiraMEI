using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.Core.Entities.Domains.Commons;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fly01.Core.Helpers;

namespace Fly01.Faturamento.BL
{
    public class OrdemVendaBL : PlataformaBaseBL<OrdemVenda>
    {
        protected OrdemVendaProdutoBL OrdemVendaProdutoBL { get; set; }
        protected OrdemVendaServicoBL OrdemVendaServicoBL { get; set; }
        protected NFeBL NFeBL { get; set; }
        protected NFSeBL NFSeBL { get; set; }
        protected NFeProdutoBL NFeProdutoBL { get; set; }
        protected NFSeServicoBL NFSeServicoBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected NotaFiscalItemTributacaoBL NotaFiscalItemTributacaoBL { get; set; }

        private readonly string descricaoVenda = @"Venda nº: {0}";
        private readonly string observacaoVenda = @"Observação gerada pela venda nº {0} applicativo Bemacash Faturamento: {1}";
        private readonly string routePrefixNameMovimentoOrdemVenda = @"MovimentoOrdemVenda";
        private readonly string routePrefixNameContaPagar = @"ContaPagar";
        private readonly string routePrefixNameContaReceber = @"ContaReceber";

        public OrdemVendaBL(AppDataContextBase context, OrdemVendaProdutoBL ordemVendaProdutoBL, OrdemVendaServicoBL ordemVendaServicoBL, NFeBL nfeBL, NFSeBL nfseBL, NFeProdutoBL nfeProdutoBL, NFSeServicoBL nfseServicoBL, TotalTributacaoBL totalTributacaoBL, NotaFiscalItemTributacaoBL notaFiscalItemTributacaoBL) : base(context)
        {
            OrdemVendaProdutoBL = ordemVendaProdutoBL;
            OrdemVendaServicoBL = ordemVendaServicoBL;
            NFeBL = nfeBL;
            NFeProdutoBL = nfeProdutoBL;
            NFSeBL = nfseBL;
            NFSeServicoBL = nfseServicoBL;
            TotalTributacaoBL = totalTributacaoBL;
            NotaFiscalItemTributacaoBL = notaFiscalItemTributacaoBL;
        }

        public override void ValidaModel(OrdemVenda entity)
        {
            if (!string.IsNullOrEmpty(entity.MensagemPadraoNota))
                entity.Fail(entity.MensagemPadraoNota.Length > 4000, new Error("O SEFAZ permite até 4000 caracteres."));
            entity.Fail(entity.ValorFrete.HasValue && entity.ValorFrete.Value < 0, new Error("Valor frete não pode ser negativo", "valorFrete"));
            entity.Fail(entity.PesoBruto.HasValue && entity.PesoBruto.Value < 0, new Error("Peso bruto não pode ser negativo", "pesoBruto"));
            entity.Fail(entity.PesoLiquido.HasValue && entity.PesoLiquido.Value < 0, new Error("Peso liquido não pode ser negativo", "pesoLiquido"));
            entity.Fail(entity.QuantidadeVolumes.HasValue && entity.QuantidadeVolumes.Value < 0, new Error("Quantidade de volumes não pode ser negativo", "quantidadeVolumes"));
            entity.Fail(entity.Observacao != null && entity.Observacao.Length > 200, new Error("A observacao não poder ter mais de 200 caracteres", "observacao"));
            entity.Fail(entity.Numero < 1, new Error("O número do orçamento/pedido é inválido"));
            entity.Fail(All.Any(x => x.Numero == entity.Numero && x.Id != entity.Id && x.Ativo), new Error("O número do orçamento/pedido já foi utilizado"));
            entity.Fail(entity.TipoVenda == TipoFinalidadeEmissaoNFe.Devolucao && !string.IsNullOrEmpty(entity.ChaveNFeReferenciada) && entity.ChaveNFeReferenciada.Length != 44, new Error("A chave da nota fiscal referenciada deve conter 44 caracteres"));

            if (entity.Status == StatusOrdemVenda.Finalizado)
            {
                var produtos = OrdemVendaProdutoBL.All.AsNoTracking().Where(x => x.OrdemVendaId == entity.Id).ToList();
                var servicos = OrdemVendaServicoBL.All.AsNoTracking().Where(x => x.OrdemVendaId == entity.Id).ToList();
                var hasEstoqueNegativo = VerificaEstoqueNegativo(entity.Id, entity.TipoVenda.ToString()).Any();

                bool pagaFrete = (
                    ((entity.TipoFrete == TipoFrete.CIF || entity.TipoFrete == TipoFrete.Remetente) && entity.TipoVenda == TipoFinalidadeEmissaoNFe.Normal) ||
                    ((entity.TipoFrete == TipoFrete.FOB || entity.TipoFrete == TipoFrete.Destinatario) && entity.TipoVenda == TipoFinalidadeEmissaoNFe.Devolucao)
                );
                entity.Fail(pagaFrete && (entity.TransportadoraId == null || entity.TransportadoraId == default(Guid)), new Error("Se configurou o frete por conta da sua empresa, informe a transportadora"));

                if (entity.GeraNotaFiscal)
                {
                    TotalTributacaoBL.DadosValidosCalculoTributario(entity, entity.ClienteId);
                    entity.Fail(entity.FormaPagamentoId == null, new Error("Para finalizar o pedido que gera nota fiscal, informe a forma de pagamento"));
                    entity.Fail(entity.TipoVenda == TipoFinalidadeEmissaoNFe.Devolucao && string.IsNullOrEmpty(entity.ChaveNFeReferenciada), new Error("Para finalizar o pedido de devolução que gera nota fiscal, informe a chave da nota fiscal referenciada"));
                }

                entity.Fail(entity.MovimentaEstoque && hasEstoqueNegativo & !entity.AjusteEstoqueAutomatico, new Error("Para finalizar o pedido o estoque não poderá ficar negativo, realize os ajustes de entrada ou marque para gerar as movimentações de entrada automáticas"));
                entity.Fail(entity.GeraNotaFiscal && string.IsNullOrEmpty(entity.NaturezaOperacao), new Error("Para finalizar o pedido que gera nota fiscal, informe a natureza de operação"));
                entity.Fail(entity.TipoOrdemVenda == TipoOrdemVenda.Orcamento, new Error("Orçamento não pode ser finalizado. Converta em pedido para finalizar"));
                entity.Fail(!produtos.Any() && !servicos.Any(), new Error("Para finalizar a venda é necessário ao menos ter adicionado um produto ou um serviço"));
                entity.Fail(entity.TipoVenda == TipoFinalidadeEmissaoNFe.Devolucao && servicos.Any(), new Error("Finalidade de devolução não pode conter serviços, somente produtos"));
                entity.Fail(
                    (entity.GeraFinanceiro && (entity.FormaPagamentoId == null || entity.CondicaoParcelamentoId == null || entity.CategoriaId == null || entity.DataVencimento == null)),
                    new Error("Venda que gera financeiro é necessário informar forma de pagamento, condição de parcelamento, categoria e data vencimento")
                    );
            }

            base.ValidaModel(entity);
        }

        public IQueryable<OrdemVenda> Everything => repository.All.Where(x => x.PlataformaId == PlataformaUrl);

        protected dynamic GetNotaFiscal(OrdemVenda entity, TipoNotaFiscal tipo)
        {
            dynamic notaFiscal;
            if (tipo == TipoNotaFiscal.NFe)
            {
                notaFiscal = new NFe();
            }
            else
            {
                notaFiscal = new NFSe();
            }

            notaFiscal.Id = Guid.NewGuid();
            notaFiscal.ChaveNFeReferenciada = tipo == TipoNotaFiscal.NFe ? entity.ChaveNFeReferenciada : null;
            notaFiscal.OrdemVendaOrigemId = entity.Id;
            notaFiscal.TipoVenda = entity.TipoVenda;
            notaFiscal.Status = StatusNotaFiscal.NaoTransmitida;
            notaFiscal.Data = entity.Data;
            notaFiscal.ClienteId = entity.ClienteId;
            notaFiscal.TransportadoraId = entity.TransportadoraId;
            notaFiscal.TipoFrete = entity.TipoFrete;
            notaFiscal.PlacaVeiculo = entity.PlacaVeiculo;
            notaFiscal.EstadoPlacaVeiculoId = entity.EstadoPlacaVeiculoId;
            notaFiscal.ValorFrete = tipo == TipoNotaFiscal.NFe ? entity.ValorFrete : 0;
            notaFiscal.PesoBruto = entity.PesoBruto;
            notaFiscal.PesoLiquido = entity.PesoLiquido;
            notaFiscal.QuantidadeVolumes = entity.QuantidadeVolumes;
            notaFiscal.FormaPagamentoId = entity.FormaPagamentoId;
            notaFiscal.CondicaoParcelamentoId = entity.CondicaoParcelamentoId;
            notaFiscal.CategoriaId = entity.CategoriaId;
            notaFiscal.DataVencimento = entity.DataVencimento;
            notaFiscal.Observacao = entity.Observacao;
            notaFiscal.NaturezaOperacao = entity.NaturezaOperacao;
            notaFiscal.MensagemPadraoNota = entity.MensagemPadraoNota;
            return notaFiscal;
        }

        protected void GeraNotasFiscais(OrdemVenda entity)
        {
            //gerar notas fiscais divididos por produtos e serviços
            var produtos = OrdemVendaProdutoBL.AllIncluding(x => x.GrupoTributario).Where(x => x.OrdemVendaId == entity.Id).ToList();
            var servicos = OrdemVendaServicoBL.All.Where(x => x.OrdemVendaId == entity.Id).ToList();

            if (produtos != null & produtos.Any())
            {
                var NFe = (NFe)GetNotaFiscal(entity, TipoNotaFiscal.NFe);
                var tributacoesProdutos = TotalTributacaoBL.TributacoesOrdemVendaProdutos(produtos, entity.ClienteId, entity.TipoVenda, entity.TipoFrete, entity.ValorFrete);
                var totalImpostosProdutos = TotalTributacaoBL.TributacaoItemAgregaNota(tributacoesProdutos.ToList<TributacaoItemRetorno>());
                var totalImpostosProdutosNaoAgrega = TotalTributacaoBL.TributacaoItemNaoAgregaNota(tributacoesProdutos.ToList<TributacaoItemRetorno>());

                NFe.TotalImpostosProdutos = totalImpostosProdutos;
                NFe.TotalImpostosProdutosNaoAgrega = totalImpostosProdutosNaoAgrega;
                entity.TotalImpostosProdutos = totalImpostosProdutos;
                entity.TotalImpostosProdutosNaoAgrega = totalImpostosProdutosNaoAgrega;

                var nfeProdutos = produtos.Select(
                        x => new NFeProduto
                        {
                            Id = Guid.NewGuid(),
                            NotaFiscalId = NFe.Id,
                            ProdutoId = x.ProdutoId,
                            GrupoTributarioId = x.GrupoTributarioId,
                            Quantidade = x.Quantidade,
                            Valor = x.Valor,
                            Desconto = x.Desconto,
                            Observacao = x.Observacao,
                            ValorBCSTRetido = x.ValorBCSTRetido,
                            ValorICMSSTRetido = x.ValorICMSSTRetido,
                            ValorCreditoICMS = x.ValorCreditoICMS,
                            ValorFCPSTRetidoAnterior = x.ValorFCPSTRetidoAnterior,
                            ValorBCFCPSTRetidoAnterior = x.ValorBCFCPSTRetidoAnterior
                        }).ToList();

                var nfeProdutosTributacao = new List<NotaFiscalItemTributacao>();
                foreach (var x in tributacoesProdutos)
                {
                    var nfeProduto = nfeProdutos.Where(y => y.ProdutoId == x.ProdutoId && y.GrupoTributarioId == x.GrupoTributarioId).FirstOrDefault();
                    var grupoTributario = TotalTributacaoBL.GetGrupoTributario(nfeProduto.GrupoTributarioId);
                    NotaFiscalItemTributacaoBL.Insert(
                        new NotaFiscalItemTributacao
                        {
                            NotaFiscalItemId = nfeProduto.Id,
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
                            FCPSTBase = x.FCPBase,
                            FCPSTAliquota = x.FCPAliquota,
                            FCPSTValor = x.FCPValor,
                            FCPBase = x.FCPBase,
                            FCPAliquota = x.FCPAliquota,
                            FCPValor = x.FCPValor,
                        });
                }

                NFeBL.Insert(NFe);

                foreach (var nfeProduto in nfeProdutos)
                {
                    NFeProdutoBL.Insert(nfeProduto);
                }
            }
            if (servicos != null & servicos.Any())
            {
                var NFSe = (NFSe)GetNotaFiscal(entity, TipoNotaFiscal.NFSe);
                var totalImpostosServicos = TotalTributacaoBL.TotalSomaOrdemVendaServicos(servicos, entity.ClienteId);

                var nfseServicos = servicos.Select(
                        x => new NFSeServico
                        {
                            NotaFiscalId = NFSe.Id,
                            ServicoId = x.ServicoId,
                            GrupoTributarioId = x.GrupoTributarioId,
                            Quantidade = x.Quantidade,
                            Valor = x.Valor,
                            Desconto = x.Desconto,
                            Observacao = x.Observacao
                        }).ToList();


                NFSe.TotalImpostosServicos = totalImpostosServicos;
                entity.TotalImpostosServicos = totalImpostosServicos;

                NFSeBL.Insert(NFSe);

                foreach (var nfseServico in nfseServicos)
                {
                    NFSeServicoBL.Insert(nfseServico);
                }
            }
        }

        protected void CopiaDadosNFeReferenciadaDevolucao(OrdemVenda entity)
        {
            Guid idPedidoReferenciado = default(Guid);
            var NFe = NFeBL.All.AsNoTracking().Where(x => x.SefazId.ToUpper() == entity.ChaveNFeReferenciada.ToUpper()).FirstOrDefault();

            if (NFe != null)
            {
                idPedidoReferenciado = NFe.OrdemVendaOrigemId;

                var pedidoReferenciado = All.AsNoTracking().Where(x => x.Id == idPedidoReferenciado).FirstOrDefault();
                if (pedidoReferenciado != null)
                {
                    var previousData = entity.Data;
                    var previousNumero = entity.Numero;
                    var previousId = entity.Id;
                    var previousClienteId = entity.ClienteId;
                    var previousGrupoTributarioPadraoId = entity.GrupoTributarioPadraoId;
                    var previousChaveNFeReferenciada = entity.ChaveNFeReferenciada;
                    var previousTipoVenda = entity.TipoVenda;

                    #region Copia os dados do pedido de origem da nota fiscal referenciada
                    var clienteReferenciado = TotalTributacaoBL.GetPessoa(pedidoReferenciado.ClienteId);
                    entity.Fail((clienteReferenciado == null || clienteReferenciado.Ativo == false), new Error("Informe um cliente ativo. Cliente da nota fiscal referenciada inexistente ou excluído.", "clienteId"));

                    if (entity.IsValid())
                    {
                        pedidoReferenciado.CopyProperties<OrdemVenda>(entity);
                        entity.Id = previousId;
                        entity.Numero = previousNumero;
                        entity.Data = previousData;
                        entity.CategoriaId = null;//inverte receita/despesa, terá que informar no front
                        entity.NaturezaOperacao = null;
                        entity.ClienteId = (clienteReferenciado != null && clienteReferenciado.Ativo == true) ? pedidoReferenciado.ClienteId : previousClienteId;
                        entity.Status = StatusOrdemVenda.Aberto;
                        entity.ChaveNFeReferenciada = previousChaveNFeReferenciada;
                        entity.TipoVenda = previousTipoVenda;
                        entity.GrupoTributarioPadraoId = previousGrupoTributarioPadraoId;

                        var produtos = OrdemVendaProdutoBL.All.AsNoTracking().Where(x => x.OrdemVendaId == idPedidoReferenciado).ToList();

                        if (produtos != null & produtos.Any())
                        {
                            foreach (var produto in produtos)
                            {
                                var produtoClonado = new OrdemVendaProduto();
                                produto.CopyProperties<OrdemVendaProduto>(produtoClonado);
                                produtoClonado.Id = default(Guid);
                                produtoClonado.OrdemVendaId = entity.Id;
                                //na devolucao o grupo tributarioPadrão informado é setado, pois os de origem são CFOP venda e teria que entrar um por um para alterar
                                produtoClonado.GrupoTributarioId = entity.GrupoTributarioPadraoId.HasValue ? entity.GrupoTributarioPadraoId.Value : produtoClonado.GrupoTributarioId;
                                OrdemVendaProdutoBL.Insert(produtoClonado);
                            }
                        }
                    }
                    #endregion
                }
            }
            else
            {
                var clienteReferenciado = TotalTributacaoBL.GetPessoa(entity.ClienteId);
                entity.Fail((clienteReferenciado == null || clienteReferenciado.Ativo == false), new Error("Informe um cliente ativo. Cliente da nota fiscal referenciada inexistente ou excluído.", "clienteId"));
            }
        }

        public override void Insert(OrdemVenda entity)
        {
            if (entity.Id == default(Guid))
            {
                entity.Id = Guid.NewGuid();
            }

            var max = Everything.Any(x => x.Id != entity.Id) ? Everything.Max(x => x.Numero) : 0;

            entity.Numero = (max == 1 && !Everything.Any(x => x.Id != entity.Id && x.Ativo && x.Numero == 1)) ? 1 : ++max;

            ValidaModel(entity);

            if (entity.Status == StatusOrdemVenda.Aberto && entity.TipoOrdemVenda == TipoOrdemVenda.Pedido && entity.TipoVenda ==  TipoFinalidadeEmissaoNFe.Devolucao && !string.IsNullOrEmpty(entity.ChaveNFeReferenciada) && entity.IsValid())
            {
                CopiaDadosNFeReferenciadaDevolucao(entity);
            }

            if (entity.Status == StatusOrdemVenda.Finalizado && entity.TipoOrdemVenda == TipoOrdemVenda.Pedido && entity.GeraNotaFiscal && entity.IsValid())
            {
                GeraNotasFiscais(entity);
            }

            base.Insert(entity);
        }

        public override void Update(OrdemVenda entity)
        {
            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            entity.Fail(previous.Status != StatusOrdemVenda.Aberto && entity.Status != StatusOrdemVenda.Aberto, new Error("Somente venda em aberto pode ser alterada", "status"));

            ValidaModel(entity);

            if (entity.Status == StatusOrdemVenda.Finalizado && entity.TipoOrdemVenda == TipoOrdemVenda.Pedido && entity.GeraNotaFiscal && entity.IsValid())
            {
                GeraNotasFiscais(entity);
            }

            base.Update(entity);
        }

        public override void Delete(OrdemVenda entityToDelete)
        {
            entityToDelete.Fail(entityToDelete.Status != StatusOrdemVenda.Aberto, new Error("Somente venda em aberto pode ser deletada", "status"));

            if (!entityToDelete.IsValid())
                throw new BusinessException(entityToDelete.Notification.Get());

            base.Delete(entityToDelete);
        }

        public override void AfterSave(OrdemVenda entity)
        {
            if (entity.Status != StatusOrdemVenda.Finalizado || entity.TipoOrdemVenda != TipoOrdemVenda.Pedido)
                return;

            var produtos = OrdemVendaProdutoBL.All.Where(e => e.OrdemVendaId == entity.Id && e.Ativo).ToList();

            if (entity.GeraFinanceiro)
            {
                bool pagaFrete = (
                    ((entity.TipoFrete == TipoFrete.CIF || entity.TipoFrete == TipoFrete.Remetente) && entity.TipoVenda == TipoFinalidadeEmissaoNFe.Normal) ||
                    ((entity.TipoFrete == TipoFrete.FOB || entity.TipoFrete == TipoFrete.Destinatario) && entity.TipoVenda == TipoFinalidadeEmissaoNFe.Devolucao)
                );
                
                var servicos = OrdemVendaServicoBL.All.Where(e => e.OrdemVendaId == entity.Id && e.Ativo).ToList();
                double totalProdutos = produtos != null ? produtos.Select(e => (e.Quantidade * e.Valor) - e.Desconto).Sum() : 0;
                double totalServicos = servicos != null ? servicos.Select(e => (e.Quantidade * e.Valor) - e.Desconto).Sum() : 0;
                double totalImpostosServicos = 0; //servicos != null ? entity.TotalImpostosServicos.Value : 0;
                double totalImpostosProdutos = produtos != null && entity.TotalImpostosProdutos.HasValue ? entity.TotalImpostosProdutos.Value : 0;
                double valorPrevisto = totalProdutos
                    + totalServicos
                    + (entity.GeraNotaFiscal ? totalImpostosProdutos + totalImpostosServicos : 0);

                if (pagaFrete)
                {
                    var contaPagarTransp = new ContaPagar()
                    {
                        ValorPrevisto = entity.ValorFrete.HasValue ? entity.ValorFrete.Value : 0,
                        CategoriaId = entity.CategoriaId.Value,
                        CondicaoParcelamentoId = entity.CondicaoParcelamentoId.Value,
                        PessoaId = entity.TransportadoraId.Value,
                        DataEmissao = entity.Data,
                        DataVencimento = entity.DataVencimento.Value,
                        Descricao = string.Format(descricaoVenda, entity.Numero),
                        Observacao = string.Format(observacaoVenda, entity.Numero, entity.Observacao),
                        FormaPagamentoId = entity.FormaPagamentoId.Value,
                        PlataformaId = PlataformaUrl,
                        UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao
                    };
                    Producer<ContaPagar>.Send(routePrefixNameContaPagar, AppUser, PlataformaUrl, contaPagarTransp, RabbitConfig.EnHttpVerb.POST);
                }

                if (entity.TipoVenda == TipoFinalidadeEmissaoNFe.Normal)
                {
                    var contaReceber = new ContaReceber()
                    {
                        ValorPrevisto = valorPrevisto,
                        CategoriaId = entity.CategoriaId.Value,
                        CondicaoParcelamentoId = entity.CondicaoParcelamentoId.Value,
                        PessoaId = entity.ClienteId,
                        DataEmissao = entity.Data,
                        DataVencimento = entity.DataVencimento.Value,
                        Descricao = string.Format(descricaoVenda, entity.Numero),
                        Observacao = string.Format(observacaoVenda, entity.Numero, entity.Observacao),
                        FormaPagamentoId = entity.FormaPagamentoId.Value,
                        PlataformaId = PlataformaUrl,
                        UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao
                    };
                    Producer<ContaReceber>.Send(routePrefixNameContaReceber, AppUser, PlataformaUrl, contaReceber, RabbitConfig.EnHttpVerb.POST);
                }
                else if(entity.TipoVenda == TipoFinalidadeEmissaoNFe.Devolucao)
                {
                    var contaPagar = new ContaPagar()
                    {
                        ValorPrevisto = valorPrevisto,
                        CategoriaId = entity.CategoriaId.Value,
                        CondicaoParcelamentoId = entity.CondicaoParcelamentoId.Value,
                        PessoaId = entity.ClienteId,
                        DataEmissao = entity.Data,
                        DataVencimento = entity.DataVencimento.Value,
                        Descricao = string.Format(descricaoVenda, entity.Numero),
                        Observacao = string.Format(observacaoVenda, entity.Numero, entity.Observacao),
                        FormaPagamentoId = entity.FormaPagamentoId.Value,
                        PlataformaId = PlataformaUrl,
                        UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao
                    };
                    Producer<ContaPagar>.Send(routePrefixNameContaPagar, AppUser, PlataformaUrl, contaPagar, RabbitConfig.EnHttpVerb.POST);
                }
            }

            if (entity.MovimentaEstoque)
            {
                var movimentos = (from ordemVendaProduto in produtos
                                  group ordemVendaProduto by ordemVendaProduto.ProdutoId into groupResult
                                  select new
                                  {
                                      ProdutoId = groupResult.Key,
                                      Total = groupResult.Sum(f => f.Quantidade)
                                  })
                    .Select(x => new MovimentoOrdemVenda
                    {
                        Quantidade = (x.Total),
                        PedidoNumero = entity.Numero,
                        ProdutoId = x.ProdutoId,
                        UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao,
                        TipoVenda = entity.TipoVenda,
                        PlataformaId = PlataformaUrl
                    }).ToList();

                foreach (var movimento in movimentos)
                    Producer<MovimentoOrdemVenda>.Send(routePrefixNameMovimentoOrdemVenda, AppUser, PlataformaUrl, movimento, RabbitConfig.EnHttpVerb.POST);
            }
        }

        public List<PedidoProdutoEstoqueNegativo> VerificaEstoqueNegativo(Guid pedidoId, string tipoVenda)
        {
            var produtos = OrdemVendaProdutoBL.AllIncluding(p => p.Produto).Where(x => x.OrdemVendaId == pedidoId)
                .GroupBy(x => x.ProdutoId).Select(y => new PedidoProdutoEstoqueNegativo()
                {
                    ProdutoId = y.Key,
                    QuantPedido = y.Sum(f => f.Quantidade),
                    QuantEstoque = y.Select(f => f.Produto.SaldoProduto.HasValue ? f.Produto.SaldoProduto.Value : 0.0).FirstOrDefault(),
                    SaldoEstoque = tipoVenda == "Normal" ? y.Select(f => f.Produto.SaldoProduto.HasValue ? f.Produto.SaldoProduto.Value : 0.0).FirstOrDefault() - y.Sum(f => f.Quantidade)
                        : y.Select(f => f.Produto.SaldoProduto.HasValue ? f.Produto.SaldoProduto.Value : 0.0).FirstOrDefault() + y.Sum(f => f.Quantidade),
                    ProdutoDescricao = y.Select(f => f.Produto.Descricao).FirstOrDefault(),
                });

            return produtos.Where(x => x.SaldoEstoque < 0).ToList();
        }

        public TotalOrdemVenda CalculaTotalOrdemVenda(Guid ordemVendaId, Guid clienteId, bool geraNotaFiscal, string tipoVenda, string tipoFrete, double? valorFrete = 0, bool onList = false)
        {
            var tipoVendaEnum = (TipoFinalidadeEmissaoNFe)Enum.Parse(typeof(TipoFinalidadeEmissaoNFe), tipoVenda, true);
            var tipoFreteEnum = (TipoFrete)Enum.Parse(typeof(TipoFrete), tipoFrete, true);

            var ordemVenda = All.Where(x => x.Id == ordemVendaId).FirstOrDefault();
            if (geraNotaFiscal && ordemVenda.Status != StatusOrdemVenda.Finalizado)
            {
                TotalTributacaoBL.DadosValidosCalculoTributario(ordemVenda, clienteId, onList);
            }

            var produtos = OrdemVendaProdutoBL.All.Where(x => x.OrdemVendaId == ordemVendaId).ToList();
            var totalProdutos = produtos != null ? produtos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;
            //se esta salvo não recalcula
            var totalImpostosProdutos = (ordemVenda.Status == StatusOrdemVenda.Finalizado && ordemVenda.TotalImpostosProdutos.HasValue) ? ordemVenda.TotalImpostosProdutos.Value
                : (produtos != null && geraNotaFiscal ? TotalTributacaoBL.TotalSomaOrdemVendaProdutos(produtos, clienteId, tipoVendaEnum, tipoFreteEnum, valorFrete) : 0.0);

            var totalImpostosProdutosNaoAgrega = ordemVenda.Status == StatusOrdemVenda.Finalizado ? ordemVenda.TotalImpostosProdutosNaoAgrega
                : (produtos != null && geraNotaFiscal ? TotalTributacaoBL.TotalSomaOrdemVendaProdutosNaoAgrega(produtos, clienteId, tipoVendaEnum, tipoFreteEnum, valorFrete) : 0.0);

            var servicos = OrdemVendaServicoBL.AllIncluding(y => y.GrupoTributario, y => y.Servico).Where(x => x.OrdemVendaId == ordemVendaId).ToList();
            var totalServicos = servicos != null ? servicos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;
            var totalImpostosServicos = (ordemVenda.Status == StatusOrdemVenda.Finalizado && ordemVenda.TotalImpostosServicos.HasValue) ? ordemVenda.TotalImpostosServicos.Value
                : (servicos != null && geraNotaFiscal ? TotalTributacaoBL.TotalSomaOrdemVendaServicos(servicos, clienteId) : 0.0);

            var result = new TotalOrdemVenda()
            {
                TotalProdutos = Math.Round(totalProdutos, 2, MidpointRounding.AwayFromZero),
                TotalServicos = Math.Round(totalServicos, 2, MidpointRounding.AwayFromZero),
                ValorFrete = Math.Round(valorFrete.Value, 2, MidpointRounding.AwayFromZero),
                TotalImpostosProdutos = Math.Round(totalImpostosProdutos, 2, MidpointRounding.AwayFromZero),
                TotalImpostosProdutosNaoAgrega = Math.Round(totalImpostosProdutosNaoAgrega, 2, MidpointRounding.AwayFromZero),
                TotalImpostosServicos = Math.Round(totalImpostosServicos, 2, MidpointRounding.AwayFromZero),
            };

            return result;
        }

        public static string ValorBCSTRetidoRequerido = "Valor da base de cálculo do ICMS substituído é obrigatório para CSOSN 500. Item {itemcount} da lista de produtos";
        public static string ValorICMSSTRetidoRequerido = "Valor do ICMS substituído é obrigatório para CSOSN 500. Item {itemcount} da lista de produtos";
        public static string ValorCreditoSNRequerido = "Valor crédito do ICMS é obrigatório para CSOSN 101 e 201. Item {itemcount} da lista de produtos";
    }
}