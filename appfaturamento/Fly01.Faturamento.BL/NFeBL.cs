using Fly01.Core;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.Core.Defaults;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fly01.Faturamento.BL.Helpers.Factory;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;

namespace Fly01.Faturamento.BL
{
    public class NFeBL : PlataformaBaseBL<NFe>
    {
        protected SerieNotaFiscalBL SerieNotaFiscalBL { get; set; }
        protected NFeProdutoBL NFeProdutoBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected PessoaBL PessoaBL { get; set; }
        protected CondicaoParcelamentoBL CondicaoParcelamentoBL { get; set; }
        protected FormaPagamentoBL FormaPagamentoBL { get; set; }
        protected SubstituicaoTributariaBL SubstituicaoTributariaBL { get; set; }
        protected NotaFiscalItemTributacaoBL NotaFiscalItemTributacaoBL { get; set; }
        protected CertificadoDigitalBL CertificadoDigitalBL { get; set; }
        protected NotaFiscalInutilizadaBL NotaFiscalInutilizadaBL { get; set; }

        public NFeBL(AppDataContext context,
                     SerieNotaFiscalBL serieNotaFiscalBL,
                     NFeProdutoBL nfeProdutoBL,
                     TotalTributacaoBL totalTributacaoBL,
                     CertificadoDigitalBL certificadoDigitalBL,
                     PessoaBL pessoaBL,
                     CondicaoParcelamentoBL condicaoParcelamentoBL,
                     SubstituicaoTributariaBL substituicaoTributariaBL,
                     NotaFiscalItemTributacaoBL notaFiscalItemTributacaoBL,
                     FormaPagamentoBL formaPagamentoBL,
                     NotaFiscalInutilizadaBL notaFiscalInutilizadaBL)
            : base(context)
        {
            SerieNotaFiscalBL = serieNotaFiscalBL;
            NFeProdutoBL = nfeProdutoBL;
            TotalTributacaoBL = totalTributacaoBL;
            CertificadoDigitalBL = certificadoDigitalBL;
            PessoaBL = pessoaBL;
            CondicaoParcelamentoBL = condicaoParcelamentoBL;
            SubstituicaoTributariaBL = substituicaoTributariaBL;
            NotaFiscalItemTributacaoBL = notaFiscalItemTributacaoBL;
            FormaPagamentoBL = formaPagamentoBL;
            NotaFiscalInutilizadaBL = notaFiscalInutilizadaBL;
        }

        public IQueryable<NFe> Everything => repository.All.Where(x => x.Ativo);

