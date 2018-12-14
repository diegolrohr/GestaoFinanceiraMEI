using Fly01.Core;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Fly01.Financeiro.BL;
using System.Threading.Tasks;

namespace Fly01.Financeiro.API.Controllers.Api
{
    [RoutePrefix("api/stone")]
    public class StoneController : ApiBaseController
    {
        #region Headers
        public Dictionary<string, string> GetDefaultHeader()
        {
            var header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            return header;
        }

        public void AddAuthorizationHeader(Dictionary<string, string> header, string token)
        {
            header.Add("Authorization", String.Format("Bearer {0}", token));
        }

        public void AddStoneCodeHeader(Dictionary<string, string> header)
        {
            ManagerEmpresaVM response = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            header.Add("StoneCode", response?.StoneCode);
        }
        public Dictionary<string, string> GetCompleteHeader(string token)
        {
            var header = GetDefaultHeader();
            AddAuthorizationHeader(header, token);
            AddStoneCodeHeader(header);
            return header;
        }
        #endregion

        private void SetSecurityProtocol()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public IHttpActionResult Get()
        {
            return Ok("SUCESSO");
        }

        #region Token
        [HttpPost]
        [Route("token")]
        public IHttpActionResult GetToken(AutenticacaoStoneVM entity)
        {
            var resource = "authenticate";
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.StoneBL.ValidaAutenticacaoStone(entity);
            }

            try
            {
                //TODO: ver https
                SetSecurityProtocol();

                var autenticacao = RestHelper.ExecutePostRequest<ResponseAutenticacaoStone>(AppDefaults.UrlStone, resource, entity, null, GetDefaultHeader());
                return Ok(
                    new StoneTokenBaseVM()
                    {
                        Token = autenticacao.Token
                    });

            }
            catch (Exception ex)
            {
                throw new BusinessException("Não foi possível obter autenticação na stone. " + ex.Message);
            }
        }
        #endregion

        #region Validar Token
        [HttpPost]
        [Route("validartoken")]
        public IHttpActionResult ValidarToken(ResponseAutenticacaoStoneVM entity)
        {
            var resource = "authenticate/validate";
            try
            {
                SetSecurityProtocol();
                var authenticate = RestHelper.ExecutePostRequest<JObject>(AppDefaults.UrlStone, resource, entity, null, GetDefaultHeader());
                return Ok(new { success = true });
            }
            catch (Exception)
            {
                return Ok(new { success = false });
            }
        }
        #endregion

        #region Simular Antecipação
        [HttpPost]
        [Route("antecipacaosimular")]
        public IHttpActionResult AntecipacaoSimular(SimularAntecipacaoStoneVM entity)
        {
            var resource = "v1/settlements/prepay/simulate";
            try
            {
                var antecipacao = new AntecipacaoStone()
                {
                    Valor = entity.Valor,
                };

                SetSecurityProtocol();
                var simulacao = RestHelper.ExecutePostRequest<ResponseAntecipacaoStone>(AppDefaults.UrlStone, resource, antecipacao, null, GetCompleteHeader(entity.Token));
                return Ok(
                    new ResponseAntecipacaoStoneVM()
                    {
                        Data = simulacao.Data,
                        DataCriacao = simulacao.DataCriacao,
                        LiquidoAntecipar = simulacao.LiquidoAntecipar,
                        SaldoLiquidoDisponivel = simulacao.SaldoLiquidoDisponivel,
                        TaxaPontual = simulacao.TaxaPontual
                    });
            }
            catch (Exception ex)
            {
                throw new BusinessException("Erro ao simular a antecipação stone: " + ex.Message);
            }
        }
        #endregion

