using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Domain;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System;
using System.Linq;
using Fly01.Core.API;

namespace Fly01.EmissaoNFE.BL
{
    public class EntidadeBL : PlataformaBaseBL<EntidadeVM>
    {
        public EntidadeBL(AppDataContextBase context) : base(context)
        {
        }
        
        public bool TSSException(Exception ex)
        {
            if (ex.Message.Contains("TOTVS Service Soa") | ex.Message.Contains("TOTVS SPED Services"))
                    return true;

            return false;
        }

        public void EmissaoNFeException(Exception ex, EntidadeVM entity)
        {
            string[] split = { "\n" };
            var message = ex.Message.Split(split, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in message)
            {
                if (!item.Contains("- Ok"))
                    entity.Notification.Errors.Add(new Error(item));
            }

            throw new BusinessException(entity.Notification.Get());
        }
        
        public void ValidaGet(string entidade, TipoAmbiente tipoAmbiente)
        {
            var entity = new EntidadeVM
            {
                Homologacao = (int)tipoAmbiente == 1 ? "000001" : entidade,
                Producao = (int)tipoAmbiente == 2 ? "000001" : entidade,
                EntidadeAmbiente = tipoAmbiente
            };

            ValidaModel(entity);
        }

        public override void ValidaModel(EntidadeVM entity)
        {
            var tipo = EnumHelper.GetDataEnumValues(typeof(TipoAmbiente));
            entity.Fail(entity.Homologacao == null || entity.Homologacao.Length != 6 || entity.Homologacao == "000000", EntidadeInvalida);
            entity.Fail(entity.Producao == null || entity.Producao.Length != 6 || entity.Producao == "000000", EntidadeProdInvalida);
            entity.Fail(string.IsNullOrEmpty(entity.EntidadeAmbiente.ToString()) || !tipo.Any(x => x.Value == ((int)entity.EntidadeAmbiente).ToString()), TipoInvalido);

            base.ValidaModel(entity);
        }

        

        public static Error EntidadeInvalida = new Error("Entidade de homologação inválida.", "Homologacao");
        public static Error EntidadeProdInvalida = new Error("Entidade de produção inválida.", "Producao");
        public static Error TipoInvalido = new Error("Tipo de ambiente inválido.", "EntidadeAmbiente");
    }
}
