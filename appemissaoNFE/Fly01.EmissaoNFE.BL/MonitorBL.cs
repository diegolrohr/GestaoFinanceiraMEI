﻿using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;

namespace Fly01.EmissaoNFE.BL
{
    public class MonitorBL : PlataformaBaseBL<MonitorVM>
    {
        protected EntidadeBL EntidadeBL;
        public MonitorBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(MonitorVM entity)
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
            var transmitida = "103/104/105";
            var autorizada = "100/150";
            var autorizadaDPEC = "124";
            var cancelada = "101/151";
            var denegada = "110/301/302/303/999";

            var FalhaSchema = recomendacao.Substring(0, 3) == "002";
            var NaoAssinada = recomendacao.Substring(0, 3) == "003";
            var EmCancelamento = recomendacao.Substring(0, 3) == "025";
            var CanceladaForaPrazo = recomendacao.Substring(0, 3) == "036";

            StatusNotaFiscal statusNFe;

            if ((string.IsNullOrEmpty(status) | string.IsNullOrWhiteSpace(status) | transmitida.Contains(status) | NaoAssinada) && !FalhaSchema)
                statusNFe = StatusNotaFiscal.Transmitida;
            else if (EmCancelamento)
                statusNFe = StatusNotaFiscal.EmCancelamento;
            else if (CanceladaForaPrazo)
                statusNFe = StatusNotaFiscal.CanceladaForaPrazo;
            else if (FalhaSchema)
                statusNFe = StatusNotaFiscal.FalhaTransmissao;
            else if (autorizada.Contains(status) | autorizadaDPEC.Contains(status))
                statusNFe = StatusNotaFiscal.Autorizada;
            else if (cancelada.Contains(status))
                statusNFe = StatusNotaFiscal.Cancelada;
            else if (denegada.Contains(status))
                statusNFe = StatusNotaFiscal.UsoDenegado;
            else
                statusNFe = StatusNotaFiscal.NaoAutorizada;

            return statusNFe;
        }
    }
}
