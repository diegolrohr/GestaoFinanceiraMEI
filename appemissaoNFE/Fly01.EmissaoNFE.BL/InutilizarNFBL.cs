using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Linq;

namespace Fly01.EmissaoNFE.BL
{
    public class InutilizarNFBL : PlataformaBaseBL<InutilizarNFVM>
    {
        protected EntidadeBL EntidadeBL;
        protected EstadoBL EstadoBL;
        protected EmpresaBL EmpresaBL;

        public InutilizarNFBL(AppDataContextBase context, EntidadeBL entidadeBL, EstadoBL estadoBl, EmpresaBL empresaBL) : base(context)
        {
            EntidadeBL = entidadeBL;
            EstadoBL = estadoBl;
            EmpresaBL = empresaBL;
        }

        public override void ValidaModel(InutilizarNFVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            entity.Fail(!EstadoBL.All.Any(e => e.CodigoIbge == entity.EmpresaCodigoUF.ToString()), new Error("O código IBGE da UF da empresa é inválido.", "EmpresaCodigoUF"));
            entity.Fail(string.IsNullOrEmpty(entity.EmpresaCnpj), new Error("Informe o CNPJ da empresa.", "EmpresaCnpj"));
            entity.Fail(!string.IsNullOrEmpty(entity.EmpresaCnpj) && (!EmpresaBL.ValidaCNPJ(entity.EmpresaCnpj) || entity.EmpresaCnpj.Length != 14),
                new Error("CNPJ da empresa inválido.", "EmpresaCnpj"));
            entity.Fail(entity.ModeloDocumentoFiscal != 55 && entity.ModeloDocumentoFiscal != 65,
                new Error("Modelo documento inválido. Informe 55 para NF-e ou 65 paraNFC-e.", "ModeloDocumentoFiscal"));
            entity.Fail(entity.Serie == default(int) || entity.Serie < 0, new Error("Série da nota fiscal é um dado obrigatório.", "Serie"));
            entity.Fail(entity.Numero == default(int) || entity.Numero < 0, new Error("Número da nota fiscal é um dado obrigatório.", "Numero"));

            base.ValidaModel(entity);
        }
    }
}
