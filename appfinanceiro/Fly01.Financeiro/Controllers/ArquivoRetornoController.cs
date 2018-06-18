using System;
using System.Web.Mvc;
using Fly01.Financeiro.ViewModel;
using Fly01.uiJS.Classes;
using System.Collections.Generic;
using Newtonsoft.Json;
using Fly01.uiJS.Defaults;
using Fly01.uiJS.Classes.Elements;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Text;
using System.IO;
using Fly01.Core.Rest;
using Fly01.Core;
using System.Linq;
using Fly01.Core.Config;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;

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
                Functions = new List<string> { "fnFormReady" },
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
                ReadyFn = "fnFormReady",
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "contaBancariaId",
                Class = "col s12 m6 l6",
                Label = "Banco cedente",
                Required = true,
                DataUrl = @Url.Action("ContaBancariaBancoEmiteBoleto", "AutoComplete") + "?emiteBoleto=true",
                LabelId = "bancoNome"
            });
            config.Elements.Add(new InputFileUI { Id = "arquivo", Class = "col s12 m6 l6", Label = "Arquivo de retorno do tipo (.ret)", Required = true, Accept = ".ret" });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12",
                Color = "blue",
                Id = "cardDuvidas",
                Title = "Dicas",
                Placeholder = "Selecione o banco de origem do arquivo, e localize o arquivo que deseja importar",
                Action = new LinkUI()
                {
                    Label = "",
                    OnClick = ""
                }
            });

            #region CnabItem
            config.Elements.Add(new TableUI
            {
                Id = "dtRetornoCnabItem",
                Class = "col s12",
                Label = "Retorno Cnab",
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Banco", Value = "0"},
                    new OptionUI { Label = "Cliente", Value = "1"},
                    new OptionUI { Label = "Valor", Value = "2"},
                    new OptionUI { Label = "Data Vencimento", Value = "3"},
                }
            });
            #endregion

            cfg.Content.Add(config);

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

        public JsonResult ImportaArquivoRetorno(string valueArquivo, Guid contaBancariaId)
        {
            var arquivoRetorno = new ArquivoRetornoCnabVM()
            {
                ValueArquivo = valueArquivo,
                ContaBancariaId = contaBancariaId,
            };

            var result = RestHelper.ExecutePostRequest<List<CnabVM>>("arquivoRetorno", JsonConvert.SerializeObject(arquivoRetorno, JsonSerializerSetting.Default));

            if (result == null) throw new Exception("Não foi possível gerar boleto.");

            return Json(new
            {
                success = true,
                data = result.Select(GetDisplayCnab()),
                recordsFiltered = result.Count,
                recordsTotal = result.Count
            }, JsonRequestBehavior.AllowGet);
        }

        private Func<CnabVM, object> GetDisplayCnab()
        {
            return x => new
            {
                id = x.Id,
                contaReceberId = x.ContaReceberId,
                contaBancariaId = x.ContaBancariaCedenteId,
                banco_nome = x.ContaBancariaCedente?.Banco?.Nome,
                pessoa_nome = x.ContaReceber?.Pessoa?.Nome,
                dataVencimento = x.DataVencimento.ToString("dd/MM/yyyy"),
                valorBoleto = x.ValorBoleto.ToString("C", AppDefaults.CultureInfoDefault),
                valorDesconto = x.ValorDesconto,
                status = x.Status,
                statusCssClass = EnumHelper.GetCSS(typeof(StatusCnab), x.Status),
                statusDescription = EnumHelper.GetDescription(typeof(StatusCnab), x.Status),
                statusTooltip = EnumHelper.GetTooltipHint(typeof(StatusCnab), x.Status),
                dataEmissao = x.DataEmissao.ToString("dd/MM/yyyy")
            };
        }
    }
}