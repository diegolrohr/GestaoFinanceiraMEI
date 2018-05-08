using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using System.Data.Entity;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.BL
{
    public class ProdutoBL : PlataformaBaseBL<Produto>
    {
        protected GrupoProdutoBL GrupoProdutoBL;

        public ProdutoBL(AppDataContextBase context, GrupoProdutoBL grupoProdutoBL) : base(context)
        {
            MustConsumeMessageServiceBus = true;
            GrupoProdutoBL = grupoProdutoBL;
        }

        public override void ValidaModel(Produto entity)
        {
            entity.Fail(entity.GrupoProdutoId == null, GrupoProdutoInvalido);
            entity.Fail(entity.UnidadeMedidaId == null, UnidadeMedidaInvalida);
            entity.Fail(string.IsNullOrEmpty(entity.Descricao), DescricaoEmBranco);
            entity.Fail(All.Where(x => x.Descricao == entity.Descricao).Any(x => x.Id != entity.Id), DescricaoDuplicada);
            entity.Fail(entity.GrupoProdutoId != null && entity.TipoProduto != GrupoProdutoBL.All.AsNoTracking().Where(x => x.Id == entity.GrupoProdutoId).FirstOrDefault().TipoProduto, TipoProdutoDiferente);

            if (!string.IsNullOrWhiteSpace(entity.CodigoProduto))
            {
                entity.Fail(All.Where(x => x.CodigoProduto == entity.CodigoProduto).Any(x => x.Id != entity.Id), CodigoProdutoDuplicado);
            }
            
            base.ValidaModel(entity);
        }

        public void Insert(Produto entity, bool MustProduceMessageServiceBus)
        {
            base.Insert(entity);
            if(entity.IsValid() && MustProduceMessageServiceBus)
               Producer<Produto>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.enHTTPVerb.POST);
        }

        public void Update(Produto entity, bool MustProduceMessageServiceBus)
        {
            base.Update(entity);
            if (entity.IsValid() && MustProduceMessageServiceBus)
                Producer<Produto>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.enHTTPVerb.PUT);
        }

        public void Delete(Produto entity, bool MustProduceMessageServiceBus)
        {
            base.Delete(entity);
            if (entity.IsValid() && MustProduceMessageServiceBus)
                Producer<Produto>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.enHTTPVerb.DELETE);
        }

        public static Error DescricaoEmBranco = new Error("Descrição não foi informada.", "descricao");
        public static Error DescricaoDuplicada = new Error("Descrição já utilizada anteriormente.", "descricao");
        public static Error GrupoProdutoInvalido = new Error("Grupo de produto não foi informado.", "grupoProdutoId");
        public static Error UnidadeMedidaInvalida = new Error("Unidade de medida não foi informada.", "unidadeMedidaId");
        public static Error CodigoProdutoDuplicado = new Error("Código do produto já utilizado anteriormente.", "codigoProduto");
        public static Error TipoProdutoDiferente = new Error("Tipo do produto é diferente do tipo do grupo de produto.", "tipoProduto");
    }
}