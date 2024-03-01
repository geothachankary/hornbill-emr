namespace Hornbill.Emr.App.Services.Authentication;

public interface IAuthenticationService
{
    Task<bool> IsAccessTokenExists();

    Task<LoginResponseDto> Login(string username, string password);

    Task Logout();
}
