using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.BL
{
    public class ServicoBL : PlataformaBaseBL<Servico>
    {
        protected ISSBL ISSBL;
        protected NBSBL NBSBL;
        protected UnidadeMedidaBL UnidadeMedidaBL;
        public ServicoBL(AppDataContextBase context, ISSBL issBL, NBSBL nbsBL, UnidadeMedidaBL unidadeMedidaBL) : base(context)
        {
            ISSBL = issBL;
            NBSBL = nbsBL;
            UnidadeMedidaBL = unidadeMedidaBL;
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(Servico entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Descricao), DescricaoEmBranco);
            entity.Fail(string.IsNullOrEmpty(entity.CodigoServico), CodigoServicoEmBranco);
            entity.Fail(All.Where(x => x.Descricao == entity.Descricao).Any(x => x.Id != entity.Id), DescricaoDuplicada);
            entity.Fail(All.Where(x => x.CodigoServico == entity.CodigoServico).Any(x => x.Id != entity.Id), CodigoServicoDuplicado);

            base.ValidaModel(entity);
        }

        public void GetIdIss(Servico entity)
        {
            if (!entity.IssId.HasValue && !string.IsNullOrEmpty(entity.CodigoIss))
            {
                var dadosIss = ISSBL.All.FirstOrDefault(x => x.Codigo == entity.CodigoIss);
                if (dadosIss != null)
                    entity.IssId = dadosIss.Id;
            }
        }

        public void GetIdNbs(Servico entity)
        {
            if (!entity.NbsId.HasValue && !string.IsNullOrEmpty(entity.CodigoNbs))
            {
                var dadosNbs = NBSBL.All.FirstOrDefault(x => x.Codigo == entity.CodigoNbs);
                if (dadosNbs != null)
                    entity.NbsId = dadosNbs.Id;
            }
        }

        public void GetIdUnidadeMedida(Servico entity)
        {
            if (!entity.UnidadeMedidaId.HasValue && !string.IsNullOrEmpty(entity.AbreviacaoUnidadeMedida))
            {
                var dadosUnidadeMedida = UnidadeMedidaBL.All.FirstOrDefault(x => x.Abreviacao == entity.AbreviacaoUnidadeMedida);
                if (dadosUnidadeMedida != null)
                    entity.UnidadeMedidaId = dadosUnidadeMedida.Id;
            }
        }

        public override void Insert(Servico entity)
        {
            GetIdIss(entity);
            GetIdNbs(entity);
            GetIdUnidadeMedida(entity);

            base.Insert(entity);
        }

        public override void Update(Servico entity)
        {
            GetIdIss(entity);
            GetIdNbs(entity);
            GetIdUnidadeMedida(entity);

            base.Update(entity);
        }

        public static Error DescricaoEmBranco = new Error("Descrição não foi informada.", "descricao");
        public static Error DescricaoDuplicada = new Error("Descrição já utilizada anteriormente.", "descricao");
        public static Error GrupoServicoInvalido = new Error("Grupo de Servico não foi informado.", "grupoServicoId");
        public static Error CodigoServicoEmBranco = new Error("Código do Servico em branco.", "codigoServico");
        public static Error UnidadeMedidaInvalida = new Error("Unidade de medida não foi informada.", "unidadeMedidaId");
        public static Error CodigoServicoDuplicado = new Error("Código do Servico já utilizado anteriormente.", "codigoServico");
    }
}