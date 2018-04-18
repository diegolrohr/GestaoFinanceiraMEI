using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.BL
{
    public class GrupoTributarioBL : PlataformaBaseBL<GrupoTributario>
    {
        public GrupoTributarioBL(AppDataContextBase context) : base(context) { }

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
                if (TipoTributacaoICMS == 201 || TipoTributacaoICMS == 202 || TipoTributacaoICMS == 203)
                {
                    entity.CalculaSubstituicaoTributaria = true;
                }
            }
        }

        public override void Insert(GrupoTributario entity)
        {
            ConfiguraImpostos(entity);
            base.Insert(entity);
        }

        public override void Update(GrupoTributario entity)
        {
            ConfiguraImpostos(entity);
            base.Update(entity);
        }

        public static Error DescricaoRepetida = new Error("Descrição já utilizada anteriormente.", "descricao");
    }
}