using Application.Sync.Commands.SyncPlaylists;
using Infrastructure.Jobs;
using Infrastructure.Pushover;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
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

        return services;
    }
}
