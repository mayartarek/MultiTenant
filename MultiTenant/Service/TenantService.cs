using Microsoft.Extensions.Options;
using MultiTenant.Settings;

namespace MultiTenant.Service
{
    public class TenantService : ITenantService
    {
        private readonly Tenant _currentTenant;
        private HttpContext _httpContext;
        private readonly TenantSettings _tenantSettings;
        public TenantService(IHttpContextAccessor httpContextAccessor,IOptions<TenantSettings> tenantSettings) 
        { 
            _httpContext = httpContextAccessor.HttpContext;
            _tenantSettings = tenantSettings.Value;

           _currentTenant = GetTenantFromRequest() ?? throw new Exception("Tenant not found in request.");

            if (_currentTenant.ConnectionString != null)
            {
                _currentTenant.ConnectionString = _currentTenant.ConnectionString;  
            }


        }
        private Tenant? GetTenantFromRequest()
        {
            var tenantId = _httpContext.Request.Headers["tenantId"].FirstOrDefault();
            if (tenantId == null)
            {
                return null;
            }
            return _tenantSettings.Tenants.FirstOrDefault(t => t.Id == tenantId);
        }
        public Tenant? GetCURRENT_TENANT_ID()
        {
            return _currentTenant;
        }

        public string? GetDatabaseConnectionString()
        {
            var connectionString = _currentTenant !=null? _currentTenant.ConnectionString : _tenantSettings.Configuration.ConnectionString;
            return connectionString;
        }

        public string? GetDatabaseProvider()
        {
            return _tenantSettings.Configuration.DBProvider;
        }
    }
}
