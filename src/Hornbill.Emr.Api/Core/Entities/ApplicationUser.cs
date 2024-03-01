using Microsoft.AspNetCore.Identity;

namespace Hornbill.Emr.Api.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public bool IsActive { get; set; }
}
