namespace Hornbill.Emr.Api.Infrastructure.Localization;

public interface IAppLocalizer
{
    string this[string key] { get; }

    Dictionary<string, string> GetAllResources();

    string Localize(string key);
}
