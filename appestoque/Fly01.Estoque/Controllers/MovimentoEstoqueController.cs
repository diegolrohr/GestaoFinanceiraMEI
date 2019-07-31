using Fly01.Core;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.JQueryDataTable;
using Fly01.Estoque.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fly01.Estoque.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.EstoqueEstoqueRelatorios)]
    public class MovimentoEstoqueController : BaseController<MovimentoEstoqueVM>
    {
        public MovimentoEstoqueController()
        {
            ExpandProperties = "produto($select=descricao),tipoMovimento($select=descricao),inventario($select=descricao)";
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var queryString = base.GetQueryStringDefaultGridLoad();
            queryString.Add("$orderby", "produto/descricao,dataInclusao");
            return queryString;
        }

        public override Func<MovimentoEstoqueVM, object> GetDisplayData()
        {
            return x => new
            {
                dataInclusao = x.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss"),
                observacao = x.Observacao ?? "",
                quantidadeMovimento = x.QuantidadeMovimento != null ? x.QuantidadeMovimento.Value.ToString("R", AppDefaults.CultureInfoDefault) : (0.00).ToString("R", AppDefaults.CultureInfoDefault),
                saldoAntesMovimento = x.SaldoAntesMovimento != null ? x.SaldoAntesMovimento.Value.ToString("R", AppDefaults.CultureInfoDefault) : (0.00).ToString("R", AppDefaults.CultureInfoDefault),
                tipoMovimento_descricao = x.TipoMovimento?.Descricao ?? "",
                produto_descricao = x.Produto?.Descricao ?? "",
                inventario_descricao = x.Inventario?.Descricao ?? "",
            };
        }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            if (Request.QueryString["dataFinal"] != "")
                filters.Add("dataInclusao le ", string.Format("{0}T23:59:59.99Z",Request.QueryString["dataFinal"]));
            if (Request.QueryString["dataInicial"] != "")
                filters.Add(" and dataInclusao ge ", string.Format("{0}T00:00:00.00Z", Request.QueryString["dataInicial"]));

            return base.GridLoad(filters);
        }

        public override ContentResult List()
        {
            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Movimentações de Estoque",
                    Buttons = new List<HtmlUIButton>(GetFormButtonsOnHeader())
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
            };

            var cfgForm = new FormUI
            {
                Id = "fly01frm",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = "fnChangeInput",
                Elements = new List<BaseUI>()
                {
                    new InputHiddenUI()
                    {
                        Id = "dataFinal",
                        Name = "dataFinal"
                    },
                    new InputHiddenUI()
                    {
                        Id = "dataInicial",
                        Name = "dataInicial"
                    }
                }
            };

            var config = new DataTableUI
            {
                Id = "fly01dt",
                UrlGridLoad = Url.Action("GridLoad"),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial" },
                    new DataTableUIParameter() {Id = "dataFinal" }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Options = new DataTableUIConfig
                {
                    PageLength = 20,
                    LengthChange = true
                }
            };

            config.Columns.Add(new DataTableUIColumn() { Priority = 1, Orderable = false, DataField = "produto_descricao", DisplayName = "Produto" });
            config.Columns.Add(new DataTableUIColumn() { Priority = 2, Orderable = false, DataField = "dataInclusao", DisplayName = "Data", Type = "date" });
            config.Columns.Add(new DataTableUIColumn() { Priority = 3, Orderable = false, DataField = "quantidadeMovimento", DisplayName = "Quantidade"});
            config.Columns.Add(new DataTableUIColumn() { Priority = 4, Orderable = false, DataField = "saldoAntesMovimento", DisplayName = "Saldo Anterior"});
            config.Columns.Add(new DataTableUIColumn() { Priority = 5, Orderable = false, DataField = "tipoMovimento_descricao", DisplayName = "Tipo Movimento" });
            config.Columns.Add(new DataTableUIColumn() { Priority = 6, Orderable = false, DataField = "inventario_descricao", DisplayName = "Inventário" });
            config.Columns.Add(new DataTableUIColumn() { Priority = 7, Orderable = false, DataField = "observacao", DisplayName = "Observação:" , Searchable = false});

            cfg.Content.Add(cfgForm);
            cfg.Content.Add(config);
            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        public override List<HtmlUIButton> GetFormButtonsOnHeader()
        {
            var target = new List<HtmlUIButton>();
            return target;
        }

        protected override ContentUI FormJson()
        {
            throw new NotImplementedException();
        }

        protected override List<JQueryDataTableParamsColumn> GetParamsColumns()
        {
            throw new NotImplementedException();
        }
    }
}
