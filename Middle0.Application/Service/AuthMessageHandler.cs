using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Middle0.Application.Service
{
	public class AuthMessageHandler : DelegatingHandler
	{
		private readonly IJSRuntime _js;

		public AuthMessageHandler(IJSRuntime js)
		{
			_js = js;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");

			if (!string.IsNullOrEmpty(token))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}
