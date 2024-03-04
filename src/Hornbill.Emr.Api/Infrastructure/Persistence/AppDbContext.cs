using System.Reflection;
using Hornbill.Emr.Api.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace Hornbill.Emr.Api.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    public DbSet<Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // apply identity related entity configurations
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().ToTable("application_users");
        builder.Entity<ApplicationRole>().ToTable("application_roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("user_claims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("user_logins");
        builder.Entity<IdentityUserRole<string>>().ToTable("user_roles");
        builder.Entity<IdentityUserToken<string>>().ToTable("user_tokens");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("role_claims");

        // apply openiddict related entity configurations
        builder.Entity<OpenIddictEntityFrameworkCoreApplication>().ToTable("open_iddict_applications");
        builder.Entity<OpenIddictEntityFrameworkCoreAuthorization>().ToTable("open_iddict_authorizations");
        builder.Entity<OpenIddictEntityFrameworkCoreScope>().ToTable("open_iddict_scopes");
        builder.Entity<OpenIddictEntityFrameworkCoreToken>().ToTable("open_iddict_tokens");

        // apply other entity configurations
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
