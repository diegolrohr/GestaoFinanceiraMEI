using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
{
    public class ConfiguracaoNotificacaoBL : PlataformaBaseBL<ConfiguracaoNotificacaoFinanceiro>
    {
        public ConfiguracaoNotificacaoBL(AppDataContextBase context) : base(context) {}

        public IQueryable<ConfiguracaoNotificacaoFinanceiro> AllWithoutPlataformaId => repository.All.Where(x => x.Ativo);

        public override void Insert(ConfiguracaoNotificacaoFinanceiro entity)
        {
            entity.Fail(!entity.NotificaViaSMS && !entity.NotificaViaEmail && string.IsNullOrEmpty(entity.EmailDestino) && string.IsNullOrEmpty(entity.ContatoDestino), Uncheckeed);
            entity.Fail(entity.NotificaViaEmail && string.IsNullOrEmpty(entity.EmailDestino), NotificaEmail);
            entity.Fail(entity.NotificaViaSMS && string.IsNullOrEmpty(entity.ContatoDestino), NotificaSms);
            entity.Fail(entity.HoraEnvio == null, HoraEnvioNull);

            base.Insert(entity);
        }

        public static Error NotificaEmail = new Error("O campo E-mail deve ser preenchido.", "NotificaEmail");
        public static Error NotificaSms = new Error("O campo celular destino deve ser preenchido", "NotificaSms");
        public static Error HoraEnvioNull = new Error("O campo Hora envio é de preenchimento obrigatório", "horaEnvioNull");
        public static Error Uncheckeed = new Error("Para salvar você deve marcar uma das duas opções. Notifica via e-mail ou Nofica via SMS", "uncheckeed");
    }
}