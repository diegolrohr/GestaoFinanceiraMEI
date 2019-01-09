﻿using System.Web.Http;
using Fly01.Faturamento.BL;
using System;
using System.Linq;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Data.Entity;

namespace Fly01.Faturamento.API.Controllers.Api
{
    [RoutePrefix("produtoservico")]
    public class ProdutoServicoController : ApiBaseController
    {
        public IHttpActionResult Get(string filtro = "")
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var produtos = unitOfWork.ProdutoBL.All;
                var servicos = unitOfWork.ServicoBL.All;

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    produtos = produtos.Where(x =>
                        (
                              (x.Descricao.ToUpper().Contains(filtro.ToUpper())) ||
                              (x.CodigoProduto.ToUpper().Contains(filtro.ToUpper())) ||
                              (x.CodigoBarras.ToUpper().Contains(filtro.ToUpper()))
                          ) &&
                          (
                              (x.ObjetoDeManutencao == ObjetoDeManutencao.Nao)
                          )
                      );

                    servicos = servicos.Where(x =>
                          (
                              (x.Descricao.ToUpper().Contains(filtro.ToUpper())) ||
                              (x.CodigoServico.ToUpper().Contains(filtro.ToUpper()))
                          )
                      );
                }

                var result = produtos.OrderBy(x => x.Descricao).Take(20)
                      .Select(y => new ProdutoServicoVM()
                      {
                          TipoItem = TipoItem.Produto,
                          Id = y.Id,
                          Descricao = y.Descricao,
                          Codigo = y.CodigoProduto,
                          ValorCusto = y.ValorCusto,
                          ValorVenda = y.ValorVenda
                      });

                var resultServicos = servicos.OrderBy(x => x.Descricao)
                      .Take(20)
                      .Select(y => new ProdutoServicoVM()
                      {
                          TipoItem = TipoItem.Servico,
                          Id = y.Id,
                          Descricao = y.Descricao,
                          Codigo = y.CodigoServico,
                          ValorCusto = y.ValorServico,
                          ValorVenda = y.ValorServico
                      });

                result = result.Union(resultServicos).OrderBy(x => x.Descricao);

                return Ok(
                    new
                    {
                        value = result.ToList()
                    }
                );
            }
        }
    }
}