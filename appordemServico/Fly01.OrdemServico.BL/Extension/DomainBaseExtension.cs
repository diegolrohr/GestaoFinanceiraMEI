using Fly01.Core.BL;
using Fly01.Core.Entities.Domains;
using Fly01.Core.Notifications;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fly01.OrdemServico.BL.Extension
{
    public static class DomainBaseExtension
    {
        public static TResult ValidForeignKey<TDomainBase, TForeignModel, TResult>(this TDomainBase entity, Func<TDomainBase, Guid?> foreignId,
            string foreignName, string foreignField, DomainBaseBL<TForeignModel> foreignBL, Expression<Func<TForeignModel, TResult>> resultPredicate)
            where TDomainBase : DomainBase
            where TForeignModel : DomainBase
            where TResult : class
        {
            Guid? id;
            if (!GetGuid(entity, foreignId, foreignName, foreignField, out id)) return null;
            var result = foreignBL.All.Where(x => x.Id == id).Select(resultPredicate).FirstOrDefault();
            entity.Fail(result == null, new Error($"'{foreignName}' informado não existe", "produtoId"));

            return result;
        }

        public static bool ValidForeignKey<TDomainBase, TForeignModel>(this TDomainBase entity, Func<TDomainBase, Guid?> foreignId,
            string foreignName, string foreignField, DomainBaseBL<TForeignModel> foreignBL)
            where TDomainBase : DomainBase
            where TForeignModel : DomainBase
        {
            Guid? id;
            if (!GetGuid(entity, foreignId, foreignName, foreignField, out id)) return false;
            var result = foreignBL.Exists(id);
            entity.Fail(!result, new Error($"'{foreignName}' informado não existe", foreignField));

            return result;
        }

        private static bool GetGuid<TDomainBase>(TDomainBase entity, Func<TDomainBase, Guid?> foreignId, string foreignName, string foreignField, out Guid? id)
            where TDomainBase : DomainBase
        {
            id = foreignId(entity);
            if (!id.HasValue) return false; //Se o retorno for nulo, é um campo não obrigatório, então nada é validado
            var result = (id.Value != Guid.Empty);
            entity.Fail(!result, new Error($"'{foreignName}' não informado", foreignField));

            return result;
        }
    }
}
