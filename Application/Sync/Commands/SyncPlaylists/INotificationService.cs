namespace Application.Sync.Commands.SyncPlaylists;

public interface INotificationService
{
    Task SendMessage(string message);
}
