using Application.ConnectService.Commands.GetSpotifyAuthorizeUrl;
using Application.Sync.Commands.SyncPlaylists;
using Infrastructure.Jobs;
using Infrastructure.Pushover;
using Infrastructure.Spotify.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Settings>(configuration.GetSection("Spotify:Authentication"));
        services.AddTransient<ISpotifyAuthorizeUrlBuilder, SpotifyAuthorizeUrlBuilder>();
        services.AddSingleton<CodeProvider>();
        services.AddSingleton<IStateProvider, StateProvider>();
        
        services.AddHttpClient<INotificationService, PushoverApi>(client =>
        {
            client.BaseAddress = new Uri("https://api.pushover.net/1/messages.json");
        });

        services.AddQuartz(quartz =>
        {
            const string jobKey = "SyncPlaylistsJob";
            quartz
                .AddJob<SyncPlaylistsJob>(new JobKey(jobKey))
                .AddTrigger(options =>
                {
                    options.ForJob(jobKey);
                    options.StartNow()
                        .WithSchedule(SetUpDailySchedule());
                });
            
            quartz.UseMicrosoftDependencyInjectionJobFactory();

            SimpleScheduleBuilder SetUpDailySchedule() => 
                SimpleScheduleBuilder.Create()
                    .WithIntervalInSeconds(10)
                    //.WithIntervalInHours(24)
                .RepeatForever();
        });
        
        services.AddQuartzServer(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }
}
