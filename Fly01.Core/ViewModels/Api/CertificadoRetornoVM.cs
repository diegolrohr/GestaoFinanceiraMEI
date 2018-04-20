using System;

namespace Fly01.Core.ViewModels.Api

{
    public class CertificadoRetornoVM
    {
        public string Tipo { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataExpiracao { get; set; }
        public string Emissor { get; set; }
        public string Pessoa { get; set; }
        public string Versao { get; set; }
    }
}
