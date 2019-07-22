using Fly01.Core;
using Fly01.Core.Config;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers;
using Fly01.Core.Mensageria;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Faturamento.Models.Reports;
using Fly01.Faturamento.Models.ViewModel;
using Fly01.Faturamento.ViewModel;
using Fly01.uiJS.Classes;
using Fly01.uiJS.Classes.Elements;
using Fly01.uiJS.Classes.Helpers;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoFaturamentoVendas)]
    public class OrdemVendaController : BaseController<OrdemVendaVM>
    {
        public OrdemVendaController()
        {
            ExpandProperties = "cliente($select=id,nome,email,endereco,bairro,numero,cep,complemento;$expand=cidade($select=nome),estado($select=sigla),pais($select=nome)),condicaoParcelamento($select=id,descricao, qtdParcelas,condicoesParcelamento),grupoTributarioPadrao($select=id,descricao,tipoTributacaoICMS),transportadora($select=id,nome),estadoPlacaVeiculo,formaPagamento,categoria,centroCusto,ufSaidaPais($select=id,nome)";
        }

        private JsonResult GetJson(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        public List<CondicaoParcelamentoParcelaVM> GetSimulacaoContas(OrdemVendaVM ordemVenda)
        {

            var dadosReferenciaSimulacao = new
            {
                valorReferencia = TotalOrdemVendaResponse(ordemVenda.Id.ToString(),ordemVenda.ClienteId.ToString(), ordemVenda.GeraNotaFiscal, ordemVenda.TipoNfeComplementar, ordemVenda.TipoFrete, ordemVenda?.ValorFrete ?? 0)?.Total,
                dataReferencia = ordemVenda?.DataVencimento,
                condicoesParcelamento = ordemVenda?.CondicaoParcelamento?.CondicoesParcelamento,
                qtdParcelas = ordemVenda.CondicaoParcelamento?.QtdParcelas
            };
            return RestHelper.ExecutePostRequest<ResponseSimulacaoVM>("condicaoparcelamentosimulacao", dadosReferenciaSimulacao)?.Items;
        }

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
            ConfiguracaoPersonalizacaoVM personalizacao = null;
            try
            {
                personalizacao = RestHelper.ExecuteGetRequest<ResultBase<ConfiguracaoPersonalizacaoVM>>("ConfiguracaoPersonalizacao", queryString: null)?.Data?.FirstOrDefault();
            }
            catch (Exception)
            {

            }
            var emiteNotaFiscal = personalizacao != null ? personalizacao.EmiteNotaFiscal : true;
            var exibirTransportadora = personalizacao != null ? personalizacao.ExibirStepTransportadoraVendas : true;
            var exibirProdutos = personalizacao != null ? personalizacao.ExibirStepProdutosVendas : true;
            var exibirServicos = personalizacao != null ? personalizacao.ExibirStepServicosVendas : true;

            var produtos = new List<OrdemVendaProdutoVM>();
            if (exibirProdutos) { produtos = GetProdutos(Guid.Parse(id)); };

            var servicos = new List<OrdemVendaServicoVM>();
            if (exibirServicos) { servicos = GetServicos(Guid.Parse(id)); };

            var simulacao = GetSimulacaoContas(OrdemVenda);
            var parcelas = "";

            for (var  i=0; i < simulacao.Count; i++)
            {
                parcelas += $"{simulacao[i].DescricaoParcela} - Vencimento {simulacao[i].DataVencimento.ToString("dd/MM/yyyy")} - {simulacao[i].Valor.ToString("C", AppDefaults.CultureInfoDefault)}    ";
                if (i % 2 != 0 && i > 0 && i < (simulacao.Count-1))
                {
                    parcelas += "\n";
                }    
            }

            bool calculaFrete = (
                (OrdemVenda.TipoFrete == "FOB") && exibirTransportadora
            );
            var resource = string.Format("CalculaTotalOrdemVenda?&ordemVendaId={0}&clienteId={1}&geraNotaFiscal={2}&tipoNfeComplementar={3}&tipoFrete={4}&valorFrete={5}&onList={6}",
                id.ToString(),
                OrdemVenda.ClienteId.ToString(),
                (OrdemVenda.GeraNotaFiscal && emiteNotaFiscal).ToString(),
                OrdemVenda.TipoNfeComplementar,
                (exibirTransportadora ? OrdemVenda.TipoFrete : "SemFrete"),
                (calculaFrete ? OrdemVenda.ValorFrete.ToString().Replace(", ", ".") : 0.ToString()), true);
            var response = RestHelper.ExecuteGetRequest<TotalPedidoNotaFiscalVM>(resource, queryString: null);

            List<ImprimirOrcamentoPedidoVM> reportItems = new List<ImprimirOrcamentoPedidoVM>();
            foreach (OrdemVendaProdutoVM OrdemProduto in produtos)
            {
                reportItems.Add(new ImprimirOrcamentoPedidoVM
                {
                    Id = OrdemVenda.Id.ToString(),
                    CategoriaDescricao = OrdemVenda.Categoria != null ? OrdemVenda.Categoria.Descricao : string.Empty,
                    ClienteNome = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Nome : string.Empty,
                    Endereco = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Endereco : string.Empty,
                    NumeroCliente = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Numero : string.Empty,
                    Bairro = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Bairro : string.Empty,
                    Cidade = OrdemVenda.Cliente != null && OrdemVenda.Cliente.Cidade != null ? OrdemVenda.Cliente.Cidade.Nome : string.Empty,
                    Cep = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.CEP : string.Empty,
                    Complemento = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Complemento : string.Empty,
                    Estado = OrdemVenda.Cliente != null && OrdemVenda.Cliente.Estado != null ? OrdemVenda.Cliente.Estado.Sigla : string.Empty,
                    Pais = OrdemVenda.Cliente != null && OrdemVenda.Cliente.Pais != null ? OrdemVenda.Cliente.Pais.Nome : string.Empty,
                    Data = OrdemVenda.Data.ToString(),
                    CondicaoParcelamentoDescricao = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.Descricao : string.Empty,
                    CondicaoParcelamentoQtdParcelas = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.QtdParcelas : 0,
                    FormaPagamentoDescricao = OrdemVenda.FormaPagamento != null ? OrdemVenda.FormaPagamento.Descricao : string.Empty,
                    PesoBruto = OrdemVenda.PesoBruto.HasValue ? OrdemVenda.PesoBruto.Value : 0,
                    PesoLiquido = OrdemVenda.PesoLiquido.HasValue ? OrdemVenda.PesoLiquido.Value : 0,
                    Status = OrdemVenda.Status.ToString(),
                    TipoFrete = EnumHelper.GetValue(typeof(TipoFrete), OrdemVenda.TipoFrete),
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
                    TotalRetencoesServicos = response.TotalRetencoesServicos.HasValue ? response.TotalRetencoesServicos.Value : 0,
                    ValorFreteTotal = response.ValorFrete.HasValue ? response.ValorFrete.Value : 0,
                    Total = response.Total,
                    Finalidade = OrdemVenda.TipoVenda,
                    Marca = OrdemVenda.Marca,
                    NumeracaoVolumesTrans = OrdemVenda.NumeracaoVolumesTrans,
                    TipoEspecie = OrdemVenda.TipoEspecie,
                    EmiteNotaFiscal = emiteNotaFiscal.ToString(),
                    ExibirProdutos = exibirProdutos.ToString(),
                    ExibirServicos = exibirServicos.ToString(),
                    ExibirTransportadora = exibirTransportadora.ToString(),
                    ParcelaConta = parcelas
                });
            }

            foreach (OrdemVendaServicoVM OrdemServico in servicos)
            {
                reportItems.Add(new ImprimirOrcamentoPedidoVM
                {
                    Id = OrdemVenda.Id.ToString(),
                    CategoriaDescricao = OrdemVenda.Categoria != null ? OrdemVenda.Categoria.Descricao : string.Empty,
                    ClienteNome = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Nome : string.Empty,
                    Endereco = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Endereco : string.Empty,
                    NumeroCliente = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Numero : string.Empty,
                    Bairro = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Bairro : string.Empty,
                    Cidade = OrdemVenda.Cliente != null && OrdemVenda.Cliente.Cidade != null ? OrdemVenda.Cliente.Cidade.Nome : string.Empty,
                    Cep = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.CEP : string.Empty,
                    Complemento = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Complemento : string.Empty,
                    Estado = OrdemVenda.Cliente != null && OrdemVenda.Cliente.Estado != null ? OrdemVenda.Cliente.Estado.Sigla : string.Empty,
                    Pais = OrdemVenda.Cliente != null && OrdemVenda.Cliente.Pais != null ? OrdemVenda.Cliente.Pais.Nome : string.Empty,
                    Data = OrdemVenda.Data.ToString(),
                    CondicaoParcelamentoDescricao = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.Descricao : string.Empty,
                    CondicaoParcelamentoQtdParcelas = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.QtdParcelas : 0,
                    FormaPagamentoDescricao = OrdemVenda.FormaPagamento != null ? OrdemVenda.FormaPagamento.Descricao : string.Empty,
                    PesoBruto = OrdemVenda.PesoBruto.HasValue ? OrdemVenda.PesoBruto.Value : 0,
                    PesoLiquido = OrdemVenda.PesoLiquido.HasValue ? OrdemVenda.PesoLiquido.Value : 0,
                    Status = OrdemVenda.Status.ToString(),
                    TipoFrete = EnumHelper.GetValue(typeof(TipoFrete), OrdemVenda.TipoFrete),
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
                    TotalRetencoesServicos = response.TotalRetencoesServicos.HasValue ? response.TotalRetencoesServicos.Value : 0,
                    ValorFreteTotal = response.ValorFrete.HasValue ? response.ValorFrete.Value : 0,
                    Total = response.Total,
                    Finalidade = OrdemVenda.TipoVenda,
                    Marca = OrdemVenda.Marca,
                    NumeracaoVolumesTrans = OrdemVenda.NumeracaoVolumesTrans,
                    TipoEspecie = OrdemVenda.TipoEspecie,
                    EmiteNotaFiscal = emiteNotaFiscal.ToString(),
                    ExibirProdutos = exibirProdutos.ToString(),
                    ExibirServicos = exibirServicos.ToString(),
                    ExibirTransportadora = exibirTransportadora.ToString(),
                    ParcelaConta = parcelas
                });
            }

            if (!produtos.Any() && !servicos.Any())
            {
                reportItems.Add(new ImprimirOrcamentoPedidoVM
                {
                    Id = OrdemVenda.Id.ToString(),
                    CategoriaDescricao = OrdemVenda.Categoria != null ? OrdemVenda.Categoria.Descricao : string.Empty,
                    ClienteNome = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Nome : string.Empty,
                    Endereco = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Endereco : string.Empty,
                    NumeroCliente = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Numero : string.Empty,
                    Bairro = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Bairro : string.Empty,
                    Cidade = OrdemVenda.Cliente != null && OrdemVenda.Cliente.Cidade != null ? OrdemVenda.Cliente.Cidade.Nome : string.Empty,
                    Cep = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.CEP : string.Empty,
                    Complemento = OrdemVenda.Cliente != null ? OrdemVenda.Cliente.Complemento : string.Empty,
                    Estado = OrdemVenda.Cliente != null && OrdemVenda.Cliente.Estado != null ? OrdemVenda.Cliente.Estado.Sigla : string.Empty,
                    Pais = OrdemVenda.Cliente != null && OrdemVenda.Cliente.Pais != null ? OrdemVenda.Cliente.Pais.Nome : string.Empty,
                    Data = OrdemVenda.Data.ToString(),
                    CondicaoParcelamentoDescricao = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.Descricao : string.Empty,
                    CondicaoParcelamentoQtdParcelas = OrdemVenda.CondicaoParcelamento != null ? OrdemVenda.CondicaoParcelamento.QtdParcelas : 0,
                    FormaPagamentoDescricao = OrdemVenda.FormaPagamento != null ? OrdemVenda.FormaPagamento.Descricao : string.Empty,
                    PesoBruto = OrdemVenda.PesoBruto.HasValue ? OrdemVenda.PesoBruto.Value : 0,
                    PesoLiquido = OrdemVenda.PesoLiquido.HasValue ? OrdemVenda.PesoLiquido.Value : 0,
                    Status = OrdemVenda.Status.ToString(),
                    TipoFrete = EnumHelper.GetValue(typeof(TipoFrete), OrdemVenda.TipoFrete),
                    TipoOrdemVenda = OrdemVenda.TipoOrdemVenda.ToString(),
                    NumeroNota = OrdemVenda.Numero.ToString(),
                    EstadoPlacaVeiculo = OrdemVenda.EstadoPlacaVeiculo != null ? OrdemVenda.EstadoPlacaVeiculo.Sigla : string.Empty,
                    PlacaVeiculo = OrdemVenda.PlacaVeiculo,
                    TransportadoraNome = OrdemVenda.Transportadora != null ? OrdemVenda.Transportadora.Nome : string.Empty,
                    ValorFrete = OrdemVenda.ValorFrete.HasValue ? OrdemVenda.ValorFrete.Value : 0,
                    Observacao = OrdemVenda.Observacao,
                    QuantidadeVolumes = OrdemVenda.QuantidadeVolumes.HasValue ? OrdemVenda.QuantidadeVolumes.Value : 0,
                    Finalidade = OrdemVenda.TipoVenda,
                    Marca = OrdemVenda.Marca,
                    ValorFreteTotal = response.ValorFrete.HasValue ? response.ValorFrete.Value : 0,
                    NumeracaoVolumesTrans = OrdemVenda.NumeracaoVolumesTrans,
                    TipoEspecie = OrdemVenda.TipoEspecie,
                    EmiteNotaFiscal = emiteNotaFiscal.ToString(),
                    ExibirProdutos = exibirProdutos.ToString(),
                    ExibirServicos = exibirServicos.ToString(),
                    ExibirTransportadora = exibirTransportadora.ToString(),
                    ParcelaConta = parcelas
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
        public virtual ActionResult ImprimirOrcamentoPedido(string id)
        {
            try
            {
                var ordemVenda = Get(Guid.Parse(id));
                var reportViewer = new WebReportViewer<ImprimirOrcamentoPedidoVM>(ReportOrcamentoPedido.Instance);
                return File(GetPDFFile(ordemVenda), "application/pdf");
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

                if (ordemVenda.Cliente == null)
                {
                    return JsonResponseStatus.GetFailure("Nenhum cliente foi encontrado.");
                }

                if (string.IsNullOrEmpty(ordemVenda.Cliente.Email))
                {
                    return JsonResponseStatus.GetFailure("Não foi encontrado um email válido para este cliente.");
                }

                if (string.IsNullOrEmpty(empresa.Email))
                {
                    return JsonResponseStatus.GetFailure("Você ainda não configurou um email válido para sua empresa.");
                }

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
            dtOrdemVendaServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "valorOutrasRetencoes", DisplayName = "Outras Retenções", Priority = 7, Type = "currency", Searchable = false, Orderable = false });
            dtOrdemVendaServicosCfg.Columns.Add(new DataTableUIColumn() { DataField = "total", DisplayName = "Total", Priority = 6, Type = "currency", Searchable = false, Orderable = false });

            return dtOrdemVendaServicosCfg;
        }

        public override Dictionary<string, string> GetQueryStringDefaultGridLoad()
        {
            var queryString = base.GetQueryStringDefaultGridLoad();
            queryString.Add("$select", "id,numero,tipoOrdemVenda,data,total,status,tipoVenda,geraNotaFiscal");
            return queryString;
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
                statusDescription = EnumHelper.GetDescription(typeof(Status), x.Status),
                statusCssClass = EnumHelper.GetCSS(typeof(Status), x.Status),
                statusValue = EnumHelper.GetValue(typeof(Status), x.Status),
                total = x.Total.ToString("C", AppDefaults.CultureInfoDefault),
                tipoVenda = x.TipoVenda,
                tipoVendaDescription = EnumHelper.GetDescription(typeof(TipoCompraVenda), x.TipoVenda),
                tipoVendaCssClass = EnumHelper.GetCSS(typeof(TipoCompraVenda), x.TipoVenda),
                tipoVendaValue = EnumHelper.GetValue(typeof(TipoCompraVenda), x.TipoVenda),
                cliente_nome = x.Cliente.Nome,
                geraNotaFiscal = x.GeraNotaFiscal
            };
        }

        public override ContentResult List()
        {
            return ListOrdemVenda();
        }

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
            ConfiguracaoPersonalizacaoVM personalizacao = null;
            try
            {
                personalizacao = RestHelper.ExecuteGetRequest<ResultBase<ConfiguracaoPersonalizacaoVM>>("ConfiguracaoPersonalizacao", queryString: null)?.Data?.FirstOrDefault();
            }
            catch (Exception)
            {

            }
            var emiteNotaFiscal = personalizacao != null ? personalizacao.EmiteNotaFiscal : true;

            var buttonLabel = "Mostrar todas as vendas";
            var buttonOnClick = "fnRemoveFilter";

            if (Request.QueryString["action"] == "GridLoadNoFilter")
            {
                gridLoad = Request.QueryString["action"];
                buttonLabel = "Mostrar vendas do mês atual";
                buttonOnClick = "fnAddFilter";
            }

            var cfg = new ContentUIBase(Url.Action("Sidebar", "Home"))
            {
                History = new ContentUIHistory { Default = Url.Action("Index") },
                Header = new HtmlUIHeader
                {
                    Title = "Vendas",
                    Buttons = new List<HtmlUIButton>(GetListButtonsOnHeaderCustom(buttonLabel, buttonOnClick))
                },
                UrlFunctions = Url.Action("Functions") + "?fns="
            };

            var cfgForm = new FormUI
            {
                Id = "fly01frm",
                UrlFunctions = Url.Action("Functions") + "?fns=",
                ReadyFn = gridLoad == "GridLoad" ? "" : "fnChangeInput",
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

            if (gridLoad == "GridLoad")
            {
                cfgForm.Elements.Add(new PeriodPickerUI()
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
                });
                cfgForm.ReadyFn = "fnUpdateDataFinal";
            }

            cfg.Content.Add(cfgForm);
            var config = new DataTableUI
            {
                Id = "fly01dt",
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
                new DataTableUIAction { OnClickFn = "fnFinalizarPedido", Label = "Finalizar pedido", ShowIf = "(row.status == 'Aberto' && row.tipoOrdemVenda == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnFinalizarFaturarPedido", Label = "Finalizar e faturar", ShowIf = $"({emiteNotaFiscal.ToString().ToLower()} && row.status == 'Aberto' && row.tipoOrdemVenda == 'Pedido')" },
                new DataTableUIAction { OnClickFn = "fnImprimirOrcamentoPedido", Label = "Imprimir" },
                new DataTableUIAction { OnClickFn = "fnEnviarEmailOrcamentoPedido", Label = "Enviar por e-mail" },
                new DataTableUIAction { OnClickFn = "fnClonarPedido", Label = "Clonar Pedido", ShowIf = "(row.status == 'Finalizado' && row.tipoOrdemVenda == 'Pedido')" }
            }));

            config.Columns.Add(new DataTableUIColumn { DataField = "numero", DisplayName = "Número", Priority = 1, Type = "numbers" });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "status",
                DisplayName = "Status",
                Priority = 5,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(Status))),
                RenderFn = "fnRenderEnum(full.statusCssClass, full.statusDescription)"
            });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoOrdemVenda",
                DisplayName = "Tipo",
                Priority = 6,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoOrdemVenda))),
                RenderFn = "fnRenderEnum(full.tipoOrdemVendaCssClass, full.tipoOrdemVendaDescription)"
            });
            config.Columns.Add(new DataTableUIColumn
            {
                DataField = "tipoVenda",
                DisplayName = "Finalidade",
                Priority = 7,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCompraVenda))),
                RenderFn = "fnRenderEnum(full.tipoVendaCssClass, full.tipoVendaDescription)"
            });
            config.Columns.Add(new DataTableUIColumn { DataField = "cliente_nome", DisplayName = "Cliente", Priority = 2 });
            config.Columns.Add(new DataTableUIColumn { DataField = "total", DisplayName = "Total", Priority = 3, Type = "currency", Class = "dt_center" });
            config.Columns.Add(new DataTableUIColumn { DataField = "data", DisplayName = "Data", Priority = 4, Type = "date" });

            cfg.Content.Add(config);

            return Content(JsonConvert.SerializeObject(cfg, JsonSerializerSetting.Front), "application/json");
        }

        protected override ContentUI FormJson() { throw new NotImplementedException(); }

        public override JsonResult GridLoad(Dictionary<string, string> filters = null)
        {
            if (filters == null)
                filters = new Dictionary<string, string>();

            if (Request.QueryString["dataFinal"] != "")
                filters.Add("data le ", Request.QueryString["dataFinal"]);
            if (Request.QueryString["dataInicial"] != "")
                filters.Add(" and data ge ", Request.QueryString["dataInicial"]);

            return base.GridLoad(filters);
        }

        public JsonResult GridLoadNoFilter()
        {
            return GridLoad();
        }

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
            ConfiguracaoPersonalizacaoVM personalizacao = null;
            try
            {
                personalizacao = RestHelper.ExecuteGetRequest<ResultBase<ConfiguracaoPersonalizacaoVM>>("ConfiguracaoPersonalizacao", queryString: null)?.Data?.FirstOrDefault();
            }
            catch (Exception)
            {

            }
            var emiteNotaFiscal = personalizacao != null ? personalizacao.EmiteNotaFiscal : true;
            var exibirTransportadora = personalizacao != null ? personalizacao.ExibirStepTransportadoraVendas : true;
            var exibirProdutos = personalizacao != null ? personalizacao.ExibirStepProdutosVendas : true;
            var exibirServicos = personalizacao != null ? personalizacao.ExibirStepServicosVendas : true;

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
                Id = "fly01mdlfrmVisualizarOrdemVenda",
                Functions = new List<string>() { "fnChangeCollapseExpand" }
            };

            config.Elements.Add(new ButtonGroupUI()
            {
                Id = "fly01btngrpExpandCollapse",
                Class = "col s12 m12",
                OnClickFn = "fnChangeCollapseExpand",
                Label = "Tipo do fator de conversão",
                Options = new List<ButtonGroupOptionUI>
                {
                    new ButtonGroupOptionUI { Id = "btnExpandAll", Value = "expandAll", Label = "Exibir todos"},
                    new ButtonGroupOptionUI { Id = "btnCollapseAll", Value = "collapseAll", Label = "Recolher todos"},
                }
            });

            config.Elements.Add(new InputHiddenUI { Id = "id" });
            config.Elements.Add(new InputHiddenUI { Id = "emiteNotaFiscal", Value = emiteNotaFiscal.ToString() });
            config.Elements.Add(new InputHiddenUI { Id = "exibeStepTransportadora", Value = exibirTransportadora.ToString() });
            config.Elements.Add(new InputHiddenUI { Id = "exibeStepProdutos", Value = exibirProdutos.ToString() });
            config.Elements.Add(new InputHiddenUI { Id = "exibeStepServicos", Value = exibirServicos.ToString() });

            #region Cadastro
            config.Elements.Add(new DivElementUI { Id = "collapseCadastro", Class = "col s12 visible" });

            config.Elements.Add(new InputNumbersUI { Id = "numero", Class = "col s12 m4 ", Label = "Número", Disabled = true });
            config.Elements.Add(new InputDateUI { Id = "data", Class = "col s12 m4", Label = "Data", Disabled = true });
            config.Elements.Add(new SelectUI
            {
                Id = "tipoVenda",
                Class = "col s12 m4",
                Label = "Tipo Venda",
                Value = "Normal",
                Disabled = true,
                Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoCompraVenda)).
                ToList().FindAll(x => "Normal,Devolucao,Complementar".Contains(x.Value)))
            });

            if (emiteNotaFiscal)
            {
                config.Elements.Add(new InputCheckboxUI { Id = "nFeRefComplementarIsDevolucao", Class = "col s12 m4", Label = "NF Referenciada é de Devolução", Disabled = true });
                config.Elements.Add(new SelectUI
                {
                    Id = "tipoNfeComplementar",
                    Class = "col s12 m4",
                    Label = "Tipo Complemento",
                    Disabled = true,
                    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoNfeComplementar))
                        .ToList().FindAll(x => "NaoComplementar,ComplPrecoQtd,ComplIcms".Contains(x.Value)))
                });
                config.Elements.Add(new InputNumbersUI { Id = "chaveNFeReferenciada", Class = "col s12 m4", Label = "Chave SEFAZ Nota Fiscal Referenciada", Disabled = true });
            }
            else
            {
                config.Elements.Add(new InputHiddenUI { Id = "tipoNfeComplementar" });
            }

            config.Elements.Add(new AutoCompleteUI
            {
                Id = "clienteId",
                Class = "col s12",
                Label = "Cliente",
                Disabled = true,
                DataUrl = Url.Action("Cliente", "AutoComplete"),
                LabelId = "clienteNome"
            });
            config.Elements.Add(new TextAreaUI
            {
                Id = "observacao",
                Class = "col s12",
                Label = "Observação",
                MaxLength = 200,
                Disabled = true
            });
            #endregion

            #region Produtos
            if (exibirProdutos)
            {
                config.Elements.Add(new DivElementUI { Id = "collapseProdutos", Class = "col s12 visible" });

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
                config.Elements.Add(new InputCurrencyUI { Id = "totalProdutosDt", Class = "col s12 m4 right", Label = "Total Produtos", Readonly = true });
            }
            #endregion

            #region Serviços
            if (exibirServicos)
            {
                config.Elements.Add(new DivElementUI { Id = "collapseServicos", Class = "col s12 visible" });
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
                    new OptionUI { Label = "Outras Retenções",Value = "5"},
                    new OptionUI { Label = "Total",Value = "4"},
                }
                });
                config.Elements.Add(new InputCurrencyUI { Id = "totalServicosDt", Class = "col s12 m4 right", Label = "Total Serviços", Readonly = true });
            }
            #endregion

            #region Financeiro
            config.Elements.Add(new DivElementUI { Id = "collapseFinanceiro", Class = "col s12 visible" });

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
            config.Elements.Add(new AutoCompleteUI
            {
                Id = "centroCustoId",
                Class = "col s12 m6",
                Label = "Centro de Custo",
                Disabled = true,
                DataUrl = @Url.Action("CentroCusto", "AutoComplete"),
                LabelId = "centroCustoDescricao",
            });
            config.Elements.Add(new InputDateUI { Id = "dataVencimento", Class = "col s12 m6", Label = "Data Vencimento", Disabled = true });
            #endregion

            #region Transporte
            if (exibirTransportadora)
            {
                config.Elements.Add(new DivElementUI { Id = "collapseTransporte", Class = "col s12 visible" });

                config.Elements.Add(new SelectUI
                {
                    Id = "tipoFrete",
                    Class = "col s12 m6",
                    Label = "Tipo Frete",
                    Value = "SemFrete",
                    Disabled = true,
                    Options = new List<SelectOptionUI>(SystemValueHelper.GetUIElementBase(typeof(TipoFrete))),
                });
                config.Elements.Add(new AutoCompleteUI
                {
                    Id = "transportadoraId",
                    Class = "col s12 m6",
                    Label = "Transportadora",
                    Disabled = true,
                    DataUrl = Url.Action("Transportadora", "AutoComplete"),
                    LabelId = "transportadoraNome"
                });
                config.Elements.Add(new InputCustommaskUI
                {
                    Id = "placaVeiculo",
                    Class = "col s12 m4",
                    Label = "Placa Veículo",
                    Disabled = true,
                    Data = new { inputmask = "'mask':'AAA[-9999]|[9A99]', 'showMaskOnHover': false, 'autoUnmask':true, 'greedy':true" }
                });
                config.Elements.Add(new AutoCompleteUI
                {
                    Id = "estadoPlacaVeiculoId",
                    Class = "col s12 m4",
                    Label = "UF Placa Veículo",
                    Disabled = true,
                    DataUrl = Url.Action("Estado", "AutoComplete"),
                    LabelId = "estadoPlacaVeiculoNome"
                });
                config.Elements.Add(new InputCurrencyUI { Id = "valorFrete", Class = "col s12 m4", Label = "Valor Frete", Disabled = true });
                config.Elements.Add(new InputFloatUI { Id = "pesoBruto", Class = "col s12 m4", Label = "Peso Bruto", Digits = 3, Disabled = true });
                config.Elements.Add(new InputTextUI { Id = "marca", Class = "col s12 m4", Label = "Marca", Disabled = true, MaxLength = 60 });
                config.Elements.Add(new InputFloatUI { Id = "pesoLiquido", Class = "col s12 m4", Label = "Peso Líquido", Digits = 3, Disabled = true });
                config.Elements.Add(new InputNumbersUI { Id = "quantidadeVolumes", Class = "col s12 m4", Label = "Quantidade Volumes", Disabled = true });
                config.Elements.Add(new InputTextUI { Id = "tipoEspecie", Class = "col s12 m4", Label = "Tipo Espécie", Disabled = true, MaxLength = 60 });
                config.Elements.Add(new InputTextUI { Id = "numeracaoVolumesTrans", Class = "col s12 m4", Label = "Numeração", Disabled = true, MaxLength = 60 });
            }
            else
            {
                config.Elements.Add(new InputHiddenUI() { Id = "tipoFrete" });
                config.Elements.Add(new InputHiddenUI() { Id = "valorFrete" });
            }
            #endregion

            #region Totais
            config.Elements.Add(new DivElementUI { Id = "collapseTotais", Class = "col s12 visible" });

            if (exibirProdutos)
            {
                config.Elements.Add(new InputCurrencyUI { Id = "totalProdutos", Class = "col s12 m4", Label = "Total produtos", Readonly = true });
                if (emiteNotaFiscal)
                {
                    config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutos", Class = "col s12 m4", Label = "Total impostos produtos incidentes", Readonly = true });
                    config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosProdutosNaoAgrega", Class = "col s12 m4", Label = "Total de impostos não incidentes", Readonly = true });
                }
            }
            if (exibirServicos)
            {
                config.Elements.Add(new InputCurrencyUI { Id = "totalServicos", Class = "col s12 m4", Label = "Total serviços", Readonly = true });
                if (emiteNotaFiscal)
                {
                    config.Elements.Add(new InputCurrencyUI { Id = "totalRetencoesServicos", Class = "col s12 m4", Label = "Total retenções serviços", Readonly = true });
                    config.Elements.Add(new InputCurrencyUI { Id = "totalImpostosServicosNaoAgrega", Class = "col s12 m4", Label = "Total de impostos não incidentes", Readonly = true });
                }
            }
            if (exibirTransportadora)
            {
                config.Elements.Add(new InputCurrencyUI { Id = "totalFrete", Class = "col s12 m4", Label = "Frete a pagar", Readonly = true });
            }
            config.Elements.Add(new InputCurrencyUI { Id = "totalOrdemVenda", Class = "col s12 m4", Label = "Total", Readonly = true });

            if (emiteNotaFiscal)
            {
                config.Elements.Add(new InputTextUI { Id = "naturezaOperacao", Class = "col s12", Label = "Natureza de Operação", Disabled = true });
            }

            config.Elements.Add(new InputCheckboxUI { Id = "movimentaEstoque", Class = "col s12 m4", Label = "Movimenta estoque", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "geraNotaFiscal", Class = "col s12 m4", Label = "Faturar", Disabled = true });
            config.Elements.Add(new InputCheckboxUI { Id = "geraFinanceiro", Class = "col s12 m4", Label = "Gera financeiro", Disabled = true });
            #endregion

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        private TotalPedidoNotaFiscalVM TotalOrdemVendaResponse(string id, string clienteId, bool geraNotaFiscal, string tipoNfeComplementar, string tipoFrete, double? valorFrete = 0)
        {
            var resource = string.Format("CalculaTotalOrdemVenda?&ordemVendaId={0}&clienteId={1}&geraNotaFiscal={2}&tipoNfeComplementar={3}&tipoFrete={4}&valorFrete={5}&onList={6}", id, clienteId, geraNotaFiscal.ToString(), tipoNfeComplementar, tipoFrete, valorFrete.ToString().Replace(",", "."), false);
            var response = RestHelper.ExecuteGetRequest<TotalPedidoNotaFiscalVM>(resource, queryString: null);
            return response;
        }

        [OperationRole(PermissionValue = EPermissionValue.Read)]
        [HttpGet]
        public JsonResult TotalOrdemVenda(string id, string clienteId, bool geraNotaFiscal, string tipoNfeComplementar, string tipoFrete, double? valorFrete = 0)
        {
            try
            {
                var response = TotalOrdemVendaResponse(id, clienteId, geraNotaFiscal, tipoNfeComplementar, tipoFrete, valorFrete);
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
                var response = RestHelper.ExecuteGetRequest<ParametroTributarioVM>("parametrotributario");

                return Json(
                    new { success = true, infcomp = response?.MensagemPadraoNota },
                    JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                var error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                return JsonResponseStatus.GetFailure(error.Message);
            }
        }

        public ContentResult ModalKit()
        {
            ModalUIForm config = new ModalUIForm()
            {
                Title = "Adicionar Kit Produtos/Serviços",
                UrlFunctions = @Url.Action("Functions") + "?fns=",
                ConfirmAction = new ModalUIAction() { Label = "Salvar" },
                CancelAction = new ModalUIAction() { Label = "Cancelar" },
                Action = new FormUIAction
                {
                    Create = @Url.Action("AdicionarKit", "OrdemVenda")
                },
                Id = "fly01mdlfrmOrdemVendaKit",
                ReadyFn = "fnFormReadyOrdemVendaKit"
            };
            config.Elements.Add(new InputHiddenUI { Id = "orcamentoPedidoId" });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "kitId",
                Class = "col s12",
                Label = "Kit",
                Required = true,
                DataUrl = Url.Action("Kit", "AutoComplete"),
                LabelId = "kitDescricao",
            }, ResourceHashConst.FaturamentoCadastrosKit));

            config.Elements.Add(new InputCheckboxUI
            {
                Id = "adicionarProdutos",
                Class = "col s12 m4",
                Label = "Adicionar produtos do kit"
            });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "adicionarServicos",
                Class = "col s12 m4",
                Label = "Adicionar serviços do kit"
            });
            config.Elements.Add(new InputCheckboxUI
            {
                Id = "somarExistentes",
                Class = "col s12 m4",
                Label = "Somar com existentes"
            });

            config.Elements.Add(new LabelSetUI() { Id = "lblGrupoTribuarioPadrao", Label = "Grupo Tributário Padrão", Class = "col s12" });

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "grupoTributarioProdutoIdKit",
                Class = "col s12 m6",
                Label = "Para Produtos",
                Name = "grupoTributarioProdutoId",
                DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
                LabelId = "grupoTributarioProdutoDescricaoKit",
                LabelName = "grupoTributarioProdutoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "GrupoTributario"),
                DataPostField = "descricao"
            }, ResourceHashConst.FaturamentoCadastrosGrupoTributario));

            config.Elements.Add(ElementUIHelper.GetAutoComplete(new AutoCompleteUI
            {
                Id = "grupoTributarioServicoIdKit",
                Class = "col s12 m6",
                Label = "Para Serviços",
                Name = "grupoTributarioServicoId",
                DataUrl = Url.Action("GrupoTributario", "AutoComplete"),
                LabelId = "grupoTributarioServicoDescricaoKit",
                LabelName = "grupoTributarioServicoDescricao",
                DataUrlPostModal = Url.Action("FormModal", "GrupoTributario"),
                DataPostField = "descricao"
            }, ResourceHashConst.FaturamentoCadastrosGrupoTributario));

            config.Elements.Add(new DivElementUI { Id = "infoGrupoTributario", Class = "col s12 text-justify visible", Label = "Informação" });

            #region Helpers            
            config.Helpers.Add(new TooltipUI
            {
                Id = "kitId",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Vai ser adicionado os produtos/serviços cadastrados no Kit."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "grupoTributarioProdutoIdKit",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se desejar, informe um grupo tributário padrão para todos os produtos do kit, que vão ser adicionados ao orçamento/pedido."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "grupoTributarioServicoIdKit",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Se desejar, informe um grupo tributário padrão para todos os serviços do kit, que vão ser adicionados ao orçamento/pedido."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "adicionarProdutos",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe se deseja adicionar todos itens cadastrados no kit, somente serviços ou somente produtos."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "adicionarServicos",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Informe se deseja adicionar todos itens cadastrados no kit, somente serviços ou somente produtos."
                }
            });
            config.Helpers.Add(new TooltipUI
            {
                Id = "somarExistentes",
                Tooltip = new HelperUITooltip()
                {
                    Text = "Os produtos/serviços cadastrados no kit, serão somados com a quantidade já existente no orçamento/pedido."
                }
            });
            #endregion

            return Content(JsonConvert.SerializeObject(config, JsonSerializerSetting.Front), "application/json");
        }

        [OperationRole(PermissionValue = EPermissionValue.Write)]
        [HttpPost]
        public JsonResult AdicionarKit(UtilizarKitVM entityVM)
        {
            try
            {
                RestHelper.ExecutePostRequest("kitordemvenda", JsonConvert.SerializeObject(entityVM, JsonSerializerSetting.Default));
                return JsonResponseStatus.GetSuccess("Itens do kit adicionados com sucesso.");
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
                {
                    tipoCategoria = "1";
                }
                else
                {
                    tipoCategoria = "2";
                }

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