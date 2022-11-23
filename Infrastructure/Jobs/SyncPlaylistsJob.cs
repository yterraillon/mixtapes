using MediatR;

namespace Infrastructure.Jobs;

public class SyncPlaylistsJob 
{

    private readonly IMediator _mediator;

    public SyncPlaylistsJob(IMediator mediator) => _mediator = mediator;
}
