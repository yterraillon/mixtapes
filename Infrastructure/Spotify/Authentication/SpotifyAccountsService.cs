using System.Text.Json;
using System.Text.Json.Serialization;
using Application.ConnectService.Commands.VerifySpotifyAuthentication;
using Microsoft.Extensions.Options;

namespace Infrastructure.Spotify.Authentication;

public class SpotifyAccountsService : ISpotifyAccountsService 
{
    private readonly HttpClient _client;
    private readonly CodeProvider _codeProvider;

    private readonly Settings _settings;

    public SpotifyAccountsService(HttpClient client, IOptions<Settings> settings, CodeProvider codeProvider)
    {
        _client = client;
        _codeProvider = codeProvider;
        _settings = settings.Value;
    }

    public async Task<(string accessToken, string refreshToken, int expiresIn)> GetAccessTokenAsync(string code)
    {
        var parameters = new Dictionary<string, string>
        {
            {"client_id", _settings.ClientId},
            {"grant_type", "authorization_code"},
            {"code", code},
            {"redirect_uri", _settings.AuthorizeCallbackUrl + "/authorize"},
            {"code_verifier", _codeProvider.Verifier}
        };

        var tokens = await GetTokenAsync(new Uri(_settings.AccessTokenUrl), parameters);
        return (tokens.AccessToken, tokens.RefreshToken, tokens.ExpiresIn);
    }

    public async Task<(string accessToken, int expiresIn)> RefreshTokenAsync(string refreshToken)
    {
        var parameters = new Dictionary<string, string>
        {
            {"client_id", _settings.ClientId},
            {"grant_type", "refresh_token"},
            {"refresh_token", refreshToken}
        };

        var tokens = await GetTokenAsync(new Uri(_settings.AccessTokenUrl), parameters);
        return (tokens.AccessToken, tokens.ExpiresIn);
    }

    private async Task<TokensClientResponse> GetTokenAsync(Uri requestUri, IDictionary<string, string> parameters, CancellationToken cancellationToken = new())
    {
        var response = await _client
            .SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = parameters != null ? new FormUrlEncodedContent(parameters!) : null
            }, cancellationToken);

        response.EnsureSuccessStatusCode();

        var content = await response.Content
            .ReadAsStringAsync(cancellationToken);
        
        return JsonSerializer.Deserialize<TokensClientResponse>(content) ?? throw new Exception("Failed to deserialize response content.");
    }
    
    public class TokensClientResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}