﻿using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.Financeiro.BL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [ODataRoutePrefix("contareceber")]
    public class ContaReceberController : ApiPlataformaController<ContaReceber, ContaReceberBL>
    {
        public ContaReceberController()
        {
            MustProduceMessageServiceBus = true;
        }

        [EnableQuery(PageSize = 1000, MaxTop = 1000, MaxExpansionDepth = 10)]
        public override IHttpActionResult Get()
        {
            return Ok(All().AsQueryable());
        }

        [HttpDelete]
        public override Task<IHttpActionResult> Delete(Guid key)
        {
            var allUrlKeyValues = ControllerContext.Request.GetQueryNameValuePairs();
            bool excluirRecorrencias;
            bool.TryParse(allUrlKeyValues.LastOrDefault(x => x.Key.ToLower() == "excluirrecorrencias").Value, out excluirRecorrencias);

            if (excluirRecorrencias)
                return DeleteAll(key);

            return base.Delete(key);
        }

        public async Task<IHttpActionResult> DeleteAll(Guid key)
        {
            if (key == default(Guid))
                return BadRequest();

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                throw new BusinessException("Registro não encontrado ou já excluído");

            if (entity.RegistroFixo)
                throw new BusinessException("Registro não pode ser excluído (RegistroFixo)");

            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                var recorrencias= GetRecorrencias(entity, unitOfWork); 

                foreach (var child in recorrencias.OrderByDescending(x => x.Numero))
                {
                    Delete(child);
                    await unitOfWork.Save();
                    if (MustProduceMessageServiceBus)
                        Producer<ContaReceber>.Send(child.GetType().Name, AppUser, PlataformaUrl, child, RabbitConfig.EnHttpVerb.DELETE);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Delta<ContaReceber> model)
        {
            if (model == null || key == default(Guid) || key == null)
                return BadRequest(ModelState);

            var entity = Find(key);

            if (entity == null || !entity.Ativo)
                throw new BusinessException("Registro não encontrado ou já excluído");

            if (entity.RegistroFixo)
                throw new BusinessException("Registro não pode ser editado (RegistroFixo)");

            var numero = entity.Numero;
            ModelState.Clear();
            model.Patch(entity);
            Update(entity);
            entity.Numero = numero;

            Validate(entity);

            if (!ModelState.IsValid)
            {
                AddErrorModelState(ModelState);
            }
            else
            {
                var allUrlKeyValues = ControllerContext.Request.GetQueryNameValuePairs();
                bool editarRecorrencias;
                bool.TryParse(allUrlKeyValues.LastOrDefault(x => x.Key.ToLower() == "editarrecorrencias").Value, out editarRecorrencias);

                if (editarRecorrencias)
                    await PutAllRecorrenciasAsync(entity);
            }

            try
            {
                await UnitSave();

                if (MustProduceMessageServiceBus)
                    Producer<ContaReceber>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.EnHttpVerb.PUT);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(key))
                    return NotFound();
                else
                    throw;
            }

            return Ok();
        }

        public async Task<IHttpActionResult> PutAllRecorrenciasAsync(ContaReceber entity)
        {
            using (var unitOfWork = new UnitOfWork(ContextInitialize))
            {
                List<ContaReceber> recorrencias = GetRecorrencias(entity, unitOfWork);

                foreach (var item in recorrencias.OrderBy(x => x.Numero))
                {
                    if (item.Id == entity.Id)
                    {
                        item.DataVencimento = entity.DataVencimento;
                        item.DataEmissao = entity.DataEmissao;
                    }

                    item.ValorPrevisto = entity.ValorPrevisto;
                    item.Descricao = entity.Descricao;
                    item.PessoaId = entity.PessoaId;
                    item.CategoriaId = entity.CategoriaId;
                    item.FormaPagamentoId = entity.FormaPagamentoId;
                    item.CentroCustoId = entity.CentroCustoId;
                    item.Observacao = entity.Observacao;

                    Update(item);

                    await unitOfWork.Save();

                    if (MustProduceMessageServiceBus)
                        Producer<ContaReceber>.Send(item.GetType().Name, AppUser, PlataformaUrl, item, RabbitConfig.EnHttpVerb.PUT);
                }
            }
            return Ok();
        }

        private static List<ContaReceber> GetRecorrencias(ContaReceber entity, UnitOfWork unitOfWork)
        {
            var isParent = entity.ContaFinanceiraRepeticaoPaiId == null && entity.Repetir;
            List<ContaReceber> recorrencias;

            if (isParent)
                return recorrencias = unitOfWork.ContaReceberBL.All.Where(x => (x.ContaFinanceiraRepeticaoPaiId == entity.Id || x.Id == entity.Id) && x.StatusContaBancaria == StatusContaBancaria.EmAberto).ToList();
            else
                return recorrencias = unitOfWork.ContaReceberBL.All.Where(x => x.Numero >= entity.Numero && x.ContaFinanceiraRepeticaoPaiId == entity.ContaFinanceiraRepeticaoPaiId  && x.StatusContaBancaria == StatusContaBancaria.EmAberto).ToList();
        }
    }
}