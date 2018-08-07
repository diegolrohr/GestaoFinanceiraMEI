using System;
using System.Net;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Fly01.Core.SOAManager.ViewModel;
using Fly01.Core.SOAManager.SOAManager;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels;

namespace Fly01.Core.SOAManager
{
    public static class SOADataManager
    {
        private readonly static string SESSION_NAME = "_SOADATA_SESSION_";

        public static SOADataSessionVM GetServiceData(HttpSessionStateBase Session)
        {
            if (Session[SESSION_NAME] == null)
                return new SOADataSessionVM();
            else
                return (SOADataSessionVM)Session[SESSION_NAME];
        }

        public static void SetServiceData(HttpSessionStateBase Session, SOADataSessionVM soaDataSession)
        {
            Session[SESSION_NAME] = soaDataSession;
        }

        public static BuscaCepVM BuscaCEP(string cep, SOAConnectionConfig conn)
        {
            SOAManager.SOAManager client = GetManagerClient();
            SOAConnectionData connectionData = GetConnectionData(conn);

            SOAServiceData serviceData = client.GetService(connectionData, "SOAMashupStudioService", "Correios.PesquisaCEP", 1, true);
            if (serviceData.Data.Count(sd => sd.DataKind == SOADataKind.Param && sd.Name.ToUpper().Equals("CEP")) > 0)
            {
                SOAData cepParam = serviceData.Data.Where(sd => sd.DataKind == SOADataKind.Param && sd.Name.ToUpper().Equals("CEP")).FirstOrDefault();
                cepParam.Value = cep;
            }
            string resultMessage;
            int resultMessageCode;
            bool resultMessageCodeSpecified;
            SOAResultType resultTypeCEP;
            bool resultTypeCepSpecified;

            client.Execute(
                connectionData,
                ref serviceData,
                false,
                false,
                "", out resultTypeCEP,
                out resultTypeCepSpecified,
                out resultMessage,
                out resultMessageCode,
                out resultMessageCodeSpecified
            );
            List<SOAData> results = serviceData.Data.Where(p => p.DataKind == SOADataKind.Result).ToList();

            if (resultTypeCEP != SOAResultType.Error)
            {
                return new BuscaCepVM()
                {
                    ZipCode = results.Where(r => r.Name.Equals("CEP")).FirstOrDefault().Value,
                    Address = results.Where(r => r.Name.Equals("Endereço")).FirstOrDefault().Value,
                    Neighborhood = results.Where(r => r.Name.Equals("Bairro")).FirstOrDefault().Value,
                    City = results.Where(r => r.Name.Equals("Cidade")).FirstOrDefault().Value,
                    State = results.Where(r => r.Name.Equals("Estado")).FirstOrDefault().Value,
                    StateId = "",
                    CityId = "",
                    StateCodeIbge = "",
                    CityCodeIbge = ""
                };
            }

            throw new Exception(resultMessage);
        }

        public static SOAManager.SOAManager GetManagerClient()
        {
            // cria o proxy...
            SOAManager.SOAManager manager = new SOAManager.SOAManager();
            manager.Proxy = HttpWebRequest.GetSystemWebProxy();
            manager.Proxy.Credentials = CredentialCache.DefaultCredentials;
            manager.Credentials = CredentialCache.DefaultCredentials;
            manager.Timeout = 300000;

            // retorna o proxy...
            return manager;
        }

        public static SOAConnectionData GetConnectionData(SOAConnectionConfig conn)
        {
            return new SOAConnectionData()
            {
                ClientId = conn.MashupClientId,
                Username = conn.MashupUser,
                Password = conn.MashupPassword
            };
        }

        public static object GetReceitaFederalData(HttpSessionStateBase Session, SOAConnectionConfig conn, string documento = "", string codigo = "", string dataNasc = "")
        {
            SOAManager.SOAManager client = GetManagerClient();
            SOADataSessionVM dataSession = null;
            documento = FormatterUtils.RemoveNotNumbers(documento);
            bool tipoPessoaCPF = documento.Length > 0 && documento.Length <= 11;
            bool getImageState = !string.IsNullOrWhiteSpace(documento) && string.IsNullOrWhiteSpace(codigo);

            SOAConnectionData connectionData = GetConnectionData(conn);
            SOAServiceData serviceData = client.GetService(connectionData, "SOAMashupStudioService", "ReceitaFederal.CPF_CNPJ", 2, true);