        public override void ValidaModel(NFe entity)
        {
            entity.Fail(entity.TipoNotaFiscal != TipoNotaFiscal.NFe, new Error("Permitido somente nota fiscal do tipo NFe"));

            if (entity.TipoFrete != TipoFrete.SemFrete)
            {
                entity.Fail(entity.ValorFrete.HasValue && entity.ValorFrete.Value < 0, new Error("Valor frete não pode ser negativo", "valorFrete"));
                entity.Fail(entity.PesoBruto.HasValue && entity.PesoBruto.Value < 0, new Error("Peso bruto não pode ser negativo", "pesoBruto"));
                entity.Fail(entity.PesoLiquido.HasValue && entity.PesoLiquido.Value < 0, new Error("Peso liquido não pode ser negativo", "pesoLiquido"));
            }

            entity.Fail(entity.QuantidadeVolumes.HasValue && entity.QuantidadeVolumes.Value < 0, new Error("Quantidade de volumes não pode ser negativo", "quantidadeVolumes"));
            entity.Fail((entity.NumNotaFiscal.HasValue || entity.SerieNotaFiscalId.HasValue) && (!entity.NumNotaFiscal.HasValue || !entity.SerieNotaFiscalId.HasValue), new Error("Informe série e número da nota fiscal"));
            entity.Fail((entity.Status == StatusNotaFiscal.Transmitida && (!entity.SerieNotaFiscalId.HasValue || !entity.NumNotaFiscal.HasValue)), new Error("Para transmitir, informe série e número da nota fiscal"));

            var serieNotaFiscal = SerieNotaFiscalBL.All.AsNoTracking().FirstOrDefault(x => x.Id == entity.SerieNotaFiscalId);
            if (entity.SerieNotaFiscalId.HasValue)
            {
                entity.Fail(serieNotaFiscal == null || (serieNotaFiscal.TipoOperacaoSerieNotaFiscal != TipoOperacaoSerieNotaFiscal.NFe && serieNotaFiscal.TipoOperacaoSerieNotaFiscal != TipoOperacaoSerieNotaFiscal.Ambas), new Error("Selecione uma série ativa do tipo NF-e ou tipo ambas"));
            }


            if (entity.Status == StatusNotaFiscal.Transmitida && entity.SerieNotaFiscalId.HasValue && entity.NumNotaFiscal.HasValue)
            {
                var serieENumeroJaUsado = All.AsNoTracking().Any(x => x.Id != entity.Id && (x.SerieNotaFiscalId == entity.SerieNotaFiscalId && x.NumNotaFiscal == entity.NumNotaFiscal));
                var serieENumeroInutilizado = NotaFiscalInutilizadaBL.All.AsNoTracking().Any(x =>
                    x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() &&
                    x.NumNotaFiscal == entity.NumNotaFiscal);

                if (serieENumeroJaUsado || serieENumeroInutilizado)
                {
                    var sugestaoProximoNumNota = All.Max(x => x.NumNotaFiscal);
                    if (!sugestaoProximoNumNota.HasValue)
                    {
                        sugestaoProximoNumNota = entity.NumNotaFiscal;
                    }

                    do
                    {
                        sugestaoProximoNumNota += 1;
                    }//enquanto sugestão possa estar na lista de inutilizadas
                    while (NotaFiscalInutilizadaBL.All.AsNoTracking().Any(x =>
                        x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() &&
                        x.NumNotaFiscal == sugestaoProximoNumNota) || sugestaoProximoNumNota == entity.NumNotaFiscal);

                    entity.Fail(true, new Error("Série e número já utilizados ou inutilizados, sugestão de número: " + sugestaoProximoNumNota.ToString(), "numNotaFiscal"));
                }
                else
                {
                    var proximoNumNota = entity.NumNotaFiscal.Value + 1;
                    var ProximoNumeroInutilizado = NotaFiscalInutilizadaBL.All.AsNoTracking().Any(x =>
                    x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() &&
                    x.NumNotaFiscal == proximoNumNota);

                    if (ProximoNumeroInutilizado)
                    {
                        var proximoNumNotaOK = All.Max(x => x.NumNotaFiscal);
                        if (!proximoNumNotaOK.HasValue)
                        {
                            proximoNumNotaOK = proximoNumNota;
                        }

                        do
                        {
                            proximoNumNotaOK += 1;
                        }//enquanto incremento para próxima nota, possa estar na lista de inutilizadas
                        while (NotaFiscalInutilizadaBL.All.AsNoTracking().Any(x =>
                            x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() &&
                            x.NumNotaFiscal == proximoNumNotaOK) || proximoNumNotaOK == entity.NumNotaFiscal);

                        proximoNumNota = proximoNumNotaOK.Value;
                    }

                    var serie = SerieNotaFiscalBL.All.Where(x => x.Id == entity.SerieNotaFiscalId).FirstOrDefault();
                    serie.NumNotaFiscal = proximoNumNota;
                    SerieNotaFiscalBL.Update(serie);
                };
            }

            base.ValidaModel(entity);
        }

