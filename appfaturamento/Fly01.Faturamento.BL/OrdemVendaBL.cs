using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.Core.Helpers.Attribute;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Faturamento.BL
{
    public class OrdemVendaBL : PlataformaBaseBL<OrdemVenda>
    {
        public const int MaxLengthObservacao = 200;

        protected OrdemVendaProdutoBL OrdemVendaProdutoBL { get; set; }
        protected OrdemVendaServicoBL OrdemVendaServicoBL { get; set; }
        protected NFeBL NFeBL { get; set; }
        protected NFSeBL NFSeBL { get; set; }
        protected NFeProdutoBL NFeProdutoBL { get; set; }
        protected NFSeServicoBL NFSeServicoBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected NotaFiscalItemTributacaoBL NotaFiscalItemTributacaoBL { get; set; }
        protected EstadoBL EstadoBL;
        protected KitItemBL KitItemBL { get; set; }
        protected ConfiguracaoPersonalizacao ConfiguracaoPersonalizacao { get; set; }
        protected bool emiteNotaFiscal;
        protected bool exibirProdutos;
        protected bool exibirServicos;
        protected bool exibirTransportadora;

        private readonly string descricaoVenda = @"Venda nº: {0} de {1}";
        private readonly string observacaoVenda = @"Obs. gerada pela venda nº {0} de {1} : {2}";
        private readonly string routePrefixNameMovimentoOrdemVenda = @"MovimentoOrdemVenda";
        private readonly string routePrefixNameContaPagar = @"ContaPagar";
        private readonly string routePrefixNameContaReceber = @"ContaReceber";

        public OrdemVendaBL(AppDataContextBase context, OrdemVendaProdutoBL ordemVendaProdutoBL, OrdemVendaServicoBL ordemVendaServicoBL, NFeBL nfeBL, NFSeBL nfseBL, NFeProdutoBL nfeProdutoBL, NFSeServicoBL nfseServicoBL, TotalTributacaoBL totalTributacaoBL, NotaFiscalItemTributacaoBL notaFiscalItemTributacaoBL, KitItemBL kitItemBl, EstadoBL estadoBL, ConfiguracaoPersonalizacaoBL configuracaoPersonalizacaoBL) : base(context)
        {
            MustConsumeMessageServiceBus = true;
            OrdemVendaProdutoBL = ordemVendaProdutoBL;
            OrdemVendaServicoBL = ordemVendaServicoBL;
            NFeBL = nfeBL;
            NFeProdutoBL = nfeProdutoBL;
            NFSeBL = nfseBL;
            NFSeServicoBL = nfseServicoBL;
            TotalTributacaoBL = totalTributacaoBL;
            NotaFiscalItemTributacaoBL = notaFiscalItemTributacaoBL;
            EstadoBL = estadoBL;
            KitItemBL = kitItemBl;
            ConfiguracaoPersonalizacao = configuracaoPersonalizacaoBL.All.AsNoTracking().FirstOrDefault();
            emiteNotaFiscal = ConfiguracaoPersonalizacao != null ? ConfiguracaoPersonalizacao.EmiteNotaFiscal : true;
            exibirProdutos = ConfiguracaoPersonalizacao != null ? ConfiguracaoPersonalizacao.ExibirStepProdutosVendas : true;
            exibirServicos = ConfiguracaoPersonalizacao != null ? ConfiguracaoPersonalizacao.ExibirStepServicosVendas : true;
            exibirTransportadora = ConfiguracaoPersonalizacao != null ? ConfiguracaoPersonalizacao.ExibirStepTransportadoraVendas : true;
        }

        public override void ValidaModel(OrdemVenda entity)
        {
            if (!string.IsNullOrEmpty(entity.MensagemPadraoNota))
                entity.Fail(entity.MensagemPadraoNota.Length > 4000, new Error("O SEFAZ permite até 4000 caracteres."));

            if (entity.TipoVenda == TipoCompraVenda.Complementar)
            {
                entity.TipoFrete = TipoFrete.SemFrete;
                entity.ValorFrete = 0.0;
                if (entity.TipoNfeComplementar != TipoNfeComplementar.ComplPrecoQtd)
                {
                    entity.MovimentaEstoque = false;
                }
                else if (entity.TipoNfeComplementar != TipoNfeComplementar.ComplIcms)
                {
                    entity.GeraFinanceiro = false;
                }
            }

            entity.Fail(entity.ValorFrete.HasValue && entity.ValorFrete.Value < 0, new Error("Valor frete não pode ser negativo", "valorFrete"));
            entity.Fail(entity.PesoBruto.HasValue && entity.PesoBruto.Value < 0, new Error("Peso bruto não pode ser negativo", "pesoBruto"));
            entity.Fail(entity.PesoLiquido.HasValue && entity.PesoLiquido.Value < 0, new Error("Peso liquido não pode ser negativo", "pesoLiquido"));
            entity.Fail(entity.QuantidadeVolumes.HasValue && entity.QuantidadeVolumes.Value < 0, new Error("Quantidade de volumes não pode ser negativo", "quantidadeVolumes"));
            entity.Fail(entity.Observacao != null && entity.Observacao.Length > MaxLengthObservacao, new Error($"A observacao não poder ter mais de {MaxLengthObservacao} caracteres", "observacao"));
            entity.Fail(entity.Numero < 1, new Error("O número do orçamento/pedido é inválido"));
            entity.Fail(All.Any(x => x.Numero == entity.Numero && x.Id != entity.Id && x.Ativo), new Error("O número do orçamento/pedido já foi utilizado"));
            entity.Fail((entity.TipoVenda == TipoCompraVenda.Devolucao || entity.TipoVenda == TipoCompraVenda.Complementar) && !string.IsNullOrEmpty(entity.ChaveNFeReferenciada) && entity.ChaveNFeReferenciada.Length != 44, new Error("A chave da nota fiscal referenciada deve conter 44 caracteres"));
            entity.Fail((entity.TipoVenda == TipoCompraVenda.Complementar && entity.TipoNfeComplementar == TipoNfeComplementar.NaoComplementar), new Error("Tipo do complemento inválido para pedido complementar.", "tipoNfeComplementar"));

            if (entity.Status == Status.Finalizado)
            {
                var produtos = OrdemVendaProdutoBL.All.AsNoTracking().Where(x => x.OrdemVendaId == entity.Id).ToList();
                var servicos = OrdemVendaServicoBL.All.AsNoTracking().Where(x => x.OrdemVendaId == entity.Id).ToList();
                var hasEstoqueNegativo = exibirProdutos && VerificaEstoqueNegativo(entity.Id, entity.TipoVenda.ToString(), entity.TipoNfeComplementar.ToString(), entity.NFeRefComplementarIsDevolucao).Any();

                bool freteEmpresa = (
                    (entity.TipoFrete == TipoFrete.CIF || entity.TipoFrete == TipoFrete.Remetente) && exibirTransportadora
                );
                entity.Fail(freteEmpresa && (entity.TransportadoraId == null || entity.TransportadoraId == default(Guid)), new Error("Se configurou o frete por conta da sua empresa, informe a transportadora"));

                if (emiteNotaFiscal && entity.GeraNotaFiscal)
                {
                    TotalTributacaoBL.DadosValidosCalculoTributario(entity, entity.ClienteId);

                    if (servicos != null)
                    {
                        var totalOutrasRetencoesServicos = servicos.Sum(x => x.ValorOutrasRetencoes);
                        var totalServicos = servicos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto));
                        var tributacoesServicos = TotalTributacaoBL.TributacoesOrdemVendaServicos(servicos, entity.ClienteId);
                        var totalRetencoesServicos = TotalTributacaoBL.TributacaoServicoRetencao(tributacoesServicos);
                        entity.Fail((totalOutrasRetencoesServicos + totalRetencoesServicos) > totalServicos, new Error("Total de retenções dos serviços, não pode ser superior ao total de serviços", "totalRetencoesServicos"));
                    }

                    entity.Fail(entity.TipoNfeComplementar != TipoNfeComplementar.ComplIcms && entity.FormaPagamentoId == null && produtos.Any(), new Error("Para finalizar o pedido que gera nota fiscal, informe a forma de pagamento"));
                    entity.Fail(entity.TipoVenda == TipoCompraVenda.Devolucao && string.IsNullOrEmpty(entity.ChaveNFeReferenciada), new Error("Para finalizar o pedido de devolução que gera nota fiscal, informe a chave da nota fiscal referenciada"));
                    entity.Fail(entity.TipoVenda == TipoCompraVenda.Complementar && string.IsNullOrEmpty(entity.ChaveNFeReferenciada), new Error("Para finalizar o pedido de complemento que gera nota fiscal, informe a chave da nota fiscal referenciada a ser complementada"));

                    var ehExportacao = TotalTributacaoBL.GetPessoa(entity.ClienteId)?.Estado?.Sigla == "EX";
                    entity.Fail(ehExportacao && ((entity.UFSaidaPaisId == null) || (string.IsNullOrEmpty(entity.LocalEmbarque))), new Error("Se UF do destinatário é exterior, informe a UF e o local de embarque da exportação."));
                }

                entity.Fail(exibirProdutos && entity.MovimentaEstoque && hasEstoqueNegativo & !entity.AjusteEstoqueAutomatico, new Error("Para finalizar o pedido o estoque não poderá ficar negativo, realize os ajustes de entrada ou marque para gerar as movimentações de entrada automáticas"));
                entity.Fail((emiteNotaFiscal && entity.GeraNotaFiscal) && string.IsNullOrEmpty(entity.NaturezaOperacao) && produtos.Any(), new Error("Para finalizar o pedido que gera nota fiscal, informe a natureza de operação"));
                entity.Fail(entity.TipoOrdemVenda == TipoOrdemVenda.Orcamento, new Error("Orçamento não pode ser finalizado. Converta em pedido para finalizar"));
                entity.Fail(!produtos.Any() && !servicos.Any(), new Error("Para finalizar a venda é necessário ao menos ter adicionado um produto ou um serviço"));
                entity.Fail(entity.TipoVenda != TipoCompraVenda.Normal && servicos.Any(), new Error("Somente finalidade normal, pode conter serviços"));
                entity.Fail(
                    (entity.GeraFinanceiro && (entity.FormaPagamentoId == null || entity.CondicaoParcelamentoId == null || entity.CategoriaId == null || entity.DataVencimento == null)),
                    new Error("Venda que gera financeiro é necessário informar forma de pagamento, condição de parcelamento, categoria e data vencimento")
                    );

                if (entity.TipoFrete != TipoFrete.SemFrete)
                {
                    entity.Fail(!string.IsNullOrEmpty(entity.Marca) && (entity.Marca.Replace(" ", "").Length == 0 || (entity.Marca?.Length > 60)), new Error("Marca do volume inválido. No máximo 60 caracteres ou vazio e sem espaços.", "marca"));
                    entity.Fail(!string.IsNullOrEmpty(entity.NumeracaoVolumesTrans) && (entity.NumeracaoVolumesTrans.Replace(" ", "").Length == 0 || (entity.NumeracaoVolumesTrans?.Length > 60)), new Error("Numeração do volume inválido. No máximo 60 caracteres ou vazio e sem espaços.", "numeracaoVolumesTrans"));
                    entity.Fail(!string.IsNullOrEmpty(entity.TipoEspecie) && (entity.TipoEspecie.Replace(" ", "").Length == 0 || (entity.NumeracaoVolumesTrans?.Length > 60)), new Error("Espécie do volume inválido. No máximo 60 caracteres ou vazio e sem espaços.", "tipoEspecie"));
                }
            }

            base.ValidaModel(entity);
        }

        public IQueryable<OrdemVenda> Everything => repository.All.Where(x => x.PlataformaId == PlataformaUrl);

        protected dynamic GetNotaFiscal(OrdemVenda entity, TipoNotaFiscal tipo)
        {
            dynamic notaFiscal;
            var mensagemComplementar = "";

            if (tipo == TipoNotaFiscal.NFe)
            {
                notaFiscal = new NFe();
                notaFiscal.TipoNfeComplementar = entity.TipoNfeComplementar;
                notaFiscal.UFSaidaPaisId = entity.UFSaidaPaisId;
                notaFiscal.LocalEmbarque = entity.LocalEmbarque;
                notaFiscal.LocalDespacho = entity.LocalDespacho;
            }
            else
            {
                notaFiscal = new NFSe();
            }

            if (entity.TipoVenda == TipoCompraVenda.Complementar)
            {
                var NFeComplementar = NFeBL.All.AsNoTracking().Where(x => x.SefazId.ToUpper() == entity.ChaveNFeReferenciada.ToUpper()).FirstOrDefault();

                if (NFeComplementar != null)
                {
                    mensagemComplementar = $"NFe Complementar a NFe {NFeComplementar.NumNotaFiscal} emitida em {NFeComplementar.Data.ToShortDateString()}";
                }
                else
                {
                    var numNotaFiscal = entity.ChaveNFeReferenciada.Substring(25, 9);
                    var ano = entity.ChaveNFeReferenciada.Substring(2, 2);
                    var mes = entity.ChaveNFeReferenciada.Substring(4, 2);
                    mensagemComplementar = $"NFe Complementar a NFe {numNotaFiscal} emitida em {mes}/{ano}";
                }
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
            notaFiscal.TipoEspecie = entity.TipoEspecie;
            notaFiscal.Marca = entity.Marca;
            notaFiscal.NumeracaoVolumesTrans = entity.NumeracaoVolumesTrans;
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
            notaFiscal.MensagemPadraoNota = (mensagemComplementar + " " + entity.MensagemPadraoNota ?? "").Trim();
            notaFiscal.ContaFinanceiraParcelaPaiIdServicos = entity.ContaFinanceiraParcelaPaiIdServicos;
            notaFiscal.ContaFinanceiraParcelaPaiIdProdutos = entity.ContaFinanceiraParcelaPaiIdProdutos;
            notaFiscal.InformacoesCompletamentaresNFS = entity.InformacoesCompletamentaresNFS;
            return notaFiscal;
        }

        protected void GeraNotasFiscais(OrdemVenda entity)
        {
            //antes de gerar a nota fiscal tira os itens zerados Qtd =0 e Preço=0
            if (entity.TipoVenda == TipoCompraVenda.Complementar)
            {
                var produtosSemQtdValor = OrdemVendaProdutoBL.All.Where(e => e.OrdemVendaId == entity.Id && e.Ativo && e.Quantidade == 0 && e.Valor == 0).ToList();
                if (produtosSemQtdValor != null)
                {
                    foreach (var produto in produtosSemQtdValor)
                    {
                        OrdemVendaProdutoBL.Delete(produto);
                    }
                }
            }

            //gerar notas fiscais divididos por produtos e serviços
            var produtos = OrdemVendaProdutoBL.AllIncluding(x => x.GrupoTributario).Where(x => x.OrdemVendaId == entity.Id && (x.Quantidade > 0 || x.Valor > 0)).ToList();
            var servicos = OrdemVendaServicoBL.All.Where(x => x.OrdemVendaId == entity.Id).ToList();

            if (produtos != null & produtos.Any())
            {
                var NFe = (NFe)GetNotaFiscal(entity, TipoNotaFiscal.NFe);
                var tributacoesProdutos = TotalTributacaoBL.TributacoesOrdemVendaProdutos(produtos, entity.ClienteId, entity.TipoVenda, entity.TipoNfeComplementar, entity.TipoFrete, entity.NFeRefComplementarIsDevolucao, entity.ValorFrete);
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
                            OrdemVendaProdutoId = x.Id
                        }).ToList();

                var nfeProdutosTributacao = new List<NotaFiscalItemTributacao>();
                foreach (var x in tributacoesProdutos)
                {
                    var nfeProduto = nfeProdutos.Where(y => y.OrdemVendaProdutoId == x.OrdemVendaProdutoId).FirstOrDefault();
                    var grupoTributario = TotalTributacaoBL.GetGrupoTributario(nfeProduto.GrupoTributarioId);
                    NotaFiscalItemTributacaoBL.Insert(
                        new NotaFiscalItemTributacao
                        {
                            NotaFiscalItemId = nfeProduto.Id,
                            FreteValorFracionado = x.FreteValorFracionado,
                            #region ICMS
                            CalculaICMS = grupoTributario.CalculaIcms,
                            ICMSBase = x.ICMSBase,
                            ICMSAliquota = x.ICMSAliquota,
                            ICMSValor = x.ICMSValor,
                            #endregion
                            #region IPI
                            CalculaIPI = grupoTributario.CalculaIpi,
                            IPIBase = x.IPIBase,
                            IPIAliquota = x.IPIAliquota,
                            IPIValor = x.IPIValor,
                            #endregion
                            #region ST
                            CalculaST = grupoTributario.CalculaSubstituicaoTributaria,
                            STBase = x.STBase,
                            STAliquota = x.STAliquota,
                            STValor = x.STValor,
                            #endregion
                            #region COFINS
                            CalculaCOFINS = grupoTributario.CalculaCofins,
                            COFINSBase = x.COFINSBase,
                            COFINSAliquota = x.COFINSAliquota,
                            COFINSValor = x.COFINSValor,
                            RetemCOFINS = grupoTributario.RetemCofins,
                            COFINSValorRetencao = x.COFINSValorRetencao,
                            #endregion
                            #region PIS
                            CalculaPIS = grupoTributario.CalculaPis,
                            PISBase = x.PISBase,
                            PISAliquota = x.PISAliquota,
                            PISValor = x.PISValor,
                            RetemPIS = grupoTributario.RetemPis,
                            #endregion
                            #region FCPST
                            FCPSTBase = x.FCPSTBase,
                            FCPSTAliquota = x.FCPSTAliquota,
                            FCPSTValor = x.FCPSTValor,
                            #endregion
                            #region FCP
                            FCPBase = x.FCPBase,
                            FCPAliquota = x.FCPAliquota,
                            FCPValor = x.FCPValor
                            #endregion
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
                var tributacoesServicos = TotalTributacaoBL.TributacoesOrdemVendaServicos(servicos, entity.ClienteId);
                var totalRetencoesServicos = TotalTributacaoBL.TributacaoServicoRetencao(tributacoesServicos);
                var totalImpostosServicoesNaoAgrega = TotalTributacaoBL.TributacaoServicoNaoAgregaNota(tributacoesServicos);

                NFSe.TotalRetencoesServicos = totalRetencoesServicos;
                NFSe.TotalImpostosServicosNaoAgrega = totalImpostosServicoesNaoAgrega;
                entity.TotalRetencoesServicos = totalRetencoesServicos;
                entity.TotalImpostosServicosNaoAgrega = totalImpostosServicoesNaoAgrega;

                var nfseServicos = servicos.Select(
                        x => new NFSeServico
                        {
                            Id = Guid.NewGuid(),
                            NotaFiscalId = NFSe.Id,
                            ServicoId = x.ServicoId,
                            GrupoTributarioId = x.GrupoTributarioId.Value,
                            Quantidade = x.Quantidade,
                            Valor = x.Valor,
                            Desconto = x.Desconto,
                            Observacao = x.Observacao,
                            ValorOutrasRetencoes = x.ValorOutrasRetencoes,
                            DescricaoOutrasRetencoes = x.DescricaoOutrasRetencoes,
                            IsServicoPrioritario = x.IsServicoPrioritario,
                            OrdemVendaServicoId = x.Id
                        }).ToList();

                var nfeServicosTributacao = new List<NotaFiscalItemTributacao>();

                foreach (var x in tributacoesServicos)
                {
                    var nfseServico = nfseServicos.Where(y => y.OrdemVendaServicoId == x.OrdemVendaServicoId).FirstOrDefault();
                    var grupoTributario = TotalTributacaoBL.GetGrupoTributario(nfseServico.GrupoTributarioId);
                    NotaFiscalItemTributacaoBL.Insert(
                        new NotaFiscalItemTributacao
                        {
                            NotaFiscalItemId = nfseServico.Id,
                            #region COFINS
                            CalculaCOFINS = grupoTributario.CalculaCofins,
                            COFINSBase = x.COFINSBase,
                            COFINSAliquota = x.COFINSAliquota,
                            COFINSValor = x.COFINSValor,
                            RetemCOFINS = grupoTributario.RetemCofins,
                            COFINSValorRetencao = x.COFINSValorRetencao,
                            #endregion
                            #region PIS
                            CalculaPIS = grupoTributario.CalculaPis,
                            PISBase = x.PISBase,
                            PISAliquota = x.PISAliquota,
                            PISValor = x.PISValor,
                            RetemPIS = grupoTributario.RetemPis,
                            PISValorRetencao = x.PISValorRetencao,
                            #endregion
                            #region CSLL
                            CalculaCSLL = grupoTributario.CalculaCSLL,
                            CSLLBase = x.CSLLBase,
                            CSLLAliquota = x.CSLLAliquota,
                            CSLLValor = x.CSLLValor,
                            RetemCSLL = grupoTributario.RetemCSLL,
                            CSLLValorRetencao = x.CSLLValorRetencao,
                            #endregion
                            #region ISS
                            CalculaISS = grupoTributario.CalculaIss,
                            ISSBase = x.ISSBase,
                            ISSAliquota = x.ISSAliquota,
                            ISSValor = x.ISSValor,
                            RetemISS = grupoTributario.RetemISS,
                            ISSValorRetencao = x.ISSValorRetencao,
                            #endregion
                            #region INSS
                            CalculaINSS = grupoTributario.CalculaINSS,
                            INSSBase = x.INSSBase,
                            INSSAliquota = x.INSSAliquota,
                            INSSValor = x.INSSValor,
                            RetemINSS = grupoTributario.RetemINSS,
                            INSSValorRetencao = x.INSSValorRetencao,
                            #endregion
                            #region ImpostoRenda
                            CalculaImpostoRenda = grupoTributario.CalculaImpostoRenda,
                            ImpostoRendaBase = x.ImpostoRendaBase,
                            ImpostoRendaAliquota = x.ImpostoRendaAliquota,
                            ImpostoRendaValor = x.ImpostoRendaValor,
                            RetemImpostoRenda = grupoTributario.RetemImpostoRenda,
                            ImpostoRendaValorRetencao = x.ImpostoRendaValorRetencao,
                            #endregion
                        });
                }

                NFSeBL.Insert(NFSe);

                foreach (var nfseServico in nfseServicos)
                {
                    NFSeServicoBL.Insert(nfseServico);
                }
            }
        }

        protected void CopiaDadosNFeReferenciada(OrdemVenda entity)
        {
            Guid? idPedidoReferenciado = default(Guid);
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
                    var previousTipoNfeComplementar = entity.TipoNfeComplementar;

                    #region Copia os dados do pedido de origem da nota fiscal referenciada
                    var clienteReferenciado = TotalTributacaoBL.GetPessoa(pedidoReferenciado.ClienteId);
                    entity.Fail(entity.TipoVenda == TipoCompraVenda.Devolucao && pedidoReferenciado.TipoVenda == TipoCompraVenda.Devolucao, new Error("Não é possível realizar devolução de uma nota fiscal de devolução. Referencie outra nota fiscal.", "tipoVenda"));
                    entity.Fail(entity.TipoVenda == TipoCompraVenda.Complementar && pedidoReferenciado.TipoVenda == TipoCompraVenda.Complementar, new Error("Não é possível realizar complemento de uma nota fiscal complementar. Referencie outra nota fiscal.", "tipoVenda"));
                    entity.Fail((clienteReferenciado == null || clienteReferenciado.Ativo == false), new Error("Informe um cliente ativo. Cliente da nota fiscal referenciada inexistente ou excluído.", "clienteId"));

                    if (entity.IsValid())
                    {
                        pedidoReferenciado.CopyProperties<OrdemVenda>(entity);
                        entity.Id = previousId;
                        entity.Numero = previousNumero;
                        entity.Data = previousData;
                        entity.TipoVenda = previousTipoVenda;
                        entity.TipoNfeComplementar = previousTipoNfeComplementar;
                        entity.NFeRefComplementarIsDevolucao = pedidoReferenciado.TipoVenda == TipoCompraVenda.Devolucao;
                        if (entity.TotalImpostosProdutos.HasValue) { entity.TotalImpostosProdutos = null; };
                        if (entity.TotalRetencoesServicos.HasValue) { entity.TotalRetencoesServicos = null; };
                        entity.TotalImpostosProdutosNaoAgrega = 0;
                        entity.TotalImpostosServicosNaoAgrega = 0;

                        if (entity.TipoVenda == TipoCompraVenda.Devolucao)
                        {
                            entity.NaturezaOperacao = null;
                            entity.GrupoTributarioPadraoId = previousGrupoTributarioPadraoId;
                            entity.CategoriaId = null;//inverte receita/despesa, terá que informar no front
                        }

                        entity.ClienteId = (clienteReferenciado != null && clienteReferenciado.Ativo == true) ? pedidoReferenciado.ClienteId : previousClienteId;
                        entity.Status = Status.Aberto;
                        entity.ChaveNFeReferenciada = previousChaveNFeReferenciada;
                        entity.TipoVenda = previousTipoVenda;

                        var produtos = OrdemVendaProdutoBL.All.AsNoTracking().Where(x => x.OrdemVendaId == idPedidoReferenciado).ToList();

                        if (produtos != null & produtos.Any())
                        {
                            foreach (var produto in produtos)
                            {
                                var produtoClonado = new OrdemVendaProduto();
                                produto.CopyProperties<OrdemVendaProduto>(produtoClonado);
                                produtoClonado.Id = default(Guid);
                                produtoClonado.OrdemVendaId = entity.Id;
                                if (entity.TipoVenda == TipoCompraVenda.Devolucao)
                                {
                                    //na devolucao o grupo tributarioPadrão informado é setado, pois os de origem são CFOP venda e teria que entrar um por um para alterar
                                    produtoClonado.GrupoTributarioId = entity.GrupoTributarioPadraoId.HasValue ? entity.GrupoTributarioPadraoId.Value : produtoClonado.GrupoTributarioId;
                                }
                                else if (entity.TipoVenda == TipoCompraVenda.Complementar && entity.TipoNfeComplementar == TipoNfeComplementar.ComplPrecoQtd)
                                {
                                    //na nfe de complemento de preço zeramos os valores para que o usuário possa informar apenas os valores a serem complementados
                                    produtoClonado.GrupoTributarioId = entity.GrupoTributarioPadraoId.HasValue ? entity.GrupoTributarioPadraoId.Value : produtoClonado.GrupoTributarioId;
                                    produtoClonado.Quantidade = 0.00;
                                    produtoClonado.Valor = 0.00;
                                    produtoClonado.Desconto = 0.00;
                                }
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

            if (entity.TipoVenda == TipoCompraVenda.Complementar)
            {
                entity.TipoFrete = TipoFrete.SemFrete;//regra sefaz
                entity.ValorFrete = 0.00;

                switch (entity.TipoNfeComplementar)
                {
                    case TipoNfeComplementar.NaoComplementar:
                        break;
                    case TipoNfeComplementar.ComplPrecoQtd:
                        entity.NaturezaOperacao = "Complemento de Preco/Quantidade";
                        break;
                    case TipoNfeComplementar.ComplIcms:
                        entity.NaturezaOperacao = "Complemento de ICMS";
                        break;
                    case TipoNfeComplementar.ComplIcmsST:
                        entity.NaturezaOperacao = "Complemento de ICMS ST";
                        break;
                    case TipoNfeComplementar.ComplIpi:
                        entity.NaturezaOperacao = "Complemento de IPI";
                        break;
                    default:
                        break;
                }
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

            if (entity.Status == Status.Aberto && entity.TipoOrdemVenda == TipoOrdemVenda.Pedido && (entity.TipoVenda == TipoCompraVenda.Devolucao || entity.TipoVenda == TipoCompraVenda.Complementar) && !string.IsNullOrEmpty(entity.ChaveNFeReferenciada) && entity.IsValid() && emiteNotaFiscal)
            {
                CopiaDadosNFeReferenciada(entity);
            }

            if (entity.Status == Status.Finalizado && entity.TipoOrdemVenda == TipoOrdemVenda.Pedido && (entity.GeraNotaFiscal && emiteNotaFiscal) && entity.IsValid())
            {
                if (entity.TipoVenda == TipoCompraVenda.Complementar)
                {
                    var produtosSemQtdValor = OrdemVendaProdutoBL.All.Where(e => e.OrdemVendaId == entity.Id && e.Ativo && e.Quantidade == 0 && e.Valor == 0).ToList();
                    if (produtosSemQtdValor != null)
                    {
                        foreach (var produto in produtosSemQtdValor)
                        {
                            OrdemVendaProdutoBL.Delete(produto.Id);
                        }
                    }
                }
                GeraNotasFiscais(entity);
            }
            GetIdPlacaEstado(entity);
            base.Insert(entity);
        }

        public void GetIdPlacaEstado(OrdemVenda entity)
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

        public override void Update(OrdemVenda entity)
        {
            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            entity.Fail(previous.Status != Status.Aberto && entity.Status != Status.Aberto, new Error("Somente venda em aberto pode ser alterada", "status"));

            ValidaModel(entity);

            GeraIdContaFinanceiraRecuperarDadosParcela(entity);

            //se estava marcado, mas desabilitou depois via personalização, desmarcar
            if (entity.Status == Status.Finalizado)
            {
                entity.GeraNotaFiscal = entity.GeraNotaFiscal ? (entity.GeraNotaFiscal && emiteNotaFiscal) : false;
                entity.MovimentaEstoque = entity.MovimentaEstoque ? (entity.MovimentaEstoque && exibirProdutos) : false;
                entity.ValorFrete = exibirTransportadora ? entity.ValorFrete : 0.0;
            }

            if (entity.Status == Status.Finalizado && entity.TipoOrdemVenda == TipoOrdemVenda.Pedido && (entity.GeraNotaFiscal && emiteNotaFiscal) && entity.IsValid())
            {
                GeraNotasFiscais(entity);
            }

            base.Update(entity);
        }

        private void GeraIdContaFinanceiraRecuperarDadosParcela(OrdemVenda entity)
        {
            if (entity.GeraFinanceiro)
            {
                if (entity.ContaFinanceiraParcelaPaiIdProdutos == default(Guid) || entity.ContaFinanceiraParcelaPaiIdProdutos == null)
                {
                    entity.ContaFinanceiraParcelaPaiIdProdutos = Guid.NewGuid();
                }
                if (entity.ContaFinanceiraParcelaPaiIdServicos == default(Guid) || entity.ContaFinanceiraParcelaPaiIdServicos == null)
                {
                    entity.ContaFinanceiraParcelaPaiIdServicos = Guid.NewGuid();
                }
            }
        }

        public override void Delete(OrdemVenda entityToDelete)
        {
            entityToDelete.Fail(entityToDelete.Status != Status.Aberto, new Error("Somente venda em aberto pode ser deletada", "status"));

            if (!entityToDelete.IsValid())
                throw new BusinessException(entityToDelete.Notification.Get());

            base.Delete(entityToDelete);
        }

        public override void AfterSave(OrdemVenda entity)
        {
            if (entity.Status != Status.Finalizado || entity.TipoOrdemVenda != TipoOrdemVenda.Pedido)
                return;

            var produtos = OrdemVendaProdutoBL.All.Where(e => e.OrdemVendaId == entity.Id && e.Ativo).ToList();

            if (entity.GeraFinanceiro)
            {
                bool freteEmpresa = (
                    (entity.TipoFrete == TipoFrete.CIF || entity.TipoFrete == TipoFrete.Remetente) && exibirTransportadora
                );

                var servicos = OrdemVendaServicoBL.All.Where(e => e.OrdemVendaId == entity.Id && e.Ativo).ToList();
                var totalProdutos = 0.0;
                if (produtos != null && exibirProdutos)
                {
                    if (entity.TipoVenda == TipoCompraVenda.Normal || entity.TipoVenda == TipoCompraVenda.Devolucao)
                    {
                        totalProdutos = produtos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto));
                    }
                    else if (entity.TipoVenda == TipoCompraVenda.Complementar && entity.TipoNfeComplementar == TipoNfeComplementar.ComplPrecoQtd)
                    {
                        totalProdutos += produtos.Where(x => x.Quantidade != 0 && x.Valor != 0).Sum(x => ((x.Quantidade * x.Valor) - x.Desconto));
                        totalProdutos += produtos.Where(x => x.Quantidade == 0 && x.Valor != 0).Sum(x => (x.Valor - x.Desconto));
                    }
                }

                double totalServicos = 0.0;
                double totalOutrasRetencoesServicos = 0.0;
                if (servicos != null && exibirServicos)
                {
                    if (entity.TipoVenda == TipoCompraVenda.Normal)
                    {
                        totalServicos = servicos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto));
                        totalOutrasRetencoesServicos = servicos.Sum(x => x.ValorOutrasRetencoes);
                    }
                }

                double totalRetencoesServicos = (servicos != null && exibirServicos) && entity.TotalRetencoesServicos.HasValue ? entity.TotalRetencoesServicos.Value : 0; ;
                double totalImpostosProdutos = (produtos != null && exibirProdutos) && entity.TotalImpostosProdutos.HasValue ? entity.TotalImpostosProdutos.Value : 0;

                double valorPrevistoProdutos = totalProdutos + ((entity.GeraNotaFiscal && emiteNotaFiscal) ? totalImpostosProdutos : 0);
                double valorPrevistoServicos = totalServicos - ((entity.GeraNotaFiscal && emiteNotaFiscal) ? (totalRetencoesServicos + totalOutrasRetencoesServicos) : 0);

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
                        Descricao = GetDescricaoTitulo(TipoItem.Produto, entity),
                        Observacao = GetObservacaoTitulo(TipoItem.Produto, entity),
                        FormaPagamentoId = entity.FormaPagamentoId.Value,
                        CentroCustoId = entity.CentroCustoId,
                        PlataformaId = PlataformaUrl,
                        UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao
                    };
                    Producer<ContaPagar>.Send(routePrefixNameContaPagar, AppUser, PlataformaUrl, contaPagarTransp, RabbitConfig.EnHttpVerb.POST);
                }

                if ((entity.TipoVenda == TipoCompraVenda.Normal || (entity.TipoVenda == TipoCompraVenda.Complementar && !entity.NFeRefComplementarIsDevolucao)))
                {
                    if (valorPrevistoProdutos > 0)
                    {
                        GeraContaReceber(TipoItem.Produto, valorPrevistoProdutos, entity);
                    }
                    if (valorPrevistoServicos > 0)
                    {
                        GeraContaReceber(TipoItem.Servico, valorPrevistoServicos, entity);
                    }
                }
                else if ((entity.TipoVenda == TipoCompraVenda.Devolucao || (entity.TipoVenda == TipoCompraVenda.Complementar && entity.NFeRefComplementarIsDevolucao)))
                {
                    if (valorPrevistoProdutos > 0)
                    {
                        GeraContaPagar(TipoItem.Produto, valorPrevistoProdutos, entity);
                    }
                    if (valorPrevistoServicos > 0)
                    {
                        GeraContaPagar(TipoItem.Servico, valorPrevistoServicos, entity);
                    }
                }
            }

            if ((entity.MovimentaEstoque && exibirProdutos) &&
                (entity.TipoVenda == TipoCompraVenda.Normal
                 || entity.TipoVenda == TipoCompraVenda.Devolucao
                 || (entity.TipoVenda == TipoCompraVenda.Complementar && entity.TipoNfeComplementar == TipoNfeComplementar.ComplPrecoQtd)
                ))
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
                        TipoNfeComplementar = entity.TipoNfeComplementar,
                        PlataformaId = PlataformaUrl,
                        NFeRefComplementarIsDevolucao = entity.NFeRefComplementarIsDevolucao
                    }).ToList();

                foreach (var movimento in movimentos)
                    Producer<MovimentoOrdemVenda>.Send(routePrefixNameMovimentoOrdemVenda, AppUser, PlataformaUrl, movimento, RabbitConfig.EnHttpVerb.POST);
            }
        }

        private string LimitarMaxLength(string text)
        {
            if (text.Length > MaxLengthObservacao)
                text = text.Substring(0, MaxLengthObservacao);

            return text;
        }

        private string GetObservacaoTitulo(TipoItem tipoItem, OrdemVenda entity)
        {
            var tipoItemDescription = EnumHelper.GetDescription(typeof(TipoItem), tipoItem.ToString());
            var observacaoTitulo = string.Format(observacaoVenda, entity.Numero, tipoItemDescription, entity.Observacao);
            return LimitarMaxLength(observacaoTitulo);
        }

        private string GetDescricaoTitulo(TipoItem tipoItem, OrdemVenda entity)
        {
            var tipoItemDescription = EnumHelper.GetDescription(typeof(TipoItem), tipoItem.ToString());
            var descricaoTitulo = string.Format(descricaoVenda, entity.Numero, tipoItemDescription);
            return LimitarMaxLength(descricaoTitulo);
        }

        private void GeraContaReceber(TipoItem tipoItem, double valorPrevisto, OrdemVenda entity)
        {
            var contaReceber = new ContaReceber()
            {
                Id = GetContaFinanceiraParcelaId(tipoItem, entity),
                ValorPrevisto = valorPrevisto,
                CategoriaId = entity.CategoriaId.Value,
                CondicaoParcelamentoId = entity.CondicaoParcelamentoId.Value,
                PessoaId = entity.ClienteId,
                DataEmissao = entity.Data,
                DataVencimento = entity.DataVencimento.Value,
                Descricao = GetDescricaoTitulo(tipoItem, entity),
                Observacao = GetObservacaoTitulo(tipoItem, entity),
                FormaPagamentoId = entity.FormaPagamentoId.Value,
                CentroCustoId = entity.CentroCustoId,
                PlataformaId = PlataformaUrl,
                UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao
            };
            Producer<ContaReceber>.Send(routePrefixNameContaReceber, AppUser, PlataformaUrl, contaReceber, RabbitConfig.EnHttpVerb.POST);
        }

        private Guid GetContaFinanceiraParcelaId(TipoItem tipoItem, OrdemVenda entity)
        {
            var idConta = default(Guid);
            if (tipoItem == TipoItem.Produto)
            {
                idConta = entity.ContaFinanceiraParcelaPaiIdProdutos ?? default(Guid);
            }
            else
            {
                idConta = entity.ContaFinanceiraParcelaPaiIdServicos ?? default(Guid);
            }
            return idConta;
        }

        private void GeraContaPagar(TipoItem tipoItem, double valorPrevisto, OrdemVenda entity)
        {
            var contaPagar = new ContaPagar()
            {
                Id = GetContaFinanceiraParcelaId(tipoItem, entity),
                ValorPrevisto = valorPrevisto,
                CategoriaId = entity.CategoriaId.Value,
                CondicaoParcelamentoId = entity.CondicaoParcelamentoId.Value,
                PessoaId = entity.ClienteId,
                DataEmissao = entity.Data,
                DataVencimento = entity.DataVencimento.Value,
                Descricao = GetDescricaoTitulo(tipoItem, entity),
                Observacao = GetObservacaoTitulo(tipoItem, entity),
                FormaPagamentoId = entity.FormaPagamentoId.Value,
                CentroCustoId = entity.CentroCustoId,
                PlataformaId = PlataformaUrl,
                UsuarioInclusao = entity.UsuarioAlteracao ?? entity.UsuarioInclusao
            };
            Producer<ContaPagar>.Send(routePrefixNameContaPagar, AppUser, PlataformaUrl, contaPagar, RabbitConfig.EnHttpVerb.POST);
        }

        public List<PedidoProdutoEstoqueNegativo> VerificaEstoqueNegativo(Guid pedidoId, string tipoVenda, string tipoNfeComplementar, bool isComplementarDevolucao)
        {
            var produtos = new List<PedidoProdutoEstoqueNegativo>();
            if (tipoNfeComplementar == "NaoComplementar" || tipoNfeComplementar == "ComplPrecoQtd")
            {
                var result = OrdemVendaProdutoBL.AllIncluding(p => p.Produto).Where(x => x.OrdemVendaId == pedidoId)
                    .GroupBy(x => x.ProdutoId).Select(y => new PedidoProdutoEstoqueNegativo()
                    {
                        ProdutoId = y.Key,
                        QuantPedido = y.Sum(f => f.Quantidade),
                        QuantEstoque = y.Select(f => f.Produto.SaldoProduto.HasValue ? f.Produto.SaldoProduto.Value : 0.0).FirstOrDefault(),
                        SaldoEstoque = (tipoVenda == "Normal" || (tipoVenda == "Complementar" && !isComplementarDevolucao))
                            ? y.Select(f => f.Produto.SaldoProduto.HasValue ? f.Produto.SaldoProduto.Value : 0.0).FirstOrDefault() - y.Sum(f => f.Quantidade)
                            : y.Select(f => f.Produto.SaldoProduto.HasValue ? f.Produto.SaldoProduto.Value : 0.0).FirstOrDefault() + y.Sum(f => f.Quantidade),
                        ProdutoDescricao = y.Select(f => f.Produto.Descricao).FirstOrDefault(),
                    });

                produtos = result.Where(x => x.SaldoEstoque < 0).ToList();
            }
            return produtos;
        }

        public List<OrdemVenda> GetOrdemVendas()
        {
            var ordemVendas = new List<OrdemVenda>();

            ordemVendas = All.Where(x => x.GeraNotaFiscal == true && x.Status == Status.Aberto && x.Ativo == true).ToList();

            return ordemVendas;
        }

        public TotalPedidoNotaFiscal CalculaTotalOrdemVenda(Guid ordemVendaId, Guid clienteId, bool geraNotaFiscal, string tipoNfeComplementar = "NaoComplementar", string tipoFrete = "SemFrete", double? valorFrete = 0, bool onList = false)
        {
            var tipoFreteEnum = (TipoFrete)Enum.Parse(typeof(TipoFrete), tipoFrete, true);
            if (tipoFreteEnum != TipoFrete.FOB) { valorFrete = 0;}
            var tipoNfeComplementarEnum = (TipoNfeComplementar)Enum.Parse(typeof(TipoNfeComplementar), tipoNfeComplementar, true);

            var ordemVenda = All.Where(x => x.Id == ordemVendaId).FirstOrDefault();
            if ((geraNotaFiscal && emiteNotaFiscal) && ordemVenda.Status != Status.Finalizado)
            {
                TotalTributacaoBL.DadosValidosCalculoTributario(ordemVenda, clienteId, onList);
            }

            var produtos = OrdemVendaProdutoBL.All.Where(x => x.OrdemVendaId == ordemVendaId).ToList();
            var totalProdutos = 0.0;
            if (produtos != null && exibirProdutos)
            {
                if (ordemVenda.TipoVenda == TipoCompraVenda.Normal || ordemVenda.TipoVenda == TipoCompraVenda.Devolucao)
                {
                    totalProdutos = produtos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto));
                }
                else if (ordemVenda.TipoVenda == TipoCompraVenda.Complementar && tipoNfeComplementarEnum == TipoNfeComplementar.ComplPrecoQtd)
                {
                    //quando complemento de impostos, não considerar o totalProdutos
                    totalProdutos += produtos.Where(x => x.Quantidade != 0 && x.Valor != 0).Sum(x => ((x.Quantidade * x.Valor) - x.Desconto));
                    totalProdutos += produtos.Where(x => x.Quantidade == 0 && x.Valor != 0).Sum(x => (x.Valor - x.Desconto));
                }
                //else if (ordemVenda.TipoVenda == TipoVenda.Ajuste)
                //{

                //}
            }
            //se esta salvo não recalcula
            var totalImpostosProdutos = (ordemVenda.Status == Status.Finalizado && ordemVenda.TotalImpostosProdutos.HasValue) ? ordemVenda.TotalImpostosProdutos.Value
                : ((produtos != null && exibirProdutos) && (geraNotaFiscal && emiteNotaFiscal) ? TotalTributacaoBL.TotalSomaOrdemVendaProdutos(produtos, clienteId, ordemVenda.TipoVenda, tipoNfeComplementarEnum, tipoFreteEnum, ordemVenda.NFeRefComplementarIsDevolucao, valorFrete) : 0.0);
            var totalImpostosProdutosNaoAgrega = ordemVenda.Status == Status.Finalizado ? ordemVenda.TotalImpostosProdutosNaoAgrega
                : ((produtos != null && exibirProdutos) && (geraNotaFiscal && emiteNotaFiscal) ? TotalTributacaoBL.TotalSomaOrdemVendaProdutosNaoAgrega(produtos, clienteId, ordemVenda.TipoVenda, tipoNfeComplementarEnum, tipoFreteEnum, ordemVenda.NFeRefComplementarIsDevolucao, valorFrete) : 0.0);

            var servicos = OrdemVendaServicoBL.AllIncluding(y => y.GrupoTributario, y => y.Servico).Where(x => x.OrdemVendaId == ordemVendaId).ToList();
            var totalServicos = (servicos != null && exibirServicos) ? servicos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;
            var totalOutrasRetencoesServicos = ((servicos != null && exibirServicos) && (geraNotaFiscal && emiteNotaFiscal)) ? servicos.Sum(x => x.ValorOutrasRetencoes) : 0.0;

            var totalRetencoesServicos = (ordemVenda.Status == Status.Finalizado && ordemVenda.TotalRetencoesServicos.HasValue) ? ordemVenda.TotalRetencoesServicos.Value
                : ((servicos != null && exibirServicos) && (geraNotaFiscal && emiteNotaFiscal) ? TotalTributacaoBL.TotalSomaRetencaoOrdemVendaServicos(servicos, clienteId) : 0.0);

            totalRetencoesServicos += totalOutrasRetencoesServicos;

            var totalImpostosServicosNaoAgrega = ordemVenda.Status == Status.Finalizado ? ordemVenda.TotalImpostosServicosNaoAgrega
                : ((servicos != null && exibirServicos) && (geraNotaFiscal && emiteNotaFiscal) ? TotalTributacaoBL.TotalSomaOrdemVendaServicosNaoAgrega(servicos, clienteId) : 0.0);

            var result = new TotalPedidoNotaFiscal()
            {
                TotalProdutos = Math.Round(totalProdutos, 2, MidpointRounding.AwayFromZero),
                TotalServicos = Math.Round(totalServicos, 2, MidpointRounding.AwayFromZero),
                ValorFrete = Math.Round(valorFrete.Value, 2, MidpointRounding.AwayFromZero),
                TotalImpostosProdutos = Math.Round(totalImpostosProdutos, 2, MidpointRounding.AwayFromZero),
                TotalImpostosProdutosNaoAgrega = Math.Round(totalImpostosProdutosNaoAgrega, 2, MidpointRounding.AwayFromZero),
                TotalRetencoesServicos = Math.Round(totalRetencoesServicos, 2, MidpointRounding.AwayFromZero),
                TotalImpostosServicosNaoAgrega = Math.Round(totalImpostosServicosNaoAgrega, 2, MidpointRounding.AwayFromZero),
            };

            return result;
        }

        

        public void UtilizarKitOrdemVenda(UtilizarKitVM entity)
        {
            try
            {
                if (All.Any(x => x.Id == entity.OrcamentoPedidoId))
                {
                    if (KitItemBL.All.Any(x => x.KitId == entity.KitId))
                    {
                        #region Produtos
                        if (entity.AdicionarProdutos && exibirProdutos)
                        {
                            var kitProdutos = KitItemBL.All.Where(x => x.KitId == entity.KitId && x.TipoItem == TipoItem.Produto);

                            var existentesOrcamentoPedido =
                                from ovp in OrdemVendaProdutoBL.AllIncluding(x => x.Produto).Where(x => x.OrdemVendaId == entity.OrcamentoPedidoId)
                                join ki in kitProdutos on ovp.ProdutoId equals ki.ProdutoId
                                select new { ProdutoId = ki.ProdutoId, OrdemVendaProdutoId = ovp.Id, Quantidade = ki.Quantidade };

                            var novasOrdemVendaProdutos =
                                from kit in kitProdutos
                                where !existentesOrcamentoPedido.Select(x => x.ProdutoId).Contains(kit.ProdutoId)
                                select new
                                {
                                    GrupoTributarioId = entity.GrupoTributarioProdutoId,
                                    OrdemVendaId = entity.OrcamentoPedidoId,
                                    ProdutoId = kit.ProdutoId.Value,
                                    Valor = kit.Produto.ValorVenda,
                                    Quantidade = kit.Quantidade
                                };

                            foreach (var item in novasOrdemVendaProdutos)
                            {
                                OrdemVendaProdutoBL.Insert(new OrdemVendaProduto()
                                {
                                    GrupoTributarioId = item.GrupoTributarioId != default(Guid) ? item.GrupoTributarioId : (Guid?)null,
                                    ProdutoId = item.ProdutoId,
                                    OrdemVendaId = item.OrdemVendaId,
                                    Valor = item.Valor,
                                    Quantidade = item.Quantidade
                                });
                            }

                            if (entity.SomarExistentes)
                            {
                                foreach (var item in existentesOrcamentoPedido)
                                {
                                    var ordemVendaProduto = OrdemVendaProdutoBL.Find(item.OrdemVendaProdutoId);
                                    ordemVendaProduto.Quantidade += item.Quantidade;
                                    OrdemVendaProdutoBL.Update(ordemVendaProduto);
                                }
                            }
                        }
                        #endregion
                        #region Servicos
                        if (entity.AdicionarServicos && exibirServicos)
                        {
                            var kitServicos = KitItemBL.All.Where(x => x.KitId == entity.KitId && x.TipoItem == TipoItem.Servico);

                            var existentesOrcamentoPedido =
                                from ovs in OrdemVendaServicoBL.AllIncluding(x => x.Servico).Where(x => x.OrdemVendaId == entity.OrcamentoPedidoId)
                                join ki in kitServicos on ovs.ServicoId equals ki.ServicoId
                                select new { ServicoId = ki.ServicoId, OrdemVendaServicoId = ovs.Id, Quantidade = ki.Quantidade };

                            var ex = existentesOrcamentoPedido.ToList();

                            var novasOrdemVendaServicos =
                                from kit in kitServicos
                                where !existentesOrcamentoPedido.Select(x => x.ServicoId).Contains(kit.ServicoId)
                                select new
                                {
                                    GrupoTributarioId = entity.GrupoTributarioServicoId,
                                    OrdemVendaId = entity.OrcamentoPedidoId,
                                    ServicoId = kit.ServicoId.Value,
                                    Valor = kit.Servico.ValorServico,
                                    Quantidade = kit.Quantidade
                                };

                            var novas = novasOrdemVendaServicos.ToList();

                            foreach (var item in novasOrdemVendaServicos)
                            {
                                OrdemVendaServicoBL.Insert(new OrdemVendaServico()
                                {
                                    GrupoTributarioId = item.GrupoTributarioId != default(Guid) ? item.GrupoTributarioId : (Guid?)null,
                                    ServicoId = item.ServicoId,
                                    OrdemVendaId = item.OrdemVendaId,
                                    Valor = item.Valor,
                                    Quantidade = item.Quantidade
                                });
                            }

                            if (entity.SomarExistentes)
                            {
                                foreach (var item in existentesOrcamentoPedido)
                                {
                                    var ordemVendaServico = OrdemVendaServicoBL.Find(item.OrdemVendaServicoId);
                                    ordemVendaServico.Quantidade += item.Quantidade;
                                    OrdemVendaServicoBL.Update(ordemVendaServico);
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