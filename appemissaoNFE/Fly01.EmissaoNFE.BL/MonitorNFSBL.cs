﻿using Fly01.EmissaoNFE.Domain.ViewModel;
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

        public StatusNotaFiscal ValidaStatus(string status, string recomendacao)
        {
            //campo status do MONITORNFSE

            //TODO ver códigos e novo enum de status
            StatusNotaFiscal statusNFe = StatusNotaFiscal.Autorizada;

            //if ((string.IsNullOrEmpty(status) | string.IsNullOrWhiteSpace(status) | transmitida.Contains(status) | NaoAssinada) && !FalhaSchema)
            //    statusNFe = StatusNotaFiscal.Transmitida;
            //else if (EmCancelamento)
            //    statusNFe = StatusNotaFiscal.EmCancelamento;
            //else if (CanceladaForaPrazo)
            //    statusNFe = StatusNotaFiscal.CanceladaForaPrazo;
            //else if (FalhaSchema)
            //    statusNFe = StatusNotaFiscal.FalhaTransmissao;
            //else if (inutilizada.Contains(status))
            //    statusNFe = StatusNotaFiscal.Inutilizada;
            //else if (autorizada.Contains(status) | autorizadaDPEC.Contains(status))
            //    statusNFe = StatusNotaFiscal.Autorizada;
            //else if (falhaCancelamento.Contains(status))
            //    statusNFe = StatusNotaFiscal.FalhaNoCancelamento;
            //else if (cancelada.Contains(status))
            //    statusNFe = StatusNotaFiscal.Cancelada;
            //else if (denegada.Contains(status))
            //    statusNFe = StatusNotaFiscal.UsoDenegado;
            //else
            //    statusNFe = StatusNotaFiscal.NaoAutorizada;

            return statusNFe;
        }
    }
}
