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
using Newtonsoft.Json;

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
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.StoneBL.ValidaAutenticacaoStone(entity);
            }

            try
            {
                SetSecurityProtocol();
                var resource = "authenticate";
                var autenticacao = RestHelper.ExecutePostRequest<ResponseAutenticacaoStone>(AppDefaults.UrlStone, resource, entity, null, GetDefaultHeader());
                return Ok(
                    new StoneTokenBaseVM()
                    {
                        Token = autenticacao.Token
                    });

            }
            catch (Exception)
            {
                throw new BusinessException("Não foi possível obter autenticação na stone. Verifique a senha digitada e tente novamente.");
            }
        }
        #endregion

        #region Validar Token
        [HttpPost]
        [Route("validartoken")]
        public IHttpActionResult ValidarToken(StoneAutenticacaoVM entity)
        {
            try
            {
                SetSecurityProtocol();
                var resource = "authenticate/validate";
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
        public IHttpActionResult AntecipacaoSimular(StoneAntecipacaoSimularVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.StoneBL.ValidaAntecipacaoSimular(entity);
            }

            try
            {
                var antecipacao = new AntecipacaoStone()
                {
                    Valor = entity.Valor,
                };

                SetSecurityProtocol();
                //TODO:Stone publicacao
                //var resource = "v1/settlements/prepay/simulate";
                //var simulacao = RestHelper.ExecutePostRequest<ResponseAntecipacaoStone>(AppDefaults.UrlStone, resource, antecipacao, null, GetCompleteHeader(entity.Token));
                //return Ok(
                //    new StoneAntecipacaoVM()
                //    {
                //        Id = Guid.NewGuid(),
                //        Data = simulacao.Data,
                //        DataCriacao = simulacao.DataCriacao,
                //        LiquidoAntecipar = simulacao.LiquidoAntecipar,
                //        SaldoLiquidoDisponivel = simulacao.SaldoLiquidoDisponivel,
                //        TaxaPontual = simulacao.TaxaPontual,
                //        BrutoAntecipar = entity.Valor
                //    });

                return Ok(
                    new StoneAntecipacaoVM()
                    {
                        Id = Guid.NewGuid(),
                        Data = DateTime.Now,
                        DataCriacao = DateTime.Now,
                        LiquidoAntecipar = entity.Valor * 0.9641,
                        SaldoLiquidoDisponivel = 123.00,
                        TaxaPontual = 3.59,
                        BrutoAntecipar = entity.Valor
                    });
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var error = JsonConvert.DeserializeObject<StoneErrorVM>(ex.Message);
                if (error != null && !string.IsNullOrEmpty(error?.Message))
                {
                    message = error?.Message;
                }
                throw new BusinessException("Erro ao simular a antecipação stone: " + message);
            }
        }
        #endregion

        #region Efetivar Antecipação
        [HttpPost]
        [Route("antecipacaoefetivar")]
        public async Task<IHttpActionResult> AntecipacaoEfetivar(StoneAntecipacaoEfetivarPostVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.StoneBL.ValidaAntecipacaoEfetivar(entity);
            }

            try
            {
                var antecipacao = new AntecipacaoStone()
                {
                    Valor = entity.Valor,
                };

                SetSecurityProtocol();
                //TODO:Stone publicacao
                //var resource = "v1/settlements/prepay/proposals";
                //var efetivacao = RestHelper.ExecutePostRequest<ResponseAntecipacaoStone>(AppDefaults.UrlStone, resource, antecipacao, null, GetCompleteHeader(entity.Token));
                var efetivacao = new ResponseAntecipacaoStone()
                {
                    Data = DateTime.Now,
                    DataCriacao = DateTime.Now,
                    Id = 152515,
                    LiquidoAnteciparCentavos = (int)(antecipacao.ValorCentavos * 0.9641),
                    SaldoLiquidoDisponivel = entity.Valor * 0.9641,
                    TaxaPontual = 3.59
                };

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
                    new StoneAntecipacaoEfetivarVM()
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
                var message = ex.Message;
                var error = JsonConvert.DeserializeObject<StoneErrorVM>(ex.Message);
                if (error != null && !string.IsNullOrEmpty(error?.Message))
                {
                    message = error?.Message;
                }
                throw new BusinessException("Erro ao efetivar a antecipação stone: " + message);
            }
        }
        #endregion

        #region Consultar Total
        [HttpPost]
        [Route("antecipacaoconsultar")]
        public IHttpActionResult AntecipacaoConsultar(StoneTokenBaseVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.StoneBL.ValidaAntecipacaoConsultar(entity);
            }

            try
            {
                SetSecurityProtocol();
                var resource = "v1/settlements/prepay/informations";
                var total = RestHelper.ExecuteGetRequest<ResponseConsultaTotalStone>(AppDefaults.UrlStone, resource, GetCompleteHeader(entity.Token), null);
                return Ok(
                    new StoneTotaisVM()
                    {
                        SaldoDevedor = total.SaldoDevedor,
                        TotalBrutoAntecipavel = 75690.25//total.TotalBrutoAntecipavel //TODO:Stone publicacao
                    });
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var error = JsonConvert.DeserializeObject<StoneErrorVM>(ex.Message);
                if (error != null && !string.IsNullOrEmpty(error?.Message))
                {
                    message = error?.Message;
                }
                throw new BusinessException("Erro ao consultar o total disponível para antecipação: " + message);
            }
        }
        #endregion

        #region Consultar Configuração
        [HttpPost]
        [Route("antecipacaoconfiguracao")]
        public IHttpActionResult AntecipacaoConfiguracao(StoneTokenBaseVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.StoneBL.ValidaAntecipacaoConfiguracao(entity);
            }

            try
            {
                SetSecurityProtocol();
                var resource = "v1/settlements/prepay/configurations";
                var config = RestHelper.ExecuteGetRequest<ResponseConfiguracaoStone>(AppDefaults.UrlStone, resource, GetCompleteHeader(entity.Token), null);
                return Ok(
                    new StoneConfiguracaoVM()
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
                var message = ex.Message;
                var error = JsonConvert.DeserializeObject<StoneErrorVM>(ex.Message);
                if (error != null && !string.IsNullOrEmpty(error?.Message))
                {
                    message = error?.Message;
                }
                throw new BusinessException("Erro ao consultar a configuração de antecipação stone: " + message);
            }
        }
        #endregion

        #region Consultar Dados Bancários
        [HttpPost]
        [Route("dadosbancarios")]
        public IHttpActionResult DadosBancarios(StoneTokenBaseVM entity)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
            {
                unitOfWork.StoneBL.ValidaAntecipacaoDadosBancarios(entity);
            }

            try
            {
                SetSecurityProtocol();
                var resource = "v1/configurations/bank-details";
                var dados = RestHelper.ExecuteGetRequest<List<ResponseDadosBancariosStone>>(AppDefaults.UrlStone, resource, GetCompleteHeader(entity.Token), null).FirstOrDefault();
                return Ok(
                    new StoneDadosBancariosVM()
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
                var message = ex.Message;
                var error = JsonConvert.DeserializeObject<StoneErrorVM>(ex.Message);
                if (error != null && !string.IsNullOrEmpty(error?.Message))
                {
                    message = error?.Message;
                }
                throw new BusinessException("Erro ao consultar dados bancários da stone: " + message);
            }
        }
        #endregion
    }
}