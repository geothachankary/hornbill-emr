using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Hornbill.Emr.App.Services.Authentication;

public class AppAuthStateProvider(
    ILocalStorageService localStorage,
    HttpClient httpClient) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        var accessToken = await localStorage.GetItemAsStringAsync("accessToken");
        if (string.IsNullOrEmpty(accessToken))
        {
            return authenticationState;
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var claimsIdentity = new ClaimsIdentity(ParseClaimsFromJwt(accessToken), "hornbill");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        authenticationState = new AuthenticationState(claimsPrincipal);
        NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));

        return authenticationState;
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt) => new JsonWebTokenHandler().ReadJsonWebToken(jwt).Claims;
}
