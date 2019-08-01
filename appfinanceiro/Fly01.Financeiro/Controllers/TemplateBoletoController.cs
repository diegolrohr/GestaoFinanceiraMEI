using System;
using System.Web.Mvc;
using Fly01.Core.Presentation;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes;
using System.Collections.Generic;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core;
using Fly01.Core.Rest;
using System.Linq;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Defaults;
using Fly01.uiJS.Classes.Helpers;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroConfiguracoesTemplateBoleto)]
    public class TemplateBoletoController : BaseController<TemplateBoletoVM>
    {
        public override Func<TemplateBoletoVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List() => Form();


        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
                target.Add(new HtmlUIButton { Id = "save", Label = "Salvar", OnClickFn = "fnSalvaTemplate", Type = "submit" });

            return target;
        }

        protected override ContentUI FormJson()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Template de E-mail",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };
            var config = new FormUI
            {
                Id = "fly01frm",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new InputTextUI { Id = "assunto", Class = "col s12 m12 l12", Label = "Assunto", MaxLength = 200 });
            config.Elements.Add(new TextAreaUI { Id = "mensagem", Class = "col s12 m12 l12", Label = "Mensagem" , MaxLength = 1000});

            #region Helpers            
            config.Helpers.Add(new TooltipUI
            {
                Id = "assunto",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Permite configurar assunto padrão no envio de boletos por e-mail."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "mensagem",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Permite configurar mensagem padrão no envio de boleto por e-mail."
                }
            });
            #endregion 

            cfg.Content.Add(config);

            return cfg;
        }
        public JsonResult CarregaTemplate()
        {
            var template = GetTemplate();

            return Json(
                new {
                sucess = true,
                id = template.Id,
                assunto = template.Assunto,
                mensagem = template.Mensagem
            } , JsonRequestBehavior.AllowGet);
        }

        public TemplateBoletoVM GetTemplate()
        {
            var response = RestHelper.ExecuteGetRequest<ResultBase<TemplateBoletoVM>>(ResourceName);

            if (response == null || response.Data == null)
                return null;

            return response.Data.FirstOrDefault();
        }

        public JsonResult SalvaTemplate(string assunto, string mensagem)
        {
            try
            {
                var dadosTemplate = new
                {
                    assunto  = assunto,
                    mensagem = mensagem,
                };

                TemplateBoletoVM templateRetorno;

                var existeTemplate = GetTemplate();

                if (existeTemplate == null)
                    templateRetorno = RestHelper.ExecutePostRequest<TemplateBoletoVM>(ResourceName, JsonConvert.SerializeObject(dadosTemplate, JsonSerializerSetting.Default));
                else
                    templateRetorno = RestHelper.ExecutePutRequest<TemplateBoletoVM>($"{ResourceName}/{existeTemplate.Id}", JsonConvert.SerializeObject(dadosTemplate, JsonSerializerSetting.Edit));

                return Json(new
                {
                    success = true,
                    data = templateRetorno,
                    recordsFiltered = 1,
                    recordsTotal = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}