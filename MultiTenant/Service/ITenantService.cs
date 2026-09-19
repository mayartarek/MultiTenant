using MultiTenant.Settings;

namespace MultiTenant.Service
{
    public interface ITenantService
    {
        public string? GetDatabaseConnectionString();
        public string? GetDatabaseProvider();
        public Tenant? GetCURRENT_TENANT_ID();

    }
}
