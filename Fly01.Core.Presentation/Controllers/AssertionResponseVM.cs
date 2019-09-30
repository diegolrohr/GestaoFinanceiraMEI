using Fly01.Core.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Fly01.Core.Presentation.Controllers
{
    public class AssertionResponseVM
    {
        public string AccessToken { get; set; }

        public string ClientToken { get; set; }

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
            ClientToken = formCollection["clientToken"];
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
    }
}
