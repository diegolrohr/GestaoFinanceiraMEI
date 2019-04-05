using System;
using System.Linq;
using System.Web.Http;
using Fly01.Financeiro.BL;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Entities.Domains.Commons;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Diagnostics;
using System.Collections.Generic;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("fluxocaixarecalcularsaldos")]
    public class FluxoCaixaRecalcularSaldosController : ApiBaseController
    {
        #region Instruções Leia
        /// <summary>
        /// ATENÇÃO LEIA ATÉ O FINAL ANTES DE INICIAR O PROCEDIMENTO
        /// 
        /// https://azure.microsoft.com/pt-br/blog/exporting-data-from-sql-azure-importexport-wizard/
        /// 
        /// Sempre rode local, com backup de prod, teste a situação de cada cliente
        /// Trocar a string de conexão no web config e build
        /// Importante: se a plataforma tiver movimentações avulsas, não vinculadas a contas financeiras
        /// necessário revisar o algoritmo, atualmente, trata somente de baixas vinculadas as contas
        ///
        /// Passo 1 - "Excluir"(inativar) os registros existentes da tabela saldoHistorico e tabela de movimentações
        ///     executar os seguinte comandos no sql: 
        ///         ***IMPORTANTE*** informar a @plataforma e o @usuarioExclusao
        ///         
        ///         declare @plataforma varchar(100) = '05279777000150.fly01.com.br'
        ///         declare @usuarioExclusao varchar(100) = '5394881@totvs.com.br'
        ///         update movimentacao set ativo = 0, usuarioExclusao = @usuarioExclusao, dataExclusao = GETDATE()
        ///         where plataformaid = @plataforma and ativo = 1
        ///         update saldohistorico set ativo = 0, usuarioExclusao = @usuarioExclusao, dataExclusao = GETDATE()
        ///         where plataformaid = @plataforma and ativo = 1
        ///     
        ///     Se necessário desfazer a exclusão deste passo 1
        ///     executar os seguinte comandos no sql: 
        ///         ***IMPORTANTE*** informar a @plataforma e o @usuarioExclusao
        ///         
        ///         declare @plataforma varchar(100) = '05279777000150.fly01.com.br'
        ///         declare @usuarioExclusao varchar(100) = '5394881@totvs.com.br'
        ///         update movimentacao set ativo = 1, usuarioExclusao = null, dataExclusao = null
        ///         where plataformaid = @plataforma and ativo = 0 and usuarioExclusao = @usuarioExclusao
        ///         update saldohistorico set ativo = 1, usuarioExclusao = null, dataExclusao = null
        ///         where plataformaid = @plataforma and ativo = 0 and usuarioExclusao = @usuarioExclusao
        ///
        /// Posterior a execução do passo 1 no banco, realizar a chamada deste controller via postamn, para
        /// Importante***, especificar no header o AppUser no formato: 5394881@totvs.com.br, neste exemplo 5394881 é o número do ticket em que o
        /// cliente autorizou a manipulação dos dados em prod
        /// Cria as novas movimentações e exclui as antigas, baseadas nas baixas das contas financeiras, atualiza o valor pago e status das contas
        /// gerando novos registros no saldohistorico com o saldo do dia certo, porém o consolidado ainda errado.
        /// Por isso necessário rodar o script do fim da página, um cursor que atualiza o consolidado, baseado no saldo do dia
        /// </summary>
        #endregion
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            //TODO: logs
            var executionKey = "12CCF0B3-8A3E-42DA-8EDC-AAB171E07430.fly01.com.br";

            if (executionKey != ExecutionKey)
                return BadRequest("ExecutionKey Inválida! Rotina restrita a uso interno");

            try
            {
                Stopwatch sp = new Stopwatch();
                sp.Start();
                int countContaPagar = 0, countBaixaPagar = 0, countContaReceber = 0, countBaixaReceber = 0;
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    using (UnitOfWork unitOfWork2 = new UnitOfWork(ContextInitialize))
                    {
                        #region Recuperar todas as contas a Pagar com alguma baixa
                        foreach (var contaPagar in unitOfWork.ContaPagarBL.All.AsNoTracking().Where(x => x.StatusContaBancaria == StatusContaBancaria.Pago || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente))
                        {
                            countContaPagar++;
                            var contaPagarUpdate = unitOfWork2.ContaPagarBL.Find(contaPagar.Id);
                            contaPagarUpdate.ValorPrevisto = Math.Abs(contaPagarUpdate.ValorPrevisto);
                            contaPagarUpdate.ValorPago = 0.0;
                            unitOfWork2.ContaPagarBL.Update(contaPagarUpdate);

                            foreach (var baixaPagar in unitOfWork.ContaFinanceiraBaixaBL.All.AsNoTracking().Where(x => x.ContaFinanceiraId == contaPagar.Id))
                            {
                                countBaixaPagar++;
                                var baixaPagarDelete = unitOfWork2.ContaFinanceiraBaixaBL.Find(baixaPagar.Id);
                                unitOfWork2.ContaFinanceiraBaixaBL.DeleteWithoutRecalc(baixaPagarDelete);
                                unitOfWork2.ContaFinanceiraBaixaBL.Insert(new ContaFinanceiraBaixa()
                                {
                                    ContaBancariaId = baixaPagar.ContaBancariaId,
                                    ContaFinanceiraId = baixaPagar.ContaFinanceiraId,
                                    Data = baixaPagar.Data,
                                    Observacao = baixaPagar.Observacao,
                                    Valor = Math.Abs(baixaPagar.Valor)
                                });
                                await unitOfWork2.Save();
                            }
                        }
                        #endregion
                        #region Recuperar todas as contas a Receber com alguma baixa
                        foreach (var contaReceber in unitOfWork.ContaReceberBL.All.AsNoTracking().Where(x => x.StatusContaBancaria == StatusContaBancaria.Pago || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente))
                        {
                            countContaReceber++;
                            var contaReceberUpdate = unitOfWork2.ContaReceberBL.Find(contaReceber.Id);
                            contaReceberUpdate.ValorPrevisto = Math.Abs(contaReceberUpdate.ValorPrevisto);
                            contaReceberUpdate.ValorPago = 0.0;
                            unitOfWork2.ContaReceberBL.Update(contaReceberUpdate);

                            foreach (var baixaReceber in unitOfWork.ContaFinanceiraBaixaBL.All.AsNoTracking().Where(x => x.ContaFinanceiraId == contaReceber.Id))
                            {
                                countBaixaReceber++;
                                var baixaReceberDelete = unitOfWork2.ContaFinanceiraBaixaBL.Find(baixaReceber.Id);
                                unitOfWork2.ContaFinanceiraBaixaBL.DeleteWithoutRecalc(baixaReceberDelete);
                                unitOfWork2.ContaFinanceiraBaixaBL.Insert(new ContaFinanceiraBaixa()
                                {
                                    ContaBancariaId = baixaReceber.ContaBancariaId,
                                    ContaFinanceiraId = baixaReceber.ContaFinanceiraId,
                                    Data = baixaReceber.Data,
                                    Observacao = baixaReceber.Observacao,
                                    Valor = Math.Abs(baixaReceber.Valor)
                                });
                                await unitOfWork2.Save();
                            }
                        }
                        #endregion
                    }
                }

                sp.Stop();
                return Ok(new
                {
                    totalContasPagarProcessadas = countContaPagar,
                    totalBaixasPagarProcessadas = countBaixaPagar,
                    totalContasReceber = countContaReceber,
                    totalBaixasReceberProcessadas = countBaixaReceber,
                    elapsedMilliseconds = sp.ElapsedMilliseconds
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public string ExecutionKey
        {
            get
            {
                IEnumerable<string> values;
                if (Request.Headers.TryGetValues("ExecutionKey", out values))
                    return values.FirstOrDefault();

                throw new ArgumentException("ExecutionKey não informada.");
            }
        }

        #region Script cursor atualizar consolida, baseado no saldo do dia, se ele estiver correto
        /*
        declare @plataforma varchar(200)
        set @plataforma = '05279777000150.fly01.com.br'

        declare @id varchar(200)
        declare @data date
        declare @contaBancariaId varchar(200)
        declare @saldoDia float
        declare @saldoConsolidado float

        declare cur_temp cursor for
        select
            id
            , data
            , contaBancariaId
            , saldoDia
            , saldoConsolidado
        from
            saldoHistorico
        where
            plataformaId = @plataforma and ativo = 1
        order by contaBancariaId, data

        open cur_temp

        fetch next from cur_temp into @id, @data, @contaBancariaId, @saldoDia, @saldoConsolidado
        while (@@fetch_status = 0) --while eof
        begin

            declare @dataIni date
            select @dataIni = min(data) from saldoHistorico where plataformaId = @plataforma and ativo = 1 and contaBancariaId = @contaBancariaId


            declare @saldoAnterior float
            declare @saldoData date

            select
                top 1 @saldoAnterior = coalesce(saldoConsolidado, 0)
                --, @saldoData = data
            from

                saldoHistorico
            where plataformaId = @plataforma and ativo = 1 and contaBancariaId = @contaBancariaId and data<@data

            order by data desc

            if(@dataIni = @data)

            begin
                fetch next from cur_temp into @id, @data, @contaBancariaId, @saldoDia, @saldoConsolidado
            end
            else

            begin
                update saldoHistorico set saldoConsolidado = saldoDia + coalesce(@saldoAnterior, 0)
                where id = @id


                fetch next from cur_temp into @id, @data, @contaBancariaId, @saldoDia, @saldoConsolidado

            end
            --print 'conta: ' + @contaBancariaId + ' data: ' + cast(@data as varchar) + ' saldoAnterior ' + cast(@saldoAnterior as varchar)
        end

        close cur_temp
        DEALLOCATE cur_temp
        */
        #endregion
    }
}