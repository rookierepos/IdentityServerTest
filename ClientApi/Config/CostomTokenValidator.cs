using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ClientApi.Config
{
    public class CostomTokenValidator : ISecurityTokenValidator
    {
        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; }

        public bool CanReadToken(string securityToken)
        {
            return !string.IsNullOrEmpty(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;

            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            if (securityToken.StartsWith("expcat"))
            {
                identity.AddClaim(new Claim("name", "admin"));
                identity.AddClaim(new Claim("role", "admin"));
                identity.AddClaim(new Claim("AdminOnly", "true"));
            }
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
