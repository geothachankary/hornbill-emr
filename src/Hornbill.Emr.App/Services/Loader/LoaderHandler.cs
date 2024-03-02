namespace Hornbill.Emr.App.Services.Loader;

public class LoaderHandler(LoaderService loaderService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        loaderService.Show();
        var response = await base.SendAsync(request, cancellationToken);
        loaderService.Hide();
        return response;
    }
}
