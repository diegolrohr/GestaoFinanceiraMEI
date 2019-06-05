using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens;
using System.Text;


namespace Fly01.Core.Helpers
{
    public static class JWTHelper
    {
        public const string SECRET_KEY_JWT = "*_uPjr=*FLy01*WvEY6$6k9mmG";

        private static SymmetricSecurityKey GetSecurityKey(string secretKey)
        {
            return new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }

        public static string Encode(Dictionary<string, string> payload, string issuer, DateTime? tokenExpires = null, string secretKey = SECRET_KEY_JWT)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(issuer))
                    throw new ArgumentException("Issuer inválido.");

                var claimsIdentity = new ClaimsIdentity(payload.Select(item => new Claim(item.Key, item.Value)), "Custom");

                var signingCredentials = new SigningCredentials(GetSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
                var securityTokenDescriptor = new SecurityTokenDescriptor() { TokenIssuerName = issuer, Subject = claimsIdentity, SigningCredentials = signingCredentials };
                if (!tokenExpires.HasValue)
                    tokenExpires = DateTime.Now.AddDays(3);

                securityTokenDescriptor.Lifetime = new System.IdentityModel.Protocols.WSTrust.Lifetime(DateTime.Now.AddHours(-12), (DateTime)tokenExpires);

                var tokenHandler = new JwtSecurityTokenHandler();
                var plainToken = new JwtSecurityTokenHandler().CreateToken(securityTokenDescriptor);
                return tokenHandler.WriteToken(plainToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public static ClaimsPrincipal Decode(string token, string[] issuers, string secretKey = SECRET_KEY_JWT)
        {
            try
            {
                if (!issuers.Any())
                    throw new ArgumentException("Issuer inválido.");

                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidIssuers = issuers,
                    IssuerSigningKey = GetSecurityKey(secretKey)
                };
                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken validatedToken;
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                return claimsPrincipal;
            }
            catch (Exception ex)
            {
                return new ClaimsPrincipal();
            }
        }

        public static string GetClaimValue(ClaimsPrincipal claimsPrincipal, string claimName)
        {
            ClaimsIdentity claims = claimsPrincipal.Identity as ClaimsIdentity;
            Claim claim = claims.Claims.FirstOrDefault(c => c.Type.ToUpper().Equals(claimName.ToUpper()));
            if (claim != null)
                return claim.Value;

            throw new ApplicationException(String.Format("Claim '{0}' não encontrado.", claimName));
        }
    }
}