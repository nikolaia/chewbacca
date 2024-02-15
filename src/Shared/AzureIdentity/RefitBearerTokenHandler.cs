using System.Net.Http.Headers;

using Azure.Core;

namespace Shared.AzureIdentity;

public class RefitBearerTokenHandler : DelegatingHandler
{
    private readonly TokenCredential _credential;
    private readonly string _scope;

    public RefitBearerTokenHandler(TokenCredential credential, string scope)
    {
        _credential = credential ?? throw new ArgumentNullException(nameof(credential));
        _scope = scope ?? throw new ArgumentNullException(nameof(scope));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _credential.GetTokenAsync(new TokenRequestContext(new[] { _scope }), cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        return await base.SendAsync(request, cancellationToken);
    }
}
