using Application.Sync.Commands.SyncPlaylists;
using Infrastructure.Pushover;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<INotificationService, PushoverApi>(client =>
        {
            client.BaseAddress = new Uri("https://api.pushover.net/1/messages.json");
        });

        return services;
    }
}
