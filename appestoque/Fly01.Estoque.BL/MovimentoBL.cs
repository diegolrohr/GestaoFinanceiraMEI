using System;
using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.ServiceBus;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core;
using Newtonsoft.Json;

namespace Fly01.Estoque.BL
{
    public class MovimentoBL : PlataformaBaseBL<MovimentoEstoque>
    {
        protected ProdutoBL ProdutoBL;
        protected TipoMovimentoBL TipoMovimentoBL;

        public MovimentoBL(AppDataContextBase context,
                           ProdutoBL produtoBL,
                           TipoMovimentoBL tipoMovimentoBL)
            : base(context)
        {
            ProdutoBL = produtoBL;
            TipoMovimentoBL = tipoMovimentoBL;
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(MovimentoEstoque entity)
        {
            entity.Fail(entity.ProdutoId == Guid.Empty, ProdutoNaoInformado);
            entity.Fail(entity.TipoMovimentoId == Guid.Empty, TipoMovimentoNaoInformado);

            base.ValidaModel(entity);
        }

        public override void Insert(MovimentoEstoque entity)
        {
            var produto = ProdutoBL.All.FirstOrDefault(x => x.Id == entity.ProdutoId);
            var tipoMovimento = TipoMovimentoBL.All.FirstOrDefault(x => x.Id == entity.TipoMovimentoId);

            if (produto == null || tipoMovimento == null) return;

            entity.SaldoAntesMovimento = produto.SaldoProduto;

            produto.SaldoProduto = tipoMovimento.TipoEntradaSaida == TipoEntradaSaida.Entrada
                                    ? produto.SaldoProduto + entity.QuantidadeMovimento
                                    : produto.SaldoProduto - entity.QuantidadeMovimento;

            entity.PlataformaId = produto.PlataformaId; // TODO: Ver se tem como não precisar passar PlataformaId
            entity.UsuarioInclusao = produto.UsuarioInclusao; // TODO: Ver se tem como não precisar passar UsuarioInclusao

            base.Insert(entity);
            ProdutoBL.Update(produto, true);
        }

        public void Movimenta(InventarioItem entity)
        {
            var produto = ProdutoBL.All.FirstOrDefault(x => x.Id == entity.ProdutoId);

            if (produto.SaldoProduto == entity.SaldoInventariado)
                return;

            double saldoProduto = produto.SaldoProduto.HasValue ? produto.SaldoProduto.Value : default(double);

            MovimentoEstoque m = new MovimentoEstoque()
            {
                QuantidadeMovimento = entity.SaldoInventariado - saldoProduto,
                ProdutoId = entity.ProdutoId,
                InventarioId = entity.InventarioId,
                SaldoAntesMovimento = saldoProduto,
                PlataformaId = PlataformaUrl,
                UsuarioInclusao = AppUser,
                Id = Guid.NewGuid(),
                DataInclusao = DateTime.Now,
                Ativo = true
            };

            repository.Insert(m);

            produto.SaldoProduto = entity.SaldoInventariado;

            ProdutoBL.Update(produto, true);
        }

        public void Movimenta(MovimentoEstoque entity)
        {
            var produto = ProdutoBL.All.FirstOrDefault(x => x.Id == entity.ProdutoId);

            entity.SaldoAntesMovimento = produto.SaldoProduto;

            entity.DataInclusao = DateTime.Now;

            entity.Id = Guid.NewGuid();

            entity.Ativo = true;

            produto.SaldoProduto = produto.SaldoProduto + entity.QuantidadeMovimento;

            repository.Insert(entity);

            ProdutoBL.Update(produto, true);
        }

        public override void PersistMessage(MovimentoEstoque message, RabbitConfig.EnHttpVerb httpMethod)
        {
            var data = JsonConvert.SerializeObject(message);

            foreach (var item in MessageType.Resolve<dynamic>(data))
                Movimenta(item);
        }

        public static Error DescricaoEmBranco = new Error("Descrição não foi informada.", "descricao");
        public static Error ProdutoNaoInformado = new Error("O produto não foi informado ou o Id é inválido.", "produtoId");
        public static Error TipoMovimentoNaoInformado = new Error("O tipo de movimento não foi informado ou o Id é inválido.", "tipoMovimentoId");
    }
}