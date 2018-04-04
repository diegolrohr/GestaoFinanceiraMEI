using Fly01.Core.Api.BL;
using Fly01.Faturamento.Domain.Entities;
using Fly01.Faturamento.DAL;
using Fly01.Faturamento.Domain.Enums;
using Fly01.Core.Notifications;
using System.Data.Entity;
using System.Linq;
using System;

namespace Fly01.Faturamento.BL
{
    public class NFSeBL : PlataformaBaseBL<NFSe>
    {
        protected SerieNotaFiscalBL SerieNotaFiscalBL;
        protected NFSeServicoBL NFSeServicoBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }

        public NFSeBL(AppDataContext context, SerieNotaFiscalBL serieNotaFiscalBL, NFSeServicoBL nfseServicoBL, TotalTributacaoBL totalTributacaoBL) : base(context)
        {
            SerieNotaFiscalBL = serieNotaFiscalBL;
            NFSeServicoBL = nfseServicoBL;
            TotalTributacaoBL = totalTributacaoBL;
        }

        public IQueryable<NFSe> AllWithoutPlataformaId => repository.All.Where(x => x.Ativo);

        public override void ValidaModel(NFSe entity)
        {
            entity.Fail(entity.TipoNotaFiscal != TipoNotaFiscal.NFSe, new Error("Permitido somente nota fiscal do tipo NFSe"));
            entity.Fail(entity.ValorFrete.HasValue && entity.ValorFrete.Value < 0, new Error("Valor frete não pode ser negativo", "valorFrete"));
            entity.Fail(entity.PesoBruto.HasValue && entity.PesoBruto.Value < 0, new Error("Peso bruto não pode ser negativo", "pesoBruto"));
            entity.Fail(entity.PesoLiquido.HasValue && entity.PesoLiquido.Value < 0, new Error("Peso liquido não pode ser negativo", "pesoLiquido"));
            entity.Fail(entity.QuantidadeVolumes.HasValue && entity.QuantidadeVolumes.Value < 0, new Error("Quantidade de volumes não pode ser negativo", "quantidadeVolumes"));
            entity.Fail((entity.NumNotaFiscal.HasValue || entity.SerieNotaFiscalId.HasValue) && (!entity.NumNotaFiscal.HasValue || !entity.SerieNotaFiscalId.HasValue), new Error("Informe série e número da nota fiscal"));

            var serieNotaFiscal = SerieNotaFiscalBL.All.AsNoTracking().Where(x => x.Id == entity.SerieNotaFiscalId).FirstOrDefault();
            if (entity.SerieNotaFiscalId.HasValue)
            {
                entity.Fail(serieNotaFiscal == null || serieNotaFiscal.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Inutilizada || (serieNotaFiscal.TipoOperacaoSerieNotaFiscal != TipoOperacaoSerieNotaFiscal.NFSe && serieNotaFiscal.TipoOperacaoSerieNotaFiscal != TipoOperacaoSerieNotaFiscal.Ambas), new Error("Selecione uma série ativa do tipo NFS-e ou tipo ambas"));
            }

            if (entity.Status == StatusNotaFiscal.Transmitida && entity.SerieNotaFiscalId.HasValue && entity.NumNotaFiscal.HasValue)
            {
                var serieENumeroJaUsado = All.AsNoTracking().Any(x => x.Id != entity.Id && (x.SerieNotaFiscalId == entity.SerieNotaFiscalId && x.NumNotaFiscal == entity.NumNotaFiscal));
                //varios numeros de uma mesma serie/tipo inutilizados
                var serieENumeroInutilizado = SerieNotaFiscalBL.All.AsNoTracking().Any(x =>
                    x.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Inutilizada &&
                    x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() &&
                    x.NumNotaFiscal == entity.NumNotaFiscal &&
                    (x.TipoOperacaoSerieNotaFiscal == serieNotaFiscal.TipoOperacaoSerieNotaFiscal || x.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas));

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
                    while (SerieNotaFiscalBL.All.AsNoTracking().Any(x =>
                         x.StatusSerieNotaFiscal == StatusSerieNotaFiscal.Inutilizada &&
                         x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() &&
                         x.NumNotaFiscal == sugestaoProximoNumNota &&
                         (x.TipoOperacaoSerieNotaFiscal == serieNotaFiscal.TipoOperacaoSerieNotaFiscal || x.TipoOperacaoSerieNotaFiscal == TipoOperacaoSerieNotaFiscal.Ambas)));

                    entity.Fail(true, new Error("Série e número já utilizados ou inutilizados, sugestão de número: " + sugestaoProximoNumNota.ToString(), "numNotaFiscal"));
                }
                else
                {
                    var serie = SerieNotaFiscalBL.All.Where(x => x.Id == entity.SerieNotaFiscalId).FirstOrDefault();
                    serie.NumNotaFiscal = entity.NumNotaFiscal.Value + 1;
                    SerieNotaFiscalBL.Update(serie);
                };
            }
            //TODO: ver regras para faturar e transmitir, update e delete

            base.ValidaModel(entity);
        }

        public override void Insert(NFSe entity)
        {
            entity.Fail(entity.Status != StatusNotaFiscal.NaoTransmitida, new Error("Uma nova NFS-e só pode estar com status 'Não Transmitida'", "status"));
            base.Insert(entity);
        }

        public override void Update(NFSe entity)
        {
            entity.Fail(entity.Status == StatusNotaFiscal.Transmitida, new Error("Ainda não é possível transmitir notas de serviço, aguarde a atualização do sistema", "status"));

            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            entity.Fail(previous.Status != StatusNotaFiscal.NaoTransmitida & previous.Status != StatusNotaFiscal.NaoAutorizada && entity.Status == StatusNotaFiscal.Transmitida, new Error("Para transmitir, somente notas fiscais com status anterior igual a Não Transmitida ou Não Autorizada", "status"));
            entity.Fail(
                previous.Status != StatusNotaFiscal.NaoTransmitida &&
                entity.Status != StatusNotaFiscal.Transmitida &&
                (entity.SerieNotaFiscalId != previous.SerieNotaFiscalId || entity.NumNotaFiscal != previous.NumNotaFiscal)
                , new Error("Para alterar série e número, somente notas fiscais que ainda não foram transmitidas", "status"));

            base.Update(entity);
        }

        public override void Delete(NFSe entityToDelete)
        {
            entityToDelete.Fail(true, new Error("Não é possível deletar"));
        }

        public TotalNotaFiscal CalculaTotalNFSe(Guid nfseId, double? valorFreteCIF = 0)
        {
            var nfse = All.Where(x => x.Id == nfseId).FirstOrDefault();

            var servicos = NFSeServicoBL.All.Where(x => x.NotaFiscalId == nfseId).ToList();
            var totalServicos = servicos != null ? servicos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;
            var totalImpostosServicos = nfse.TotalImpostosServicos;

            var result = new TotalNotaFiscal()
            {
                TotalServicos = Math.Round(totalServicos, 2, MidpointRounding.AwayFromZero),
                ValorFreteCIF = 0,
                TotalImpostosServicos = Math.Round(totalImpostosServicos, 2, MidpointRounding.AwayFromZero),
            };

            return result;
        }
    }
}