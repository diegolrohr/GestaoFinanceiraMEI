using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.BL
{
    public class MonitorEventoBL : PlataformaBaseBL<MonitorEventoVM>
    {
        protected EntidadeBL EntidadeBL;
        public MonitorEventoBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(MonitorEventoVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            entity.Fail(string.IsNullOrEmpty(entity.IdEvento), new Error("Informe o id do evento.", "IdEvento"));
            entity.Fail(string.IsNullOrEmpty(entity.SefazChaveAcesso), new Error("Informe a chave da nota fiscal.", "SefazChaveAcesso"));
            //ID + 110110 + chavenfe + sequencia evento
            entity.Fail(!string.IsNullOrEmpty(entity.IdEvento) && entity.IdEvento.Length != 54, new Error("Tamanho id do evento deve conter 54 caracteres.", "IdEvento"));
            entity.Fail(!string.IsNullOrEmpty(entity.SefazChaveAcesso) && entity.SefazChaveAcesso.Length != 44, new Error("Tamanho da chave de acesso deve conter 44 caracteres.", "SefazChaveAcesso"));

            base.ValidaModel(entity);
        }

        public StatusCartaCorrecao ValidaStatus(string status)
        {
            var Transmitido = "0";
            var RegistradoEVinculado = "135";
            var Rejeitado = "489/490/491/492/493/494/501/572/573/574/575/576/577/578/579/580/587/588/594";
            var RegistradoENaoVinculado = "136";


            StatusCartaCorrecao statusCCe;

            if (RegistradoEVinculado.Contains(status))
                statusCCe = StatusCartaCorrecao.RegistradoEVinculado;
            else if (Transmitido.Contains(status))
                statusCCe = StatusCartaCorrecao.Transmitida;
            else if (Rejeitado.Contains(status))
                statusCCe = StatusCartaCorrecao.Rejeitado;
            else if (RegistradoENaoVinculado.Contains(status))
                statusCCe = StatusCartaCorrecao.RegistradoENaoVinculado;
            else
                statusCCe = StatusCartaCorrecao.FalhaTransmissao;

            return statusCCe;
        }
    }
}
