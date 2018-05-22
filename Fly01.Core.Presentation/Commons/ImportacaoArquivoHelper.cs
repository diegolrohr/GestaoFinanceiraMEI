using Fly01.Core.Helpers;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation;
using Fly01.uiJS.Defaults;
using Newtonsoft.Json;

namespace Fly01.Core.Presentation.Commons
{
    public class ImportacaoArquivoHelper
    {
        public static ArquivoVM ImportaArquivo(string descricao, string conteudo)
        {
            var arquivo = new
            {
                descricao = descricao,
                conteudo = Base64Helper.CodificaBase64(conteudo),
                cadastro = "Pessoa",
                md5 = Base64Helper.CalculaMD5Hash(conteudo)
            };

            return RestHelper.ExecutePostRequest<ArquivoVM>("arquivo", JsonConvert.SerializeObject(arquivo, JsonSerializerSetting.Default));
        }
    }
}