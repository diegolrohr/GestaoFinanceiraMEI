using Fly01.Core.Entities.Domains;
using System;
using System.Collections.Generic;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
{
    public class MovimentacaoPorCategoriaBL : EmpresaBase
    {
        protected ReceitaPorCategoriaBL ReceitaPorCategoriaBL;
        protected DespesaPorCategoriaBL DespesaPorCategoriaBL;

        public MovimentacaoPorCategoriaBL(ReceitaPorCategoriaBL receitaPorCategoriaBL,
                                          DespesaPorCategoriaBL despesaPorCategoriaBL)
        {
            ReceitaPorCategoriaBL = receitaPorCategoriaBL;
            DespesaPorCategoriaBL = despesaPorCategoriaBL;
        }

        public List<MovimentacaoFinanceiraPorCategoria> Get(DateTime dataInicial,
                                                  DateTime dataFinal,
                                                  bool somaRealizados = true,
                                                  bool somaPrevistos = false)
        {
            var receitas = ReceitaPorCategoriaBL.Get(dataInicial, dataFinal, somaRealizados, somaPrevistos);
            var despesas = DespesaPorCategoriaBL.Get(dataInicial, dataFinal, somaRealizados, somaPrevistos);
            var movimentacoes = new List<MovimentacaoFinanceiraPorCategoria>();

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