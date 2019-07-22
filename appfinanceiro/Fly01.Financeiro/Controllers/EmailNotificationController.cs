using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Fly01.Financeiro.ViewModel;
using Fly01.Financeiro.Models.Utils;
using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.Config;
using Newtonsoft.Json;
using Fly01.Core.Rest;
using Fly01.Core.Mensageria;
using System.Configuration;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(NotApply = true)]
    [AllowAnonymous]
    public class EmailNotificationController
    {
        private void NotifyUsers(List<ContaPagarVM> recordsToExport, List<UserEnvironmentVM> users)
        {
            DateTime tomorry = DateTime.Now.AddDays(1);
            DateTime dateToSend = new DateTime(tomorry.Year, tomorry.Month, tomorry.Day, 7, 0, 0);

            string serviceCode = ConfigurationManager.AppSettings["MensageriaServiceCodeEmail"];
            foreach (var item in users)
            {
                MessengerHelper.Send(item.PlatformUser, item.PlatformUserName, EmailBodyHelper.GetEmailReportContasPagar(item.PlatformUserName, recordsToExport), AppDefaults.LicenciamentoProdutoId.ToString(), dateToSend, serviceCode);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ReportDialyAccountsPayable(RequestBodyBasicAuth auth)
        {
            JsonResult jsonResult = new JsonResult();
            ReponseEmailNotification responseRequest = new ReponseEmailNotification();
            Stopwatch stopWatch = new Stopwatch();

            if (auth.Authentication.UserName == AppDefaults.SchedulerAzureBasicUserName && auth.Authentication.Password == AppDefaults.SchedulerAzureBasicPassword)
            {
                DateTime dataFiltro = DateTime.Now.AddDays(1);


                stopWatch.Start();
                try
                {
                    var requestObjectUserEnvironment = new
                    {
                        clientId = AppDefaults.GatewayUserName,
                        verificationKey = Base64Helper.GenerateVerificationKey(AppDefaults.GatewayVerificationKeyPassword)
                    };

                    List<PlatformEnvironmentVM> responseUserPlatform = RestHelper.ExecutePostRequest<List<PlatformEnvironmentVM>>("UserEnvironment", JsonConvert.SerializeObject(requestObjectUserEnvironment));
                    responseRequest.TotalPlatforms = responseUserPlatform.Count();
                    responseRequest.TotalUsers = responseUserPlatform.Sum(x => x.PlatformUsers.Count());
                    
                    foreach (var item in responseUserPlatform)
                    {
                        string userName = item.PlatformUsers.FirstOrDefault().PlatformUser;
                        string platformUrl = item.PlatformUrl;

                        TokenDataVM tokenData = null;
                        try
                        {
                            tokenData = RestHelper.ExecuteGetAuthToken(AppDefaults.UrlGateway, AppDefaults.GatewayUserName, AppDefaults.GatewayPassword, platformUrl, userName);

                            Dictionary<string, string> queryString = AppDefaults.GetQueryStringDefault(string.Empty, string.Empty, AppDefaults.MaxRecordsPerPageAPI);
                            queryString.AddParam("dueDate", dataFiltro.ToString("yyyyMMdd"));
                            queryString.AddParam("hasBalance", "");
                            queryString.AddParam("hasInstallments", "0");
                            queryString.AddParam("page", "1");
                            queryString.AddParam("emptyFields", "false");
                            queryString.AddParam("subtitleCode", "1"); //Contas em aberto

                            int page = Convert.ToInt32(queryString.FirstOrDefault(x => x.Key == "page").Value);
                            string resource = AppDefaults.GetResourceName(typeof(ContaPagarVM));

                            List<ContaPagarVM> recordsToExport = new List<ContaPagarVM>();
                            bool hasNextRecord = true;
                            while (hasNextRecord)
                            {
                                queryString.AddParam("page", page.ToString());
                                ResultBase<ContaPagarVM> response = RestHelper.ExecuteGetRequestWithoutSession<ResultBase<ContaPagarVM>>(resource, queryString, tokenData.AccessToken);
                                recordsToExport.AddRange(response.Data);
                                hasNextRecord = response.HasNext && response.Data.Count > 0;
                                page++;
                            }

                            if (recordsToExport.Count() > 0)
                                NotifyUsers(recordsToExport, item.PlatformUsers);
                        }
                        catch (Exception ex)
                        {
                            responseRequest.TotalPlatformsWithError++;
                            responseRequest.Errors.Add(new ReponseEmailItemError { PlatformUrl = platformUrl, ErrorMsg = ex.Message });
                        }
                    }
                }
                catch (Exception ex)
                {
                    responseRequest.Errors.Add(new ReponseEmailItemError { PlatformUrl = string.Empty, ErrorMsg = ex.Message });
                }
            }
            else
            {
                responseRequest.Errors.Add(new ReponseEmailItemError { PlatformUrl = string.Empty, ErrorMsg = "UserName and Password do not match" });
            }

            stopWatch.Stop();
            responseRequest.ElapsedMilliseconds = stopWatch.ElapsedMilliseconds;
            jsonResult.Data = responseRequest;

            return jsonResult;
        }
    }
}
