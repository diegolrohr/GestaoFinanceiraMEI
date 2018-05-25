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

            cfg.Content.Add(config);

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
            var entityContaBancaria = GetContaBancaria(contaBancariaId);
            var formatoArquivo = 1;
            var boletos = new Boleto2Net.Boletos()
            {
                Banco = Boleto2Net.Banco.Instancia(Convert.ToInt32(entityContaBancaria.Data.FirstOrDefault().Banco.Codigo))
            };

            boletos.Banco.Cedente = GetCedente(entityContaBancaria);

            byte[] byteArray = Encoding.ASCII.GetBytes(valueArquivo);
            MemoryStream pConteudo = new MemoryStream(byteArray);

            var arquivoRetorno = new Boleto2Net.ArquivoRetorno(boletos.Banco, (Boleto2Net.TipoArquivo)formatoArquivo);
            var boletosRetorno = arquivoRetorno.LerArquivoRetorno(pConteudo);

            return null;
        }

        private Boleto2Net.Cedente GetCedente(ResultBase<ContaBancariaVM> entityContaBancaria)
        {
            var contaBancaria = entityContaBancaria.Data.FirstOrDefault();
            var dadosCedente = ApiEmpresaManager.GetEmpresa(SessionManager.Current.UserData.PlatformUrl);

            return new Boleto2Net.Cedente
            {
                CPFCNPJ = dadosCedente.CNPJ,
                Nome = dadosCedente.NomeFantasia,
                Observacoes = "",
                ContaBancaria = new Boleto2Net.ContaBancaria
                {
                    Agencia = contaBancaria.Agencia,
                    DigitoAgencia = contaBancaria.DigitoAgencia,
                    OperacaoConta = "",
                    Conta = contaBancaria.Conta,
                    DigitoConta = contaBancaria.DigitoConta,
                    CarteiraPadrao = "11",
                    VariacaoCarteiraPadrao = "019",
                    TipoCarteiraPadrao = Boleto2Net.TipoCarteira.CarteiraCobrancaSimples,
                    TipoFormaCadastramento = Boleto2Net.TipoFormaCadastramento.ComRegistro,
                    TipoImpressaoBoleto = Boleto2Net.TipoImpressaoBoleto.Empresa,
                    TipoDocumento = Boleto2Net.TipoDocumento.Tradicional
                },
                Codigo = "",
                CodigoDV = "",
                CodigoTransmissao = ""
            };
        }

        private ResultBase<ContaBancariaVM> GetContaBancaria(Guid contaBancariaId)
        {
            var resourceName = AppDefaults.GetResourceName(typeof(ContaBancariaVM));

            var queryString = new Dictionary<string, string>();
            queryString.AddParam("$expand", "banco($select=codigo)");
            queryString.AddParam("$filter", $"id eq {contaBancariaId}");
                      
            return RestHelper.ExecuteGetRequest<ResultBase<ContaBancariaVM>>(resourceName, queryString);
        }
    }
}