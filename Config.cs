using Duende.IdentityServer.Models;
using System.Collections.Generic;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("proxy-api", "C# Proxy API")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "angular-spa",
                ClientName = "Angular SPA",
                AllowedGrantTypes = GrantTypes.Code, // Use AllowedGrantTypes instead of AllowedGrantTypes
                RequirePkce = true,
                RequireClientSecret = false, // SPA clients typically don't send secrets
                RedirectUris = { "http://localhost:4200/movies" },
                PostLogoutRedirectUris = { "http://localhost:4200/" },
                AllowedScopes = { "openid", "profile", "proxy-api" },
                AllowedCorsOrigins = { "http://localhost:4200" },
                AllowAccessTokensViaBrowser = true
            }
        };
}