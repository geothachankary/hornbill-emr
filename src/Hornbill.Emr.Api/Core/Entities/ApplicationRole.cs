using Microsoft.AspNetCore.Identity;

namespace Hornbill.Emr.Api.Core.Entities;

public class ApplicationRole : IdentityRole
{
    public bool IsActive { get; set; }
}
