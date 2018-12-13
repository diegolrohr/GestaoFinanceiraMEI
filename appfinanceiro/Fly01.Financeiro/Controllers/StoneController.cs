using Fly01.Core.Presentation;
using Fly01.Financeiro.ViewModel;
using System;
using Fly01.uiJS.Classes;
using System.Web.Mvc;
using Fly01.uiJS.Classes.Elements;
using System.Collections.Generic;
using Newtonsoft.Json;
using Fly01.Core.Defaults;

namespace Fly01.Financeiro.Controllers
{
    public class StoneController : BaseController<StoneVM>
    {
        public override Func<StoneVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }


        protected FormUI FormLogin()
        {
            var config = new FormUI
            {
                Id = "fly01frm",
                Class = "col s12 m8 offset-m2 center",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                //ReadyFn = "fnFormReady",
                //AfterLoadFn = "fnAfterLoad"
            };

            config.Elements.Add(new StaticTextUI
            {
                Id = "textInfoStone",
                Lines = new List<LineUI>
                {
                    new LineUI()
                    {
                        Tag = "h2",
                        Class = "strong green-text",
                        Text = "Login Stone",
                    }
                }
            });

            config.Elements.Add(new StaticTextUI
            {
                Id = "textInfoBoletos",
                Class = "col s12",
                Lines = new List<LineUI>
                {
                    new LineUI()
                    {
                        Tag = "h5",
                        Class = "light",
                        Text = "Digite sua senha do portal da Stone, para ter acesso à funcionalidade de antecipação de recebíveis.",
                    }
                }
            });

            config.Elements.Add(new InputPasswordUI { Id = "senha", Class = "col s12 m6 offset-m3", Label = "Senha Stone", MaxLength = 200 });

            config.Elements.Add(new ButtonUI
            {

                Id = "start",
                Value = "Entrar",
                ClassBtn = "green",
                OnClickFn = ""
            });

            return config;
        }

        public bool ValidaToken()
        {
            //dar get na api do honatel
            return true;
        }

        public override ContentResult List()
        {
            ContentUI result = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory
                {
                    Default = Url.Action("Index")
                },
                Header = new HtmlUIHeader
                {
                    Title = "Stone - Antecipação de recebíveis",
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            if (ValidaToken()) // se tem TOKEN VALIDO
                result.Content.Add(FormSimulacao());
            else
                result.Content.Add(FormLogin());

            return Content(JsonConvert.SerializeObject(result, JsonSerializerSetting.Front), "application/json"); ;
        }

        protected FormUI FormSimulacao()
        {
            var config = new FormUI
            {
                Id = "formAntecipacao",
                Class = "col s12 m8 offset-m2 center",
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    List = Url.Action("Form")
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnReadyRecebiveis",
                //AfterLoadFn = "fnAfterLoad"
            };

            config.Elements.Add(new InputCurrencyUI
            {
                Id = "valorRecebivel",
                Label = "Valor",
                Class = "col s12 offset-m4 m4",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "change", Function = "fnRangeChange" }
                }

            });

            config.Elements.Add(new InputRangeUI
            {
                Id = "rangeRecebivel",
                Class = "col s12 offset-m2 m8 green-text",
                DomEvents = new List<DomEventUI>
                {
                    new DomEventUI { DomEvent = "input mousemove touchmove", Function = "fnRangeChange" }
                }
            });

            config.Elements.Add(new ButtonUI
            {

                Id = "simular",
                Value = "Simular",
                Class = "col s12 offset-m4 m4",
                ClassBtn = "btn-jumbo green",
                OnClickFn = ""
            });
            
            return config;
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }
    }
}