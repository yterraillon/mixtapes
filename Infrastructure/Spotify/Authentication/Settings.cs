namespace Infrastructure.Spotify.Authentication;

public class Settings
{
    public string ClientId { get; set; } = string.Empty;

    public string AccountsServiceUrl { get; set; } = string.Empty;
    public string AuthorizeUrl { get; set; } = string.Empty;

    public string AccessTokenUrl { get; set; } = string.Empty;

    public string AuthorizeCallbackUrl { get; set; } = string.Empty;
}