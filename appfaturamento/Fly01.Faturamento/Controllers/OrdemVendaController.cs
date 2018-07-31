using Fly01.Faturamento.ViewModel;
using Fly01.Faturamento.Helpers;
using Fly01.Faturamento.Models.Reports;
using Fly01.Faturamento.Models.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Defaults;
using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Fly01.Core.Mensageria;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.ViewModels;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoFaturamentoVendas)]
    public class OrdemVendaController : BaseController<OrdemVendaVM>
    {
        public OrdemVendaController()
        {
            ExpandProperties = "cliente($select=id,nome,email),grupoTributarioPadrao($select=id,descricao,tipoTributacaoICMS),transportadora($select=id,nome),estadoPlacaVeiculo,condicaoParcelamento,formaPagamento,categoria";
        }

        private JsonResult GetJson(object data)
            => Json(data, JsonRequestBehavior.AllowGet);

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public List<OrdemVendaProdutoVM> GetProdutos(Guid id)
        {
            var queryString = new Dictionary<string, string>();
            queryString.AddParam("$filter", $"ordemVendaId eq {id}");
            queryString.AddParam("$expand", "produto");

            return RestHelper.ExecuteGetRequest<ResultBase<OrdemVendaProdutoVM>>("OrdemVendaProduto", queryString).Data;
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public List<OrdemVendaServicoVM> GetServicos(Guid id)
        {
            var queryString = new Dictionary<string, string>();
            queryString.AddParam("$filter", $"ordemVendaId eq {id}");
            queryString.AddParam("$expand", "servico");

            return RestHelper.ExecuteGetRequest<ResultBase<OrdemVendaServicoVM>>("OrdemVendaServico", queryString).Data;
        }

        private List<ImprimirOrcamentoPedidoVM> GetDadosOrcamentoPedido(string id, OrdemVendaVM OrdemVenda)
        {
            var produtos = GetProdutos(Guid.Parse(id));
            var servicos = GetServicos(Guid.Parse(id));
            bool calculaFrete = (
                ((OrdemVenda.TipoFrete == "CIF" || OrdemVenda.TipoFrete == "Remetente") && OrdemVenda.TipoVenda == "Normal") ||
                ((OrdemVenda.TipoFrete == "FOB" || OrdemVenda.TipoFrete == "Destinatario") && OrdemVenda.TipoVenda == "Devolucao")
            );
            var resource = string.Format("CalculaTotalOrdemVenda?&ordemVendaId={0}&clienteId={1}&geraNotaFiscal={2}&tipoVenda={3}&tipoFrete={4}&valorFrete={5}&onList={6}", id.ToString(), OrdemVenda.ClienteId.ToString(), OrdemVenda.GeraNotaFiscal.ToString(),
                 OrdemVenda.TipoVenda, OrdemVenda.TipoFrete, calculaFrete ? OrdemVenda.ValorFrete.ToString().Replace(", ", ".") : 0.ToString(), true);
            var response = RestHelper.ExecuteGetRequest<TotalOrdemVendaVM>(resource, queryString: null);

            List<ImprimirOrcamentoPedidoVM> reportItems = new List<ImprimirOrcamentoPedidoVM>();

            foreach (OrdemVendaProdutoVM OrdemProduto in produtos)

                reportItems.Add(new ImprimirOrcamentoPedidoVM
                {
                    Id = OrdemVenda.Id.ToString(),
                    CategoriaDescricao = OrdemVenda.Categoria != null ? OrdemVenda.Categoria.Descricao : string.Empty,
                    ClienteNome = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Nome : string.Empty,
                    Data = OrdemVenda.Data.ToString(),
                    CondicaoParcelamentoDescricao = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.Descricao : string.Empty,
                    CondicaoParcelamentoQtdParcelas = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.QtdParcelas : 0,
                    FormaPagamentoDescricao = OrdemVenda.FormaPagamento != null ? OrdemVenda.FormaPagamento.Descricao : string.Empty,
                    PesoBruto = OrdemVenda.PesoBruto.HasValue ? OrdemVenda.PesoBruto.Value : 0,
                    PesoLiquido = OrdemVenda.PesoLiquido.HasValue ? OrdemVenda.PesoLiquido.Value : 0,
                    Status = OrdemVenda.Status.ToString(),
                    TipoFrete = OrdemVenda.TipoFrete.ToString(),
                    TipoOrdemVenda = OrdemVenda.TipoOrdemVenda.ToString(),
                    NumeroNota = OrdemVenda.Numero.ToString(),
                    EstadoPlacaVeiculo = OrdemVenda.EstadoPlacaVeiculo != null ? OrdemVenda.EstadoPlacaVeiculo.Sigla : string.Empty,
                    PlacaVeiculo = OrdemVenda.PlacaVeiculo,
                    TransportadoraNome = OrdemVenda.Transportadora != null ? OrdemVenda.Transportadora.Nome : string.Empty,
                    ValorFrete = OrdemVenda.ValorFrete.HasValue ? OrdemVenda.ValorFrete.Value : 0,
                    Observacao = OrdemVenda.Observacao,
                    QuantidadeVolumes = OrdemVenda.QuantidadeVolumes.HasValue ? OrdemVenda.QuantidadeVolumes.Value : 0,
                    ItemId = OrdemProduto.Produto != null ? OrdemProduto.Produto.Id : Guid.Empty,
                    ItemNome = OrdemProduto.Produto != null ? OrdemProduto.Produto.Descricao : string.Empty,
                    ItemQtd = OrdemProduto.Quantidade,
                    ItemValor = OrdemProduto.Valor,
                    ItemDesconto = OrdemProduto.Desconto,
                    ItemTotal = OrdemProduto.Total,
                    TotalProdutos = response.TotalProdutos.HasValue ? response.TotalProdutos.Value : 0,
                    TotalImpostosProdutos = response.TotalImpostosProdutos.HasValue ? response.TotalImpostosProdutos.Value : 0,
                    TotalServicos = response.TotalServicos.HasValue ? response.TotalServicos.Value : 0,
                    TotalImpostosServicos = response.TotalImpostosServicos.HasValue ? response.TotalImpostosServicos.Value : 0,
                    ValorFreteTotal = response.ValorFrete.HasValue ? response.ValorFrete.Value : 0,
                    Total = response.Total,
                    Finalidade = OrdemVenda.TipoVenda
                });

            foreach (OrdemVendaServicoVM OrdemServico in servicos)

                reportItems.Add(new ImprimirOrcamentoPedidoVM
                {
                    Id = OrdemVenda.Id.ToString(),
                    CategoriaDescricao = OrdemVenda.Categoria != null ? OrdemVenda.Categoria.Descricao : string.Empty,
                    ClienteNome = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Nome : string.Empty,
                    Data = OrdemVenda.Data.ToString(),
                    CondicaoParcelamentoDescricao = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.Descricao : string.Empty,
                    CondicaoParcelamentoQtdParcelas = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.QtdParcelas : 0,
                    FormaPagamentoDescricao = OrdemVenda.FormaPagamento != null ? OrdemVenda.FormaPagamento.Descricao : string.Empty,
                    PesoBruto = OrdemVenda.PesoBruto.HasValue ? OrdemVenda.PesoBruto.Value : 0,
                    PesoLiquido = OrdemVenda.PesoLiquido.HasValue ? OrdemVenda.PesoLiquido.Value : 0,
                    Status = OrdemVenda.Status.ToString(),
                    TipoFrete = OrdemVenda.TipoFrete.ToString(),
                    TipoOrdemVenda = OrdemVenda.TipoOrdemVenda.ToString(),
                    NumeroNota = OrdemVenda.Numero.ToString(),
                    EstadoPlacaVeiculo = OrdemVenda.EstadoPlacaVeiculo != null ? OrdemVenda.EstadoPlacaVeiculo.Sigla : string.Empty,
                    PlacaVeiculo = OrdemVenda.PlacaVeiculo,
                    TransportadoraNome = OrdemVenda.Transportadora != null ? OrdemVenda.Transportadora.Nome : string.Empty,
                    ValorFrete = OrdemVenda.ValorFrete.HasValue ? OrdemVenda.ValorFrete.Value : 0,
                    Observacao = OrdemVenda.Observacao,
                    QuantidadeVolumes = OrdemVenda.QuantidadeVolumes.HasValue ? OrdemVenda.QuantidadeVolumes.Value : 0,
                    ItemId = OrdemServico.Servico != null ? OrdemServico.Servico.Id : Guid.Empty,
                    ItemNome = OrdemServico.Servico != null ? OrdemServico.Servico.Descricao : string.Empty,
                    ItemQtd = OrdemServico.Quantidade,
                    ItemValor = OrdemServico.Valor,
                    ItemDesconto = OrdemServico.Desconto,
                    ItemTotal = OrdemServico.Total,
                    TotalProdutos = response.TotalProdutos.HasValue ? response.TotalProdutos.Value : 0,
                    TotalImpostosProdutos = response.TotalImpostosProdutos.HasValue ? response.TotalImpostosProdutos.Value : 0,
                    TotalServicos = response.TotalServicos.HasValue ? response.TotalServicos.Value : 0,
                    TotalImpostosServicos = response.TotalImpostosServicos.HasValue ? response.TotalImpostosServicos.Value : 0,
                    ValorFreteTotal = response.ValorFrete.HasValue ? response.ValorFrete.Value : 0,
                    Total = response.Total,
                    Finalidade = OrdemVenda.TipoVenda
                });

            if (!produtos.Any() && !servicos.Any())
            {
                reportItems.Add(new ImprimirOrcamentoPedidoVM
                {
                    Id = OrdemVenda.Id.ToString(),
                    CategoriaDescricao = OrdemVenda.Categoria != null ? OrdemVenda.Categoria.Descricao : string.Empty,
                    ClienteNome = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Nome : string.Empty,
                    Data = OrdemVenda.Data.ToString(),
                    CondicaoParcelamentoDescricao = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.Descricao : string.Empty,
                    CondicaoParcelamentoQtdParcelas = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.QtdParcelas : 0,
                    FormaPagamentoDescricao = OrdemVenda.FormaPagamento != null ? OrdemVenda.FormaPagamento.Descricao : string.Empty,
                    PesoBruto = OrdemVenda.PesoBruto.HasValue ? OrdemVenda.PesoBruto.Value : 0,
                    PesoLiquido = OrdemVenda.PesoLiquido.HasValue ? OrdemVenda.PesoLiquido.Value : 0,
                    Status = OrdemVenda.Status.ToString(),
                    TipoFrete = OrdemVenda.TipoFrete.ToString(),
                    TipoOrdemVenda = OrdemVenda.TipoOrdemVenda.ToString(),
                    NumeroNota = OrdemVenda.Numero.ToString(),
                    EstadoPlacaVeiculo = OrdemVenda.EstadoPlacaVeiculo != null ? OrdemVenda.EstadoPlacaVeiculo.Sigla : string.Empty,
                    PlacaVeiculo = OrdemVenda.PlacaVeiculo,
                    TransportadoraNome = OrdemVenda.Transportadora != null ? OrdemVenda.Transportadora.Nome : string.Empty,
                    ValorFrete = OrdemVenda.ValorFrete.HasValue ? OrdemVenda.ValorFrete.Value : 0,
                    Observacao = OrdemVenda.Observacao,
                    QuantidadeVolumes = OrdemVenda.QuantidadeVolumes.HasValue ? OrdemVenda.QuantidadeVolumes.Value : 0,
                    Finalidade = OrdemVenda.TipoVenda
                });
            }

            return reportItems;
        }

        private byte[] GetPDFFile(OrdemVendaVM ordemVenda)
        {
            var reportViewer = new WebReportViewer<ImprimirOrcamentoPedidoVM>(ReportOrcamentoPedido.Instance);
            return reportViewer.Print(GetDadosOrcamentoPedido(ordemVenda.Id.ToString(), ordemVenda), SessionManager.Current.UserData.PlatformUrl);
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public virtual JsonResult ImprimirOrcamentoPedido(string id)
        {
            try
            {
                var ordemVenda = Get(Guid.Parse(id));
                var fileName = "OrdemVenda" + ordemVenda.Numero.ToString() + ".pdf";
                var fileBase64 = Convert.ToBase64String(GetPDFFile(ordemVenda));

                Session.Add(fileName, fileBase64);

                return Json(new
                {
                    success = true,
                    fileName = fileName,
                    recordsFiltered = 1,
                    recordsTotal = 1
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public JsonResult EnviaEmailOrcamentoPedido(string id)
        {
            try
            {
                var empresa = GetDadosEmpresa();
                var ordemVenda = Get(Guid.Parse(id));

                if (ordemVenda.Cliente == null) return JsonResponseStatus.GetFailure("Nenhum cliente foi encontrado.");
                if (string.IsNullOrEmpty(ordemVenda.Cliente.Email)) return JsonResponseStatus.GetFailure("Não foi encontrado um email válido para este cliente.");
                if (string.IsNullOrEmpty(empresa.Email)) return JsonResponseStatus.GetFailure("Você ainda não configurou um email válido para sua empresa.");

                var anexo = File(GetPDFFile(ordemVenda), "application/pdf");
                var tituloEmail = $"{empresa.NomeFantasia} {ordemVenda.TipoOrdemVenda} - Nº {ordemVenda.Numero}".ToUpper();
                var mensagemPrincipal = $"Você está recebendo uma cópia de seu {ordemVenda.TipoOrdemVenda}.".ToUpper();
                var conteudoEmail = Mail.FormataMensagem(EmailFilesHelper.GetTemplate("Templates.OrdemVenda.html").Value, tituloEmail, mensagemPrincipal, empresa.Email);
                var arquivoAnexo = new FileStreamResult(new MemoryStream(anexo.FileContents), anexo.ContentType);

                Mail.Send(empresa.NomeFantasia, ordemVenda.Cliente.Email, tituloEmail, conteudoEmail, arquivoAnexo.FileStream);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        protected DataTableUI GetDtOrdemVendaProdutosCfg()
        {
            DataTableUI dtOrdemVendaProdutosCfg = new DataTableUI
            {
                Parent = "ordemVendaProdutosField",
                Id = "dtOrdemVendaProdutos",
                UrlGridLoad = Url.Action("GetOrdemVendaProdutos", "OrdemVendaProduto"),
                UrlFunctions = Url.Action("Functions", "OrdemVendaProduto") + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                }
            };

            dtOrdemVendaProdutosCfg.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditarOrdemVendaProduto", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluirOrdemVendaProduto", Label = "Excluir" }
            }));

            dtOrdemVendaProdutosCfg.Columns.Add(new DataTableUIColumn() { DataField = "produto_descricao", DisplayName = "Produto", Priority = 1, Searchable = false, Orderable = false });
            dtOrdemVendaProdutosCfg.Columns.Add(new DataTableUIColumn() { DataField = "grupoTributario_descricao", DisplayName = "Grupo Tributário", Priority = 2, Searchable = false, Orderable = false });
            dtOrdemVendaProdutosCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quantidade", Priority = 3, Type = "float", Searchable = false, Orderable = false });
            dtOrdemVendaProdutosCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 4, Type = "currency", Searchable = false, Orderable = false });
            dtOrdemVendaProdutosCfg.Columns.Add(new DataTableUIColumn() { DataField = "desconto", DisplayName = "Desconto", Priority = 5, Type = "currency", Searchable = false, Orderable = false });
            dtOrdemVendaProdutosCfg.Columns.Add(new DataTableUIColumn() { DataField = "total", DisplayName = "Total", Priority = 6, Type = "currency", Searchable = false, Orderable = false });

            return dtOrdemVendaProdutosCfg;
        }

        protected DataTableUI GetDtOrdemVendaServicosCfg()
        {
            DataTableUI dtOrdemVendaServicosCfg = new DataTableUI
            {
                Parent = "ordemVendaServicosField",
                Id = "dtOrdemVendaServicos",
                UrlGridLoad = Url.Action("GetOrdemVendaServicos", "OrdemVendaServico"),
                UrlFunctions = Url.Action("Functions", "OrdemVendaServico") + "?fns=",
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter { Id = "id", Required = true }
                }
            };

            dtOrdemVendaServicosCfg.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnEditarOrdemVendaServico", Label = "Editar" },
                new DataTableUIAction { OnClickFn = "fnExcluirOrdemVendaServico", Label = "Excluir" }
            }));

            dtOrdemVendaServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "servico_descricao", DisplayName = "Serviço", Priority = 1, Searchable = false, Orderable = false });
            dtOrdemVendaServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "grupoTributario_descricao", DisplayName = "Grupo Tributário", Priority = 2, Searchable = false, Orderable = false });
            dtOrdemVendaServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "quantidade", DisplayName = "Quantidade", Priority = 3, Type = "float", Searchable = false, Orderable = false });
            dtOrdemVendaServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "valor", DisplayName = "Valor", Priority = 4, Type = "currency", Searchable = false, Orderable = false });
            dtOrdemVendaServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "desconto", DisplayName = "Desconto", Priority = 5, Type = "currency", Searchable = false, Orderable = false });
            dtOrdemVendaServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "total", DisplayName = "Total", Priority = 6, Type = "currency", Searchable = false, Orderable = false });

            return dtOrdemVendaServicosCfg;
        }

        public override Func<OrdemVendaVM, object> GetDisplayData()
        {
            return x => new
            {
                id = x.Id.ToString(),
                numero = x.Numero.ToString(),
                tipoOrdemVenda = x.TipoOrdemVenda,
                tipoOrdemVendaDescription = EnumHelper.GetDescription(typeof(TipoOrdemVenda), x.TipoOrdemVenda),
                tipoOrdemVendaCssClass = EnumHelper.GetCSS(typeof(TipoOrdemVenda), x.TipoOrdemVenda),
                tipoOrdemVendaValue = EnumHelper.GetValue(typeof(TipoOrdemVenda), x.TipoOrdemVenda),
                data = x.Data.ToString("dd/MM/yyyy"),
                status = x.Status,
                statusDescription = EnumHelper.GetDescription(typeof(StatusOrdemVenda), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(StatusOrdemVenda), x.Status),
                statusValue = EnumHelper.GetValue(typeof(StatusOrdemVenda), x.Status),
                tipoVenda = x.TipoVenda,
                tipoVendaDescription = EnumHelper.GetDescription(typeof(TipoVenda), x.TipoVenda),
                tipoVendaCssClass = EnumHelper.GetCSS(typeof(TipoVenda), x.TipoVenda),
                tipoVendaValue = EnumHelper.GetValue(typeof(TipoVenda), x.TipoVenda),
                cliente_nome = x.Cliente.Nome,
                geraNotaFiscal = x.GeraNotaFiscal
            };
        }

        public override ContentResult List()
            => ListOrdemVenda();

        public List<HtmlUIButton> GetListButtonsOnHeaderCustom(string buttonLabel, string buttonOnClick)
        {
            var target = new List<HtmlUIButton>();

            if (UserCanWrite)
            {
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo pedido", OnClickFn = "fnNovoPedido" });
                target.Add(new HtmlUIButton { Id = "new", Label = "Novo orçamento", OnClickFn = "fnNovoOrcamento" });
                target.Add(new HtmlUIButton { Id = "filterGrid1", Label = buttonLabel, OnClickFn = buttonOnClick });

            }

            return target;
        }
        
        public ContentResult ListOrdemVenda(string gridLoad = "GridLoad")
        {
            var buttonLabel = "Mostrar todas as vendas";
            var buttonOnClick = "fnRemoveFilter";

            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                gridLoad = Request.QueryString["action"];
                buttonLabel = "Mostrar vendas do mês atual";
                buttonOnClick = "fnAddFilter";
            }

            var cfg = new ContentUI
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Vendas",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeaderCustom(buttonLabel, buttonOnClick))
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            if (gridLoad == "GridLoad")
            {
                var cfgForm = new FormUI
                {
                    ReadyFn = "fnUpdateDataFinal",
                    UrlFunctions = Url.Action("Functions") + "?fns=",
                    Elements = new List<BaseUI>()
                    {
                        new PeriodPickerUI()
                        {
                            Label = "Selecione o período",
                            Id = "mesPicker",
                            Name = "mesPicker",
                            Class = "col s12 m6 offset-m3 l4 offset-l4",
                            DomEvents = new List<DomEventUI>()
                            {
                                new DomEventUI()
                                {
                                    DomEvent = "change",
                                    Function = "fnUpdateDataFinal"
                                }
                            }
                        },
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

                cfg.Content.Add(cfgForm);
            }

            var config = new DataTableUI
            {
                UrlGridLoad = Url.Action(gridLoad),
                Parameters = new List<DataTableUIParameter>
                {
                    new DataTableUIParameter() {Id = "dataInicial", Required = (gridLoad == "GridLoad") },
                    new DataTableUIParameter() {Id = "dataFinal", Required = (gridLoad == "GridLoad") }
                },
                UrlFunctions = Url.Action("Functions") + "?fns=",
                Functions = new List<string>() { "fnRenderEnum" }
            };

            config.Actions.AddRange(GetActionsInGrid(new List<DataTableUIAction>()
            {
                new DataTableUIAction { OnClickFn = "fnVisualizar", Label = "Visualizar" },
                new DataTableUIAction { OnClickFn = "fnEditarPedido", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemVenda == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnEditarOrcamento", Label = "Editar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemVenda == 'Orcamento')" },
                new DataTableUIAction { OnClickFn = "fnExcluir", Label = "Excluir", ShowIf = "(row.status == 'Aberto')" },
                new DataTableUIAction { OnClickFn = "fnConverterParaPedido", Label = "Converter em pedido", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemVenda == 'Orcamento')" },
                new DataTableUIAction { OnClickFn = "fnFinalizarPedido", Label = "Finalizar pedido", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemVenda == 'Pedido' && row.geraNotaFiscal == false)" },
                new DataTableUIAction { OnClickFn = "fnFinalizarFaturarPedido", Label = "Finalizar e faturar", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemVenda == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnImprimirOrcamentoPedido", Label = "Imprimir" },
                new DataTableUIAction { OnClickFn = "fnEnviarEmailOrcamentoPedido", Label = "Enviar por email" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Número", Priority = 1, Type = "numbers" });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 2,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(StatusOrdemVenda))),
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription)"
            });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoOrdemVenda",
                DisplayName = "Tipo",
                Priority = 3,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoOrdemVenda))),
                RenderFn = "fnRenderEnum(full.tipoOrdemVendaCssClass, full.tipoOrdemVendaDescription)"
            });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoVenda",
                DisplayName = "Finalidade",
                Priority = 4,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoVenda))),
                RenderFn = "fnRenderEnum(full.tipoVendaCssClass, full.tipoVendaDescription)"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "cliente_nome", DisplayName = "Cliente", Priority = 5 });
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 6, Type = "date" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            filters.Add("data le ", Request.QueryString["dataFinal"]);
            filters.Add(" and data ge ", Request.QueryString["dataInicial"]);

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
            => base.GridLoad();

        [HttpPost]
        public override JsonResult Create(OrdemVendaVM entityVM)
        {
            try
            {
                var postResponse = RestHelper.ExecutePostRequest(ResourceName, JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                OrdemVendaVM postResult = JsonConvert.DeserializeObject<OrdemVendaVM>(postResponse);
                var response = new JsonResult
                {
                    Data = new { success = true, message = AppDefaults.EditSuccessMessage, id = postResult.Id.ToString(), numero = postResult.Numero.ToString() }
                };
                return (response);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public ContentResult Visualizar()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Visualizar",
                UrlFunctions = @Url.Action("Functions", "OrdemVenda") + "?fns=",
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("Create"),
                    Edit = @Url.Action("Edit"),
                    Get = @Url.Action("Json") + "/",
                    List = @Url.Action("List")
                },
                ReadyFn = "fnFormReadyVisualizarOrdemVenda",
                Id = "fly01mdlfrmVisualizarOrdemVenda"
            };

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m4", Label = "Número", Disabled = true });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoVenda",
                Class = "col s12 m4",
                Label = "Tipo Venda",
                Value = "Normal",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoVenda)).
                ToList().FindAll(x => "Normal,Devolucao".Contains(x.Value)))
            });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m4", Label = "Data", Disabled = true });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "clienteId",
                Class = "col s12 m6",
                Label = "Cliente",
                Disabled = true,
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNome"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "grupoTributarioPadraoId",
                Class = "col s12 m6",
                Label = "Grupo Tributário Padrão",
                Disabled = true,
                DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
                LabelId = "grupoTributarioPadraoDescricao"
            });
            config.Elements.Add(new TextAreaUI
            {
                Id = "observacao",
                Class = "col s12",
                Label = "Observação",
                MaxLength = 200,
                Disabled = true
            });

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "formaPagamentoId",
                Class = "col s12 m6",
                Label = "Forma Pagamento",
                Disabled = true,
                DataUrl = Url.Action("FormaPagamento", "AutoComplete"),
                LabelId = "formaPagamentoDescricao"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "condicaoParcelamentoId",
                Class = "col s12 m6",
                Label = "Condição Parcelamento",
                Disabled = true,
                DataUrl = Url.Action("CondicaoParcelamento", "AutoComplete"),
                LabelId = "condicaoParcelamentoDescricao"
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "categoriaId",
                Class = "col s12 m6",
                Label = "Categoria",
                Disabled = true,
                DataUrl = @Url.Action("Categoria", "AutoComplete"),
                LabelId = "categoriaDescricao",
            });
            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m6", Label = "Data Vencimento", Disabled = true });

            config.Elements.Add(new SelectUI
            {
                Id = "tipoFrete",
                Class = "col s12 m3",
                Label = "Tipo Frete",
                Value = "SemFrete",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFrete))),
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "transportadoraId",
                Class = "col s12 m8",
                Label = "Transportadora",
                Disabled = true,
                DataUrl = Url.Action("Transportadora", "AutoComplete"),
                LabelId = "transportadoraNome"
            });
            config.Elements.Add(new InputCustommaskUI
            {
                Id = "placaVeiculo",
                Class = "col s12 m3",
                Label = "Placa Veículo",
                Disabled = true,
                Data = new { inputmask = "'mask':'AAA-9999', 'showMaskOnHover': false, 'autoUnmask':true" }
            });
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "estadoPlacaVeiculoId",
                Class = "col s12 m3",
                Label = "UF Placa Veículo",
                Disabled = true,
                DataUrl = Url.Action("Estado", "AutoComplete"),
                LabelId = "estadoPlacaVeiculoNome"
            });
            config.Elements.Add(new InputCurrencyUI { Id = "valorFrete", Class = "col s12 m3", Label = "Valor Frete", Disabled = true });
            config.Elements.Add(new InputFloatUI { Id = "pesoBruto", Class = "col s12 m3", Label = "Peso Bruto", Disabled = true });
            config.Elements.Add(new InputTextUI { Id = "marca", Class = "col s12 m3", Label = "Marca", Disabled = true });

            config.Elements.Add(new InputFloatUI { Id = "pesoLiquido", Class = "col s12 m3", Label = "Peso Líquido", Disabled = true });
            config.Elements.Add(new InputNumbersUI { Id = "quantidadeVolumes", Class = "col s12 m3", Label = "Quantidade Volumes", Disabled = true });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoEspecie",
                Class = "col s12 m3",
                Label = "Tipo Espécie",
                //Value = "SemFrete",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoEspecie))),
            });
            config.Elements.Add(new InputNumbersUI { Id = "numeracaoVolumesTrans", Class = "col s12 m3", Label = "Numeração", Disabled = true });

            config.Elements.Add(new InputTextUI { Id = "naturezaOperacao", Class = "col s12", Label = "Natureza de Operação", Disabled = true });

            config.Elements.Add(new LabelSetUI { Id = "labelSetTotais", Class = "col s12", Label = "Totais" });
            config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m6", Label = "Total produtos", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m6", Label = "Frete a pagar", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m6", Label = "Total impostos produtos incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutosNaoAgrega", Class = "col s12 m6", Label = "Total de impostos não incidentes", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalServicos", Class = "col s12 m6", Label = "Total serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosServicos", Class = "col s12 m6", Label = "Total impostos serviços", Readonly = true });
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrdemVenda", Class = "col s12", Label = "Total (produtos + serviços + impostos + frete)", Readonly = true });
            config.Elements.Add(new InputCheckboxUI { Id = "movimentaEstoque", Class = "col s12 m4", Label = "Movimenta estoque", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "geraNotaFiscal", Class = "col s12 m4", Label = "Faturar", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "geraFinanceiro", Class = "col s12 m4", Label = "Gera financeiro", Disabled = true });

            config.Elements.Add(new LabelSetUI { Id = "labelSetProdutos", Class = "col s12", Label = "Produtos" });
            config.Elements.Add(new TableUI
            {
                Id = "ordemVendaProdutosDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Produto", Value = "0"},
                    new OptionUI { Label = "GrupoTributário", Value = "0"},
                    new OptionUI { Label = "Quant.", Value = "1"},
                    new OptionUI { Label = "Valor",Value = "2"},
                    new OptionUI { Label = "Desconto",Value = "3"},
                    new OptionUI { Label = "Total",Value = "4"},
                }
            });
            config.Elements.Add(new LabelSetUI { Id = "labelSetServico", Class = "col s12", Label = "Serviços" });
            config.Elements.Add(new TableUI
            {
                Id = "ordemVendaServicosDataTable",
                Class = "col s12",
                Disabled = true,
                Options = new List<OptionUI>
                {
                    new OptionUI { Label = "Serviço", Value = "0"},
                    new OptionUI { Label = "GrupoTributário", Value = "0"},
                    new OptionUI { Label = "Quant.", Value = "1"},
                    new OptionUI { Label = "Valor",Value = "2"},
                    new OptionUI { Label = "Desconto",Value = "3"},
                    new OptionUI { Label = "Total",Value = "4"},
                }
            });
            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        [HttpGet]
        public JsonResult TotalOrdemVenda(string id, string clienteId, bool geraNotaFiscal, string tipoVenda, string tipoFrete, double? valorFrete = 0)
        {
            try
            {
                var resource = string.Format("CalculaTotalOrdemVenda?&ordemVendaId={0}&clienteId={1}&geraNotaFiscal={2}&tipoVenda={3}&tipoFrete={4}&valorFrete={5}&onList={6}", id, clienteId, geraNotaFiscal.ToString(), tipoVenda, tipoFrete, valorFrete.ToString().Replace(",", "."), false);
                var response = RestHelper.ExecuteGetRequest<TotalOrdemVendaVM>(resource, queryString: null);

                return Json(
                    new { success = true, total = response },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        [HttpGet]
        public JsonResult GetInformacoesComplementares()
        {
            try
            {
                var response = RestHelper.ExecuteGetRequest<ResultBase<ParametroTributarioVM>>("parametrotributario");

                return Json(
                    new { success = true, infcomp = response.Data.FirstOrDefault()?.MensagemPadraoNota },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        #region OnDemmand
        [HttpPost]
        public JsonResult NovaCategoria(string term)
        {
            try
            {
                var tipoCategoria = "";
                var tipoCarteira = Request.QueryString["tipo"];

                if (tipoCarteira == "Receita")
                    tipoCategoria = "1";
                else
                    tipoCategoria = "2";

                var entity = new CategoriaVM
                {
                    Descricao = term,
                    TipoCarteira = tipoCategoria
                };

                var resourceName = AppDefaults.GetResourceName(typeof(CategoriaVM));
                var data = RestHelper.ExecutePostRequest<CategoriaVM>(resourceName, entity, AppDefaults.GetQueryStringDefault());

                return JsonResponseStatus.Get(new ErrorInfo() { HasError = false }, Operation.Create, data.Id);
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }
        #endregion
    }
}