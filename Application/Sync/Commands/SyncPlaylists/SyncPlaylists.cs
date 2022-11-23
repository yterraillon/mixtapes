using MediatR;

namespace Application.Sync.Commands.SyncPlaylists;

public static class SyncPlaylists
{
    public record Command() : IRequest;

    public class Handler : AsyncRequestHandler<Command>
    {
        private readonly INotificationService _notificationService;

        public Handler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        protected override async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _notificationService.SendMessage("pouet");
        }
    }
}
