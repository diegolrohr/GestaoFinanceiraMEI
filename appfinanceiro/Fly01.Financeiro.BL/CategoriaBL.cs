using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Collections.Generic;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using System.Data.Entity;

namespace Fly01.Financeiro.BL
{
    public class CategoriaBL : EmpresaBaseBL<Categoria>
    {
        //private ContaPagarBL contaPagarBL;
        //private ContaReceberBL contaReceberBL;
        private ContaFinanceiraBL contaFinanceiraBL;

        public CategoriaBL(AppDataContext context, ContaFinanceiraBL contaFinanceiraBL) : base(context)
        {
            //this.contaPagarBL = contaPagarBL;
            //this.contaReceberBL = contaReceberBL;
            this.contaFinanceiraBL = contaFinanceiraBL;
        }

        public override void Update(Categoria entity)
        {
            var categoriaPaiIdAlterada = base.All.AsNoTracking().Where(x => x.Id == entity.Id).Any(x => x.CategoriaPaiId != entity.CategoriaPaiId);
            var previous = base.All.AsNoTracking().Where(x => x.Id == entity.Id).FirstOrDefault();
            bool categoriaTemFilho = base.All.Where(x => x.CategoriaPaiId == entity.Id).Any();
            bool temContaReceberRelacionada = contaFinanceiraBL.All.Where(e => e.CategoriaId == entity.Id && e.TipoContaFinanceira == TipoContaFinanceira.ContaReceber  && e.Ativo).Any();
            bool temContaPagarRelacionada = contaFinanceiraBL.All.Where(e => e.CategoriaId == entity.Id && e.TipoContaFinanceira == TipoContaFinanceira.ContaPagar && e.Ativo).Any();


            entity.Fail((previous != null) && (entity.TipoCarteira != previous.TipoCarteira) && (temContaPagarRelacionada && entity.TipoCarteira == TipoCarteira.Receita) || (temContaReceberRelacionada && entity.TipoCarteira == TipoCarteira.Despesa), AlterarTipoInvalidaFK);
            entity.Fail(categoriaTemFilho && entity.CategoriaPaiId.HasValue, AlteracaoCategoriaSuperiorInvalida);
            entity.Fail(categoriaPaiIdAlterada && base.All.Any(x => x.CategoriaPaiId == entity.Id), AlteracaoCategoriaSuperiorInvalida);
            entity.Fail(base.All.Any(x => x.CategoriaPaiId == entity.Id && x.TipoCarteira != entity.TipoCarteira), AlteracaoTipoInvalida);
            ValidaModel(entity);
            base.Update(entity);
        }

        public override void Delete(Categoria entityToDelete)
        {
            entityToDelete.Fail(base.All.Any(x => x.CategoriaPaiId == entityToDelete.Id), ExclusaoInvalida);
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
            var categoriaPai = base.All.FirstOrDefault(x => x.Id == entity.CategoriaPaiId);
            entity.Fail(categoriaPai != null && base.All.Any(x => categoriaPai.CategoriaPaiId != null), PaiJaEFilho);

            TipoCarteiraBL.ValidaTipoCarteira(entity.TipoCarteira);
            entity.Fail(base.All.Any(x => x.Id != entity.Id && x.Descricao.ToUpper() == entity.Descricao.ToUpper()),
                new Error("Descrição da categoria já utilizada anteriormente.", "descricao", base.All.FirstOrDefault(x => x.Id != entity.Id && x.Descricao.ToUpper() == entity.Descricao.ToUpper())?.Id.ToString()));
            entity.Fail(entity.Id == entity.CategoriaPaiId, CategoriaPropria);

            entity.Fail(base.All.Where(x => x.Id == entity.CategoriaPaiId).Any(x => x.TipoCarteira != entity.TipoCarteira), TipoCarteiraDiferente);
            base.ValidaModel(entity);
        }

        #endregion

        public static Error CategoriaPropria = new Error("Não é possível definir a própria categoria, como sua Categoria Superior.");
        public static Error TipoCarteiraDiferente = new Error("Não foi possível salvar este registro. O tipo da carteira deve ser igual ao da Categoria Superior.");
        public static Error ExclusaoInvalida = new Error("Não é possível excluir este registro, pois o mesmo possui filhos.");
        public static Error AlteracaoCategoriaSuperiorInvalida = new Error("Não é possível alterar a Categoria Superior desta Categoria, pois a mesma já possui filhos.");
        public static Error AlteracaoTipoInvalida = new Error("Não é possível alterar o Tipo desta Categoria, pois a mesma já possui filhos.");
        public static Error PaiJaEFilho = new Error("Não é possível definir como pai uma categoria que já seja filha.");
        public static Error AlterarTipoInvalidaFK = new Error("Não é possível alterar o Tipo de Carteira desta Categoria, pois a mesma possui relação com Conta(Pagar/Receber).");
    }
}