            if (serviceData.Data.Count(sd => sd.DataKind == SOADataKind.Param && (sd.Name.ToUpper().Equals("CPF") || sd.Name.ToUpper().Equals("CNPJ"))) > 0)
            {
                SOAData tipoPessoaParam = serviceData.Data.Where(sd => sd.DataKind == SOADataKind.Param && sd.Name.ToUpper().Equals("TIPOPESSOA")).FirstOrDefault();
                tipoPessoaParam.Value = tipoPessoaCPF ? "F" : "J";

                if (!getImageState)
                {

                    if (tipoPessoaCPF)
                    {
                        SOAData cpfParam = serviceData.Data.Where(sd => sd.DataKind == SOADataKind.Param && sd.Name.ToUpper().Equals("CPF")).FirstOrDefault();
                        cpfParam.Value = documento;

                        SOAData dataNascParam = serviceData.Data.Where(sd => sd.DataKind == SOADataKind.Param && sd.Name.ToUpper().Equals("DATANASC")).FirstOrDefault();
                        dataNascParam.Value = dataNasc;

                        SOAData codigoParam = serviceData.Data.Where(sd => sd.DataKind == SOADataKind.Param && sd.Name.ToUpper().Equals("CODIGO") && sd.StateName.ToUpper().Equals("STATEGETCPF")).FirstOrDefault();
                        codigoParam.Value = codigo;
                    }
                    else
                    {
                        SOAData cnpjParam = serviceData.Data.Where(sd => sd.DataKind == SOADataKind.Param && sd.Name.ToUpper().Equals("CNPJ")).FirstOrDefault();
                        cnpjParam.Value = documento;

                        SOAData codigoParam = serviceData.Data.Where(sd => sd.DataKind == SOADataKind.Param && sd.Name.ToUpper().Equals("CODIGO") && sd.StateName.ToUpper().Equals("STATEGETCNPJ")).FirstOrDefault();
                        codigoParam.Value = codigo;
                    }

                    //repassa as infos da requisição anterior
                    dataSession = GetServiceData(Session);
                    if (dataSession != null && dataSession.DataList != null)
                    {
                        List<SOAData> dataList = new List<SOAData>();
                        dataList.AddRange(serviceData.Data.Where(x => x.DataKind != SOADataKind.Internal));
                        dataList.AddRange(dataSession.DataList);
                        serviceData.Data = dataList.ToArray();
                        serviceData.CurrentState = dataSession.CurrentState;
                        serviceData.ServiceExecutionId = dataSession.ServiceExecutionId;
                        //limpa dados em sessão
                        SetServiceData(Session, null);
                    }
                }
            }

            string resultMessage;
            int resultMessageCode;
            bool resultMessageCodeSpecified;
            SOAResultType resultTypeDOC;
            bool resultTypeDocSpecified;

            client.Execute(
                connectionData,
                ref serviceData,
                false,
                false,
                "", out resultTypeDOC,
                out resultTypeDocSpecified,
                out resultMessage,
                out resultMessageCode,
                out resultMessageCodeSpecified
            );

            if (resultTypeDOC != SOAResultType.Error)
            {
                //se é uma req. do tipo continue, guarda 
                if (getImageState && resultTypeDOC == SOAResultType.Continue)
                {
                    dataSession = new SOADataSessionVM();
                    dataSession.DataList = serviceData.Data.Where(sd => sd.DataKind == SOADataKind.Internal).ToList();
                    dataSession.CurrentState = serviceData.CurrentState;
                    dataSession.ServiceExecutionId = serviceData.ServiceExecutionId;
                    SetServiceData(Session, dataSession);

                    //retorna codigo captcha
                    return new
                    {
                        captchaImage = serviceData.Data.Where(p => p.DataKind == SOADataKind.Info && p.DataType == SOADataType.Image).FirstOrDefault().Value
                    };
                }
                else
                {
                    List<SOAData> results = serviceData.Data.Where(p => p.DataKind == SOADataKind.Result).ToList();

                    return new
                    {
                        RazaoSocial = results.Where(x => x.Name.Equals("Razão Social")).FirstOrDefault().Value,
                        NomeFantasia = results.Where(x => x.Name.Equals("Nome")).FirstOrDefault().Value,
                        CEP = FormatterUtils.RemoveNotNumbers(results.Where(x => x.Name.Equals("CEP")).FirstOrDefault().Value),
                        Endereco = results.Where(x => x.Name.Equals("Endereço")).FirstOrDefault().Value,
                        Bairro = results.Where(x => x.Name.Equals("Bairro")).FirstOrDefault().Value,
                        Numero = results.Where(x => x.Name.Equals("Número")).FirstOrDefault().Value,
                        Complemento = results.Where(x => x.Name.Equals("Complemento")).FirstOrDefault().Value,
                        Estado = results.Where(r => r.Name.Equals("Estado")).FirstOrDefault().Value,
                        Cidade = results.Where(r => r.Name.Equals("Cidade")).FirstOrDefault().Value
                    };
                }
            }
            else
            {
                return new
                {
                    error = !string.IsNullOrWhiteSpace(resultMessage) ? resultMessage : "Erro desconhecido. Favor tentar novamente."
                };
            }
        }
    }
}
