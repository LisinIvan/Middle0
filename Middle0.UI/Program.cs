using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Middle0.UI;
using Middle0.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped<EventServiceUi>();

builder.Services.AddScoped(sp => new HttpClient 
{ 
	BaseAddress = new Uri("https://localhost:7060/") /*new Uri(builder.HostEnvironment.BaseAddress)*/
});

await builder.Build().RunAsync();
