﻿using Fly01.Compras.DAL;
using Fly01.Compras.Domain.Entities;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.Compras.BL
{
    public class CategoriaBL : PlataformaBaseBL<Categoria>
    {
        public CategoriaBL(AppDataContext context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void Insert(Categoria entity)
        {
            ValidaModel(entity);
            base.Insert(entity);
        }

        public override void Update(Categoria entity)
        {
            var categoriaPaiIdAlterada = All.Where(x => x.Id == entity.Id).Any(x => x.CategoriaPaiId != entity.CategoriaPaiId);

            entity.Fail(categoriaPaiIdAlterada && All.Any(x => x.CategoriaPaiId == entity.Id), AlteracaoCategoriaSuperiorInvalida);
            entity.Fail(All.Any(x => x.CategoriaPaiId == entity.Id && x.TipoCarteira != entity.TipoCarteira), AlteracaoTipoInvalida);
            ValidaModel(entity);
            base.Update(entity);
        }

        public override void Delete(Categoria entityToDelete)
        {
            entityToDelete.Fail(All.Any(x => x.CategoriaPaiId == entityToDelete.Id), ExclusaoInvalida);
            base.ValidaModel(entityToDelete);
            base.Delete(entityToDelete);
        }

        public override IQueryable<Categoria> All
        {
            get
            {
                var pais = base.All.Where(e => e.CategoriaPai == null)
                                   .OrderBy(x => x.TipoCarteira)
                                   .ThenBy(x => x.Descricao)
                                   .ThenBy(x => x.CategoriaPai.Descricao);
                IList<Categoria> listResult = new List<Categoria>();

                foreach (var catPai in pais)
                {
                    listResult.Add(catPai);

                    foreach (var catFilho in base.All.Where(x => x.CategoriaPaiId == catPai.Id)
                                                     .OrderBy(x => x.Descricao)
                                                     .ToList())
                    {
                        listResult.Add(catFilho);
                    }
                }

                return listResult.AsQueryable();
            }
        }

        #region Private Methods

        public override void ValidaModel(Categoria entity)
        {
            var categoriaPai = All.FirstOrDefault(x => x.Id == entity.CategoriaPaiId);
            entity.Fail(categoriaPai != null && All.Any(x => categoriaPai.CategoriaPaiId != null), PaiJaEFilho);

            TipoCarteiraBL.ValidaTipoCarteira(entity.TipoCarteira);
            entity.Fail(All.Any(x => x.Id != entity.Id && x.Descricao.ToUpper() == entity.Descricao.ToUpper()), DescricaoDuplicada);
            entity.Fail(entity.Id == entity.CategoriaPaiId, CategoriaPropria);

            entity.Fail(All.Where(x => x.Id == entity.CategoriaPaiId).Any(x => x.TipoCarteira != entity.TipoCarteira), TipoCarteiraDiferente);
            base.ValidaModel(entity);
        }

        #endregion

        public static Error DescricaoDuplicada = new Error("Descrição já utilizada anteriormente.", "descricao");
        public static Error CategoriaPropria = new Error("Não é possível definir a própria categoria, como sua Categoria Superior.");
        public static Error TipoCarteiraDiferente = new Error("Não foi possível salvar este registro. O tipo da carteira deve ser igual ao da Categoria Superior.");
        public static Error ExclusaoInvalida = new Error("Não é possível excluir este registro, pois o mesmo possui filhos.");
        public static Error AlteracaoCategoriaSuperiorInvalida = new Error("Não é possível alterar a Categoria Superior desta Categoria, pois a mesma já possui filhos.");
        public static Error AlteracaoTipoInvalida = new Error("Não é possível alterar o Tipo desta Categoria, pois a mesma já possui filhos.");
        public static Error PaiJaEFilho = new Error("Não é possível definir como pai uma categoria que já seja filha.");
    }
}