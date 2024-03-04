using System.Reflection;
using Microsoft.Extensions.Localization;

namespace Hornbill.Emr.Api.Infrastructure.Localization;

public class AppLocalizer : IAppLocalizer
{
    private readonly IStringLocalizer _localizer;

    public AppLocalizer(IStringLocalizerFactory factory)
    {
        var type = typeof(ISharedResource);
        var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
        _localizer = factory.Create("ISharedResource", assemblyName.Name);
    }

    public string this[string key] => _localizer[key];

    public Dictionary<string, string> GetAllResources()
    {
        var localizedStrings = _localizer.GetAllStrings();
        return localizedStrings.Select(x => new
        {
            Key = x.Name,
            x.Value
        }).ToDictionary(x => x.Key, x => x.Value);
    }

    public string Localize(string key) => _localizer[key];
}
