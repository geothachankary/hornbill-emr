namespace Hornbill.Emr.Api.Infrastructure.DependencyInjection;

public class InfrastructureOptions
{
    public EfCoreOptions DbContextOptions { get; set; } = new();

    public class EfCoreOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public bool EnableSensitiveLogging { get; set; }
    }
}
