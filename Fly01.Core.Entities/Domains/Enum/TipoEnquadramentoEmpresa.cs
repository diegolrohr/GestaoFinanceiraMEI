using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    //http://www.planalto.gov.br/ccivil_03/Leis/LCP/Lcp123.htm
    //Recomendamos fortemente que você acesse o site do Planalto para ler tanto a Lei Complementar n.º 155 quanto a Lei Complementar n.º 123 atualizada e conferir em qual anexo a sua empresa se enquadra.
    //Também peça ajuda ao seu contador sempre que possível.
    public enum TipoEnquadramentoEmpresa
    {
        [Subtitle("Anexo1", "Anexo I - Empresas de comércio; lojas em geral...")]
        Anexo1 = 1,

        [Subtitle("Anexo2", "Anexo II - Empresas industriais/fábricas...")]
        Anexo2 = 2,

        [Subtitle("Anexo3", "Anexo III - Empresas de serviços de instalação, reparos e manutenção; de medicina e odontologia; agências de viagens; academias; escritórios de contabilidade; laboratórios...")]
        Anexo3 = 3,

        [Subtitle("Anexo4", "Anexo IV - Empresas que fornecem serviço de limpeza, vigilância, obras, construção de imóveis; serviços advocatícios...")]
        Anexo4 = 4,

        [Subtitle("Anexo5", "Anexo V - Empresas de serviços de auditoria, tecnologia, jornalismo, engenharia, publicidade...")]
        Anexo5 = 5
    }
}