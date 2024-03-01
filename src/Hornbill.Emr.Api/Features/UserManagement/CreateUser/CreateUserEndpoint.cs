using Hornbill.Emr.Api.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hornbill.Emr.Api.Features.UserManagement.CreateUser;

public class CreateUserEndpoint : Endpoint<CreateUserRequest, string>
{
    public UserManager<ApplicationUser> UserManager { get; set; }

    public override void Configure()
    {
        Post("users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var user = new ApplicationUser
        {
            UserName = req.Email,
            EmailConfirmed = true,
            IsActive = true,
        };

        var result = await UserManager.CreateAsync(user, req.Password);
        await SendAsync(result.Succeeded ? user.Id : string.Empty);
    }
}

public record CreateUserRequest(string Email, string Password);
