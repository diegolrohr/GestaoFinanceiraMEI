using System;
using System.Linq;
using System.Web.Http;
using Fly01.Financeiro.BL;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.BL;
using Fly01.Core.Notifications;
using System.Threading.Tasks;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("fluxocaixarecalcularsaldos")]
    public class FluxoCaixaRecalcularSaldosController : ApiBaseController
    {
        /// <summary>
        /// ATENÇÃO LEIA ATÉ O FINAL ANTES DE INICIAR O PROCEDIMENTO
        /// 
        /// Importante, especificar no header o AppUser no formato: 5394881@totvs.com.br, neste exemplo 5394881 é o número do ticket em que o
        /// cliente autorizou a manipulação dos dados em prod
        /// Se rodar local, trocar a string de conexão no web config e build
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
        /// Posterior a execução do passo 1 no banco, realizar a chamada deste controller via postamn, para realizar o recálculo
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            //TODO: Diego Segurança, logs
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    int countContaPagar = 0, countBaixaPagar = 0, countContaReceber = 0, countBaixaReceber = 0;

                    #region Recuperar todas as contas a Pagar com alguma baixa
                    foreach (var contaPagar in unitOfWork.ContaPagarBL.All.Where(x => x.StatusContaBancaria == StatusContaBancaria.Pago || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente).OrderBy(x => x.DataVencimento))
                    {
                        countContaPagar++;
                        contaPagar.ValorPrevisto = Math.Abs(contaPagar.ValorPrevisto);
                        contaPagar.ValorPago = 0.0;
                        unitOfWork.ContaPagarBL.Update(contaPagar);

                        foreach (var baixaPagar in unitOfWork.ContaFinanceiraBaixaBL.All.Where(x => x.ContaFinanceiraId == contaPagar.Id).OrderBy(x => x.Data))
                        {
                            countBaixaPagar++;
                            unitOfWork.ContaFinanceiraBaixaBL.DeleteWithoutRecalc(baixaPagar);
                            //verificar se no insert o .all já vai considerar o valor pago 0
                            unitOfWork.ContaFinanceiraBaixaBL.Insert(new ContaFinanceiraBaixa()
                            {
                                ContaBancariaId = baixaPagar.ContaBancariaId,
                                ContaFinanceiraId = baixaPagar.ContaFinanceiraId,
                                Data = baixaPagar.Data,
                                Observacao = baixaPagar.Observacao,
                                Valor = Math.Abs(baixaPagar.Valor)
                            });
                        }
                    }
                    #endregion

                    #region Recuperar todas as contas a Receber com alguma baixa
                    foreach (var contaReceber in unitOfWork.ContaReceberBL.All.Where(x => x.StatusContaBancaria == StatusContaBancaria.Pago || x.StatusContaBancaria == StatusContaBancaria.BaixadoParcialmente).OrderBy(x => x.DataVencimento))
                    {
                        countContaReceber++;
                        contaReceber.ValorPrevisto = Math.Abs(contaReceber.ValorPrevisto);
                        contaReceber.ValorPago = 0.0;
                        unitOfWork.ContaReceberBL.Update(contaReceber);

                        foreach (var baixaPagar in unitOfWork.ContaFinanceiraBaixaBL.All.Where(x => x.ContaFinanceiraId == contaReceber.Id).OrderBy(x => x.Data))
                        {
                            countBaixaReceber++;
                            unitOfWork.ContaFinanceiraBaixaBL.DeleteWithoutRecalc(baixaPagar);
                            //verificar se no insert o .all já vai considerar o valor pago 0
                            unitOfWork.ContaFinanceiraBaixaBL.Insert(new ContaFinanceiraBaixa()
                            {
                                ContaBancariaId = baixaPagar.ContaBancariaId,
                                ContaFinanceiraId = baixaPagar.ContaFinanceiraId,
                                Data = baixaPagar.Data,
                                Observacao = baixaPagar.Observacao,
                                Valor = Math.Abs(baixaPagar.Valor)
                            });
                        }
                    }
                    #endregion

                    await unitOfWork.Save();

                    return Ok(new
                    {
                        totalContasPagarProcessadas = countContaPagar,
                        totalBaixasPagarProcessadas = countBaixaPagar,
                        totalContasReceber = countContaReceber,
                        totalBaixasReceberProcessadas = countBaixaReceber
                    });
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}