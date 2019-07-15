﻿using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using System.Data.Entity;

namespace Fly01.Faturamento.BL
{
    public class GrupoTributarioBL : PlataformaBaseBL<GrupoTributario>
    {
        protected CfopBL CfopBL;
        public GrupoTributarioBL(AppDataContextBase context, CfopBL cfopBL) : base(context)
        {
            CfopBL = cfopBL;
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(GrupoTributario entity)
        {
            entity.Fail(All.Any(x => x.Descricao.ToUpper() == entity.Descricao.ToUpper() && x.Id != entity.Id), DescricaoRepetida);

            base.ValidaModel(entity);
        }

        protected void ConfiguraImpostos(GrupoTributario entity)
        {
            //regras das tributações do  emissao NFe
            if (entity.TipoTributacaoICMS.HasValue)
            {
                var TipoTributacaoICMS = (int)entity.TipoTributacaoICMS;
                if (TipoTributacaoICMS == 201 || TipoTributacaoICMS == 202 || TipoTributacaoICMS == 203 || TipoTributacaoICMS == 500 || TipoTributacaoICMS == 10 || TipoTributacaoICMS == 70)
                {
                    entity.CalculaSubstituicaoTributaria = true;
                }
            }
        }

        public void GetIdCfop(GrupoTributario entity)
        {
            if (!entity.CfopId.HasValue && !string.IsNullOrEmpty(entity.CodigoCfop.ToString()))
            {
                var dadosCfop = CfopBL.All.AsNoTracking().FirstOrDefault(x => x.Codigo == entity.CodigoCfop);
                if (dadosCfop != null)
                    entity.CfopId = dadosCfop.Id;
            }
        }

        public override void Insert(GrupoTributario entity)
        {
            GetIdCfop(entity);
            ConfiguraImpostos(entity);
            base.Insert(entity);
        }

        public override void Update(GrupoTributario entity)
        {
            GetIdCfop(entity);
            ConfiguraImpostos(entity);
            base.Update(entity);
        }

        public static Error DescricaoRepetida = new Error("Descrição do grupo tributário já utilizada anteriormente.", "descricao");
    }
}