        #region Efetivar Antecipação
        [HttpPost]
        [Route("antecipacaoefetivar")]
        public async Task<IHttpActionResult> AntecipacaoEfetivar(EfetivarAntecipacaoStoneVM entity)
        {
            var resource = "v1/settlements/prepay/proposals";
            try
            {
                var antecipacao = new AntecipacaoStone()
                {
                    Valor = entity.Valor,
                };

                SetSecurityProtocol();
                var efetivacao = RestHelper.ExecutePostRequest<ResponseAntecipacaoStone>(AppDefaults.UrlStone, resource, antecipacao, null, GetCompleteHeader(entity.Token));

                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    unitOfWork.StoneAntecipacaoRecebiveisBL.Insert(new StoneAntecipacaoRecebiveis()
                    {
                        StoneId = efetivacao.Id,
                        StoneBancoId = entity.StoneBancoId,
                        TaxaPontual = efetivacao.TaxaPontual,
                        Data = efetivacao.Data,
                        ValorBruto = entity.Valor,
                        ValorAntecipado = efetivacao.LiquidoAntecipar
                    });
                    await unitOfWork.Save();
                }

                return Ok(
                    new ResponseAntecipacaoStoneVM()
                    {
                        Id = efetivacao.Id,
                        Data = efetivacao.Data,
                        DataCriacao = efetivacao.DataCriacao,
                        LiquidoAntecipar = efetivacao.LiquidoAntecipar,
                        SaldoLiquidoDisponivel = efetivacao.SaldoLiquidoDisponivel,
                        TaxaPontual = efetivacao.TaxaPontual
                    });
            }
            catch (Exception ex)
            {
                throw new BusinessException("Erro ao efetivar a antecipação stone: " + ex.Message);
            }
        }
        #endregion

        #region Consultar Total
        [HttpPost]
        [Route("antecipacaoconsultar")]
        public IHttpActionResult AntecipacaoConsultar(StoneTokenBaseVM entity)
        {
            var resource = "v1/settlements/prepay/informations";
            try
            {
                SetSecurityProtocol();
                var total = RestHelper.ExecuteGetRequest<ResponseConsultaTotalStone>(AppDefaults.UrlStone, resource, GetCompleteHeader(entity.Token), null);
                return Ok(
                    new ResponseConsultaTotalStoneVM()
                    {
                        SaldoDevedor = total.SaldoDevedor,
                        TotalBrutoAntecipavel = total.TotalBrutoAntecipavel
                    });
            }
            catch (Exception ex)
            {
                throw new BusinessException("Erro ao consultar o total disponível para antecipação: " + ex.Message);
            }
        }
        #endregion

        #region Consultar Configuração
        [HttpPost]
        [Route("antecipacaoconfiguracao")]
        public IHttpActionResult AntecipacaoConfiguracao(StoneTokenBaseVM entity)
        {
            var resource = "v1/settlements/prepay/configurations";
            try
            {
                SetSecurityProtocol();
                var config = RestHelper.ExecuteGetRequest<ResponseConfiguracaoStone>(AppDefaults.UrlStone, resource, GetCompleteHeader(entity.Token), null);
                return Ok(
                    new ResponseConfiguracaoStoneVM()
                    {
                        AntecipacaoAutomaticaAtivada = config.AntecipacaoAutomaticaAtivada,
                        Bloqueado = config.Bloqueado,
                        TaxaAntecipacaoAutomatica = config.TaxaAntecipacaoAutomatica,
                        TaxaAntecipacaoPontual = config.TaxaAntecipacaoPontual,
                        TaxaGarantia = config.TaxaGarantia
                    });
            }
            catch (Exception ex)
            {
                throw new BusinessException("Erro ao consultar a configuração de antecipação stone: " + ex.Message);
            }
        }
        #endregion

        #region Consultar Dados Bancários
        [HttpPost]
        [Route("dadosbancarios")]
        public IHttpActionResult DadosBancarios(StoneTokenBaseVM entity)
        {
            var resource = "v1/configurations/bank-details";
            try
            {
                SetSecurityProtocol();
                var dados = RestHelper.ExecuteGetRequest<List<ResponseDadosBancariosStone>>(AppDefaults.UrlStone, resource, GetCompleteHeader(entity.Token), null).FirstOrDefault();
                return Ok(
                    new ResponseDadosBancariosStoneVM()
                    {
                        Agencia = dados.Agencia,
                        AgenciaDigito = dados.AgenciaDigito,
                        BancoCodigo = dados.BancoCodigo,
                        BancoNome = dados.BancoNome,
                        ContaDigito = dados.ContaDigito,
                        ContaNumero = dados.ContaNumero,
                        ContaTipo = dados.ContaTipo,
                        Id = dados.Id
                    });
            }
            catch (Exception ex)
            {
                throw new BusinessException("Erro ao consultar dados bancários da stone: " + ex.Message);
            }
        }
        #endregion
    }
}