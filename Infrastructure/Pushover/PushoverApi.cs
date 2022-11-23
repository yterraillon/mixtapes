using Application.Sync.Commands.SyncPlaylists;

namespace Infrastructure.Pushover;

public class PushoverApi : INotificationService
{
    private readonly HttpClient _httpClient;

    public PushoverApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendMessage(string message)
    {
        // pb docker
        // https://github.com/grpc/grpc-dotnet/issues/1567
        // https://stackoverflow.com/questions/65961176/ssl-broken-with-ld-library-path-and-matlab

        var nvc = new List<KeyValuePair<string, string>>
        {
            new ("token", "a5x9bbigzbe3vsrzucbcv5z6yk91yz"),
            new ("user", "c7RBFdS5WcEA5MAhddldUQTWNzi4Fw"),
            new ("message", message)
        };

        var req = new HttpRequestMessage(HttpMethod.Post, _httpClient.BaseAddress) { 
            Content = new FormUrlEncodedContent(nvc) 
        };
        var res = await _httpClient.SendAsync(req);

        var status = res.StatusCode;
    }

    public class Response
    {
        public int Status { get; set; }
        public string Request { get; set; } = string.Empty;
    }
}