        public void TransmitirNFe(NFe entity)
        {
            try
            {
                if (!TotalTributacaoBL.ConfiguracaoTSSOK())
                {
                    throw new BusinessException("Configuração inválida para comunicação com TSS");
                }
                else
                {
                    var factory = new ConcreteTransmissaoFactory();
                    var transmissao = factory.ObterTransmissao(entity, ObterTransmissaoBLs());

                    TransmitirNotaFiscal(entity, transmissao);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        private void TransmitirNotaFiscal(NFe entity, TransmissaoVM transmissao)
        {
            var header = new Dictionary<string, string>()
                    {
                        { "AppUser", AppUser },
                        { "PlataformaUrl", PlataformaUrl }
                    };

            entity.Mensagem = null;
            entity.Recomendacao = null;
            entity.XML = null;
            entity.PDF = null;

            var response = RestHelper.ExecutePostRequest<TransmissaoRetornoVM>(AppDefaults.UrlEmissaoNfeApi, "transmissao", JsonConvert.SerializeObject(transmissao, JsonSerializerSetting.Edit), null, header);
            var retorno = response.Notas.FirstOrDefault();
            if (retorno.Error != null)
            {
                entity.Status = StatusNotaFiscal.FalhaTransmissao;
                var mensagens = "";
                foreach (var item in retorno.Error)
                {
                    var schemaMensagens = "";
                    foreach (var schema in item.SchemaMensagem)
                    {
                        schemaMensagens += string.Format("  Erro: {0}\n  Descrição: {1}\n  Campo: {2}\n", schema.Erro, schema.Descricao, schema.Campo);
                    }
                    mensagens += string.Format("Mensagem: {0}\n SchemaXMLMensagens: \n{1} \n\n", item.Mensagem, schemaMensagens);
                }
                entity.Mensagem = mensagens;
            }
            else
            {
                entity.SefazId = retorno.NotaId;
            }
        }

        public override void Insert(NFe entity)
        {
            entity.Fail(entity.Status != StatusNotaFiscal.NaoTransmitida, new Error("Uma nova NF-e só pode estar com status 'Não Transmitida'", "status"));
            base.Insert(entity);
        }

        public override void Update(NFe entity)
        {
            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            entity.Fail(previous.Status != StatusNotaFiscal.FalhaTransmissao && previous.Status != StatusNotaFiscal.NaoTransmitida & previous.Status != StatusNotaFiscal.NaoAutorizada && entity.Status == StatusNotaFiscal.Transmitida, new Error("Para transmitir, somente notas fiscais com status anterior igual a Não Transmitida ou Não Autorizada", "status"));
            entity.Fail(
                previous.Status != StatusNotaFiscal.NaoTransmitida &&
                entity.Status != StatusNotaFiscal.Transmitida &&
                (entity.SerieNotaFiscalId != previous.SerieNotaFiscalId || entity.NumNotaFiscal != previous.NumNotaFiscal)
                , new Error("Para alterar série e número, somente notas fiscais que ainda não foram transmitidas", "status"));

            ValidaModel(entity);

            if (entity.Status == StatusNotaFiscal.Transmitida && entity.SerieNotaFiscalId.HasValue && entity.NumNotaFiscal.HasValue && entity.IsValid())
            {
                TransmitirNFe(entity);
            }

            base.Update(entity);
        }

public override void Delete(NFe entityToDelete)
{
    var status = entityToDelete.Status;
    entityToDelete.Fail(status != StatusNotaFiscal.NaoAutorizada && status != StatusNotaFiscal.NaoTransmitida && status != StatusNotaFiscal.FalhaTransmissao, new Error("Só é possível deletar NF-e com status Não Autorizada, Não Transmitida ou Falha na Transmissão", "status"));
    if (entityToDelete.IsValid())
    {
        base.Delete(entityToDelete);
    }
    else
    {
        throw new BusinessException(entityToDelete.Notification.Get());
    }
}
        public TotalNotaFiscal CalculaTotalNFe(Guid nfeId)
        {
            var nfe = All.Where(x => x.Id == nfeId).AsNoTracking().FirstOrDefault();

            var produtos = NFeProdutoBL.All.AsNoTracking().Where(x => x.NotaFiscalId == nfeId).ToList();
            var totalProdutos = 0.0;
            if (produtos != null)
            {
                if (nfe.TipoVenda == TipoVenda.Normal || nfe.TipoVenda == TipoVenda.Devolucao)
                {
                    totalProdutos = produtos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto));
                }
                else if (nfe.TipoVenda == TipoVenda.Complementar)
                {
                    totalProdutos = +produtos.Where(x => x.Quantidade != 0 && x.Valor != 0).Sum(x => ((x.Quantidade * x.Valor) - x.Desconto));
                    totalProdutos = +produtos.Where(x => x.Quantidade == 0 && x.Valor != 0).Sum(x => (x.Valor - x.Desconto));
                }
                //else if (ordemVenda.TipoVenda == TipoVenda.Ajuste)
                //{

                //}
            }

            var totalImpostosProdutos = nfe.TotalImpostosProdutos;
            var totalImpostosProdutosNaoAgrega = nfe.TotalImpostosProdutosNaoAgrega;
            bool calculaFrete = (
                ((nfe.TipoFrete == TipoFrete.CIF || nfe.TipoFrete == TipoFrete.Remetente) && nfe.TipoVenda == TipoVenda.Normal) ||
                ((nfe.TipoFrete == TipoFrete.FOB || nfe.TipoFrete == TipoFrete.Destinatario) && nfe.TipoVenda == TipoVenda.Devolucao)
            );

            var result = new TotalNotaFiscal()
            {
                TotalProdutos = Math.Round(totalProdutos, 2, MidpointRounding.AwayFromZero),
                ValorFrete = (calculaFrete && nfe.ValorFrete.HasValue) ? Math.Round(nfe.ValorFrete.Value, 2, MidpointRounding.AwayFromZero) : 0,
                TotalImpostosProdutos = Math.Round(totalImpostosProdutos, 2, MidpointRounding.AwayFromZero),
                TotalImpostosProdutosNaoAgrega = Math.Round(totalImpostosProdutosNaoAgrega, 2, MidpointRounding.AwayFromZero),
            };

            return result;
        }

        private TransmissaoBLs ObterTransmissaoBLs()
        {
            return new TransmissaoBLs()
            {
                CertificadoDigitalBL = CertificadoDigitalBL,
                CondicaoParcelamentoBL = CondicaoParcelamentoBL,
                FormaPagamentoBL = FormaPagamentoBL,
                NFeProdutoBL = NFeProdutoBL,
                NotaFiscalInutilizadaBL = NotaFiscalInutilizadaBL,
                NotaFiscalItemTributacaoBL = NotaFiscalItemTributacaoBL,
                PessoaBL = PessoaBL,
                SerieNotaFiscalBL = SerieNotaFiscalBL,
                SubstituicaoTributariaBL = SubstituicaoTributariaBL,
                TotalTributacaoBL = TotalTributacaoBL,
                PlataformaUrl = PlataformaUrl
            };
        }
    }
}