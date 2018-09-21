using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Faturamento.DAL;
using Fly01.Core.Notifications;
using System.Data.Entity;
using System.Linq;
using System;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Faturamento.BL.Helpers;
using Fly01.EmissaoNFE.Domain.ViewModelNFS;

namespace Fly01.Faturamento.BL
{
    public class NFSeBL : PlataformaBaseBL<NFSe>
    {
        protected SerieNotaFiscalBL SerieNotaFiscalBL { get; set; }
        protected NFSeServicoBL NFSeServicoBL { get; set; }
        protected TotalTributacaoBL TotalTributacaoBL { get; set; }
        protected NotaFiscalInutilizadaBL NotaFiscalInutilizadaBL { get; set; }

        public NFSeBL(AppDataContext context, SerieNotaFiscalBL serieNotaFiscalBL, NFSeServicoBL nfseServicoBL, TotalTributacaoBL totalTributacaoBL, NotaFiscalInutilizadaBL notaFiscalInutilizadaBL) : base(context)
        {
            SerieNotaFiscalBL = serieNotaFiscalBL;
            NFSeServicoBL = nfseServicoBL;
            TotalTributacaoBL = totalTributacaoBL;
            NotaFiscalInutilizadaBL = notaFiscalInutilizadaBL;
        }

        public IQueryable<NFSe> Everything => repository.All.Where(x => x.Ativo);

        public override void ValidaModel(NFSe entity)
        {
            entity.Fail(entity.TipoNotaFiscal != TipoNotaFiscal.NFSe, new Error("Permitido somente nota fiscal do tipo NFSe"));
            entity.Fail(entity.ValorFrete.HasValue && entity.ValorFrete.Value < 0, new Error("Valor frete não pode ser negativo", "valorFrete"));
            entity.Fail(entity.PesoBruto.HasValue && entity.PesoBruto.Value < 0, new Error("Peso bruto não pode ser negativo", "pesoBruto"));
            entity.Fail(entity.PesoLiquido.HasValue && entity.PesoLiquido.Value < 0, new Error("Peso liquido não pode ser negativo", "pesoLiquido"));
            entity.Fail(entity.QuantidadeVolumes.HasValue && entity.QuantidadeVolumes.Value < 0, new Error("Quantidade de volumes não pode ser negativo", "quantidadeVolumes"));
            entity.Fail((entity.NumNotaFiscal.HasValue || entity.SerieNotaFiscalId.HasValue) && (!entity.NumNotaFiscal.HasValue || !entity.SerieNotaFiscalId.HasValue), new Error("Informe série e número da nota fiscal"));
            entity.Fail((entity.Status == StatusNotaFiscal.Transmitida && (!entity.SerieNotaFiscalId.HasValue || !entity.NumNotaFiscal.HasValue)), new Error("Para transmitir, informe série e número da nota fiscal"));

            var serieNotaFiscal = SerieNotaFiscalBL.All.AsNoTracking().Where(x => x.Id == entity.SerieNotaFiscalId).FirstOrDefault();
            if (entity.SerieNotaFiscalId.HasValue)
            {
                entity.Fail(serieNotaFiscal == null || (serieNotaFiscal.TipoOperacaoSerieNotaFiscal != TipoOperacaoSerieNotaFiscal.NFSe && serieNotaFiscal.TipoOperacaoSerieNotaFiscal != TipoOperacaoSerieNotaFiscal.Ambas), new Error("Selecione uma série ativa do tipo NFS-e ou tipo ambas"));
            }

            if (entity.Status == StatusNotaFiscal.Transmitida && entity.SerieNotaFiscalId.HasValue && entity.NumNotaFiscal.HasValue)
            {
                var serieENumeroJaUsado = All.AsNoTracking().Any(x => x.Id != entity.Id && (x.SerieNotaFiscalId == entity.SerieNotaFiscalId && x.NumNotaFiscal == entity.NumNotaFiscal));
                //varios numeros de uma mesma serie/tipo inutilizados
                var serieENumeroInutilizado = NotaFiscalInutilizadaBL.All.AsNoTracking().Any(x =>
                    x.Serie.ToUpper() == serieNotaFiscal.Serie.ToUpper() &&
                    x.NumNotaFiscal == entity.NumNotaFiscal);

                if (serieENumeroJaUsado || serieENumeroInutilizado)
                {
                    var sugestaoProximoNumNota = All.AsNoTracking().Where(x => x.Id != entity.Id && (x.SerieNotaFiscalId == entity.SerieNotaFiscalId && x.NumNotaFiscal == entity.NumNotaFiscal)).Max(x => x.NumNotaFiscal);
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
                         x.NumNotaFiscal == sugestaoProximoNumNota));

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


            if (entity.Status == StatusNotaFiscal.Transmitida && entity.SerieNotaFiscalId.HasValue && entity.NumNotaFiscal.HasValue && entity.IsValid())
            {
                TransmitirNFS(entity);
            }

            base.Update(entity);
        }

        private void TransmitirNFS(NFSe entity)
        {
            try
            {
                var resourceOK = "configuracaoOKNFS";
                if (!TotalTributacaoBL.ConfiguracaoTSSOK(null, resourceOK))
                {
                    throw new BusinessException("Configuração inválida para comunicação com TSS");
                }
                else
                {
                    var transmisao = new TransmissaoNFS();
                    var transmissaoVM = transmisao.ObterTransmissaoNFSVM();
                    TransmitirNotaFiscalDeServico(entity, transmissaoVM);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        private void TransmitirNotaFiscalDeServico(NFSe entity, ItemTransmissaoNFSVM transmissaoVM)
        {
            throw new NotImplementedException();
        }

        public override void Delete(NFSe entityToDelete)
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

        public TotalNotaFiscal CalculaTotalNFSe(Guid nfseId)
        {
            var nfse = All.Where(x => x.Id == nfseId).FirstOrDefault();

            var servicos = NFSeServicoBL.All.Where(x => x.NotaFiscalId == nfseId).ToList();
            var totalServicos = servicos != null ? servicos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;
            var totalImpostosServicos = nfse.TotalImpostosServicos;

            var result = new TotalNotaFiscal()
            {
                TotalServicos = Math.Round(totalServicos, 2, MidpointRounding.AwayFromZero),
                ValorFrete = 0,
                TotalImpostosServicos = Math.Round(totalImpostosServicos, 2, MidpointRounding.AwayFromZero),
            };

            return result;
        }
    }
}