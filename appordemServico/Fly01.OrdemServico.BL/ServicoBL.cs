using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System.Linq;

namespace Fly01.OrdemServico.BL
{
    public class ServicoBL : PlataformaBaseBL<Servico>
    {
        public ServicoBL(AppDataContextBase context) : base(context) { }

        public override void ValidaModel(Servico entity)
        {
            entity.Fail(string.IsNullOrEmpty(entity.Descricao), DescricaoEmBranco);
            entity.Fail(string.IsNullOrEmpty(entity.CodigoServico), CodigoServicoEmBranco);
            entity.Fail(All.Where(x => x.Descricao == entity.Descricao).Any(x => x.Id != entity.Id), DescricaoDuplicada);
            entity.Fail(All.Where(x => x.CodigoServico == entity.CodigoServico).Any(x => x.Id != entity.Id), CodigoServicoDuplicado);

            base.ValidaModel(entity);
        }

        public static Error DescricaoEmBranco = new Error("Descrição não foi informada.", "descricao");
        public static Error DescricaoDuplicada = new Error("Descrição já utilizada anteriormente.", "descricao");
        public static Error GrupoServicoInvalido = new Error("Grupo de Servico não foi informado.", "grupoServicoId");
        public static Error CodigoServicoEmBranco = new Error("Código do Servico em branco.", "codigoServico");
        public static Error UnidadeMedidaInvalida = new Error("Unidade de medida não foi informada.", "unidadeMedidaId");
        public static Error CodigoServicoDuplicado = new Error("Código do Servico já utilizado anteriormente.", "codigoServico");
    }
}