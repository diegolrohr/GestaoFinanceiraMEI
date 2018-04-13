using Fly01.Financeiro.Domain.Entities;
using Fly01.Financeiro.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Fly01.Core.Entities.Domains;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class DespesaPorCategoriaBL : PlataformaBase
    {
        protected ContaPagarBL ContaPagarBL;
        protected CategoriaBL CategoriaBL;

        public DespesaPorCategoriaBL(ContaPagarBL contaPagarBL,
                                     CategoriaBL categoriaBL)
        {
            ContaPagarBL = contaPagarBL;
            CategoriaBL = categoriaBL;
        }

        public List<MovimentacaoPorCategoria> Get(DateTime dataInicial,
                                                  DateTime dataFinal,
                                                  bool somaRealizados = true,
                                                  bool somaPrevistos = false)
        {
            var contasPagar = ContaPagarBL
                                    .AllIncluding(c => c.Categoria)
                                    .Where(x => x.DataVencimento >= dataInicial &&
                                                  x.DataVencimento <= dataFinal)
                                    .ToList();

            var despesasPorCategoria = contasPagar
                                        .Select(x => new
                                        {
                                            x.CategoriaId,
                                            x.Categoria,
                                            x.ValorPrevisto,
                                            x.ValorPago
                                        })
                                        .GroupBy(x => x.Categoria)
                                        .Select(g => new MovimentacaoPorCategoria()
                                        {
                                            Categoria = g.Key.Descricao,
                                            CategoriaId = g.Key.Id,
                                            CategoriaPaiId = g.Key.CategoriaPaiId,
                                            Previsto = g.Sum(s => s.ValorPrevisto - (s.ValorPago ?? 0)) * -1,
                                            Realizado = g.Sum(s => s.ValorPago) * -1,
                                            Soma = g.Sum(s => somaRealizados ? s.ValorPago : s.ValorPrevisto - s.ValorPago) * -1,
                                            TipoCarteira = g.Key.TipoCarteira,
                                            TipoContaFinanceira = TipoContaFinanceira.ContaPagar,
                                        })
                                        .ToList();

            // Completa as categorias faltantes e sem movimentação
            var categoriasComDespesa = contasPagar.Select(x => x.CategoriaId).Distinct();
            var categoriasSemDespesa = CategoriaBL.All.Where(x => !categoriasComDespesa.Contains(x.Id) && x.TipoCarteira == TipoCarteira.Despesa);
            var despesasPorCategoriaZeradas = categoriasSemDespesa
                                                .Select(x => new MovimentacaoPorCategoria()
                                                {
                                                    Categoria = x.Descricao,
                                                    CategoriaId = x.Id,
                                                    CategoriaPaiId = x.CategoriaPaiId,
                                                    Previsto = 0,
                                                    Realizado = 0,
                                                    Soma = 0,
                                                    TipoCarteira = x.TipoCarteira,
                                                    TipoContaFinanceira = TipoContaFinanceira.ContaPagar,
                                                })
                                                .ToList();
            despesasPorCategoria.AddRange(despesasPorCategoriaZeradas);

            // Ordena as categorias por pai e filho
            var pais = CategoriaBL
                        .All
                        .Where(e => e.CategoriaPaiId == null &&
                                    e.TipoCarteira == TipoCarteira.Despesa)
                        .Select(x => new MovimentacaoPorCategoria()
                        {
                            Categoria = x.Descricao,
                            CategoriaId = x.Id,
                            CategoriaPaiId = x.CategoriaPaiId,
                            Previsto = contasPagar
                                    .Where(r => r.CategoriaId == x.Id ||
                                                r.Categoria.CategoriaPaiId == x.Id)
                                    .Sum(s => s.ValorPrevisto - (s.ValorPago ?? 0)) * -1,
                            Realizado = contasPagar
                                    .Where(r => r.CategoriaId == x.Id ||
                                                r.Categoria.CategoriaPaiId == x.Id)
                                    .Sum(s => s.ValorPago) * -1,
                            Soma = contasPagar.Where(r => r.CategoriaId == x.Id || r.Categoria.CategoriaPaiId == x.Id).Sum(s => somaRealizados ? s.ValorPago : s.ValorPrevisto - (s.ValorPago ?? 0)) * -1,
                            TipoCarteira = x.TipoCarteira,
                            TipoContaFinanceira = TipoContaFinanceira.ContaPagar
                        })
                        .OrderBy(x => x.Categoria)
                        .ToList();

            var listaOrdenada = new List<MovimentacaoPorCategoria>();

            foreach (var categoria in pais)
            {
                listaOrdenada.Add(categoria);

                foreach (var catFilho in despesasPorCategoria
                                            .Where(x => x.CategoriaPaiId == categoria.CategoriaId)
                                            .OrderBy(x => x.Categoria)
                                            .ToList())
                {
                    listaOrdenada.Add(catFilho);
                }
            }

            var jaOrdenados = listaOrdenada.Select(x => x.CategoriaId).ToArray();
            var semPaisNemFilhos = despesasPorCategoria.Where(x => !jaOrdenados.Contains(x.CategoriaId));
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