namespace Application.ConnectService.Commands.VerifySpotifyAuthentication;

public interface ISpotifyAccountsService
{
    public Task<(string accessToken, string refreshToken, int expiresIn)> GetAccessTokenAsync(string code);

    public Task<(string accessToken, int expiresIn)> RefreshTokenAsync(string refreshToken);
}