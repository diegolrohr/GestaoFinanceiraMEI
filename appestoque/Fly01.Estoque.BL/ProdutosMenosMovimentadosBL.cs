﻿using Fly01.Estoque.Domain.Entities;
using Fly01.Core.Domain;
using Fly01.Core.BL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fly01.Estoque.BL
{
    public class ProdutosMenosMovimentadosBL : PlataformaBaseBL<Produto>
    {
        protected MovimentoBL MovimentoBL;

        public ProdutosMenosMovimentadosBL(AppDataContextBase context,
                                           MovimentoBL movimentoBL) 
            : base(context)
        {
            MovimentoBL = movimentoBL;
        }

        public List<ProdutoMovimentado> Get(DateTime dataInicial, DateTime dataFinal)
        {
            var movimentos = MovimentoBL
                                .AllIncluding(x => x.Produto)
                                .Where(x => x.DataInclusao >= dataInicial &&
                                            x.DataInclusao <= dataFinal &&
                                            x.Produto.Ativo)
                                .ToList();

            var menosMovimentados = movimentos
                                    .GroupBy(x => x.Produto)
                                    .Select(x => new
                                    {
                                        x.Key.Id,
                                        x.Key.Descricao,
                                        x.Key.SaldoProduto,
                                        x.Key.PlataformaId,
                                        x.Key.UsuarioInclusao,
                                        x.Key.DataAlteracao,
                                        Movimentos = x.Count()
                                    })
                                    .OrderBy(o => o.Movimentos)
                                    .ThenByDescending(o => o.DataAlteracao)
                                    .ThenBy(o => o.Id)
                                    .Take(5)
                                    .Select(x => new ProdutoMovimentado
                                    {
                                        Id = x.Id,
                                        Descricao = x.Descricao,
                                        SaldoProduto = x.SaldoProduto,
                                        TotalMovimentos = x.Movimentos,
                                        PlataformaId = PlataformaUrl,
                                        UsuarioInclusao = AppUser
                                    })
                                    .ToList();

            return menosMovimentados;
        }
    }
}