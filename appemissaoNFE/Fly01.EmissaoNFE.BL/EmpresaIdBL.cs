using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.EmissaoNFE.BL
{
    public class EmpresaIdBL : PlataformaBaseBL<EmpresaVM>
    {
        private static string msgError;
        protected EmpresaBL EmpresaBL;

        public EmpresaIdBL(AppDataContextBase context, EmpresaBL empresaBL) : base(context)
        {
            EmpresaBL = empresaBL;
        }
        
        public override void ValidaModel(EmpresaVM entity)
        {
            entity.Fail(entity.UF == null, UFRequerido);
            entity.Fail(entity.Cpf != null && !EmpresaBL.ValidaCPF(entity.Cpf), CpfInvalido);
            entity.Fail(entity.Cnpj != null && !EmpresaBL.ValidaCNPJ(entity.Cnpj), CnpjInvalido);
            if(entity.InscricaoEstadual != null)
                if(!EmpresaBL.ValidaIE(entity.UF, entity.InscricaoEstadual, out msgError))
                {
                    switch (msgError)
                    {
                        case "1":
                            entity.Fail(true, IEDigitoVerificador);
                            break;
                        case "2":
                            entity.Fail(true, IEQuantidadeDeDigitos);
                            break;
                        case "3":
                            entity.Fail(true, IEInvalida);
                            break;
                        case "4":
                            entity.Fail(true, UFInvalida);
                            break;
                        default:
                            break;
                    }
                }

            base.ValidaModel(entity);
        }
        
        public static Error UFRequerido = new Error("UF é um campo obrigatório.", "UF");
        public static Error CpfInvalido = new Error("CPF inválido.", "Cpf");
        public static Error CnpjInvalido = new Error("CNPJ inválido.", "Cnpj");
        public static Error IEDigitoVerificador = new Error("Digito verificador inválido (para este estado).", "InscricaoEstadual");
        public static Error IEQuantidadeDeDigitos = new Error("Quantidade de dígitos inválido (para este estado).", "InscricaoEstadual");
        public static Error IEInvalida = new Error("Inscrição Estadual inválida (para este estado).", "InscricaoEstadual");
        public static Error UFInvalida = new Error("Sigla da UF inválida.", "UF");
    }
}
