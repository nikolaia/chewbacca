using Azure.Identity;

using CvPartner.Repositories;
using CvPartner.Service;

using Employees.Repositories;
using Employees.Service;

using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;

using Refit;

using Shared;
using Shared.AzureIdentity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    // appsettings.Local.json is in the .gitignore. Using a local config instead of userSecrets to avoid references in the .csproj:
    .AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Bind configuration "TestApp:Settings" section to the Settings object
var appSettingsSection = builder.Configuration
    .GetSection("AppSettings");
var appSettings = appSettingsSection.Get<AppSettings>();

builder.Services.AddSingleton(new AzureServiceTokenProvider());

builder.Services.AddScoped<CvPartnerService>();
builder.Services.AddScoped<CvPartnerRepository>();
builder.Services.AddScoped<EmployeesService>();
builder.Services.AddScoped<EmployeesRepository>();

// Refit
builder.Services.AddRefitClient<ICvPartnerApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = appSettings.CvPartner.Uri);

if (appSettings.UseAzureAppConfig)
{
    builder.Services.AddAzureAppConfiguration();
    // Load configuration from Azure App Configuration
    builder.Configuration.AddAzureAppConfiguration(options => options
        .Connect(appSettings.AzureAppConfigUri, new DefaultAzureCredential()).ConfigureKeyVault(
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

var app = builder.Build();

/*
 * Migrate the database.
 * Ideally the app shouldn't have access to alter the database schema, but we do it for simplicity's sake,
 * both here and in the bicep/infrastructure-as-code.
 */
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
    var isInMemoryDatabase = db.Database.ProviderName?.StartsWith("Microsoft.EntityFrameworkCore.InMemory") ?? false;
    if (!isInMemoryDatabase)
    {
        db.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

if (appSettings.UseAzureAppConfig)
{
    app.UseAzureAppConfiguration();
}

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapControllers();

app.Run();

// Needed for testing:
public partial class Program
{
}