using System.Text.Json;
using Blazored.LocalStorage;
using Hornbill.Emr.App.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Hornbill.Emr.App.Services.Authentication;

public class AuthenticationService(
    HttpClient httpClient,
    ILocalStorageService localStorage,
    AuthenticationStateProvider authenticationStateProvider) : IAuthenticationService
{
    private readonly AppAuthStateProvider _authenticationStateProvider = (AppAuthStateProvider)authenticationStateProvider;

    public async Task<LoginResponseDto> Login(string username, string password)
    {
        var requestParams = new Dictionary<string, string>
        {
            ["username"] = username,
            ["password"] = password,
            ["scope"] = "offline_access",
            ["grant_type"] = "password"
        };

        var result = await httpClient.PostAsync("connect/token", new FormUrlEncodedContent(requestParams));
        if (result.IsSuccessStatusCode)
        {
            var responseContent = await result.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<AuthTokenResponse>(responseContent);
            await localStorage.SetItemAsStringAsync("accessToken", tokenData.AccessToken);
            await localStorage.SetItemAsStringAsync("refreshToken", tokenData.RefreshToken);
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated)
            {
                return new LoginResponseDto(true, string.Empty);
            }
        }
        return new LoginResponseDto(false, "Invalid credentials!!! Please try again");
    }

    public async Task<bool> IsAccessTokenExists()
    {
        var accessToken = await localStorage.GetItemAsStringAsync("accessToken");
        return !string.IsNullOrEmpty(accessToken);
    }

    public async Task Logout()
    {
        await localStorage.RemoveItemAsync("accessToken");
        await localStorage.RemoveItemAsync("refreshToken");
        await _authenticationStateProvider.GetAuthenticationStateAsync();
    }
}

public record LoginResponseDto(bool IsSuccess, string Message);
