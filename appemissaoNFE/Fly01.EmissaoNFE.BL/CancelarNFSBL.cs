using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Linq;
using System.Data.Entity;
using System;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.BL
{
    public class CancelarNFSBL : PlataformaBaseBL<CancelarNFSVM>
    {
        protected EntidadeBL EntidadeBL;
        protected CidadeBL CidadeBL;
        
        public CancelarNFSBL(AppDataContextBase context, EntidadeBL entidadeBL, CidadeBL cidadeBL) : base(context)
        {
            EntidadeBL = entidadeBL;
            CidadeBL = cidadeBL;
        }

        public override void ValidaModel(CancelarNFSVM entity)
        {
            EntidadeBL.ValidaModel(entity);
            var codigoIBGEValido = CidadeBL.All.AsNoTracking().Any(e => e.CodigoIbge.ToUpper() == entity.CodigoIBGE.ToUpper());

            entity.Fail(string.IsNullOrEmpty(entity.CodigoIBGE), new Error("Código IBGE é um campo obrigatório.", "CodigoIBGE"));
            entity.Fail(!string.IsNullOrEmpty(entity.CodigoIBGE) && !codigoIBGEValido, new Error("Código IBGE do município informado é inválido.", "CodigoIBGE"));
            entity.Fail(string.IsNullOrEmpty(entity.IdNotaFiscal), new Error("Id da nota fiscal é um campo obrigatório.", "IdNotaFiscal"));
            entity.Fail(string.IsNullOrEmpty(entity.XMLUnicoTSSString), new Error("XML é um campo obrigatório.", "XMLString"));

            base.ValidaModel(entity);
        }
    }
}
