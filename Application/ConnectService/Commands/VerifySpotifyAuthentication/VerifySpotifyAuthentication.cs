using Application.ConnectService.Commands.GetSpotifyAuthorizeUrl;
using MediatR;

namespace Application.ConnectService.Commands.VerifySpotifyAuthentication;

public static class VerifySpotifyAuthentication
{
    public record Request(string Code, string State) : IRequest<Response>
        {
            public override string ToString() => $"{nameof(Code)}: {Code}";
        }

        public record Response
        {
            public bool IsSuccess { get; init; }

            public Response(bool isSuccess) => IsSuccess = isSuccess;

            public static Response Success() => new (true);
            public static Response Failure() => new (false);
        };

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IStateProvider _stateProvider;

            public Handler(IStateProvider stateProvider)
            {
                _stateProvider = stateProvider;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var (code, state) = request;
                if (!IsStateValid(state)) return Response.Failure();

                // (string accessToken, string refreshToken, var expiresIn) = await _spotifyTokensClient.GetAccessTokenAsync(code);
                // var tokens = Tokens.Create(accessToken, refreshToken, expiresIn);
                // _tokensRepository.Create(tokens);

                return Response.Success();
            }

            private bool IsStateValid(string state)
            {
                var expectedState = _stateProvider.State;
                return expectedState == state;
            }
        }
}