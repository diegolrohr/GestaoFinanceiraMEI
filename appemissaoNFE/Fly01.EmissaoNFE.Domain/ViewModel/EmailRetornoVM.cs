using System.ComponentModel.DataAnnotations;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class EmailRetornoVM
    {
        public bool Autenticacao { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string EmailAdicional { get; set; }
        public string Senha { get; set; }
        public string Servidor { get; set; }
        public bool SSL { get; set; }
        public bool TLS { get; set; }
    }
}
