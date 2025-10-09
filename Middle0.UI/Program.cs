using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Middle0.UI;
using Middle0.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);


builder.Configuration.AddJsonFile("appsettings.json");

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"];

builder.Services.AddScoped<EventServiceUi>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, MinimalAuthProvider>();


builder.Services.AddScoped(sp => new HttpClient 
{ 
	BaseAddress = new Uri(apiBaseUrl!) /*new Uri(builder.HostEnvironment.BaseAddress)*/
});

await builder.Build().RunAsync();
