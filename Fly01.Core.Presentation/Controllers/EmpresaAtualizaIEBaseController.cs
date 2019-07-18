using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Mensageria;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ValueObjects;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Fly01.Core.Presentation.Controllers
{
    public class EmpresaAtualizaIEBaseController<T> : BaseController<T> where T : DomainBaseVM
    {
        public override Func<T, object> GetDisplayData() { throw new NotImplementedException(); }
        public override ContentResult List() { throw new NotImplementedException();        }
        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public ContentResult ModalAtualizaIE()
        {
            var empresa = new ManagerEmpresaVM();
            try
            {
                empresa = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);
            }
            catch (Exception){}
             
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Atualizar Inscrição Estadual:",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Enviar", OnClickFn = "fnConfirmAtualizaIE" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                Id = "fly01mdlfrmAtualizaIE"
            };

            if(empresa?.CidadeId == 0)
            {
                config.Elements.Add(new AutoCompleteUI
                {
                    Id = "estadoId",
                    Class = "col s12 l6",
                    Label = "Estado",
                    MaxLength = 35,
                    Required = true,
                    DataUrl = Url.Action("EstadoManagerNew", "AutoComplete"),
                    LabelId = "estadoNome",
                    DomEvents = new List<DomEventUI>
                {
                    new DomEventUI() { DomEvent = "autocompleteselect", Function = "fnStateSelect" }
                }
                });

                config.Elements.Add(new AutoCompleteUI
                {
                    Id = "cidadeId",
                    Class = "col s12 l6",
                    Label = "Cidade (Escolha o estado antes)",
                    MaxLength = 35,
                    Required = true,
                    DataUrl = Url.Action("CidadeManagerNew", "AutoComplete"),
                    LabelId = "cidadeNome",
                    PreFilter = "estadoId"
                });
            }
            else
            {
                config.Elements.Add(new InputHiddenUI() { Id = "cidadeId", Value = "0" });
            }

            config.Elements.Add(new InputTextUI
            {
                Id = "inscricaoEstadualId",
                Class = "col s12 m6",
                Label = "Inscrição Estadual",
                Required = true
            });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "chkIsento",
                Class = "col s12 m6",
                Label = "Sim, é isento de Inscrição Estadual?",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI {DomEvent = "change", Function = "fnChkIsentoInscricaoEstadual"}
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "inscricaoEstadualId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Verificamos que você não possui cadastrado sua Inscrição Estadual nos dados de sua Empresa. Por favor, insira sua inscrição estadual para realizarmos a atualização dos seus parâmetros tributários."
                }
            });

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        public JsonResult ValidaDadosEmpresa()
        {
            try
            {
                ManagerEmpresaVM empresa = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);
                if (!string.IsNullOrWhiteSpace(empresa.InscricaoEstadual) || empresa.CidadeId > 0)
                {
                    return Json(new
                    {
                        success = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public JsonResult PostAtualizacaoIE(string inscricaoEstadual, string cidadeId)
        {
            try
            {
                ManagerEmpresaVM empresa = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);

                var msgErrorInscricaoEstadual = string.Empty;
                if(empresa.CEP == null) { empresa.CEP = string.Empty; };
                if (!string.IsNullOrEmpty(cidadeId) && int.Parse(cidadeId) > 0) { empresa.CidadeId = int.Parse(cidadeId); };

                if (InscricaoEstadualHelper.IsValid(empresa.Cidade?.Estado?.Sigla, inscricaoEstadual, out msgErrorInscricaoEstadual))
                {
                    empresa.InscricaoEstadual = inscricaoEstadual;
                    ApiEmpresaManager.AtualizaDadosEmpresa(empresa, SessionManager.Current.UserData.PlatformUrl);

                    return Json(new
                    {
                        success = true,
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    success = false,
                    message = "Inscrição Estudal Inválida."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [HttpGet]
        public JsonResult ExisteParametroSalvo()
        {
            var queryString = new Dictionary<string, string> {
                    { "$select", "id" }
            };

            try
            {
                var response = RestHelper.ExecuteGetRequest<ParametroTributarioVM>("ParametroTributario", queryString);
                return Json(new { existeParametro = (response?.Id != null && response?.Id != default(Guid)) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { success = false, message = ex } };
            }
        }
    }
}