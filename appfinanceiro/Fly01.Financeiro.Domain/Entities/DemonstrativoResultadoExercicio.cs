//using Fly01.Financeiro.Domain.Enums;
//using Fly01.Core.Domain;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Fly01.Financeiro.Domain.Entities
//{
//    [Serializable]
//    public class DemonstrativoResultadoExercicio : PlataformaBase
//    {
//        public DemonstrativoResultadoExercicio(List<ContaReceber> contasReceber, List<ContaPagar> contasPagar, List<Categoria> categoriasFinaceiras)
//        {
//            DespesasPrevistas = contasPagar.Sum(x => x.ValorPrevisto) * -1;
//            ReceitasPrevistas = contasReceber.Sum(x => x.ValorPrevisto);
//            TotalPrevisto = ReceitasPrevistas + DespesasPrevistas;

//            DespesasRealizadas = contasPagar.Sum(x => x.ValorPago) * -1;
//            ReceitasRealizadas = contasReceber.Sum(x => x.ValorPago);
//            TotalRealizado = ReceitasRealizadas + DespesasRealizadas;

//            DespesasTotais = DespesasPrevistas * -1;
//            ReceitasTotais = ReceitasPrevistas;

//            Total = TotalPrevisto;

//            MovimentacoesPorCategoria = new List<MovimentacaoPorCategoria>();

//            var receitasPorCategoria = contasReceber
//                                        .Select(x => new
//                                        {
//                                            x.CategoriaId,
//                                            x.Categoria,
//                                            x.ValorPrevisto,
//                                            x.ValorPago
//                                        })
//                                        .GroupBy(x => x.Categoria)
//                                        .Select(g => new MovimentacaoPorCategoria()
//                                        {
//                                            Categoria = g.Key.Descricao,
//                                            CategoriaId = g.Key.Id,
//                                            CategoriaPaiId = g.Key.CategoriaPaiId,
//                                            Previsto = g.Sum(s => s.ValorPrevisto),
//                                            Realizado = g.Sum(s => s.ValorPago),
//                                            Soma = g.Sum(s => s.ValorPago),
//                                            TipoCarteira = g.Key.TipoCarteira,
//                                            TipoContaFinanceira = TipoContaFinanceira.ContaReceber
//                                        })
//                                        .ToList();

//            MovimentacoesPorCategoria.AddRange(receitasPorCategoria);

//            var despesasPorCategoria = contasPagar
//                                        .Select(x => new
//                                        {
//                                            x.CategoriaId,
//                                            x.Categoria,
//                                            x.ValorPrevisto,
//                                            x.ValorPago
//                                        })
//                                        .GroupBy(x => x.Categoria)
//                                        .Select(g => new MovimentacaoPorCategoria()
//                                        {
//                                            Categoria = g.Key.Descricao,
//                                            CategoriaId = g.Key.Id,
//                                            CategoriaPaiId = g.Key.CategoriaPaiId,
//                                            Previsto = g.Sum(s => s.ValorPrevisto) * -1,
//                                            Realizado = g.Sum(s => s.ValorPago) * -1,
//                                            Soma = g.Sum(s => s.ValorPago) * -1,
//                                            TipoCarteira = g.Key.TipoCarteira,
//                                            TipoContaFinanceira = TipoContaFinanceira.ContaPagar
//                                        })
//                                        .ToList();

//            MovimentacoesPorCategoria.AddRange(despesasPorCategoria);

//            MovimentacoesPorCategoria = CompletaCategorias(movimentacoesPorCategoria: MovimentacoesPorCategoria,
//                                                           categoriasFinaceiras: categoriasFinaceiras);

//            MovimentacoesPorCategoria = SomaCategoriasSinteticas(movimentacoesPorCategoria: MovimentacoesPorCategoria);

//        }

//        public DemonstrativoResultadoExercicio() { }

//        public double ReceitasPrevistas { get; set; }
//        public double DespesasPrevistas { get; set; }
//        public double TotalPrevisto { get; set; }
//        public double? ReceitasRealizadas { get; set; }
//        public double? DespesasRealizadas { get; set; }
//        public double? TotalRealizado { get; set; }
//        public double? ReceitasTotais { get; set; }
//        public double? DespesasTotais { get; set; }
//        public double? Total { get; set; }

//        public List<MovimentacaoPorCategoria> MovimentacoesPorCategoria { get; set; }

//        private static List<MovimentacaoPorCategoria> CompletaCategorias(List<MovimentacaoPorCategoria> movimentacoesPorCategoria,
//                                                                         List<Categoria> categoriasFinaceiras)
//        {
//            foreach (var categoria in categoriasFinaceiras)
//            {
//                var movimentacao = movimentacoesPorCategoria.Where(x => x.CategoriaId == categoria.Id);
//                if (!movimentacao.Any())
//                {
//                    movimentacoesPorCategoria.Add(new MovimentacaoPorCategoria
//                    {
//                        Categoria = categoria.Descricao,
//                        CategoriaId = categoria.Id,
//                        CategoriaPaiId = categoria.CategoriaPaiId,
//                        TipoCarteira = categoria.TipoCarteira,
//                        Previsto = 0.0D,
//                        Realizado = 0.0D,
//                        Soma = 0.0D
//                    });
//                }
//            }

//            var pais = movimentacoesPorCategoria.Where(e => e.CategoriaPaiId == null)
//                                                .OrderBy(x => x.TipoCarteira)
//                                                .ThenBy(x => x.CategoriaPaiId);

//            IList<MovimentacaoPorCategoria> listResult = new List<MovimentacaoPorCategoria>();

//            foreach (var catPai in pais)
//            {
//                listResult.Add(catPai);

//                foreach (var catFilho in movimentacoesPorCategoria.Where(x => x.CategoriaPaiId == catPai.CategoriaId)
//                    .OrderBy(x => x.Categoria)
//                    .ToList())
//                {
//                    listResult.Add(catFilho);
//                }
//            }

//            return movimentacoesPorCategoria.ToList();
//        }

//        private static List<MovimentacaoPorCategoria> SomaCategoriasSinteticas(List<MovimentacaoPorCategoria> movimentacoesPorCategoria)
//        {
//            movimentacoesPorCategoria = movimentacoesPorCategoria.ToList();
//            return movimentacoesPorCategoria;
//        }
//    }
//}