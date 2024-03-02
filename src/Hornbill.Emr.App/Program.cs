using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Hornbill.Emr.App;
using Hornbill.Emr.App.Services.Authentication;
using Hornbill.Emr.App.Services.Loader;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddRadzenComponents();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, AppAuthStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});

builder.Services.AddScoped<LoaderService>();
builder.Services.AddScoped<LoaderHandler>();
builder.Services.AddScoped(s =>
{
    var loaderHandler = s.GetRequiredService<LoaderHandler>();
    loaderHandler.InnerHandler = new HttpClientHandler();
    return new HttpClient(loaderHandler)
    {
        BaseAddress = new Uri("http://localhost:5290/")
    };
});

await builder.Build().RunAsync();
