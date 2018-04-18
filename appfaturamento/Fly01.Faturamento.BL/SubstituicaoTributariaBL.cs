using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.BL
{
    public class SubstituicaoTributariaBL : PlataformaBaseBL<SubstituicaoTributaria>
    {
        public SubstituicaoTributariaBL(AppDataContextBase context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(SubstituicaoTributaria entity)
        {
            entity.Fail(entity.NcmId == null, NcmInvalido);
            entity.Fail(entity.EstadoDestinoId == null, EstadoDestinoInvalido);
            entity.Fail(entity.EstadoOrigemId == null, EstadoOrigemInvalido);
            entity.Fail(entity.Mva <= 0, MvaInvalido);
            entity.Fail(All.Where(x => x.NcmId == entity.NcmId && x.EstadoOrigemId == entity.EstadoOrigemId && x.EstadoDestinoId == entity.EstadoDestinoId && x.TipoSubstituicaoTributaria == entity.TipoSubstituicaoTributaria).Any(x => x.Id != entity.Id), SubstituicaoTributariaDuplicada);

            base.ValidaModel(entity);
        }

        public static Error NcmInvalido = new Error("Código NCM inválido.", "ncmId");
        public static Error EstadoDestinoInvalido = new Error("Estado de destino inválido.", "estadoDestinoId");
        public static Error EstadoOrigemInvalido = new Error("Estado de origem inválido.", "estadoOrigemId");
        public static Error MvaInvalido = new Error("MVA deve ser maior que zero.", "mva");
        public static Error SubstituicaoTributariaDuplicada = new Error("Já existe uma Substituição Tributária com estas configurações.");
    }
}