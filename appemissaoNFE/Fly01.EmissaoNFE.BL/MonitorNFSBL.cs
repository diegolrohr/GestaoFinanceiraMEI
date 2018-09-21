using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.BL
{
    public class MonitorNFSBL : PlataformaBaseBL<MonitorNFSVM>
    {
        protected EntidadeBL EntidadeBL;
        public MonitorNFSBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(MonitorNFSVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            entity.Fail(string.IsNullOrEmpty(entity.NotaInicial), NotaInicialInvalida);
            entity.Fail(string.IsNullOrEmpty(entity.NotaFinal), NotaFinalInvalida);

            base.ValidaModel(entity);
        }

        public static Error NotaInicialInvalida = new Error("Nota inicial inválida", "NotaInicial");
        public static Error NotaFinalInvalida = new Error("Nota final inválida", "NotaFinal");

        public StatusNotaFiscal ValidaStatus(string protocolo, StatusNotaFiscal statusAnterior)
        {
            //Com a regrinha que te falei... se o _PROTOCOLO vier vazio é não transmitida, se vier preenchido, vc colocar Autorizada ou Cancelada, dependendo do tipo de envio.

            //TODO ver códigos e novo enum de status
            StatusNotaFiscal statusNFSe;

            if (string.IsNullOrEmpty(protocolo) && statusAnterior == StatusNotaFiscal.Transmitida)
            {
                statusNFSe = StatusNotaFiscal.NaoAutorizada;
            }
            else if (!string.IsNullOrEmpty(protocolo) && statusAnterior == StatusNotaFiscal.Transmitida)
            {
                statusNFSe = StatusNotaFiscal.Autorizada;
            }
            else if (string.IsNullOrEmpty(protocolo) && statusAnterior == StatusNotaFiscal.EmCancelamento)
            {
                statusNFSe = StatusNotaFiscal.FalhaNoCancelamento;
            }
            else if (!string.IsNullOrEmpty(protocolo) && statusAnterior == StatusNotaFiscal.EmCancelamento)
            {
                statusNFSe = StatusNotaFiscal.Cancelada;
            }
            else
            {
                statusNFSe = statusAnterior;
            }
            
            return statusNFSe;
        }
    }
}
