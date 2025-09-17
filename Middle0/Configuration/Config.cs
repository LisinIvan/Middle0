using Duende.IdentityServer.Models;

namespace Middle0.Configuration
{
	public static class Config
	{
		public static IEnumerable<Client> Clients => new List<Client>
	{
		new Client
		{
			ClientId = "client",
			AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
			ClientSecrets = { new Secret("secret".Sha256()) },
			AllowedScopes = { "api1" }
		}
	};

		public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
	{
		new ApiScope("api1", "My API")
	};

		public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
	{
		new IdentityResources.OpenId(),
		new IdentityResources.Profile()
	};
	}
}
