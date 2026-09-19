using Microsoft.EntityFrameworkCore;
using MultiTenant.Service;
using MultiTenant.Settings;

namespace MultiTenant.Data
{
    public class ApplicationDBContext:DbContext
    {
        private readonly ITenantService tenantService;
        public  string _currentTenantId;
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options,ITenantService tenantService)
            : base(options)
        {
            this.tenantService = tenantService;
            var currentTenant = tenantService.GetCURRENT_TENANT_ID();
            string? tenantId = currentTenant!.Id;
            this._currentTenantId = tenantId;
        }

        public DbSet<Entities.Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = tenantService.GetDatabaseConnectionString();
            if (tenantService.GetDatabaseProvider()!.ToLower() == "mssql") {
                optionsBuilder.UseSqlServer(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Product>().HasQueryFilter(p => p.TenantId == _currentTenantId);
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var item in ChangeTracker.Entries<IMustHaveTenant>().Where(a=>a.State == EntityState.Added))
            {
                item.Entity.TenantId = _currentTenantId;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
