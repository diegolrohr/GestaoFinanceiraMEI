using System.Linq;
using Fly01.Estoque.DAL;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.BL
{
    public class TipoMovimentoBL : PlataformaBaseBL<TipoMovimento>
    {
        public TipoMovimentoBL(AppDataContext context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void Insert(TipoMovimento entity)
        {
            ValidaModel(entity);
            if (!entity.IsValid())
            {
                var errors = entity.Notification.Errors.Cast<object>().Aggregate("", (current, item) => current + (item + "\n"));
                throw new BusinessException(errors);
            }
            base.Insert(entity);
        }

        public override void ValidaModel(TipoMovimento entity)
        {
            entity.Fail(string.IsNullOrWhiteSpace(entity.Descricao), DescricaoEmBranco);
            entity.Fail(entity.Descricao.Length > TipoMovimento.DescricaoMaxLength, DescricaoMaiorQuePermitido);
            entity.Fail(All.Any(x => x.Descricao.ToUpper() == entity.Descricao.ToUpper() && x.Id != entity.Id), DescricaoUtilizada);

            base.ValidaModel(entity);
        }

        public static Error DescricaoEmBranco = new Error("Descrição não foi informada.", "descricao");
        public static Error DescricaoMaiorQuePermitido = new Error($"A descrição deve conter no máximo {TipoMovimento.DescricaoMaxLength} caracteres.", "descricao");
        public static Error DescricaoUtilizada = new Error("Descrição já informada anteriormente.", "descricao");
    }
}