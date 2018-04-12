using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;

namespace Fly01.EmissaoNFE.BL
{
    public class CancelarFaixaBL : PlataformaBaseBL<CancelarFaixaVM>
    {
        protected EntidadeBL EntidadeBL;

        public CancelarFaixaBL(AppDataContextBase context, EntidadeBL entidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
        }

        public override void ValidaModel(CancelarFaixaVM entity)
        {
            EntidadeBL.ValidaModel(entity);
            
            entity.Fail(entity.NotaInicial == null, NotaInicialRequerida);
            entity.Fail(entity.NotaFinal == null, NotaFinalRequerida);

            base.ValidaModel(entity);
        }

        public static Error NotaInicialRequerida = new Error("Nota inicial é um campo obrigatório.", "NotaInicial");
        public static Error NotaFinalRequerida = new Error("Nota final é um campo obrigatório.", "NotaFinal");
    }
}
