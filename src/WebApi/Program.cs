using Azure.Identity;
using Employee.Repositories;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Ini;

using Shared;
using Shared.AzureIdentity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Employee key

// var EmployeeApiKey = builder.Configuration["Employee:ServiceApiKey"];
//LEGG TIL EGEN USERSECRET ID NÅR DU HAR OPPRETTET EN SECRET
builder.Configuration.AddUserSecrets("d18b48ed-e001-4e15-9d23-f47a671ddd76");



// Bind configuration "TestApp:Settings" section to the Settings object
var appSettingsSection = builder.Configuration
    .GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>(); 

builder.Services.AddSingleton(new AzureServiceTokenProvider()); 

if (appSettings.UseAzureAppConfig)
{
    // Load configuration from Azure App Configuration
    builder.Configuration.AddAzureAppConfiguration(options => options.Connect(appSettings.AzureAppConfigUri, new DefaultAzureCredential()));
}

builder.Services.AddDbContext<EmployeeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDatabase"));
    // https://devblogs.microsoft.com/azure-sdk/azure-identity-with-sql-graph-ef/
    options.AddInterceptors(new AzureAdAuthenticationDbConnectionInterceptor());
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (appSettings.UseAzureAppConfig)
{
    app.UseAzureAppConfiguration();
}

app.UseHttpsRedirection();


app.UseAuthorization();



app.MapControllers();

app.Run();

// Needed for testing:
public partial class Program
{
}