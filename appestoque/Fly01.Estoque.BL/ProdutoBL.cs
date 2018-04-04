using Fly01.Core;
using System.Linq;
using Fly01.Core.Api.BL;
using Fly01.Core.Notifications;
using Fly01.Estoque.Domain.Entities;
using Fly01.Core.ServiceBus;

namespace Fly01.Estoque.BL
{
    public class ProdutoBL : PlataformaBaseBL<Produto>
    {
        public ProdutoBL(AppDataContextBase context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(Produto entity)
        {
            entity.Fail(entity.GrupoProdutoId == null, GrupoProdutoInvalido);
            entity.Fail(entity.UnidadeMedidaId == null, UnidadeMedidaInvalida);
            entity.Fail(string.IsNullOrEmpty(entity.Descricao), DescricaoEmBranco);
            entity.Fail(All.Where(x => x.Descricao == entity.Descricao).Any(x => x.Id != entity.Id), DescricaoDuplicada);

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
            {
               Producer.Send(entity.GetType().Name, entity, RabbitConfig.enHTTPVerb.POST);
            }
        }

        public void Update(Produto entity, bool MustProduceMessageServiceBus)
        {
            base.Update(entity);
            if (entity.IsValid() && MustProduceMessageServiceBus)
            {
                Producer.Send(entity.GetType().Name, entity, RabbitConfig.enHTTPVerb.PUT);
            }
        }

        public void Delete(Produto entity, bool MustProduceMessageServiceBus)
        {
            base.Delete(entity);
            if (entity.IsValid() && MustProduceMessageServiceBus)
            {
                Producer.Send(entity.GetType().Name, entity, RabbitConfig.enHTTPVerb.DELETE);
            }
        }

        public static Error DescricaoEmBranco = new Error("Descrição não foi informada.", "descricao");
        public static Error DescricaoDuplicada = new Error("Descrição já utilizada anteriormente.", "descricao");
        public static Error GrupoProdutoInvalido = new Error("Grupo de produto não foi informado.", "grupoProdutoId");
        public static Error UnidadeMedidaInvalida = new Error("Unidade de medida não foi informada.", "unidadeMedidaId");
        public static Error CodigoProdutoDuplicado = new Error("Código do produto já utilizado anteriormente.", "codigoProduto");
    }
}