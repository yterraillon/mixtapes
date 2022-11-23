using Application.Sync.Commands.SyncPlaylists;
using MediatR;
using Quartz;

namespace Infrastructure.Jobs;

public class SyncPlaylistsJob : IJob
{
    private readonly IMediator _mediator;

    public SyncPlaylistsJob(IMediator mediator) => _mediator = mediator;
    public Task Execute(IJobExecutionContext context) => _mediator.Send(new SyncPlaylists.Command());
}
