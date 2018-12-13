using Fly01.Core;
using Fly01.Core.API;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

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
            AddAuthorizationHeader(header,token);
            AddStoneCodeHeader(header);
            return header;
        }
        #endregion

        public IHttpActionResult Get()
        {
            return Ok("SUCESSO");
        }

        #region Token
        [HttpPost]
        [Route("token")]
        public IHttpActionResult GetToken(AutenticacaoStone entity)
        {
            var resource = "authenticate";
            try
            {
                //TODO: ver https
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

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
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

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
        [Route("simularantecipacao")]
        public IHttpActionResult SimularAntecipacao(AntecipacaoStoneVM entity)
        {
            var resource = "v1/settlements/prepay/simulate";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var antecipacao = new AntecipacaoStone()
                {
                    Valor = entity.Valor,
                };

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
        [Route("efetivarantecipacao")]
        public IHttpActionResult EfetivarAntecipacao(AntecipacaoStoneVM entity)
        {
            var resource = "v1/settlements/prepay/proposals";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var antecipacao = new AntecipacaoStone()
                {
                    Valor = entity.Valor,
                };

                var efetivacao = RestHelper.ExecutePostRequest<ResponseAntecipacaoStone>(AppDefaults.UrlStone, resource, antecipacao, null, GetCompleteHeader(entity.Token));
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
        //[HttpPost]
        //[Route("consultartotal")]
        //public IHttpActionResult ObterTotal(StoneTokenBaseVM entity)
        //{
        //    var resource = "v1/settlements/prepay/informations";
        //    try
        //    {
        //        ServicePointManager.Expect100Continue = true;
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //        var total = RestHelper.ExecutePostRequest<ResponseConsultaTotalStone>(AppDefaults.UrlStone, resource, null, null, GetCompleteHeader(entity.Token));
        //        return Ok(
        //            new ResponseConsultaTotalStoneVM()
        //            {
        //                Id = efetivacao.Id,
        //                Data = efetivacao.Data,
        //                DataCriacao = efetivacao.DataCriacao,
        //                LiquidoAntecipar = efetivacao.LiquidoAntecipar,
        //                SaldoLiquidoDisponivel = efetivacao.SaldoLiquidoDisponivel,
        //                TaxaPontual = efetivacao.TaxaPontual
        //            });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException("Erro ao efetivar a antecipação stone: " + ex.Message);
        //    }
        //}
        #endregion
    }
}