using SoftRig.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Shared;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using IdentityModel.Client;
using IdentityModel;

namespace SoftRig.Repositories;

using Newtonsoft.Json;

public class SoftRigRepository
{
    private readonly IOptionsSnapshot<AppSettings> _appSettings;
    private readonly ISoftRigApiClient _softRigApiClient;
    private readonly ILogger<SoftRigRepository> _logger;

    public SoftRigRepository(IOptionsSnapshot<AppSettings> appSettings, ISoftRigApiClient softRigApiClient, ILogger<SoftRigRepository> logger)
    {
        _appSettings = appSettings;
        _softRigApiClient = softRigApiClient;
        _logger = logger;
    }

    // public async Task<String> GetCompanyKey(string token, string companyName)
    // {

    //     var client = new HttpClient
    //     {
    //         BaseAddress = new Uri(_appSettings.Value.SoftRig.Endpoint.ToString())
    //     };
    //     client.SetBearerToken(token);

    //     try
    //     {
    //         var response = client.GetAsync(_appSettings.Value.SoftRig.Endpoint.ToString() + "init/companies").Result;

    //         Console.WriteLine(response);
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine("Exception: " + ex.Message + " \n stack " + ex.StackTrace);

    //     }

    // }

    public async Task<List<GadgetJournalEntry>> GetAllGadgetEntries(string token)
    {

        Console.WriteLine("SoftRigRepository:GetAllGadgetEntries");

        var client = new HttpClient
        {
            BaseAddress = new Uri(_appSettings.Value.SoftRig.APIBaseUrl.ToString())
        };

        client.SetBearerToken(token);
        client.DefaultRequestHeaders.Add("CompanyKey", _appSettings.Value.SoftRig.CompanyKey);

        const int skip = 0;

        try
        {
            const int countPerFetch = 50;
            //string url = $"biz/statistics?skip={skip}&top={countPerFetch}&model=Employee&select=BusinessRelationInfo.Name as name,Employments.StartDate as startDate,Employments.EndDate as endDate&expand=BusinessRelationInfo,Employments&distinct=true";
            string url = "biz/accounts?select=ID,AccountNumber,AccountName,VatTypeID";

            var response = await client.GetAsync(_appSettings.Value.SoftRig.APIBaseUrl.ToString() + url);


            // var content =
            //     JsonConvert.DeserializeObject<EmployeesJson>(await employeeResponse.Content
            //         .ReadAsStringAsync());

            Console.WriteLine("Result from request");
            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented, new JsonConverter[] { }));

            Console.WriteLine(JsonConvert.SerializeObject(await response.Content.ReadAsStringAsync(), Formatting.Indented, new JsonConverter[] { }));
        }
        catch (Exception ex)
        {
            Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented));

            Console.WriteLine("Exception: " + ex.Message + " \n stack " + ex.StackTrace);
        }

        return null;



        // var apiResponse = await _softRigApiClient.GetAllGadgetEntries(_appSettings.Value.SoftRig.Token);

        // if (apiResponse is { IsSuccessStatusCode: true, Content: not null })
        // {
        //     return apiResponse.Content.ToList();
        // }

        // _logger.LogCritical(apiResponse.Error, "Exception when calling SoftRig");
        // return new List<GadgetJournalEntry>();
    }


    public async Task<IdentityModel.Client.TokenResponse> RequestTokenAsync()
    {
        var client = new HttpClient();

        var disco = await client.GetDiscoveryDocumentAsync(_appSettings.Value.SoftRig.Uri.ToString());
        if (disco.IsError) throw new Exception(disco.Error);
        var clientID = _appSettings.Value.SoftRig.ClientID;

        var clientToken = CreateClientToken(clientID, disco.TokenEndpoint!);

        var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            Scope = "AppFramework",

            ClientAssertion =
                {
                    Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                    Value = clientToken
                }
        });

        Console.WriteLine(response);
        Console.WriteLine(response.ToString());
        Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented,
                   new JsonConverter[] { }));

        if (response.IsError) throw new Exception(response.Error);
        return response;
    }

    private string CreateClientToken(string clientId, string audience)
    {
        Console.WriteLine("Path to pem file");
        Console.WriteLine(_appSettings.Value.SoftRig.PathToPemFile);
        //var dir = new DirectoryInfo("/home/doraoline/Nedlastinger");
        var certificate = new X509Certificate2(_appSettings.Value.SoftRig.PathToPemFile, _appSettings.Value.SoftRig.PemPassword);
        var now = DateTime.UtcNow;

        var securityKey = new X509SecurityKey(certificate);
        var signingCredentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.RsaSha256
        );

        var token = new JwtSecurityToken(
                clientId,
                audience,
                new List<Claim>()
                {
                new Claim("jti", Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Subject, clientId),
                new Claim(JwtClaimTypes.IssuedAt, now.ToEpochTime().ToString(), ClaimValueTypes.Integer64)
                },
                now,
                now.AddMinutes(1),
                signingCredentials
            );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

}