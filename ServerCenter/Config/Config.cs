using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ServerCenter
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            var client = new Client
            {
                ClientId = "client",

                // secret for authentication
                ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                // scopes that client has access to
                AllowedScopes = { "api1", IdentityServerConstants.StandardScopes.OpenId }
            };

            if (Global.Config.IdentityMode == Global.IdentityMode.ClientCredentials)
            {
                // no interactive user, use the clientid/secret for authentication
                client.AllowedGrantTypes = GrantTypes.ClientCredentials;
            }
            else if (Global.Config.IdentityMode == Global.IdentityMode.OwnerPassword)
            {
                client.AllowedGrantTypes = GrantTypes.ResourceOwnerPassword;

                client.RequireClientSecret = false;
            }
            else if (Global.Config.IdentityMode == Global.IdentityMode.AuthorizationCode)
            {
                client.AllowedGrantTypes = GrantTypes.CodeAndClientCredentials;

                client.RequireClientSecret = true;

                client.RedirectUris = new [] { "http://localhost:5001/signin-oidc" };
                client.FrontChannelLogoutUri = "http://localhost:5001/signout-oidc" ;
                client.PostLogoutRedirectUris = new [] { "http://localhost:5001/signin-callback-oidc" };

                client.AllowOfflineAccess = true;
            }

            return new List<Client>
            {
                client
            };
        }

        public static IEnumerable<TestUser> GetTestUsers()
        {
            yield return new TestUser
            {
                SubjectId = "1",
                Username = "admin1",
                Password = "admin1"
            };
            yield return new TestUser
            {
                SubjectId = "2",
                Username = "admin2",
                Password = "admin2"
            };
        }
    }
}
