using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantService, TenantService>();    
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection("TenantSection"));

TenantSettings tenantSettings;
builder.Configuration.GetSection("TenantSection").Bind(tenantSettings = new TenantSettings());

var defaultProvider = tenantSettings.Configuration.DBProvider;
if (defaultProvider != null)
{
    if (defaultProvider.ToLower()=="mssql")
    {
        builder.Services.AddDbContext<ApplicationDBContext>(options =>
            options.UseSqlServer(tenantSettings.Configuration.ConnectionString));
    }
}
foreach (var tenant in tenantSettings.Tenants)
{
    // Configure DbContext for each tenant
    var connectionString = tenant.ConnectionString ?? tenantSettings.Configuration.ConnectionString;
    using (var scope = builder.Services.BuildServiceProvider().CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        dbContext.Database.SetConnectionString(connectionString);
        if(dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
