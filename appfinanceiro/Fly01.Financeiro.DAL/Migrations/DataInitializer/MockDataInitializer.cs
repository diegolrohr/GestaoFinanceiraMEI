using System;
using System.Linq;
using Fly01.Financeiro.DAL.Migrations.DataInitializer.Contract;
using System.Collections.Generic;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Financeiro.Domain.Entities;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.DAL.Migrations.DataInitializer
{
    public class MockDataInitializer : IDataInitializer
    {
        private const string plataformaId = "lojarodrigo.fly01dev.com.br";
        private const string usuarioSeed = "Mock";
        private const string numbers = "0123456789";
        private Random rdn = new Random();
        private const int maxRecords = 50;
        private Banco[] bancos;

        public void Initialize(AppDataContext context)
        {
            AddContasBancarias(context);
            //AddCategoriasFinanceiras(context);
            AddCondicoesParcelamento(context);
            AddContasFinanceiras(context);
            AddMovimentacoes(context);
        }

        #region Data Mock

        public void AddContasBancarias(AppDataContext context)
        {
            if (!context.ContasBancarias.Any())
            {
                bancos = context.Bancos.Where(x => x.Ativo).ToArray();
                var contasBancarias = Enumerable.Range(1, maxRecords).Select((index, x) => new ContaBancaria()
                {
                    Id = Guid.NewGuid(),
                    NomeConta = $"Conta Bancária {index}",
                    Agencia = new string(Enumerable.Repeat(numbers, 6).Select(s => s[rdn.Next(s.Length)]).ToArray()),
                    Conta = new string(Enumerable.Repeat(numbers, 6).Select(s => s[rdn.Next(s.Length)]).ToArray()),
                    DigitoConta = "1",
                    DigitoAgencia = "1",
                    BancoId = bancos[rdn.Next(0, bancos.Count() - 1)].Id,
                    PlataformaId = plataformaId,
                    DataInclusao = DateTime.Now,
                    UsuarioInclusao = usuarioSeed,
                    Ativo = true
                });

                context.ContasBancarias.AddRange(contasBancarias);
                context.SaveChanges();
            }

            if (!context.SaldosHistorico.Any())
            {
                var contasBancarias = context.ContasBancarias.Where(x => x.PlataformaId == plataformaId && x.Ativo);
                var saldosIniciais = new List<SaldoHistorico>();
                foreach (var conta in contasBancarias)
                {
                    saldosIniciais.Add(new SaldoHistorico()
                    {
                        Id = Guid.NewGuid(),
                        Data = DateTime.Now,
                        SaldoConsolidado = 0,
                        SaldoDia = 0,
                        TotalPagamentos = 0,
                        TotalRecebimentos = 0,
                        ContaBancariaId = conta.Id,
                        PlataformaId = plataformaId,
                        DataInclusao = DateTime.Now,
                        UsuarioInclusao = usuarioSeed,
                        Ativo = true
                    });
                }

                context.SaldosHistorico.AddRange(saldosIniciais);
                context.SaveChanges();
            }
        }

        public void AddMovimentacoes(AppDataContext context)
        {
            var categoriaReceber = context.Categorias
                                        .FirstOrDefault(x => x.TipoCarteira == TipoCarteira.Receita/* && x.Classe == CategoriaFinanceiraClasse.Analitico*/);
            var categoriaPagar = context.Categorias
                                            .FirstOrDefault(x => x.TipoCarteira == TipoCarteira.Despesa /*&& x.Classe == CategoriaFinanceiraClasse.Analitico*/);

            var contaBancariaOrigem = context.ConciliacoesBancarias.FirstOrDefault()?.Id;
            var contaBancariaDestino = context.ConciliacoesBancarias.FirstOrDefault()?.Id;

            var movimentacoes = new List<Movimentacao>
            {
                new Movimentacao()
                {
                    Id = Guid.NewGuid(),
                    Data = DateTime.Now,
                    Valor = -15.0,
                    CategoriaId = categoriaPagar?.Id,
                    ContaBancariaOrigemId = contaBancariaOrigem,
                    ContaBancariaDestinoId = null,
                    ContaFinanceiraId = null,
                    Descricao = categoriaPagar?.Descricao,
                    PlataformaId = plataformaId,
                    UsuarioInclusao = usuarioSeed,
                    DataInclusao = DateTime.Now,
                    Ativo = true
                },
                new Movimentacao()
                {
                    Id = Guid.NewGuid(),
                    Data = DateTime.Now,
                    Valor = 15.0,
                    CategoriaId = categoriaReceber?.Id,
                    ContaBancariaOrigemId = null,
                    ContaBancariaDestinoId = contaBancariaDestino,
                    ContaFinanceiraId = null,
                    Descricao = categoriaReceber?.Descricao,
                    PlataformaId = plataformaId,
                    UsuarioInclusao = usuarioSeed,
                    DataInclusao = DateTime.Now,
                    Ativo = true
                }
            };

            context.Movimentacao.AddRange(movimentacoes);
            context.SaveChanges();
        }

        //public void AddCategoriasFinanceiras(AppDataContext context)
        //{
        //    if (!context.Categorias.Any())
        //    {
        //        var categoriaFinanceiraId = Enumerable.Range(0, 10).Select(item => Guid.NewGuid()).ToArray();

        //        var categoriasFinanceirasReceitas = new List<Categoria>()
        //        {
        //            new Categoria
        //            {
        //                Id = categoriaFinanceiraId[0],
        //                Descricao = "RECEITAS",
        //                //Classe = CategoriaFinanceiraClasse.Sintetico,
        //                TipoCarteira = TipoCarteira.Receita,
        //                //Codigo = "01",
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = categoriaFinanceiraId[1],
        //                Descricao = "RECEITAS MEDICAS",
        //                Classe = CategoriaFinanceiraClasse.Sintetico,
        //                TipoCarteira = TipoCarteira.Receita,
        //                Codigo = "0101",
        //                CategoriaPaiId = categoriaFinanceiraId[0],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = Guid.NewGuid(),
        //                Descricao = "CONSULTAS",
        //                Classe = CategoriaFinanceiraClasse.Analitico,
        //                TipoCarteira = TipoCarteira.Receita,
        //                Codigo = "010101",
        //                CategoriaPaiId = categoriaFinanceiraId[1],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = Guid.NewGuid(),
        //                Descricao = "EXAMES",
        //                Classe = CategoriaFinanceiraClasse.Analitico,
        //                TipoCarteira = TipoCarteira.Receita,
        //                Codigo = "010102",
        //                CategoriaPaiId = categoriaFinanceiraId[1],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = Guid.NewGuid(),
        //                Descricao = "OUTRAS RECEITAS MEDICAS",
        //                Classe = CategoriaFinanceiraClasse.Analitico,
        //                TipoCarteira = TipoCarteira.Receita,
        //                Codigo = "010103",
        //                CategoriaPaiId = categoriaFinanceiraId[1],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = Guid.NewGuid(),
        //                Descricao = "JUROS APLICACOES",
        //                Classe = CategoriaFinanceiraClasse.Analitico,
        //                TipoCarteira = TipoCarteira.Receita,
        //                Codigo = "010104",
        //                CategoriaPaiId = categoriaFinanceiraId[1],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = Guid.NewGuid(),
        //                Descricao = "RENDIMENTOS",
        //                Classe = CategoriaFinanceiraClasse.Analitico,
        //                TipoCarteira = TipoCarteira.Receita,
        //                Codigo = "010105",
        //                CategoriaPaiId = categoriaFinanceiraId[1],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //        };

        //        var categoriasFinanceirasDespesas = new List<CategoriaFinanceira>()
        //        {
        //            new CategoriaFinanceira
        //            {
        //                Id = categoriaFinanceiraId[3],
        //                Descricao = "DESPESAS",
        //                Classe = CategoriaFinanceiraClasse.Sintetico,
        //                TipoCarteira = TipoCarteira.Despesa,
        //                Codigo = "02",
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = categoriaFinanceiraId[4],
        //                Descricao = "ORDENADOS E SALARIOS",
        //                Classe = CategoriaFinanceiraClasse.Sintetico,
        //                TipoCarteira = TipoCarteira.Despesa,
        //                Codigo = "0201",
        //                CategoriaPaiId = categoriaFinanceiraId[3],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = Guid.NewGuid(),
        //                Descricao = "ESTAGIARIOS",
        //                Classe = CategoriaFinanceiraClasse.Analitico,
        //                TipoCarteira = TipoCarteira.Despesa,
        //                Codigo = "020101",
        //                CategoriaPaiId = categoriaFinanceiraId[4],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = Guid.NewGuid(),
        //                Descricao = "SALARIOS",
        //                Classe = CategoriaFinanceiraClasse.Analitico,
        //                TipoCarteira = TipoCarteira.Despesa,
        //                Codigo = "020102",
        //                CategoriaPaiId = categoriaFinanceiraId[4],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = Guid.NewGuid(),
        //                Descricao = "ADIANTAMENTO DE SALARIOS",
        //                Classe = CategoriaFinanceiraClasse.Analitico,
        //                TipoCarteira = TipoCarteira.Despesa,
        //                Codigo = "020103",
        //                CategoriaPaiId = categoriaFinanceiraId[4],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = Guid.NewGuid(),
        //                Descricao = "DECIMO TERCEIRO SALARIO",
        //                Classe = CategoriaFinanceiraClasse.Analitico,
        //                TipoCarteira = TipoCarteira.Despesa,
        //                Codigo = "020104",
        //                CategoriaPaiId = categoriaFinanceiraId[4],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //            new CategoriaFinanceira
        //            {
        //                Id = Guid.NewGuid(),
        //                Descricao = "REPASSES MEDICOS",
        //                Classe = CategoriaFinanceiraClasse.Analitico,
        //                TipoCarteira = TipoCarteira.Despesa,
        //                Codigo = "020105",
        //                CategoriaPaiId = categoriaFinanceiraId[4],
        //                PlataformaId = plataformaId,
        //                UsuarioInclusao = usuarioSeed,
        //                DataInclusao = DateTime.Now,
        //                Ativo = true
        //            },
        //        };

        //        var categoriasFinanceiras = categoriasFinanceirasDespesas.Union(categoriasFinanceirasReceitas);

        //        context.Categorias.AddRange(categoriasFinanceiras);
        //        context.SaveChanges();
        //    }
        //}

        public void AddCondicoesParcelamento(AppDataContext context)
        {
            if (!context.CondicoesParcelamento.Any())
            {
                var condicoesParcelamentoQtdParcelas = Enumerable.Range(1, 12).Select((index, x) => new CondicaoParcelamento()
                {
                    Id = Guid.NewGuid(),
                    Descricao = string.Format("Condição QtdParcelas = {0}", index),
                    QtdParcelas = index,
                    CondicoesParcelamento = string.Empty,
                    PlataformaId = plataformaId,
                    DataInclusao = DateTime.Now,
                    UsuarioInclusao = usuarioSeed,
                    Ativo = true
                });

                var condicoesParcelamentoAPrazo = Enumerable.Range(1, 8).Select((index, x) => new CondicaoParcelamento()
                {
                    Id = Guid.NewGuid(),
                    Descricao = string.Concat("Condição Parc. = ", string.Join(",", Enumerable.Range(1, index).Select((index1, x1) => index1 * 30))),
                    CondicoesParcelamento = string.Join(",", Enumerable.Range(1, index).Select((index1, x1) => index1 * 30)),
                    PlataformaId = plataformaId,
                    DataInclusao = DateTime.Now,
                    UsuarioInclusao = usuarioSeed,
                    Ativo = true
                });

                var condicoesParcelamento = condicoesParcelamentoQtdParcelas.Union(condicoesParcelamentoAPrazo);

                context.CondicoesParcelamento.AddRange(condicoesParcelamento);
                context.SaveChanges();
            }
        }

        public void AddContasFinanceiras(AppDataContext context)
        {
            var condicaoParcelamentoQtdParcelas = context.CondicoesParcelamento.Where(x => x.PlataformaId == plataformaId && x.QtdParcelas.HasValue && x.Ativo).ToArray();
            var categoriasFinanceirasTC1 = context.Categorias.Where(x => x.PlataformaId == plataformaId &&
                                                                                    //x.Classe == CategoriaFinanceiraClasse.Analitico &&
                                                                                    x.Ativo &&
                                                                                    x.TipoCarteira == TipoCarteira.Receita).ToArray();
            var categoriasFinanceirasTC2 = context.Categorias.Where(x => x.PlataformaId == plataformaId &&
                                                                                    //x.Classe == CategoriaFinanceiraClasse.Analitico &&
                                                                                    x.Ativo &&
                                                                                    x.TipoCarteira == TipoCarteira.Despesa).ToArray();
            var pessoas = context.Pessoas.Where(x => x.PlataformaId == plataformaId && x.Ativo).ToArray();
            var formaPagamentoId = Guid.NewGuid();
            if (!context.FormasPagamento.Any())
            {
                context.FormasPagamento.Add(new FormaPagamento { Id = formaPagamentoId, Ativo = true, Descricao = "Dinheiro", TipoFormaPagamento = TipoFormaPagamento.Dinheiro, PlataformaId = plataformaId, UsuarioInclusao = "SEED", DataInclusao = DateTime.Now });
                context.SaveChanges();
            }
            var formasPagamento = context.FormasPagamento.Where(x => x.PlataformaId == plataformaId && x.Ativo).ToArray();

            if (!context.ContasPagar.Any())
            {
                var contasPagar = Enumerable.Range(1, maxRecords).Select((index, x) => new ContaPagar()
                {
                    Id = Guid.NewGuid(),
                    ValorPrevisto = rdn.Next(1, 10) * 100,
                    DataEmissao = DateTime.Now.Date,
                    DataVencimento = DateTime.Now.AddDays(index).Date,
                    Descricao = string.Format("Conta Pagar Mock {0}", index),
                    Observacao = string.Format("Conta Pagar Observação Mock {0}", index),
                    FormaPagamentoId = context.FormasPagamento.Any() ? formasPagamento[rdn.Next(0, formasPagamento.Count() - 1)].Id : formaPagamentoId,
                    StatusContaBancaria = StatusContaBancaria.EmAberto,
                    CategoriaId = categoriasFinanceirasTC2[rdn.Next(0, categoriasFinanceirasTC2.Count() - 1)].Id,
                    CondicaoParcelamentoId = condicaoParcelamentoQtdParcelas[rdn.Next(0, condicaoParcelamentoQtdParcelas.Count() - 1)].Id,
                    PessoaId = pessoas[rdn.Next(0, pessoas.Count() - 1)].Id,
                    Repetir = false,
                    PlataformaId = plataformaId,
                    DataInclusao = DateTime.Now,
                    UsuarioInclusao = usuarioSeed,
                    Ativo = true
                });

                context.ContasPagar.AddRange(contasPagar);
                context.SaveChanges();
            }

            if (!context.ContasReceber.Any())
            {
                var contasReceber = Enumerable.Range(1, maxRecords).Select((index, x) => new ContaReceber()
                {
                    Id = Guid.NewGuid(),
                    ValorPrevisto = rdn.Next(1, 10) * 100,
                    DataEmissao = DateTime.Now.Date,
                    DataVencimento = DateTime.Now.AddDays(index).Date,
                    Descricao = string.Format("Conta Receber Mock {0}", index),
                    Observacao = string.Format("Conta Receber Observação Mock {0}", index),
                    FormaPagamentoId = context.FormasPagamento.Any() ? formasPagamento[rdn.Next(0, formasPagamento.Count() - 1)].Id : formaPagamentoId,
                    //FormaPagamentoId = formasPagamento[rdn.Next(0, formasPagamento.Count() - 1)].Id,
                    StatusContaBancaria = StatusContaBancaria.EmAberto,
                    CategoriaId = categoriasFinanceirasTC1[rdn.Next(0, categoriasFinanceirasTC1.Count() - 1)].Id,
                    CondicaoParcelamentoId = condicaoParcelamentoQtdParcelas[rdn.Next(0, condicaoParcelamentoQtdParcelas.Count() - 1)].Id,
                    PessoaId = pessoas[rdn.Next(0, pessoas.Count() - 1)].Id,
                    Repetir = false,
                    PlataformaId = plataformaId,
                    DataInclusao = DateTime.Now,
                    UsuarioInclusao = usuarioSeed,
                    Ativo = true
                });

                context.ContasReceber.AddRange(contasReceber);
                context.SaveChanges();
            }
        }
        #endregion
    }
}
