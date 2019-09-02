using System;
using System.Collections.Generic;
using System.Linq;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.BL
{
    public class ReceitaPorCategoriaBL : EmpresaBase
    {
        protected ContaReceberBL ContaReceberBL;
        protected CategoriaBL CategoriaBL;

        public ReceitaPorCategoriaBL(ContaReceberBL contaReceberBL,
                                     CategoriaBL categoriaBL)
        {
            ContaReceberBL = contaReceberBL;
            CategoriaBL = categoriaBL;
        }

        public List<MovimentacaoFinanceiraPorCategoria> Get(DateTime dataInicial,
                                                  DateTime dataFinal,
                                                  bool somaRealizados = true,
                                                  bool somaPrevistos = false)
        {
            var contasReceber = ContaReceberBL
                                    .AllIncluding(c => c.Categoria)
                                    .Where(x => x.DataEmissao >= dataInicial &&
                                                x.DataEmissao <= dataFinal)
                                    .ToList();

            var receitasPorCategoria = contasReceber
                                        .Select(x => new
                                        {
                                            x.CategoriaId,
                                            x.Categoria,
                                            x.ValorPrevisto,
                                            x.ValorPago
                                        })
                                        .GroupBy(x => x.Categoria)
                                        .Select(g => new MovimentacaoFinanceiraPorCategoria()
                                        {
                                            Categoria = g.Key.Descricao,
                                            CategoriaId = g.Key.Id,
                                            CategoriaPaiId = g.Key.CategoriaPaiId,
                                            Previsto = g.Sum(s => s.ValorPrevisto - (s.ValorPago ?? 0)),
                                            Realizado = g.Sum(s => s.ValorPago ?? 0),
                                            Soma = g.Sum(s => s.ValorPrevisto),
                                            TipoCarteira = g.Key.TipoCarteira,
                                            TipoContaFinanceira = TipoContaFinanceira.ContaReceber,
                                        })
                                        .ToList();

            // Completa as categorias faltantes e sem movimentação
            var categoriasComReceita = contasReceber.Select(x => x.CategoriaId).Distinct();
            var categoriasSemReceita = CategoriaBL.All.Where(x => !categoriasComReceita.Contains(x.Id) && x.TipoCarteira == TipoCarteira.Receita);
            var receitasPorCategoriaZeradas = categoriasSemReceita
                                                .Select(x => new MovimentacaoFinanceiraPorCategoria()
                                                {
                                                    Categoria = x.Descricao,
                                                    CategoriaId = x.Id,
                                                    CategoriaPaiId = x.CategoriaPaiId,
                                                    Previsto = 0,
                                                    Realizado = 0,
                                                    Soma = 0,
                                                    TipoCarteira = x.TipoCarteira,
                                                    TipoContaFinanceira = TipoContaFinanceira.ContaReceber,
                                                })
                                                .ToList();
            receitasPorCategoria.AddRange(receitasPorCategoriaZeradas);

            // Ordena as categorias por pai e filho
            var pais = CategoriaBL
                        .All
                        .Where(e => e.CategoriaPaiId == null &&
                                    e.TipoCarteira == TipoCarteira.Receita)
                        .Select(x => new MovimentacaoFinanceiraPorCategoria()
                        {
                            Categoria = x.Descricao,
                            CategoriaId = x.Id,
                            CategoriaPaiId = x.CategoriaPaiId,
                            Previsto = contasReceber
                                    .Where(r => r.CategoriaId == x.Id ||
                                                r.Categoria.CategoriaPaiId == x.Id)
                                    .Sum(s => s.ValorPrevisto - (s.ValorPago ?? 0)),
                            Realizado = contasReceber
                                    .Where(r => r.CategoriaId == x.Id ||
                                                r.Categoria.CategoriaPaiId == x.Id)
                                    .Sum(s => s.ValorPago ?? 0),
                            Soma = contasReceber.Where(r => r.CategoriaId == x.Id || r.Categoria.CategoriaPaiId == x.Id).Sum(s => s.ValorPrevisto),
                            TipoCarteira = x.TipoCarteira,
                            TipoContaFinanceira = TipoContaFinanceira.ContaReceber
                        })
                        .OrderBy(x => x.Categoria)
                        .ToList();

            var listaOrdenada = new List<MovimentacaoFinanceiraPorCategoria>();

            foreach (var categoria in pais)
            {
                listaOrdenada.Add(categoria);

                foreach (var catFilho in receitasPorCategoria
                                            .Where(x => x.CategoriaPaiId == categoria.CategoriaId)
                                            .OrderBy(x => x.Categoria)
                                            .ToList())
                {
                    listaOrdenada.Add(catFilho);
                }
            }

            var jaOrdenados = listaOrdenada.Select(x => x.CategoriaId).ToArray();
            var semPaisNemFilhos = receitasPorCategoria.Where(x => !jaOrdenados.Contains(x.CategoriaId));
            listaOrdenada.AddRange(semPaisNemFilhos);

            try
            {
                return listaOrdenada;
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message);
            }
        }
    }
}