using System;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class CertificadoVM : EntidadeVM
    {
        public string Certificado { get; set; }
        public string Senha { get; set; }
        public string MD5 { get; set; }
    }
}
