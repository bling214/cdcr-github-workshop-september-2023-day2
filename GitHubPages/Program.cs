using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GitHubPages;
using PartyTriviaShared.Services;
using Blazored.LocalStorage;
using Syncfusion.Blazor;

WebAssemblyHostBuilder? builder = WebAssemblyHostBuilder.CreateDefault(args);

//Register Syncfusion license 
string? licenseKey = "";
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSyncfusionBlazor();

builder.Services.AddSingleton<OpenTriviaDbService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
