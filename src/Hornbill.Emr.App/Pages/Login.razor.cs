using Hornbill.Emr.App.Services.Authentication;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Hornbill.Emr.App.Pages;

public partial class Login : ComponentBase
{
    [Inject]
    private IAuthenticationService AuthenticationService { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    private string ErrorMessage { get; set; }

    private async Task OnLogin(LoginArgs args)
    {
        var loginResponse = await AuthenticationService.Login(args.Username, args.Password);

        if (loginResponse.IsSuccess)
        {
            NavigationManager.NavigateTo("/");
        }
        else
        {
            ErrorMessage = loginResponse.Message;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (await AuthenticationService.IsAccessTokenExists())
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
