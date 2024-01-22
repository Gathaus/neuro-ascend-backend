namespace Neuro.Application.Managers.Abstract;

public interface INotificationManager
{
    Task SendNotificationAsync(string token, string title, string body);
    Task SendNotificationToTopicAsync(string topic, string title, string body);
}