namespace Application.ConnectService.Commands.GetSpotifyAuthorizeUrl;

public interface ISpotifyAuthorizeUrlBuilder
{
    public string BuildAuthorizeUrl(IEnumerable<string> scopes);
}