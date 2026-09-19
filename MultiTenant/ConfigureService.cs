using Microsoft.EntityFrameworkCore;

namespace MultiTenant
{
    public static class ConfigureService
    {
        public static IServiceCollection AddMultiTenantservices(this IServiceCollection services,ConfigurationManager configuration)
        {

            // Add services to the container.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.Configure<TenantSettings>(configuration.GetSection("TenantSection"));

            TenantSettings tenantSettings;
            configuration.GetSection("TenantSection").Bind(tenantSettings = new TenantSettings());

            var defaultProvider = tenantSettings.Configuration.DBProvider;
            if (defaultProvider != null)
            {
                if (defaultProvider.ToLower() == "mssql")
                {
                    services.AddDbContext<ApplicationDBContext>(options =>
                        options.UseSqlServer(tenantSettings.Configuration.ConnectionString));
                }
            }
            foreach (var tenant in tenantSettings.Tenants)
            {
                // Configure DbContext for each tenant
                var connectionString = tenant.ConnectionString ?? tenantSettings.Configuration.ConnectionString;
                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                    dbContext.Database.SetConnectionString(connectionString);
                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        dbContext.Database.Migrate();
                    }
                }
            }

            return services;
        }
    }
}
