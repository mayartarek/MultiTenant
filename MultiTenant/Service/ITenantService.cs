using MultiTenant.Settings;

namespace MultiTenant.Service
{
    public interface ITenantService
    {
         string? GetDatabaseConnectionString();
         string? GetDatabaseProvider();
         Tenant? GetCURRENT_TENANT_ID();

    }
}
