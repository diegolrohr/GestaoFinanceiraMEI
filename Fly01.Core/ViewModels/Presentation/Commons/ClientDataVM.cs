using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    //[Serializable]
    //public class ClientDataVM
    //{
    //    private static readonly string erroPermissaoAcesso = "Usuário não possui acesso a plataforma Bemacash.";

    //    public string Fly01Url { get; set; }
    //    public int PlatformUrlId { get; set; }
    //    public string PlatformUserName { get; set; }
    //    public string FirstUser { get; set; }
    //    public string FirstPassword { get; set; }
    //    public string FirstConsumerKey { get; set; }
    //    public string FirstSignature { get; set; }
    //    public string FirstDocument { get; set; }
    //    public string FirstToken { get; set; }
    //    public string FirstTokenSecret { get; set; }
    //    public string NumeroSerie { get; set; }
    //    public string TenantIdCompany { get; set; }
    //    public string TenantIdBranch { get; set; }
    //    public int LicenseQuantity { get; set; }
    //    public string BearerToken { get; set; } //Token Gateway
    //    public string SSOProviderCompanyId { get; set; } //Id da Company no Identity
    //    public string SSOProviderUserId { get; set; } //id do Usuário no Identity
    //    public string SSOProviderGroupId { get; set; } //Id do grupo/plataforma no Identity

    //    public bool UserIsAdmin { get; set; }
    //    public List<PermissionResponseVM> Permissions { get; set; }
    //}

    public class AssertionResponseVM
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public int ExpiresIn { get; set; }

        public string CodigoMaxime { get; set; }

        public string Username { get; set; }

        public string LicenseExpiration { get; set; }

        public bool Trial { get; set; }

        public string Issued { get; set; }

        public string Expires { get; set; }

        public string PlatformUrl { get; set; }

        public string UserEmail { get; set; }

        public AssertionResponseVM(NameValueCollection formCollection)
        {
            var trial = false;
            bool.TryParse(formCollection["trial"], out trial);

            AccessToken = formCollection["access_token"];
            TokenType = formCollection["token_type"];
            CodigoMaxime = formCollection["codigoMaxime"];
            Username = formCollection["userName"];
            LicenseExpiration = formCollection["licenseExpiration"];
            Trial = trial;
            Issued = formCollection[".issued"];
            Expires = formCollection[".expires"];
            PlatformUrl = formCollection["platformUrl"];
            UserEmail = formCollection["userEmail"];
        }
    }

    public class DataUserPermissionVM
    {
        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("permissions")]
        public List<PermissionResponseVM> Items { get; set; }
    }

    public class UserDataCookieVM
    {
        public string Fly01Url { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public bool RememberMe { get; set; }
    }
}
