using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.JSInterop;

public class MinimalAuthProvider : AuthenticationStateProvider
{
	private readonly IJSRuntime _js;
	public MinimalAuthProvider(IJSRuntime js) => _js = js;

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
		if (string.IsNullOrWhiteSpace(token))
			return Anonymous();

		try
		{
			var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
			var identity = new ClaimsIdentity(jwt.Claims, "jwt");
			return new AuthenticationState(new ClaimsPrincipal(identity));
		}
		catch
		{
			return Anonymous();
		}
	}

	public void Notify() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	private static AuthenticationState Anonymous() =>
		new(new ClaimsPrincipal(new ClaimsIdentity()));
}
