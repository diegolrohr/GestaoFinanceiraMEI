using Fly01.Core.BL;
using Fly01.EmissaoNFE.BL.Helpers;
using Fly01.EmissaoNFE.Domain.ViewModelNfs;

namespace Fly01.EmissaoNFE.BL
{
    public class TransmissaoNFSBL : PlataformaBaseBL<TransmissaoNFSVM>
    {
        protected CidadeBL CidadeBL;
        protected EmpresaBL EmpresaBL;
        protected EntidadeBL EntidadeBL;
        protected EstadoBL EstadoBL;

        public TransmissaoNFSBL(AppDataContextBase context, CidadeBL cidadeBL, EmpresaBL empresaBL, EntidadeBL entidadeBL, EstadoBL estadoBL )
            : base(context)
        {
            CidadeBL = cidadeBL;
            EmpresaBL = empresaBL;
            EntidadeBL = entidadeBL;
            EstadoBL = estadoBL;
        }

        public override void ValidaModel(TransmissaoNFSVM entity)
        {
            EntidadeBL.ValidaModel(entity);

            var entitesBLNFS = GetEntitiesBLToValidateNFS();

            var helperValidaModelTransmissaoNFS = new HelperValidaModelTransmissaoNFS(entity, entitesBLNFS);
            helperValidaModelTransmissaoNFS.ExecutarHelperValidaModelNFS();

            base.ValidaModel(entity);
        }

        public EntitiesBLToValidateNFS GetEntitiesBLToValidateNFS()
        {
            return new EntitiesBLToValidateNFS
            {
                _cidadeBL = CidadeBL, 
                _empresaBL = EmpresaBL, 
                _entidadeBL = EntidadeBL, 
                _estadoBL = EstadoBL
            };
        }
    }
}
