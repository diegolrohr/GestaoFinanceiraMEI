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
using Fly01.Core.Rest;
using Fly01.Core;
using System.Linq;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.ViewModels;
using Fly01.Core.Presentation.JQueryDataTable;

namespace Fly01.Financeiro.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FinanceiroCobrancaArquivoRetorno)]
    public class ArquivoRetornoController : BaseController<DomainBaseVM>
    {
        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton() { Id = "save", Label = "Importar", OnClickFn = "fnImportarArquivo", Type = "submit" });
                target.Add(new HtmlUIButton() { Id = "baixar", Label = "Baixar contas", OnClickFn = "fnBaixarContas", Type = "submit" });
            }

            return target;
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public override ContentResult Form() => base.Form();

        protected override ContentUI FormJson()
        {
            if (!UserCanRead)
                return new ContentUIBase(Url.Action("Sidebar", "Home"));

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory()
                {
                    Default = Url.Action("Create"),
                },
                Header = new HtmlUIHeader()
                {
                    Title = "Importar arquivo de retorno",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                Functions = new List<string> { "fnFormReady" },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var config = new FormUI()
            {
                Id = "fly01frm",
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

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "contaBancariaId",
                Class = "col s12 m6 l6",
                Label = "Conta bancária cedente",
                Required = true,
                DataUrl = @Url.Action("ContaBancariaBancoEmiteBoleto", "AutoComplete") + "?emiteBoleto=true",
                LabelId = "bancoNome"
            }, ResourceHashConst.FinanceiroCadastrosContasBancarias));

            config.Elements.Add(new InputFileUI { Id = "arquivo", Class = "col s12 m6 l6", Label = "Arquivo de retorno do tipo (.ret)", Required = true, Accept = ".ret" });

            cfg.Content.Add(new CardUI
            {
                Class = "col s12",
                Color = "green",
                Id = "cardDuvidas",
                Title = "Dicas",
                Placeholder = "Selecione o banco de origem e localize o arquivo que deseja importar",
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
                    new OptionUI { Label = "Id" },
                    new OptionUI { Label = "Conta ReceberID" },
                    new OptionUI { Label = "IdContaBancaria" },
                    new OptionUI { Label = "Banco", Value = "0" },
                    new OptionUI { Label = "Cliente", Value = "1" },
                    new OptionUI { Label = "Valor", Value = "2" },
                    new OptionUI { Label = "Data Vencimento", Value = "3" },
                }
            });
            #endregion

            cfg.Content.Add(config);

            return cfg;
        }

        public override Func<DomainBaseVM, object> GetDisplayData()
        {
            throw new NotImplementedException();
        }

        public override ContentResult List()
        {
            throw new NotImplementedException();
        }

        [OperationRole(NotApply = true)]
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

        [OperationRole(NotApply = true)]
        [HttpPost]
        public JsonResult BaixarContasReceber(List<Guid> IdBoletos, List<string> IdToBaixa, Guid contaBancariaId)
        {
            try
            {
                var contaFinanceiraBaixaMultipla = MontarContaFinanceiraBaixaMultipla(IdToBaixa, contaBancariaId);
                UpdateStausCnabToBaixado(IdBoletos);
                RestHelper.ExecutePostRequest<ContaFinanceiraBaixaMultiplaVM>("contafinanceirabaixamultipla", JsonConvert.SerializeObject(contaFinanceiraBaixaMultipla, JsonSerializerSetting.Default));

                return JsonResponseStatus.GetSuccess("Baixas realizadas com sucesso!");
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        private void UpdateStausCnabToBaixado(List<Guid> ids)
        {
            var status = ((int)StatusCnab.Baixado).ToString();

            ids.ForEach(x =>
            {
                var resource = $"cnab/{x}";
                RestHelper.ExecutePutRequest(resource, JsonConvert.SerializeObject(new
                {
                    status = status
                }));
            });
        }

        private ContaFinanceiraBaixaMultiplaVM MontarContaFinanceiraBaixaMultipla(List<string> IdToBaixa, Guid contaBancariaId)
        {
            return new ContaFinanceiraBaixaMultiplaVM()
            {
                Data = DateTime.Now,
                ContasFinanceirasGuids = string.Join(",", IdToBaixa),
                ContaBancariaId = contaBancariaId,
                Observacao = "",
                TipoContaFinanceira = TipoContaFinanceira.ContaReceber.ToString()
            };
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns(string ResourceName = "")
        {
            throw new NotImplementedException();
        }
    }
}