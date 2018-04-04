using Fly01.Financeiro.Domain.Entities;
using Fly01.Core.Api.Domain;
using Fly01.Core.ValueObjects;
using System;
using System.Collections.Generic;

namespace Fly01.Financeiro.BL
{
    public class MovimentacaoPorCategoriaBL : PlataformaBase
    {
        protected ReceitaPorCategoriaBL ReceitaPorCategoriaBL;
        protected DespesaPorCategoriaBL DespesaPorCategoriaBL;

        public MovimentacaoPorCategoriaBL(ReceitaPorCategoriaBL receitaPorCategoriaBL,
                                          DespesaPorCategoriaBL despesaPorCategoriaBL)
        {
            ReceitaPorCategoriaBL = receitaPorCategoriaBL;
            DespesaPorCategoriaBL = despesaPorCategoriaBL;
        }

        public List<MovimentacaoPorCategoria> Get(DateTime dataInicial,
                                                  DateTime dataFinal,
                                                  bool somaRealizados = true,
                                                  bool somaPrevistos = false)
        {
            var receitas = ReceitaPorCategoriaBL.Get(dataInicial, dataFinal, somaRealizados, somaPrevistos);
            var despesas = DespesaPorCategoriaBL.Get(dataInicial, dataFinal, somaRealizados, somaPrevistos);
            var movimentacoes = new List<MovimentacaoPorCategoria>();

            movimentacoes.AddRange(receitas);
            movimentacoes.AddRange(despesas);

            try
            {
                return movimentacoes;
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message);
            }
        }
    }
}