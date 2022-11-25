using Application.ConnectService.Commands.GetSpotifyAuthorizeUrl;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Infrastructure.Spotify.Authentication;

public class SpotifyAuthorizeUrlBuilder : ISpotifyAuthorizeUrlBuilder
{
    private readonly CodeProvider _codeProvider;
    private readonly IStateProvider _stateProvider;
    private readonly Settings _settings;
    private const string ResponseType = "code";
    private const string CodeChallengeMethod = "S256";

    public SpotifyAuthorizeUrlBuilder(IOptions<Settings> settings, CodeProvider codeProvider, IStateProvider stateProvider)
    {
        _codeProvider = codeProvider;
        _stateProvider = stateProvider;
        _settings = settings.Value;
    }

    public string BuildAuthorizeUrl(IEnumerable<string> scopes)
    {
        var parameters = new Dictionary<string, string>
        {
            { "client_id", _settings.ClientId },
            { "response_type", ResponseType },
            { "redirect_uri", BuildRedirectUrl() },
            { "code_challenge_method", CodeChallengeMethod },
            { "code_challenge", _codeProvider.Challenge },
            { "scope", FormatScopes(scopes) },
            { "state", _stateProvider.State }
        };

        return QueryHelpers.AddQueryString(_settings.AuthorizeUrl, parameters);

        string BuildRedirectUrl() => _settings.AccountsServiceUrl + "/authorize";
        static string FormatScopes(IEnumerable<string> scopes) => string.Join(" ", scopes);
    }
}