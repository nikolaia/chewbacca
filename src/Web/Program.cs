using ApplicationCore.Interfaces;
using ApplicationCore.Services;

using Azure.Identity;

using CronScheduler.Extensions.Scheduler;

using Infrastructure;
using Infrastructure.ApiClients;
using Infrastructure.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Refit;

using Shared;
using Shared.AzureIdentity;

using Web;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    if (builder.Environment.EnvironmentName.Equals("Development"))
    {
        options.AddPolicy("DashCorsPolicy",
        policy =>
        {
            policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("https://dash.variant.no", "http://localhost:3000");
        });
    }
    else
    {
        {
            options.AddPolicy("DashCorsPolicy",
            policy =>
            {
                policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("https://dash.variant.no");
            });
        }
    }
});

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    // appsettings.Local.json is in the .gitignore. Using a local config instead of userSecrets to avoid references in the .csproj:
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();



// Bind configuration "TestApp:Settings" section to the Settings object
var appSettingsSection = builder.Configuration
    .GetSection("AppSettings");
var initialAppSettings = appSettingsSection.Get<AppSettings>();

if (initialAppSettings == null) throw new Exception("Unable to load app settings");


builder.Services.AddScoped<IBemanningRepository, BemanningRepository>();
builder.Services.AddScoped<BemanningRepository>();

builder.Services.AddScoped<BlobStorageService>();
builder.Services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();

builder.Services.AddScoped<IEmergencyContactRepository, EmergencyContactRepository>();
builder.Services.AddScoped<EmergencyContactRepository>();

builder.Services.AddScoped<IEmployeeAllergiesAndDietaryPreferencesRepository, EmployeeAllergiesAndDietaryPreferencesRepository>();
builder.Services.AddScoped<EmployeeAllergiesAndDietaryPreferencesRepository>();

builder.Services.AddScoped<EmployeesService>();

builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();
builder.Services.AddScoped<EmployeesRepository>();

builder.Services.AddScoped<FilteredUids>();
builder.Services.AddScoped<CvPartnerRepository>();

// ApplicationCore
builder.Services.AddScoped<OrchestratorService>();

// Refit
builder.Services.AddRefitClient<ICvPartnerApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = initialAppSettings.CvPartner.Uri);

if (initialAppSettings.UseAzureAppConfig)
{
    builder.Services.AddAzureAppConfiguration();
    // Load configuration from Azure App Configuration
    builder.Configuration.AddAzureAppConfiguration(options => options
        .Connect(initialAppSettings.AzureAppConfigUri, new DefaultAzureCredential()).ConfigureKeyVault(
            vaultOptions =>
            {
                vaultOptions.SetCredential(new DefaultAzureCredential());
            }));
}

builder.Services.Configure<AppSettings>(appSettingsSection);

builder.Services.AddDbContextPool<EmployeeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDatabase"));
    // https://devblogs.microsoft.com/azure-sdk/azure-identity-with-sql-graph-ef/
    options.AddInterceptors(new AzureAdAuthenticationDbConnectionInterceptor());
});

builder.Services.AddScheduler(ctx =>
{
    const string jobName = nameof(OrchestratorJob);
    ctx.AddJob(
        sp =>
        {
            var options = sp.GetRequiredService<IOptionsMonitor<SchedulerOptions>>().Get(jobName);
            return new OrchestratorJob(sp, options);
        },
        options =>
        {
            options.CronSchedule = "0 4 * * *";
            options.RunImmediately = true;
        },
        jobName: jobName);
});

var app = builder.Build();

app.UseCors();

/*
 * Migrate the database.
 * Ideally the app shouldn't have access to alter the database schema, but we do it for simplicity's sake,
 * both here and in the bicep/infrastructure-as-code.
 */
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
    var isInMemoryDatabase = db.Database.ProviderName?.StartsWith("Microsoft.EntityFrameworkCore.InMemory") ?? false;

    if (isInMemoryDatabase)
    {
        db.Database.EnsureCreated();
    }
    else
    {
        db.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

if (initialAppSettings.UseAzureAppConfig)
{
    app.UseAzureAppConfiguration();
}

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

// Example of Minimal API instead of using Controllers
app.MapGet("/healthcheck",
    async ([FromServices] EmployeeContext db, [FromServices] IOptionsSnapshot<AppSettings> appSettings) =>
    {
        app.Logger.LogInformation("Getting employees from database");

        var dbCanConnect = await db.Database.CanConnectAsync();
        var healthcheck = appSettings.Value.Healthcheck;

        var response = new HealthcheckResponse()
        {
            Database = dbCanConnect,
            KeyVault = healthcheck.KeyVault,
            AppConfig = healthcheck.AppConfig
        };

        return response;
    });

app.Run();

// Needed for testing:
namespace Web
{
    public partial class Program
    {
    }
}