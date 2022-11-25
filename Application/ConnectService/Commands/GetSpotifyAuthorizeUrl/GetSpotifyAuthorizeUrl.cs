using MediatR;

namespace Application.ConnectService.Commands.GetSpotifyAuthorizeUrl;

public static class GetSpotifyAuthorizeUrl
{
    public record Request : IRequest<Response>;

    public record Response(string Uri);

    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ISpotifyAuthorizeUrlBuilder _spotifyAuthorizeUrlBuilder;

        public Handler(ISpotifyAuthorizeUrlBuilder spotifyAuthorizeUrlBuilder) => _spotifyAuthorizeUrlBuilder = spotifyAuthorizeUrlBuilder;

        public Task<Response> Handle(Request request, CancellationToken cancellationToken) =>
            Task.FromResult(new Response(
                _spotifyAuthorizeUrlBuilder.BuildAuthorizeUrl(RequiredScopesForApp())
            ));

        private static IEnumerable<string> RequiredScopesForApp() =>
            new List<string>
            {
                Scopes.PlaylistReadPrivate,
                Scopes.PlaylistReadCollaborative,
                Scopes.UserReadPrivate
            };
    }

    private static class Scopes
    {
        public static string PlaylistReadPrivate => "playlist-read-private";

        public static string PlaylistReadCollaborative => "playlist-read-collaborative";

        public static string UserReadPrivate => "user-read-private";
    }
}