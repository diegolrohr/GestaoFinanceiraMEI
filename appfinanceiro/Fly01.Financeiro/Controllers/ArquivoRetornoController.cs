using System;
using System.Web.Mvc;
using Fly01.Financeiro.Controllers.Base;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes;
using System.Collections.Generic;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Text;
using System.IO;

namespace Fly01.Financeiro.Controllers
{
    public class ArquivoRetornoController : BaseController<DomainBaseVM>
    {
        public override ContentResult Form()
        {
            var cfg = new ContentUI
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("Create"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Importar arquivo de retorno",
                    Buttons = new List<HtmlUIButton>()
                    {
                        new HtmlUIButton() { Id = "save", Label = "Importar", OnClickFn = "fnImportarArquivo", Type = "submit" }
                    }
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI()
            {
                Action = new FormUIAction()
                {
                    Create = Url.Action("ImportaCadastro"),
                    Edit = Url.Action("ImportaCadastro"),
                    Get = Url.Action("Json", "Fornecedor") + "/ImportarCadastro",
                    List = @Url.Action("List")
                },
                //ReadyFn = "fnImportaCadastroFormReady",
                //UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new AutocompleteUI
            {
                Id = "bancoId",
                Class = "col s12 m6 l6",
                Label = "Banco cedente",
                Required = true,
                DataUrl = @Url.Action("ContaBancariaBancoEmiteBoleto", "AutoComplete") + "?emiteBoleto=true",
                LabelId = "bancoNome"
            });
            config.Elements.Add(new InputFileUI { Id = "arquivo", Class = "col s12 m6 l6", Label = "Arquivo de retorno do tipo (.ret)", Required = true, Accept = ".ret" });

            cfg.Content.Add(config);

            cfg.Content.Add(new CardUI
            {
                Class = "col s12",
                Color = "blue",
                //Id = "cardDuvidas",
                Title = "Dicas",
                Placeholder = "Selecione o banco de origem do arquivo, e localize o arquivo que deseja importar",
                Action = new LinkUI()
                {
                    Label = "",
                    OnClick = ""
                }
            });

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override Func<DomainBaseVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        public JsonResult ImportaArquivoRetorno(string valueArquivo)
        {
            var formatoArquivo = 1;
            var boletos = new Boleto2Net.Boletos();
            boletos.Banco =  Boleto2Net.Banco.Instancia(001);

            byte[] byteArray = Encoding.ASCII.GetBytes(valueArquivo);
            MemoryStream pConteudo = new MemoryStream(byteArray);

            var arquivoRetorno = new Boleto2Net.ArquivoRetorno(boletos.Banco, (Boleto2Net.TipoArquivo)formatoArquivo);
            var boletosRetorno = arquivoRetorno.LerArquivoRetorno(pConteudo);

            return null;
        }
    }
}