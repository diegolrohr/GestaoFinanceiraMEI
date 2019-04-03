using System;
using System.Linq;
using System.Web.Http;
using Fly01.Financeiro.BL;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Enum;

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
        public IHttpActionResult Post()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                #region Recuperar todas as contas a Pagar em aberto

                #endregion

                #region Recuperar todas as contas a Pagar em aberto

                #endregion

                #region Inativar as baixas existentes e criar as novas para recálculo

                #endregion

                return Ok(
                    new
                    {
                        totalRecords = 15
                    }
                );
            }
        }
    }
}