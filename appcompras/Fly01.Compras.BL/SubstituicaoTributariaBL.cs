using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;
using System;
using System.Data.Entity;

namespace Fly01.Compras.BL
{
    public class SubstituicaoTributariaBL : PlataformaBaseBL<SubstituicaoTributaria>
    {
        protected NCMBL NcmBL;
        protected CestBL CestBL;
        protected EstadoBL EstadoBL;
        public SubstituicaoTributariaBL(AppDataContextBase context, NCMBL ncmBL, CestBL cestBL, EstadoBL estadoBL) : base(context)
        {
            NcmBL = ncmBL;
            CestBL = cestBL;
            EstadoBL = estadoBL;
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(SubstituicaoTributaria entity)
        {
            entity.Fail(entity.NcmId == null, NcmInvalido);
            entity.Fail(entity.EstadoDestinoId == null, EstadoDestinoInvalido);
            entity.Fail(entity.EstadoOrigemId == null, EstadoOrigemInvalido);
            entity.Fail(entity.Mva <= 0, MvaInvalido);
            entity.Fail(All.Where(x => x.NcmId == entity.NcmId && x.CestId == entity.CestId && x.EstadoOrigemId == entity.EstadoOrigemId && x.EstadoDestinoId == entity.EstadoDestinoId && x.TipoSubstituicaoTributaria == entity.TipoSubstituicaoTributaria).Any(x => x.Id != entity.Id), SubstituicaoTributariaDuplicada);

            base.ValidaModel(entity);
        }

        public void GetIdEstadoOrigemEDestino(SubstituicaoTributaria entity)
        {
            if ((entity.EstadoOrigemId == null || entity.EstadoOrigemId == default(Guid)) && !string.IsNullOrEmpty(entity.EstadoOrigemCodigoIbge))
            {
                var dadosEstadoOrigem = EstadoBL.All.AsNoTracking().FirstOrDefault(x => x.CodigoIbge == entity.EstadoOrigemCodigoIbge);
                if (dadosEstadoOrigem != null)
                {
                    entity.EstadoOrigemId = dadosEstadoOrigem.Id;
                }
            }
            if ((entity.EstadoDestinoId == null || entity.EstadoDestinoId == default(Guid)) && !string.IsNullOrEmpty(entity.EstadoDestinoCodigoIbge))
            {
                var dadosEstadoDestino = EstadoBL.All.AsNoTracking().FirstOrDefault(x => x.CodigoIbge == entity.EstadoDestinoCodigoIbge);
                if (dadosEstadoDestino != null)
                {
                    entity.EstadoDestinoId = dadosEstadoDestino.Id;
                }
            }
        }

        public void GetIdCNcm(SubstituicaoTributaria entity)
        {
            if ((entity.NcmId == null || entity.NcmId == default(Guid)) && !string.IsNullOrEmpty(entity.CodigoNcm))
            {
                var dadosNcm = NcmBL.All.AsNoTracking().FirstOrDefault(x => x.Codigo == entity.CodigoNcm);
                if (dadosNcm != null)
                    entity.NcmId = dadosNcm.Id;
            }
        }

        public void GetIdCest(SubstituicaoTributaria entity)
        {
            if (entity.CestId.HasValue && !string.IsNullOrEmpty(entity.CodigoCest))
            {
                var dadosCest = CestBL.All.AsNoTracking().FirstOrDefault(x => x.Codigo == entity.CodigoCest);
                if (dadosCest != null)
                    entity.CestId = dadosCest.Id;
            }
        }

        public override void Insert(SubstituicaoTributaria entity)
        {
            GetIdCNcm(entity);
            GetIdCest(entity);
            GetIdEstadoOrigemEDestino(entity);

            ValidaModel(entity);
            base.Insert(entity);
        }

        public override void Update(SubstituicaoTributaria entity)
        {
            GetIdCNcm(entity);
            GetIdCest(entity);
            GetIdEstadoOrigemEDestino(entity);

            ValidaModel(entity);
            base.Update(entity);
        }

        public static Error NcmInvalido = new Error("Código NCM inválido.", "ncmId");
        public static Error EstadoDestinoInvalido = new Error("Estado de destino inválido.", "estadoDestinoId");
        public static Error EstadoOrigemInvalido = new Error("Estado de origem inválido.", "estadoOrigemId");
        public static Error MvaInvalido = new Error("MVA deve ser maior que zero.", "mva");
        public static Error SubstituicaoTributariaDuplicada = new Error("Já existe uma Substituição Tributária com estas configurações.");
    }
}