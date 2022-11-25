namespace Application.ConnectService.Commands.GetSpotifyAuthorizeUrl;

public interface IStateProvider
{
    string State { get; }
}