using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace ErrorCentral.Web
{
   public static class IdentityConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>() { 
                new ApiResource(
                    name: "codenation", 
                    displayName:"Codenation C#", 
                    userClaims:new [] { 
                        ClaimTypes.Role, 
                        ClaimTypes.Email,
                    }
                )
            };            
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api1", "My API"),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>() { 
                new Client
                {
                    ClientId = "codenation.api_client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {
                        new Secret("codenation.api_secret".Sha256())
                    },
                    AllowedScopes = {
                        "api1",
                        IdentityServerConstants.StandardScopes.Email
                    },
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }

        public static List<TestUser> GetUsers()
        {            
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "Anne",
                    Password = "pass1234",
                    Claims = new [] { 
                        new Claim(ClaimTypes.Role, "admin"), 
                        new Claim(ClaimTypes.Email, "anne@gmail.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "Vitor",
                    Password = "1234word",
                    Claims = new [] { 
                        new Claim(ClaimTypes.Role, "user"), 
                        new Claim(ClaimTypes.Email, "vitor@mail.com")
                    }
                }
            };
        }
    }
}