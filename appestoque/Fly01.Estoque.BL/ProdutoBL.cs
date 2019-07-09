using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using System.Data.Entity;
using Fly01.Core.Entities.Domains.Commons;
using System.Collections.Generic;

namespace Fly01.Estoque.BL
{
    public class ProdutoBL : PlataformaBaseBL<Produto>
    {
        protected GrupoProdutoBL GrupoProdutoBL;
        protected NCMBL NCMBL;
        protected UnidadeMedidaBL UnidadeMedidaBL;
        protected CestBL CestBL;
        protected EnquadramentoLegalIPIBL EnquadramentoLegalIPIBL;

        public ProdutoBL(AppDataContextBase context, GrupoProdutoBL grupoProdutoBL, NCMBL ncmBL, UnidadeMedidaBL unidadeMedidaBL, CestBL cestBL, EnquadramentoLegalIPIBL enquadramentoLegalIPIBL) : base(context)
        {
            MustConsumeMessageServiceBus = true;
            GrupoProdutoBL = grupoProdutoBL;
            NCMBL = ncmBL;
            UnidadeMedidaBL = unidadeMedidaBL;
            CestBL = cestBL;
            EnquadramentoLegalIPIBL = enquadramentoLegalIPIBL;
    }

        public void GetIdNCM(Produto entity)
        {
            if (!entity.NcmId.HasValue && !string.IsNullOrEmpty(entity.CodigoNcm))
            {
                var dadosNCM = NCMBL.All.AsNoTracking().FirstOrDefault(x => x.Codigo == entity.CodigoNcm);
                if (dadosNCM != null)
                    entity.NcmId = dadosNCM.Id;
            }
        }

        public void GetIdUnidadeMedida(Produto entity)
        {
            if (!entity.UnidadeMedidaId.HasValue && !string.IsNullOrEmpty(entity.AbreviacaoUnidadeMedida))
            {
                var dadosUnidadeMedida = UnidadeMedidaBL.All.AsNoTracking().FirstOrDefault(x => x.Abreviacao == entity.AbreviacaoUnidadeMedida);
                if (dadosUnidadeMedida != null)
                    entity.UnidadeMedidaId = dadosUnidadeMedida.Id;
            }
        }

        public void GetIdCest(Produto entity)
        {
            if (!entity.CestId.HasValue && !string.IsNullOrEmpty(entity.CodigoCest) && entity.NcmId.HasValue)
            {
                var dadosCest = CestBL.All.AsNoTracking().FirstOrDefault(x => x.Codigo == entity.CodigoCest && x.NcmId == entity.NcmId);
                if (dadosCest != null)
                    entity.CestId = dadosCest.Id;
            }
        }

        public void GetIdEnquadramentoLegalIPIBL(Produto entity)
        {
            if (!entity.EnquadramentoLegalIPIId.HasValue && !string.IsNullOrEmpty(entity.CodigoEnquadramentoLegalIPI))
            {
                var dadosEnquadramentoLegalIPI = EnquadramentoLegalIPIBL.All.AsNoTracking().FirstOrDefault(x => x.Codigo == entity.CodigoEnquadramentoLegalIPI);
                if (dadosEnquadramentoLegalIPI != null)
                    entity.EnquadramentoLegalIPIId = dadosEnquadramentoLegalIPI.Id;
            }
        }

        public override void ValidaModel(Produto entity)
        {
            entity.Fail(entity.UnidadeMedidaId == null, UnidadeMedidaInvalida);
            entity.Fail(string.IsNullOrEmpty(entity.Descricao), DescricaoEmBranco);
            entity.Fail(
                All.Any(x => x.Descricao.Trim().ToUpper() == entity.Descricao.Trim().ToUpper() && x.Id != entity.Id) ||
                ContextAddedEntriesSelfType().Any(x => x.Descricao.Trim().ToUpper() == entity.Descricao.Trim().ToUpper() && x.Id != entity.Id)
            , DescricaoDuplicada);

            if (!string.IsNullOrWhiteSpace(entity.CodigoProduto))
            {
                entity.Fail(
                    All.Any(x => x.CodigoProduto.Trim().ToUpper() == entity.CodigoProduto.Trim().ToUpper() && x.Id != entity.Id) ||
                    ContextAddedEntriesSelfType().Any(x => x.CodigoProduto?.Trim()?.ToUpper() == entity?.CodigoProduto?.Trim()?.ToUpper() && x.Id != entity.Id)
                , CodigoProdutoDuplicado);
            }

            base.ValidaModel(entity);
        }

        public override void Insert(Produto entity)
        {
            GetIdNCM(entity);
            GetIdCest(entity);
            GetIdUnidadeMedida(entity);
            GetIdEnquadramentoLegalIPIBL(entity);
            base.Insert(entity);
        }

        public void Update(Produto entity, bool MustProduceMessageServiceBus)
        {
            GetIdNCM(entity);
            GetIdCest(entity);
            GetIdUnidadeMedida(entity);
            GetIdEnquadramentoLegalIPIBL(entity);

            base.Update(entity);
            if (entity.IsValid() && MustProduceMessageServiceBus)
                Producer<Produto>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.EnHttpVerb.PUT);
        }

        public static List<string> ColunasParaImportacao()
        {
            return new List<string>
            {
                "Descricao",
                "CodigoProduto",
                "CodigoBarras",
                "AbreviacaoUnidadeMedida",
                "SaldoMinimo",
                "ValorCusto",
                "ValorVenda",
                "Observacao"
            };
        }

        public static Error DescricaoEmBranco = new Error("Descrição não foi informada.", "descricao");
        public static Error DescricaoDuplicada = new Error("Descrição do produto já utilizada anteriormente.", "descricao");
        public static Error UnidadeMedidaInvalida = new Error("Unidade de medida não foi informada.", "unidadeMedidaId");
        public static Error CodigoProdutoDuplicado = new Error("Código do produto já utilizado anteriormente.", "codigoProduto");
    }
}