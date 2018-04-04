using System;
using System.Linq;
using Fly01.Core.Api.BL;
using Fly01.Core.Notifications;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Financeiro.Domain.Entities;

namespace Fly01.Financeiro.BL
{
    public class FeriadoBL : PlataformaBaseBL<Feriado>
    {
        public FeriadoBL(AppDataContext context) : base(context)
        { }

        #region Validacoes

        /// <summary>
        /// Método responsavel em realizar as validações da entidade
        /// </summary>
        /// <param name="entity"></param>
        public override void ValidaModel(Feriado entity)
        {
            entity.Fail(ExisteDescricao(entity), new Error(string.Format("O Feriado com o descrição '{0}' já existe. Informe outra descrição.", entity.Descricao)));
            entity.Fail(ExisteFeriadoMesmaData(entity), new Error("As Datas informadas já existem cadastradas. Informe outra data."));

            entity.Recorrente = entity.Ano == 0 ? true : false;
            base.ValidaModel(entity);
        }

        /// <summary>
        /// Verifica se já existe um feriado cadastrado no mesmo dia, mes e ano,
        /// ou se existe um feriado no mesmo dia e mes e que ele seja recorrente.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool ExisteFeriadoMesmaData(Feriado entity)
        {
            return base.All.Any(x => x.Dia == entity.Dia && x.Mes == entity.Mes && (x.Ano == entity.Ano || x.Recorrente == true) && x.Id != entity.Id);
        }

        /// <summary>
        /// Verifica se a descrição do Feriado informado já existe 
        /// </summary>
        /// <param name="feriado"></param>
        /// <returns></returns>
        private bool ExisteDescricao(Feriado entity)
        {
            return base.All.Any(x => x.Descricao.ToUpper() == entity.Descricao.ToUpper() && x.Id != entity.Id);
        }

        #endregion

        /// <summary>
        /// Verifica se a data informada é um feriado pré-cadastrado na base
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool Feriado(DateTime date)
        {
            return base.All.Where(x => x.Dia == date.Day
                                    && x.Mes == date.Month
                                    && (x.Ano == date.Year || x.Recorrente == true)).ToList().Count > 0 ? true : false;
        }
    }
}
